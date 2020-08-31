namespace GTR_Watch_face
{
    public class AmPm
    {
        public long X { get; set; }
        public long Y { get; set; }
        public long ImageIndexAMCN { get; set; }
        public long ImageIndexPMCN { get; set; }
        public long? ImageIndexAMEN { get; set; }
        public long? ImageIndexPMEN { get; set; }
    }

    public class TwoDigits
    {
        public ImageSet Tens { get; set; }
        public ImageSet Ones { get; set; }
    }
}
