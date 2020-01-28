namespace GTR_Watch_face
{
    public class CaloriesContainer
    {
        public CircleScale Circle { get; set; }
        public ClockHand ClockHand { get; set; }
    }

    public class Distance
    {
        public Number Number { get; set; }
        public long? SuffixImageIndex { get; set; }
        public long? DecimalPointImageIndex { get; set; }
        public string Color { get; set; }
    }

    public class FormattedNumber
    {
        public Number Step { get; set; }
        public long? SuffixImageIndex { get; set; }
        public long? DecimalPointImageIndex { get; set; }
        public string Color { get; set; }
    }

    public class PulseContainer
    {
        public ClockHand ClockHand { get; set; }
    }
}
