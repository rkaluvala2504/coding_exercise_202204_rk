using System;
using NUnit.Framework;

namespace ReturnCalculator
{
    [TestFixture]
    public sealed class PeriodTests
    {
        #region Tests

        [Test]
        public void ConfirmConstructorWithInvalidMonth()
        {
            Assert.That(() => Period.From(-1, 2020), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(() => Period.From(13, 2020), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ConfirmConstructorWithInvalidYear()
        {
            // Invalid Year
            Assert.That(() => Period.From(12, -2020), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ConfirmNextPeriod()
        {
            for (var month = 1; month < 12; month++)
            {
                Assert.That(Period.From(month,2021).NextPeriod(), Is.EqualTo(Period.From(month + 1, 2021)));
            }
            var endOfYearPeriod = Period.From(12, 2021);
            Assert.That(endOfYearPeriod.NextPeriod(), Is.EqualTo(Period.From(1, 2022)));
        }

        [Test]
        public void ConfirmPriorPeriod()
        {
            var startOfYearPeriod = Period.From(1, 2021);
            Assert.That(startOfYearPeriod.PriorPeriod(), Is.EqualTo(Period.From(12, 2020)));

            for (var month = 2; month <= 12; month++)
            {
                Assert.That(Period.From(month,2021).PriorPeriod(), Is.EqualTo(Period.From(month -1, 2021)));
            }
        }

        [Test]
        public void ConfirmOperators()
        {
            var currentPeriod = Period.From(6, 2021);
            Assert.That(currentPeriod < currentPeriod.NextPeriod(), Is.True);
            Assert.That(currentPeriod < currentPeriod.PriorPeriod(), Is.False);

            Assert.That(currentPeriod <= currentPeriod, Is.True);
            Assert.That(currentPeriod <= currentPeriod.NextPeriod(), Is.True);
            Assert.That(currentPeriod <= currentPeriod.PriorPeriod(), Is.False);

            Assert.That(currentPeriod > currentPeriod.NextPeriod(), Is.False);
            Assert.That(currentPeriod > currentPeriod.PriorPeriod(), Is.True);

            Assert.That(currentPeriod <= currentPeriod, Is.True);
            Assert.That(currentPeriod >= currentPeriod.NextPeriod(), Is.False);
            Assert.That(currentPeriod >= currentPeriod.PriorPeriod(), Is.True);

            Assert.That(currentPeriod == currentPeriod.NextPeriod(), Is.False);
            Assert.That(currentPeriod == currentPeriod, Is.True);
            Assert.That(currentPeriod != currentPeriod, Is.False);
            Assert.That(currentPeriod != currentPeriod.NextPeriod(), Is.True);
        }



        [Test]
        public void ConfirmQuarterIsCalculatedCorrectly()
        {
            Assert.That(Period.From(1, 2020).Quarter(), Is.EqualTo(1));
            Assert.That(Period.From(2, 2020).Quarter(), Is.EqualTo(1));
            Assert.That(Period.From(3, 2020).Quarter(), Is.EqualTo(1));
            Assert.That(Period.From(4, 2020).Quarter(), Is.EqualTo(2));
            Assert.That(Period.From(5, 2020).Quarter(), Is.EqualTo(2));
            Assert.That(Period.From(6, 2020).Quarter(), Is.EqualTo(2));
            Assert.That(Period.From(7, 2020).Quarter(), Is.EqualTo(3));
            Assert.That(Period.From(8, 2020).Quarter(), Is.EqualTo(3));
            Assert.That(Period.From(9, 2020).Quarter(), Is.EqualTo(3));
            Assert.That(Period.From(10, 2020).Quarter(), Is.EqualTo(4));
            Assert.That(Period.From(11, 2020).Quarter(), Is.EqualTo(4));
            Assert.That(Period.From(12, 2020).Quarter(), Is.EqualTo(4));
        }

        #endregion
    }
}