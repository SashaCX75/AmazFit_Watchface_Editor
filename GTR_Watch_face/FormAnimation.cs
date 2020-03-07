using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTR_Watch_face
{
    public partial class FormAnimation : Form
    {
        ClassStaticAnimation StaticAnimation;
        //Bitmap PreviewBackground;
        private Bitmap SrcImg;

        public FormAnimation(Bitmap previewBackground, ClassStaticAnimation staticAnimation)
        {
            InitializeComponent();
            //PreviewBackground = previewBackground;
            pictureBox_AnimatiomPreview.BackgroundImage = previewBackground;
            //pictureBox_AnimatiomPreview.Image = previewBackground;
            StaticAnimation = staticAnimation;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Graphics gPanel = pictureBox_AnimatiomPreview.CreateGraphics();
            SrcImg = new Bitmap(pictureBox_AnimatiomPreview.Width, pictureBox_AnimatiomPreview.Height);
            Graphics gPanel = Graphics.FromImage(SrcImg);
            //gPanel.Clear(pictureBox_AnimatiomPreview.BackColor);
            //pictureBox_AnimatiomPreview.Image = PreviewBackground;
            //float scalePreview = 1.0f;


            //Form1 f1 = this.Owner as Form1;//Получаем ссылку на первую форму

            //f1.PreviewToBitmap(gPanel, scalePreview, false, false, false, false, false, false, false, true);

            //var rand = new Random();
            //Pen p = new Pen(Color.Blue, 5);// цвет линии и ширина
            //Point p1 = new Point(rand.Next(100), rand.Next(100));// первая точка
            //Point p2 = new Point(rand.Next(100), rand.Next(100));// вторая точка
            //gPanel.DrawLine(p, p1, p2);// рисуем линию
            
            pictureBox_AnimatiomPreview.Image = SrcImg;
            StaticAnimation.DrawStaticAnimation(gPanel, 100);

            gPanel.Dispose();// освобождаем все ресурсы, связанные с отрисовкой
        }
    }
}
