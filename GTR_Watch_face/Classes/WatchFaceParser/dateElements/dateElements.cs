namespace GTR_Watch_face
{
    public class DateUnknown3
    {
        public UnknownType Unknown2 { get; set; }
    }

    public class MonthAndDay
    {
        /// <summary>Отдельно число и месяц</summary>
        public SeparateMonthAndDay Separate { get; set; }
        /// <summary>Дата одной строкой</summary>
        public OneLineMonthAndDay OneLine { get; set; }
        public bool TwoDigitsMonth { get; set; }
        public bool TwoDigitsDay { get; set; }
    }

    public class OneLineMonthAndDay
    {
        public Number Number { get; set; }
        public long? DelimiterImageIndex { get; set; }
    }

    public class OneLineYear
    {
        public Number Number { get; set; }
        public long? DelimiterImageIndex { get; set; }
    }

    public class SeparateMonthAndDay
    {
        public Number Month { get; set; }
        public ImageSet MonthName { get; set; }
        public Number Day { get; set; }
    }

    public class Year
    {
        public OneLineYear OneLine { get; set; }
    }
}
