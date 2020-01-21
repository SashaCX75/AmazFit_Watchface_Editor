namespace GTR_Watch_face
{

    /// <summary>Корневая структура JSON файла</summary>
    public class WATCH_FACE_JSON
    {
        /// <summary>Задний фон</summary>
        public Background Background { get; set; }
        /// <summary>Фремя в цифровом формате</summary>
        public TimeW Time { get; set; }
        /// <summary>Активности (спорт)</summary>
        public Activity Activity { get; set; }
        /// <summary>Дата</summary>
        public Date Date { get; set; }
        /// <summary>Индикаторы даты</summary>
        public DaysProgress DaysProgress { get; set; }
        /// <summary>Погода</summary>
        public Weather Weather { get; set; }
        /// <summary>Прогресс шагов</summary>
        public StepsProgress StepsProgress { get; set; }
        /// <summary>Статусы (Bluetooth, будильник, блокировка, DND)</summary>
        public Status Status { get; set; }
        /// <summary>Батарея</summary>
        public Battery Battery { get; set; }
        /// <summary>Аналоговык часы</summary>
        public Analogdialface AnalogDialFace { get; set; }

    }

    public class Background
    {
        /// <summary>Изображение заднего фона</summary>
        public ImageW Image { get; set; }
        /// <summary>Изображение для предпросмотра</summary>
        public ImageW Preview { get; set; }
        /// <summary>Логотип</summary>
        public ImageW FrontImage { get; set; }
    }

    public class TimeW
    {
        /// <summary>Часы</summary>
        public TwoDigits Hours { get; set; }
        /// <summary>Минуты</summary>
        public TwoDigits Minutes { get; set; }
        /// <summary>Секунды</summary>
        public TwoDigits Seconds { get; set; }
        /// <summary>Значок AmPm</summary>
        public AmPm AmPm { get; set; }
        public long? DrawingOrder { get; set; }
        public long? Unknown9 { get; set; }
        /// <summary>Разделитель</summary>
        public ImageW Delimiter { get; set; }
    }

    public class Activity
    {
        /// <summary>Цель шагов</summary>
        public CircleScale StepsGoal { get; set; }
        /// <summary>Калории</summary>
        public Number Calories { get; set; }
        /// <summary>Пульс</summary>
        public Number Pulse { get; set; }
        /// <summary>Пульс круговая шкала</summary>
        public CircleScale Goal2 { get; set; }
        /// <summary>Растояние</summary>
        public Distance Distance { get; set; }
        /// <summary>Шаги</summary>
        public FormattedNumber Steps { get; set; }
        /// <summary>Шаги</summary>
        public ImageW StarImage { get; set; }
        public long? NoDataImageIndex { get; set; }
    }

    public class Date
    {
        /// <summary>Дни и месяцы</summary>
        public MonthAndDay MonthAndDay { get; set; }
        /// <summary>Номер дня недели</summary>
        public ImageSet WeekDay { get; set; }
        public DateUnknown3 Unknown3 { get; set; }
        public Coordinates Unknown4 { get; set; }
        /// <summary>Год</summary>
        public Year Year { get; set; }
    }

    public class DaysProgress
    {
        /// <summary>Стрелка месяца</summary>
        public ClockHand AnalogMonth { get; set; }
        /// <summary>Стрелка дней</summary>
        public ClockHand UnknownField2 { get; set; }
        /// <summary>Стрелка дня недели</summary>
        public ClockHand AnalogDOW { get; set; }
    }

    public class StepsProgress
    {
        public ImageSet Images1 { get; set; }
        /// <summary>Прогресс шагов в виде набора картинок и координат к ним</summary>
        public IconSet Sliced { get; set; }
        public ImageSet Images4 { get; set; }
        /// <summary>Шкала прогресса</summary>
        public CircleScale Circle { get; set; }
        /// <summary>Индикатор прогресса шагов</summary>
        public ClockHand ClockHand { get; set; }
    }

    public class Status
    {
        /// <summary>Bluetooth</summary>
        public SwitchW Bluetooth { get; set; }
        /// <summary>Будильник</summary>
        public SwitchW Alarm { get; set; }
        /// <summary>Блокировка</summary>
        public SwitchW Lock { get; set; }
        /// <summary>Не беспокоить</summary>
        public SwitchW DoNotDisturb { get; set; }
    }

    public class Battery
    {
        /// <summary>Процент заряда в виде текста</summary>
        public Number Text { get; set; }
        /// <summary>Процент заряда в виде набора картинок</summary>
        public ImageSet Images { get; set; }
        /// <summary>Процент заряда в виде набора картинок и координат к ним</summary>
        public IconSet Icons { get; set; }
        /// <summary>Индикатор</summary>
        public ClockHand Unknown4 { get; set; }
        /// <summary>Иконка %</summary>
        public ImageW Percent { get; set; }
        /// <summary>Процент заряда в виде шкалы прогрессы</summary>
        public CircleScale Scale { get; set; }
    }

    public class Analogdialface
    {
        /// <summary>Часовая стрелка</summary>
        public ClockHand Hours { get; set; }
        public ImageW HourCenterImage { get; set; }
        /// <summary>Минутная стрелка</summary>
        public ClockHand Minutes { get; set; }
        public ImageW MinCenterImage { get; set; }
        /// <summary>Секундная стрелка</summary>
        public ClockHand Seconds { get; set; }
        public ImageW SecCenterImage { get; set; }
    }

    public class Weather
    {
        /// <summary>Иконка погоды</summary>
        public IconW Icon { get; set; }
        /// <summary>Температура</summary>
        public Temperature Temperature { get; set; }
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

    public class Preview
    {
        public long X { get; set; }
        public long Y { get; set; }
        /// <summary>Номер изображения</summary>
        public long ImageIndex { get; set; }
    }

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
    }

    public class Coordinates
    {
        /// <summary>X координата</summary>
        public long X { get; set; }
        /// <summary>Y координата</summary>
        public long Y { get; set; }
    }

    public class Sector
    {
        /// <summary>Начальный угол</summary>
        public long StartAngle { get; set; }
        /// <summary>Конечный угол</summary>
        public long EndAngle { get; set; }
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


    public class IconSet
    {
        /// <summary>Номер первого изображения</summary>
        public long ImageIndex { get; set; }
        /// <summary>Координаты</summary>
        //public Coordinates Coordinates { get; set; }
        //public string Coordinates { get; set; }
        public Coordinates[] Coordinates { get; set; }
    }

    public class IconW
    {
        /// <summary>Номер первого изображения</summary>
        public ImageSet Images { get; set; }
        /// <summary>Номер изображения при отсутствии данных</summary>
        public long NoWeatherImageIndex { get; set; }
    }

    public class Temperature
    {
        /// <summary>Текущая температура</summary>
        public Number Current { get; set; }
        public Today Today { get; set; }
        /// <summary>Набор вспомогательных символов</summary>
        public Symbols Symbols { get; set; }
    }

    public class Today
    {
        public Separate Separate { get; set; }
        public bool AppendDegreesForBoth { get; set; }
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

    public class AmPm
    {
        public long X { get; set; }
        public long Y { get; set; }
        public long ImageIndexAMCN { get; set; }
        public long ImageIndexPMCN { get; set; }
        public long ImageIndexAMEN { get; set; }
        public long ImageIndexPMEN { get; set; }
    }

    public class TwoDigits
    {
        public ImageSet Tens { get; set; }
        public ImageSet Ones { get; set; }
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

    public class Year
    {
        public OneLineYear OneLine { get; set; }
    }

    public class OneLineYear
    {
        public Number Number { get; set; }
        public long? DelimiterImageIndex { get; set; }
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

    public class SeparateMonthAndDay
    {
        public Number Month { get; set; }
        public ImageSet MonthName { get; set; }
        public Number Day { get; set; }
    }

    public class OneLineMonthAndDay
    {
        public Number Number { get; set; }
        public long? DelimiterImageIndex { get; set; }
    }

    public class DateUnknown3
    {
        public UnknownType Unknown2 { get; set; }
    }

    public class SwitchW
    {
        public Coordinates Coordinates { get; set; }
        public long? ImageIndexOn { get; set; }
        public long? ImageIndexOff { get; set; }
    }

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
