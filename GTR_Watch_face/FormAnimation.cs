using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        float scalePreview = 1.0f;

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
            gPanel.ScaleTransform(scalePreview, scalePreview, MatrixOrder.Prepend);


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

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_normal.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(456, 456);
                    this.Size = new Size(456 + 20, 456 + 70);
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(392, 392);
                    this.Size = new Size(392 + 20, 392 + 70);
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(350, 444);
                    this.Size = new Size(350 + 20, 444 + 70);
                }
                else if (Model_Wath.model_TRex)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(362, 362);
                    this.Size = new Size(362 + 20, 362 + 70);
                }
                scalePreview = 1f;
            }

            if (radioButton_large.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(683, 683);
                    this.Size = new Size(683 + 20, 683 + 70);
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(586, 586);
                    this.Size = new Size(586 + 20, 586 + 70);
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(524, 665);
                    this.Size = new Size(524 + 20, 665 + 70);
                }
                else if (Model_Wath.model_TRex)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(541, 541);
                    this.Size = new Size(542 + 20, 542 + 70);
                }
                scalePreview = 1.5f;
            }

            if (radioButton_xlarge.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(909, 909);
                    this.Size = new Size(909 + 20, 909 + 70);
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(781, 781);
                    this.Size = new Size(781 + 20, 781 + 70);
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(697, 885);
                    this.Size = new Size(697 + 20, 885 + 70);
                }
                else if (Model_Wath.model_TRex)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(721, 721);
                    this.Size = new Size(721 + 20, 721 + 70);
                }
                scalePreview = 2f;
            }
        }

        public class Model_Wath
        {
            public static bool model_gtr47 { get; set; }
            public static bool model_gtr42 { get; set; }
            public static bool model_gts { get; set; }
            public static bool model_TRex { get; set; }

        }
    }
}
