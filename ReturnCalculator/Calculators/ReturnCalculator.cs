using System;
using System.Collections.Generic;
using System.Linq;

namespace ReturnCalculator
{
    public class ReturnCalculator : IReturnCalculator
    {
        private readonly IEnumerable<EndingMarketValueData> endingMarketValues;
        private readonly int returnsPrecision;

        private Dictionary<string, decimal> endingMarketValueMapping;

        #region Constructor

        public ReturnCalculator(IEnumerable<EndingMarketValueData> endingMarketValues, int returnsPrecision = 5)
        {
            this.endingMarketValues = endingMarketValues;
            this.returnsPrecision = returnsPrecision;
            endingMarketValueMapping = GetDictionaryMapping();
        }
        #endregion

        #region Properties

        public IEnumerable<EndingMarketValueData> EndingMarketValues => endingMarketValues;
        public int ReturnsPrecision => returnsPrecision;

        #endregion

        #region Public Methods

        public decimal CalculateReturns(Period period, CalcTypeEnum calcTypeEnum)
        {
            var calculatedReturns = calcTypeEnum switch
            {
                CalcTypeEnum.MTD => CalculateMTD(period),
                CalcTypeEnum.QTD => CalculateQTD(period),
                CalcTypeEnum.YTD => CalculateYTD(period),
                CalcTypeEnum.OneYearTrail => CalculateOneYearTrail(period),
                _ => throw new NotImplementedException(),
            };

            return calculatedReturns;
        }

        public IDictionary<CalcTypeEnum, PeriodReturns> GetReturnsForReporting(Period period)
        {
            Dictionary<CalcTypeEnum, PeriodReturns> reportingReturns = new();
            
            try
            {
                decimal mtd = CalculateMTD(period);
                reportingReturns.Add(CalcTypeEnum.MTD, new PeriodReturns(mtd, "Success"));
            }
            catch (Exception ex)
            {
                reportingReturns.Add(CalcTypeEnum.MTD, new PeriodReturns(0, ex.Message));
            }

            try
            {
                decimal qtd = CalculateQTD(period);
                reportingReturns.Add(CalcTypeEnum.QTD, new PeriodReturns(qtd, "Success"));
            }
            catch (Exception ex)
            {
                reportingReturns.Add(CalcTypeEnum.QTD, new PeriodReturns(0, ex.Message));
            }

            try
            {
                decimal ytd = CalculateYTD(period);
                reportingReturns.Add(CalcTypeEnum.YTD, new PeriodReturns(ytd, "Success"));
            }
            catch (Exception ex)
            {
                reportingReturns.Add(CalcTypeEnum.YTD, new PeriodReturns(0, ex.Message));
            }

            try
            {
                decimal oneYearTrail = CalculateOneYearTrail(period);
                reportingReturns.Add(CalcTypeEnum.OneYearTrail, new PeriodReturns(oneYearTrail, "Success"));
            }
            catch (Exception ex)
            {
                reportingReturns.Add(CalcTypeEnum.OneYearTrail, new PeriodReturns(0, ex.Message));
            }

            return reportingReturns;
        }

        #endregion

        #region Private Methods

        private Dictionary<string, decimal> GetDictionaryMapping()
        {
            Dictionary<string, decimal> mapping = new();
            foreach (var marketValue in endingMarketValues)
            {
                mapping.Add(string.Concat(marketValue.Month, "-", marketValue.Year), marketValue.EndingMarketValue);
            }
            return mapping;
        }


        private string GetPeriodKey(Period period) => string.Concat(period.Month, "-", period.Year);

        /// <summary>
        /// Takes a period parameter and returns MTD value
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        private decimal CalculateMTD(Period period)
        {
            var priorPeriodKey = GetPeriodKey(period.PriorPeriod());
            var periodKey = GetPeriodKey(period);

            if (!endingMarketValueMapping.ContainsKey(priorPeriodKey))
                throw new Exception($"Failed to Calculate MTD Due to missing Prior Month Data for Period: {period.Month} - {period.Year} ");

            if (!endingMarketValueMapping.ContainsKey(periodKey))
                throw new Exception($"Failed to Calculate MTD Due to missing Current Month Data for Period: {period.Month} - {period.Year} ");
             
            decimal returns = (endingMarketValueMapping[periodKey] - endingMarketValueMapping[priorPeriodKey]) /
                endingMarketValueMapping[priorPeriodKey];

            return decimal.Round(returns,ReturnsPrecision);

        }

        /// <summary>
        /// Calculates QTD for the period Passed 
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        private decimal CalculateQTD(Period period)
        {
            decimal qtd = 0;
            try
            {
                int monthEnd = period.Quarter() switch
                {
                    1 => 0,
                    2 => 3,
                    3 => 6,
                    4 => 10
                };
                
                qtd = Calculate(period.Month, monthEnd, period.Year);
                qtd -= 1;
            }
            catch (Exception)
            {
                throw new Exception($"Failed to Calculate YTD Due to Missing Yearly Data for Month: {period.Month} and Year {period.Year}");
            }

            return decimal.Round(qtd, ReturnsPrecision);
        }

        /// <summary>
        /// Calculates YTD for the period
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        private decimal CalculateYTD(Period period)
        {
            decimal ytd;
            try
            {
                ytd = Calculate(period.Month, 0,period.Year);
                ytd -= 1;
            }

            catch (Exception)
            {
                throw new Exception($"Failed to Calculate YTD Due to Missing Yearly Data for Month: {period.Month} and Year {period.Year}");
            }
           
            return decimal.Round(ytd, ReturnsPrecision);        
     
        }

        /// <summary>
        /// Calculates One Year Trail Data
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        
        private decimal CalculateOneYearTrail(Period period)
        {
            if (!IsTrailDataAvailable(period))
                throw new Exception($"One Year trailing Data is not available for Returns Calculation for Period: {period.Month} -{period.Year} ");

            int currentMonth = period.Month;
            int year = period.Year;
            decimal trailReturns = 1;
            decimal result;
            Period newPeriod = period;
            //Looping through last 12 Months
            for (int i = 12; i > 0; i--)
            {  
                result = CalculateMTD(newPeriod);
                var returns = 1 + result;
                trailReturns *= returns;
                newPeriod = newPeriod.PriorPeriod();
            }
            trailReturns -= 1;
            return decimal.Round(trailReturns, ReturnsPrecision);
        }

        private bool IsTrailDataAvailable(Period period, int yearTrail =1)
        {

            var result = endingMarketValues.Select(mv => mv).Where(mv  => mv.Year <= period.Year);
            return result.Count() >= yearTrail * 12;
            
        }

        /// <summary>
        /// calculates returns
        /// </summary>
        /// <param name="monthStart"></param>
        /// <param name="monthEnd"></param>
        /// <returns></returns>
        private decimal Calculate(int monthStart, int monthEnd,int year)
        {
            decimal result = 1;
            decimal mtd = 0;
            for (int i = monthStart; i > monthEnd; --i)
            {
                var monthlyPeriod = Period.From(i, year);
               
                mtd = CalculateMTD(monthlyPeriod);
               
                var returns = 1 + mtd;
                result *= returns;
            }
            return result;
        }
        #endregion

    }
}


