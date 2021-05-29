using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Осу
{
    public partial class Form1 : Form
    {
        public Bitmap HandlerTexure = Resource1.Handler, //Определяем два поля с кругами
                      TargetTexure = Resource1.Target;
        private Point _targetPosition = new Point(300, 300); //Описывает позиция точки, в которую мы должны попадать
        private Point _direction = Point.Empty; //Направление
        private int _score = 0; //Поле в котором храниться текущее количество очков
        public Form1()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint, true); //Артибуты, позволяют избавиться от мерцания

            UpdateStyles(); //Чтобы все стили применились
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh(); //Перерисовывает всю форму и вызывает Form1_Paint
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            timer2.Interval = r.Next(25, 1000); //Случайный интервал
            _direction.X = r.Next(-1, 2); //Каждый тик этого таймера _direction.X изменяет на случайное число
            _direction.Y = r.Next(-1, 2); //Каждый тик этого таймера _direction.Y изменяет на случайное число
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics; //Графический контекст

            var localPosition = this.PointToClient(Cursor.Position); //Точка, которая бегает за курсором

            _targetPosition.X += _direction.X * 10; //Смещение направление точки по оси X со скоростью умноженой на 10
            _targetPosition.Y += _direction.Y * 10; //Смещение направление точки по оси Y со скоростью умноженой на 10

            if (_targetPosition.X < 0 || _targetPosition.X > 500) //Если выходит за границы по оси X, то
            {
                _direction.X *= -1; //Домножает направление по X (разворачиваем)
            }
            if (_targetPosition.Y < 0 || _targetPosition.Y > 500) //Если выходит за границы по оси Y, то
            {
                _direction.Y *= -1; //Домножает направление по Y (разворачиваем)
            }
            
            //Воспользуемся теоремой Пифагора. Из одной позиции вычесть другую, соотвественно найти гипотенузу - Это будет расстояние между точками
            Point between = new Point(localPosition.X - _targetPosition.X, localPosition.Y - _targetPosition.Y);
            float distance = (float)Math.Sqrt((between.X * between.X) + (between.Y * between.Y)); //Сумма квадратов катетов

            if (distance < 20) //Если дистанция меньше 20 пикселей, то очки начисляются

            {
                AddScore(1);
            }    

            var handlerRect = new Rectangle(localPosition.X - 50, localPosition.Y - 50, 100, 100);
            var targetRect = new Rectangle(_targetPosition.X - 50, _targetPosition.Y - 50, 100, 100);

            g.DrawImage(HandlerTexure, handlerRect); //Метод, который рисует картинку
            g.DrawImage(TargetTexure, targetRect); //Метод, который рисует картинку

        }

        private void AddScore(int score) //Метод, который добавляет очки

        {
            _score += score; //К полю прибавлем значение параметра score
            scoreLabel.Text = _score.ToString(); //В поле Text можно дать текст, который будет выводиться
        }
    }
}
