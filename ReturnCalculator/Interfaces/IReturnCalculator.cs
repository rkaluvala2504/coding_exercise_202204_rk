using System;
using System.Collections.Generic;

namespace ReturnCalculator
{
	public interface IReturnCalculator
	{
        IEnumerable<EndingMarketValueData> EndingMarketValues { get; }
        /// <summary>
        /// Calculates Returns For Period And Calc Type Enum i.e. MTD,QTD,YTD etc..
        /// </summary>
        /// <param name="period"></param>
        /// <param name="calcTypeEnum"></param>
        /// <returns></returns>
        decimal CalculateReturns(Period period, CalcTypeEnum calcTypeEnum);

        /// <summary>
        /// Calculates All the Returns Needed For Reporting.
        /// Currently Supports MTD,QTD ,YTD and 1Year Trail
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        IDictionary<CalcTypeEnum,PeriodReturns> GetReturnsForReporting(Period period);
    }
}

