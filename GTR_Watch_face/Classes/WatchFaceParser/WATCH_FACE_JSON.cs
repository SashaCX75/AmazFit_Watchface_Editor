namespace GTR_Watch_face
{

    /// <summary>Корневая структура JSON файла</summary>
    public class WATCH_FACE_JSON
    {
        /// <summary>Тип часов</summary>
        public Device_Id Info { get; set; }
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
        /// <summary>Анимация</summary>
        public Animation Unknown11 { get; set; }
        /// <summary>Ярлыки</summary>
        public Shortcuts Shortcuts { get; set; }

    }

}
