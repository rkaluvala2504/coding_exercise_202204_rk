using System;
namespace ReturnCalculator
{
	public enum CalcTypeEnum
	{
        MTD = 0,
		QTD = 1,
		YTD = 2,
        OneYearTrail = 3

		// Extend Here By Adding For Future Calc Types
	}

	public sealed record PeriodReturns(decimal Value,string Message);
	
}

