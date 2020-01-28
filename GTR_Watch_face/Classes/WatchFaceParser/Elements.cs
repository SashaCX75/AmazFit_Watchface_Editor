using Newtonsoft.Json;

namespace GTR_Watch_face
{
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
        /// <summary>Шкала калорий</summary>
        public CircleScale StepsGoal { get; set; }
        /// <summary>Калории</summary>
        public Number Calories { get; set; }
        /// <summary>Пульс</summary>
        public Number Pulse { get; set; }
        /// <summary>Растояние</summary>
        public Distance Distance { get; set; }
        /// <summary>Шаги</summary>
        public FormattedNumber Steps { get; set; }
        /// <summary>Достижение цели</summary>
        public ImageW StarImage { get; set; }
        /// <summary>Пульс круговая шкала</summary>
        public CircleScale PulseMeter { get; set; }
        /// <summary>Индикатор пульса</summary>
        public IconSet ColouredSquares { get; set; }
        public long? NoDataImageIndex { get; set; }
        /// <summary>Прогрес калорий</summary>
        public CaloriesContainer CaloriesGraph { get; set; }
        /// <summary>Прогрес пульса</summary>
        public PulseContainer PulseGraph { get; set; }

        // For compatibility with "Goal2" JSON attribute
        [JsonProperty("Goal2")]
        private CircleScale Goal2
        {
            set { PulseMeter = value; }
        }
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
        /// <summary>день недели</summary>
        public IconSet WeekDayProgress { get; set; }
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

    public class Device_Id
    {
        /// <summary> код часов</summary>
        public long DeviceId { get; set; }
    }

    public class Shortcuts
    {
        /// <summary>Шаги</summary>
        public Shortcut State { get; set; }
        /// <summary>Пульс</summary>
        public Shortcut Pulse { get; set; }
        /// <summary>Погода</summary>
        public Shortcut Weather { get; set; }
        /// <summary>Энергосбережение</summary>
        public Shortcut Unknown4 { get; set; }
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

}
