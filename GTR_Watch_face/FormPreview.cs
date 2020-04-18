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
                    pictureBox_Preview.Size = new Size(230, 230);
                    this.Size = new Size(230 + (int)(22 * currentDPI), 230 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_Preview.Size = new Size(198, 198);
                    this.Size = new Size(198 + (int)(22 * currentDPI), 198 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_Preview.Size = new Size(177, 224);
                    this.Size = new Size(177 + (int)(22 * currentDPI), 224 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_Preview.Size = new Size(183, 183);
                    this.Size = new Size(183 + (int)(22 * currentDPI), 183 + (int)(66 * currentDPI));
                }
                scale = 0.5f;
            }

            if (radioButton_normal.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_Preview.Size = new Size(457, 457);
                    this.Size = new Size(457 + (int)(22 * currentDPI), 457 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_Preview.Size = new Size(393, 393);
                    this.Size = new Size(393 + (int)(22 * currentDPI), 393 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_Preview.Size = new Size(351, 445);
                    this.Size = new Size(351 + (int)(22 * currentDPI), 445 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_Preview.Size = new Size(363, 363);
                    this.Size = new Size(363 + (int)(22 * currentDPI), 363 + (int)(66 * currentDPI));
                }
                scale = 1f;
            }

            if (radioButton_large.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_Preview.Size = new Size(684, 684);
                    this.Size = new Size(684 + (int)(22 * currentDPI), 684 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_Preview.Size = new Size(588, 588);
                    this.Size = new Size(588 + (int)(22 * currentDPI), 588 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_Preview.Size = new Size(525, 666);
                    this.Size = new Size(525 + (int)(22 * currentDPI), 666 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_Preview.Size = new Size(543, 543);
                    this.Size = new Size(543 + (int)(22 * currentDPI), 543 + (int)(66 * currentDPI));
                }
                scale = 1.5f;
            }

            if (radioButton_xlarge.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_Preview.Size = new Size(911, 911);
                    this.Size = new Size(911 + (int)(22 * currentDPI), 911 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_Preview.Size = new Size(783, 783);
                    this.Size = new Size(783 + (int)(22 * currentDPI), 783 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_Preview.Size = new Size(699, 887);
                    this.Size = new Size(699 + (int)(22 * currentDPI), 887 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_Preview.Size = new Size(723, 723);
                    this.Size = new Size(723 + (int)(22 * currentDPI), 723 + (int)(66 * currentDPI));
                }
                scale = 2f;
            }

            if (radioButton_xxlarge.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_Preview.Size = new Size(1138, 1138);
                    this.Size = new Size(1138 + (int)(22 * currentDPI), 1138 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_Preview.Size = new Size(978, 978);
                    this.Size = new Size(978 + (int)(22 * currentDPI), 978 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_Preview.Size = new Size(873, 1108);
                    this.Size = new Size(873 + (int)(22 * currentDPI), 1108 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_Preview.Size = new Size(903, 903);
                    this.Size = new Size(903 + (int)(22 * currentDPI), 903 + (int)(66 * currentDPI));
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
