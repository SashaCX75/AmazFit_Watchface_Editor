using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTR_Watch_face
{
    public class ClassMotiomAnimation
    {
        Bitmap _image; // Картинка для анимации
        int _startX; // Стартовая координата X
        int _startY; // Стартовая координата Y
        int _endX; // Конечноя координата X
        int _endY; // Конечноя координата Y
        int _speedAnimation; // Скорость анимации
        int _timeAnimation; // Время анимации
        bool _bounce; // Время анимации
        int _cyclesTime; // длительность цикла
        int _time; // прошедшее время

        /// <summary>Класс для показа анимации с двидением по координатам</summary>
        /// <param name="Image">Картинка для анимации</param>
        /// <param name="StartX">Стартовая координата X</param>
        /// <param name="StartY">Стартовая координата Y</param>
        /// <param name="EndX">Конечноя координата X</param>
        /// <param name="EndY">Конечноя координата Y</param>
        /// <param name="SpeedAnimation">Скорость анимации</param>
        /// <param name="TimeAnimation">Время анимации</param>
        /// <param name="Bounce">Время анимации</param>
        public ClassMotiomAnimation(Bitmap Image, int StartX, int StartY, int EndX, int EndY, int SpeedAnimation,
            int TimeAnimation, bool Bounce)
        {
            _image = Image;
            _startX = StartX;
            _startY = StartY;
            _endX = EndX;
            _endY = EndY;
            _speedAnimation = SpeedAnimation;
            _timeAnimation = TimeAnimation;
            _bounce = Bounce;
            _time = 0;
            _cyclesTime = SpeedAnimation;
            while (_cyclesTime < TimeAnimation)
            {
                _cyclesTime = _cyclesTime + _cyclesTime;
            }
        }

        /// <summary>Отрисовываем следующий цикл анимации</summary>
        /// <param name="g">Поверхность для рисования</param>
        /// <param name="deltaTime">Шаг приращения времени</param>
        public void DrawMotiomAnimation(Graphics g, int deltaTime)
        {
            int newX = _startX;
            int newY = _startY;

            int _newtime = _time;
            if (_newtime > _cyclesTime && _timeAnimation > 0) _newtime = _cyclesTime;
            while (_newtime > _speedAnimation * 2)
            {
                _newtime = _newtime - _speedAnimation * 2;
            }

            if (_newtime > _speedAnimation)
            {
                _newtime = _newtime - _speedAnimation;
                int dX = _startX - _endX;
                int dY = _startY - _endY;

                double proportions = (double)_newtime / _speedAnimation;
                newX = (int)(_endX + dX * proportions);
                newY = (int)(_endY + dY * proportions);
            }
            else
            {
                int dX = _endX - _startX;
                int dY = _endY - _startY;

                double proportions = (double)_newtime / _speedAnimation;
                newX = (int)(_startX + dX * proportions);
                newY = (int)(_startY + dY * proportions);
            }
            if (_image != null)
            {
                g.DrawImage(_image, new Rectangle(newX, newY, _image.Width, _image.Height));
            }
            _time = _time + deltaTime;
        }

        /// <summary>Сброс анимации к начальному значению</summary>
        public void ResetDrawMotiomAnimation()
        {
            _time = 0;
        }
    }
}
