namespace GTR_Watch_face
{
    public class IconW
    {
        /// <summary>Номер первого изображения</summary>
        public ImageSet Images { get; set; }
        /// <summary>Номер изображения при отсутствии данных</summary>
        public long NoWeatherImageIndex { get; set; }
    }

    public class Separate
    {
        public Number Day { get; set; }
        public Number Night { get; set; }
    }

    public class Symbols
    {
        public long Unknown0800 { get; set; }
        public long MinusImageIndex { get; set; }
        public long DegreesImageIndex { get; set; }
        public long NoDataImageIndex { get; set; }
    }

    public class Temperature
    {
        /// <summary>Текущая температура</summary>
        public Number Current { get; set; }
        public Today Today { get; set; }
        /// <summary>Набор вспомогательных символов</summary>
        public Symbols Symbols { get; set; }
        public CircleScale TemperatureMeter { get; set; }
    }

    public class Today
    {
        public Separate Separate { get; set; }
        public bool AppendDegreesForBoth { get; set; }
    }
}
