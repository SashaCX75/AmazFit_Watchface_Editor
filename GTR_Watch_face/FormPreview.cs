using Microsoft.Win32;
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
        float currentDPI; // масштаб экрана

        public Form_Preview()
        {
            InitializeComponent();
            currentDPI = (int)Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "LogPixels", 96) / 96f;
        }

        public void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            //Form1 f1 = this.Owner as Form1;//Получаем ссылку на первую форму
            //f1.button1.PerformClick();
            if (radioButton_small.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_Preview.Size = new Size(229, 229);
                    this.Size = new Size(229 + (int)(22 * currentDPI), 229 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_Preview.Size = new Size(197, 197);
                    this.Size = new Size(197 + (int)(22 * currentDPI), 197 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_Preview.Size = new Size(176, 223);
                    this.Size = new Size(176 + (int)(22 * currentDPI), 223 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_Preview.Size = new Size(182, 182);
                    this.Size = new Size(182 + (int)(22 * currentDPI), 182 + (int)(66 * currentDPI));
                }
                scale = 0.5f;
            }

            if (radioButton_normal.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_Preview.Size = new Size(456, 456);
                    this.Size = new Size(456 + (int)(22 * currentDPI), 456 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_Preview.Size = new Size(392, 392);
                    this.Size = new Size(392 + (int)(22 * currentDPI), 392 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_Preview.Size = new Size(350, 444);
                    this.Size = new Size(350 + (int)(22 * currentDPI), 444 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_Preview.Size = new Size(362, 362);
                    this.Size = new Size(362 + (int)(22 * currentDPI), 362 + (int)(66 * currentDPI));
                }
                scale = 1f;
            }

            if (radioButton_large.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_Preview.Size = new Size(683, 683);
                    this.Size = new Size(683 + (int)(22 * currentDPI), 683 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_Preview.Size = new Size(587, 587);
                    this.Size = new Size(587 + (int)(22 * currentDPI), 587 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_Preview.Size = new Size(524, 665);
                    this.Size = new Size(524 + (int)(22 * currentDPI), 665 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_Preview.Size = new Size(542, 542);
                    this.Size = new Size(542 + (int)(22 * currentDPI), 542 + (int)(66 * currentDPI));
                }
                scale = 1.5f;
            }

            if (radioButton_xlarge.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_Preview.Size = new Size(910, 910);
                    this.Size = new Size(910 + (int)(22 * currentDPI), 910 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_Preview.Size = new Size(782, 782);
                    this.Size = new Size(782 + (int)(22 * currentDPI), 782 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_Preview.Size = new Size(698, 886);
                    this.Size = new Size(698 + (int)(22 * currentDPI), 886 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_Preview.Size = new Size(722, 722);
                    this.Size = new Size(722 + (int)(22 * currentDPI), 722 + (int)(66 * currentDPI));
                }
                scale = 2f;
            }

            if (radioButton_xxlarge.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_Preview.Size = new Size(1137, 1137);
                    this.Size = new Size(1137 + (int)(22 * currentDPI), 1137 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_Preview.Size = new Size(977, 977);
                    this.Size = new Size(977 + (int)(22 * currentDPI), 977 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_Preview.Size = new Size(872, 1107);
                    this.Size = new Size(872 + (int)(22 * currentDPI), 1107 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_Preview.Size = new Size(902, 902);
                    this.Size = new Size(902 + (int)(22 * currentDPI), 902 + (int)(66 * currentDPI));
                }
                scale = 2.5f;
            }
        }

        public class Model_Wath
        {
            public static bool model_gtr47 { get; set; }
            public static bool model_gtr42 { get; set; }
            public static bool model_gts { get; set; }
            public static bool model_TRex { get; set; }
            public static bool model_Verge { get; set; }

        }

        private void pictureBox_Preview_MouseMove(object sender, MouseEventArgs e)
        {
            int CursorX = (int)Math.Round(e.X / scale, 0);
            int CursorY = (int)Math.Round(e.Y / scale, 0);

            this.Text = Properties.FormStrings.Form_PreviewX + CursorX.ToString() +
                ";  Y=" + CursorY.ToString() + "]";
        }

        private void pictureBox_Preview_MouseLeave(object sender, EventArgs e)
        {
            this.Text = Properties.FormStrings.Form_Preview;
        }
        private void pictureBox_Preview_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MouseСoordinates.X = (int)Math.Round(e.X / scale, 0);
            MouseСoordinates.Y = (int)Math.Round(e.Y / scale, 0);
            toolTip1.Show(Properties.FormStrings.Message_CopyCoord, this, e.X-5, e.Y-7, 2000);
        }

        private void Form_Preview_Load(object sender, EventArgs e)
        {
            this.Text = Properties.FormStrings.Form_Preview;
        }
    }
    
}
