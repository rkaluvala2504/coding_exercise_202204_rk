# Coding Interview
*If you have any questions regarding what is expected or feel you need more time to complete the problem, please to not hesitate to contact us.*

## Repository Contents
This repository contains two empty C# projects. 
1. **ReturnCalculator** - This is a class library project to which you will add the classes that implement the functionality described in the problem statement.
2. **ReturnCalculator.Tests** - This is an NUnit test project, to which you should add unit tests to to demonstrate that your code is functioning properly and to provide examples of how you expect your code to be used.

Additionally, in the test project there is a static class, SampleEndingMarketValues, that provides some sample ending market values that you can use to test your code. There is table below that shows the expected output for a number of different scenarios using this
data.

## Background
As a wealth advisory firm, we spend a lot of time looking at investment returns. A return on an investment can be expressed as a change in dollar value or as a percentage calculated by dividing the profit by the investment. Because investors often want to compare the relative performance of investments of different sizes the percentage return is used quite commonly. 

Investment performance is reported to investors on a periodic basis. Often this period is monthly. This is similar to the way that your bank provides monthly statements about your accounts. When looking at the monthly performance investors are often also interested in how that value compares to performance over a longer reporting period. Examples of common reporting periods are month-to-date(mtd), quarter-to-date (qtd), year-to-date(ytd), and 1-year trailing.

When calculating the return for one of these reporting periods, the value can be calculated by linking the returns geometrically. Additionally, when the length of the reporting period is longer than 12 months the geomtrically linked return computed for the reporting period is annualized.

## Problem Statement
We would like you to write a class library that given a list of monthly ending market values (value at the end of the month) and a month of interest can calculate the return for various reporting periods for the month of interest. The compounding periods are:
1. month-to-date (mtd)
2. quarter-to-date (qtd)
3. year-to-date (ytd) 
4. 1-year trailing

For simplicity assume that there are no cash flows and that the ending market value(EMV) of the previous month is the beginning market value of the following month. 

### Return Calculation
The return for a period n is calculated as follows

 R<sub>n</sub> = (EMV<sub>n</sub> - EMV<sub>n-1</sub>) / EMV<sub>n-1</sub>

The compound return for multiple periods is calculated by geometrically linking the returns for each period

Compound Return = (1+R<sub>1</sub>) * (1+R<sub>2</sub>) * ... (1+R<sub>n</sub>) - 1

If the number of periods used to compute the compound return is 12 or more then the compound return should be annualized as follows

Annualized Return = (1 + Compound Return)<sup>(12/number of months)</sup> - 1

### Example Calculations

| Period | EMV |
| :------|----:|
| 03-2021| 101 |
| 02-2021| 100 |
| 01-2021|  99 |
| 12-2020|  98 |

Given the ending market values and periods in the table above, lets look at a few sample calculations:

### MTD for 03-2021
(101 - 100)/ 100 = 0.1

### QTD for 02-2021
(1 + (100 - 99)/99) * (1 + (99 - 98)/98) - 1 = 0.0204

### Values for sample data in test project
The table below shows the computed values for the sample
data in the test project. These values can be used to confirm your understanding of the calculations and that you implemented them correctly.

|Period	|  EMV            |  MTD     |  QTD     |  YTD     | 1-Year Trailing |
|------:|----------------:|---------:|---------:|---------:|----------------:|
|3-2021  | $12,635,789.00 | 0.01253  | -0.01978 | -0.01978 |        -0.09860 |
|2-2021  | $12,479,360.93 | -0.00287 | -0.03192 | -0.03192 |        -0.08838 |
|1-2021  | $12,515,313.89 | -0.02913 | -0.02913 | -0.02913 |        -0.07281 |
|12-2020 | $12,890,769.86 | -0.04590 |          | -0.02656 |        -0.02656 |
|11-2020 | $13,510,915.13 | 0.02578  |          | 0.020275 |        -0.00192 |
|10-2020 | $13,171,321.35 | 0.00776  |          |          |                 |
|9-2020  | $13,069,860.45 | -0.00207 |          |          |                 |
|8-2020  | $13,096,978.82 | -0.01760 |          |          |                 |
|7-2020  | $13,331,655.66 | -0.03765 |          |          |                 |
|6-2020  | $13,853,261.72 | 0.00620  |          |          |                 |
|5-2020  | $13,767,940.27 | 0.02843  |          |          |                 |
|4-2020  | $13,387,402.76 | -0.04499 |          |          |                 |
|3-2020  | $14,018,030.52 | 0.02402  |          |          |                 |
|2-2020  | $13,689,234.77 | 0.01415  |          |          |                 |
|1-2020  | $13,498,170.03 | 0.01931  |          |          |                 |
|12-2019 | $13,242,428.74 | -0.02175 |          |          |                 |
|11-2019 | $13,536,868.64 | 0.00223  |          |          |                 |
|10-2019 | $13,506,790.22 |          |          |          |                 |


If the value cannot be calculated because there is not enough data, the library should indicate an error by throwing an appropriate exception. For instance, if a client asks for the MTD return for the period 10-2019 the code should throw an exception because there is no prior period to use in the calculation.  Similarly if a 1-year trailing return is requested, but there are only eleven months of data available and exception should be thrown.

### Some design considerations
1. In the future we will likely need to add additional compounding periods, such as 3- and 5-year trailing returns.  Your solution should be able to be easily modified to add these calculations in the future.
2. If there are gaps in the data provided to the API, they should be considered an error. For example, if and ending market value is provided for 03-2021 and 01-2021 but not 02-2021 the API should throw a relevant exception.
3. Often the users of the API will need to calculate multiple reporting periods for the same set of ending market values, so that use-case should be easy to do. For example, given a particular set of ending market values a user of the library would likely need to calculate the MTD, QTD and YTD values for the same period in order to display on a report.
4. We've provided a Period class and corresponding test cases to help you get started and to serve as a guide for the style of your unit tests.  Please make use of the Period class in your solution. 

## Additional Guidance

### Solution to the Problem
Of course getting the correct output is important, but equally important the design of your solution. We would like to see how you make use object oriented design principles, C# language features and good coding practices to make your code more extensible, maintainable and understandable.

### Naming and Library Design
We recognize that you might not have worked recently with .NET, and so we realize you might not be familiar with the generally accepted conventions. To that end, when we evaluate your solution, we will not be expecting you to follow .NET conventions exactly. More important is consistency and being able to explain your choices. If you want some guidance, you may follow the guidelines here https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/. However, you overall design is more important than the exact conventions you follow.

### Reporting Errors
When a return cannot be calculated because of an error or anomaly in the data, the library should indicate the problem by throwing appropriate exceptions and not by returning an error code.

### Instrumentation
If this were actual code for production, it would require some sort of instrumentation such as logging for debugging and diagnosing errors. However, for this exercise there is no need to add any logging or other instrumentation.

### External References
Again, if this were code for actual production it is likely you would want to make use of some external libraries to reduce the amount of code you need to write and improve the robustness of your solution. However, for this example please do not add any additional package references to the project files contained in this repository.  We encourage you to use nUnit for testing, as that reference is already included in the project.


