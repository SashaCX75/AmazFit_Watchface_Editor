namespace GTR_Watch_face
{
    public class ClockHand
    {
        public bool OnlyBorder { get; set; }
        public string Color { get; set; }
        public Coordinates CenterOffset { get; set; }
        public Coordinates Shape { get; set; }
        public ImageW Image { get; set; }
        public Sector Sector { get; set; }
    }
}
