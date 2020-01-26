namespace GTR_Watch_face
{
    public class CircleScale
    {
        /// <summary>X координата центра</summary>
        public long CenterX { get; set; }
        /// <summary>Y координата центра</summary>
        public long CenterY { get; set; }
        /// <summary>Радиус</summary>
        public long RadiusX { get; set; }
        public long RadiusY { get; set; }
        /// <summary>Начальный угол</summary>
        public long StartAngle { get; set; }
        /// <summary>Конечный угол</summary>
        public long EndAngle { get; set; }
        /// <summary>Толщина линии</summary>
        public long Width { get; set; }
        /// <summary>Цвет</summary>
        public string Color { get; set; }
        /// <summary>Тип окончания линии</summary>
        public long Flatness { get; set; }
        /// <summary>Тип окончания линии</summary>
        public long? ImageIndex { get; set; }
    }

    public class Coordinates
    {
        /// <summary>X координата</summary>
        public long X { get; set; }
        /// <summary>Y координата</summary>
        public long Y { get; set; }
    }

    public class IconSet
    {
        /// <summary>Номер первого изображения</summary>
        public long ImageIndex { get; set; }
        /// <summary>Координаты</summary>
        //public Coordinates Coordinates { get; set; }
        //public string Coordinates { get; set; }
        public Coordinates[] Coordinates { get; set; }
    }

    public class ImageW
    {
        /// <summary>X координата</summary>
        public long X { get; set; }
        /// <summary>Y координата</summary>
        public long Y { get; set; }
        /// <summary>Номер изображения</summary>
        public long ImageIndex { get; set; }
    }

    public class ImageSet
    {
        /// <summary>X координата</summary>
        public long X { get; set; }
        /// <summary>Y координата</summary>
        public long Y { get; set; }
        /// <summary>Номер первого изображения</summary>
        public long ImageIndex { get; set; }
        /// <summary>Количество изображений</summary>
        public long ImagesCount { get; set; }
    }

    public class Number
    {
        public long TopLeftX { get; set; }
        public long TopLeftY { get; set; }
        public long BottomRightX { get; set; }
        public long BottomRightY { get; set; }
        public string Alignment { get; set; }
        public long Spacing { get; set; }
        public long ImageIndex { get; set; }
        public long ImagesCount { get; set; }
    }

    public class Sector
    {
        /// <summary>Начальный угол</summary>
        public long StartAngle { get; set; }
        /// <summary>Конечный угол</summary>
        public long EndAngle { get; set; }
    }

    public class UnknownType
    {
        public long TopLeftX { get; set; }
        public long TopLeftY { get; set; }
        public long BottomRightX { get; set; }
        public long BottomRightY { get; set; }
        public string Alignment { get; set; }
        public long Spacing { get; set; }
        public long ImageIndex { get; set; }
        public long ImagesCount { get; set; }
    }
}
