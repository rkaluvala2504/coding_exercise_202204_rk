using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace ReturnCalculator
{
	[TestFixture]
	public class ReturnCalculatorTests
	{
		private IReturnCalculator _returnCalculator;

        [SetUp]
		public void SetUp()
		{
			_returnCalculator = new ReturnCalculator(SampleEndingMarketValues.SampleData());
		}

		[Test]
		public void CalculateMTDTest()
		{
			Period period = Period.From(3, 2021);
			var mtd = _returnCalculator.CalculateReturns(period, CalcTypeEnum.MTD);
			Assert.AreEqual(0.01253, mtd);

			period = Period.From(2, 2021);
			mtd = _returnCalculator.CalculateReturns(period, CalcTypeEnum.MTD);
			Assert.AreEqual(-0.00287, mtd);

			period = Period.From(1, 2021);
			mtd = _returnCalculator.CalculateReturns(period, CalcTypeEnum.MTD);
			Assert.AreEqual(-0.02913, mtd);
		}

		[Test]
		public void CalculateQTDTest()
		{
			Period period = Period.From(3, 2021);
			var qtd = _returnCalculator.CalculateReturns(period, CalcTypeEnum.QTD);
			Assert.AreEqual(-0.01979, qtd);

			period = Period.From(2, 2021);
			qtd = _returnCalculator.CalculateReturns(period, CalcTypeEnum.QTD);
			Assert.AreEqual(-0.03192, qtd);

			period = Period.From(1, 2021);
			qtd = _returnCalculator.CalculateReturns(period, CalcTypeEnum.QTD);
			Assert.AreEqual(-0.02913, qtd);
			
		}

		[Test]
		public void CalculateYTDTest()
		{
			Period period = Period.From(12, 2020);
			var ytd = _returnCalculator.CalculateReturns(period, CalcTypeEnum.YTD);
			Assert.AreEqual(-0.02656, ytd);

			period = Period.From(11, 2020);
			ytd = _returnCalculator.CalculateReturns(period, CalcTypeEnum.YTD);
			Assert.AreEqual(0.02027, ytd);


			period = Period.From(2, 2021);
			ytd = _returnCalculator.CalculateReturns(period, CalcTypeEnum.YTD);
			Assert.AreEqual(-0.03192, ytd);
		}

		[Test]
		public void CalculateOneYearTrailTest()
		{
			Period period = Period.From(3, 2021);
			var ytd = _returnCalculator.CalculateReturns(period, CalcTypeEnum.OneYearTrail);
			Assert.AreEqual(0, ytd);
		}

		[Test]
		public void GetReturnsForReporting()
		{
			Period period = Period.From(3, 2021);
			IDictionary<CalcTypeEnum, PeriodReturns> result = _returnCalculator.GetReturnsForReporting(period);

			Assert.AreEqual(0.01253, result[CalcTypeEnum.MTD].Value);
			Assert.AreEqual(-0.01979, result[CalcTypeEnum.QTD].Value);
			Assert.AreEqual(-0.01979, result[CalcTypeEnum.YTD].Value);
		}
	}
}

