using System;

namespace ReturnCalculator
{
    public sealed class Period : IComparable<Period>, IEquatable<Period>
    {
        #region constructors

        private Period(int month, int year)
        {
            if (month > 12 || month < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(month), "The month must be between one and 12.");
            }

            if (year < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(year), "The year must be a positive integer.");
            }

            Month = month;
            Year = year;
        }

        #endregion

        #region public static methods

        public static Period From(int month, int year)
        {
            return new(month, year);
        }

        public static bool operator ==(Period? left, Period? right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        public static bool operator !=(Period left, Period right)
        {
            return !(left == right);
        }

        public static bool operator <(Period left, Period right)
        {
            return Compare(left, right) < 0;
        }

        public static bool operator >(Period left, Period right)
        {
            return Compare(left, right) > 0;
        }

        public static bool operator >=(Period left, Period right)
        {
            return Compare(left, right) >= 0;
        }

        public static bool operator <=(Period left, Period right)
        {
            return Compare(left, right) <= 0;
        }

        #endregion

        #region interface implementations

        public int CompareTo(Period? other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            var yearComparison = Year.CompareTo(other.Year);
            return yearComparison != 0 ? yearComparison : Month.CompareTo(other.Month);
        }

        public bool Equals(Period? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Month == other.Month && Year == other.Year;
        }

        #endregion

        #region public properties

        public int Month { get; }

        public int Year { get; }

        #endregion

        #region public methods

        public override int GetHashCode()
        {
            unchecked
            {
                return (Month * 397) ^ Year;
            }
        }

        public Period NextPeriod()
        {
            if (Month == 12)
            {
                return From(1, Year + 1);
            }
            return From(Month + 1, Year);
        }

        public Period PriorPeriod()
        {
            if (Month == 1)
            {
                return new Period(12, Year - 1);
            }

            return From(Month - 1, Year);
        }

        public int Quarter()
        {
            return (int) Math.Ceiling(Month / 3d);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Period);
        }

        #endregion

        #region private methods

        private static int Compare(Period? left, Period? right)
        {
            if (ReferenceEquals(left, right))
            {
                return 0;
            }

            if (left is null)
            {
                return -1;
            }

            return left.CompareTo(right);
        }

        #endregion
    }
}