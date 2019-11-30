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
    public partial class Form_Preview : Form
    {
        float scale = 1;
        public Form_Preview()
        {
            InitializeComponent();
        }

        public void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            //Form1 f1 = this.Owner as Form1;//Получаем ссылку на первую форму
            //f1.button1.PerformClick();
            if (radioButton_small.Checked)
            {
                if (Model_47.model_47)
                {
                    panel_Preview.Size = new Size(230, 230);
                    this.Size = new Size(230 + 22, 230 + 66); 
                }
                else
                {
                    panel_Preview.Size = new Size(198, 198);
                    this.Size = new Size(198 + 22, 198 + 66);
                }
                scale = 0.5f;
            }

            if (radioButton_normal.Checked)
            {
                if (Model_47.model_47)
                {
                    panel_Preview.Size = new Size(456, 456);
                    this.Size = new Size(456 + 22, 456 + 66);
                }
                else
                {
                    panel_Preview.Size = new Size(392, 392);
                    this.Size = new Size(392 + 22, 392 + 66);
                }
                scale = 1f;
            }

            if (radioButton_large.Checked)
            {
                if (Model_47.model_47)
                {
                    panel_Preview.Size = new Size(683, 683);
                    this.Size = new Size(683 + 22, 683 + 66);
                }
                else
                {
                    panel_Preview.Size = new Size(586, 586);
                    this.Size = new Size(586 + 22, 586 + 66);
                }
                scale = 1.5f;
            }

            if (radioButton_xlarge.Checked)
            {
                if (Model_47.model_47)
                {
                    panel_Preview.Size = new Size(909, 909);
                    this.Size = new Size(909 + 22, 909 + 66);
                }
                else
                {
                    panel_Preview.Size = new Size(781, 781);
                    this.Size = new Size(781 + 22, 781 + 66);
                }
                scale = 2f;
            }

            if (radioButton_xxlarge.Checked)
            {
                if (Model_47.model_47)
                {
                    panel_Preview.Size = new Size(1135, 1135);
                    this.Size = new Size(1135 + 22, 1135 + 66);
                }
                else
                {
                    panel_Preview.Size = new Size(975, 975);
                    this.Size = new Size(975 + 22, 97 + 66);
                }
                scale = 2.5f;
            }
        }

        public class Model_47
        {
            public static bool model_47 { get; set; }
        }

        private void panel_Preview_MouseMove(object sender, MouseEventArgs e)
        {
            int CursorX = (int)Math.Round(e.X / scale, 0);
            int CursorY = (int)Math.Round(e.Y / scale, 0);

            this.Text = "Предпросмотр [X=" + CursorX.ToString() +
                ";  Y=" + CursorY.ToString() + "]";
        }

        private void panel_Preview_MouseLeave(object sender, EventArgs e)
        {
            this.Text = "Предпросмотр";
        }
        private void panel_Preview_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MouseСoordinates.X = (int)Math.Round(e.X / scale, 0);
            MouseСoordinates.Y = (int)Math.Round(e.Y / scale, 0);
            toolTip1.Show("Координаты скопированны", this, e.X-5, e.Y-7, 2000);
        }
    }
    
}
