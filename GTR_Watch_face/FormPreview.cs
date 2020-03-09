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
                if (Model_Wath.model_gtr47)
                {
                    panel_Preview.Size = new Size(230, 230);
                    this.Size = new Size(230 + 22, 230 + 66); 
                }
                else if (Model_Wath.model_gtr42)
                {
                    panel_Preview.Size = new Size(198, 198);
                    this.Size = new Size(198 + 22, 198 + 66);
                }
                else if (Model_Wath.model_gts)
                {
                    panel_Preview.Size = new Size(176, 223);
                    this.Size = new Size(176 + 22, 223 + 66);
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    panel_Preview.Size = new Size(183, 183);
                    this.Size = new Size(183 + 22, 183 + 66);
                }
                scale = 0.5f;
            }

            if (radioButton_normal.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    panel_Preview.Size = new Size(456, 456);
                    this.Size = new Size(456 + 22, 456 + 66);
                }
                else if (Model_Wath.model_gtr42)
                {
                    panel_Preview.Size = new Size(392, 392);
                    this.Size = new Size(392 + 22, 392 + 66);
                }
                else if (Model_Wath.model_gts)
                {
                    panel_Preview.Size = new Size(350, 444);
                    this.Size = new Size(350 + 22, 444 + 66);
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    panel_Preview.Size = new Size(362, 362);
                    this.Size = new Size(362 + 22, 362 + 66);
                }
                scale = 1f;
            }

            if (radioButton_large.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    panel_Preview.Size = new Size(683, 683);
                    this.Size = new Size(683 + 22, 683 + 66);
                }
                else if (Model_Wath.model_gtr42)
                {
                    panel_Preview.Size = new Size(586, 586);
                    this.Size = new Size(586 + 22, 586 + 66);
                }
                else if (Model_Wath.model_gts)
                {
                    panel_Preview.Size = new Size(524, 665);
                    this.Size = new Size(524 + 22, 665 + 66);
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    panel_Preview.Size = new Size(541, 541);
                    this.Size = new Size(542 + 22, 542 + 66);
                }
                scale = 1.5f;
            }

            if (radioButton_xlarge.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    panel_Preview.Size = new Size(909, 909);
                    this.Size = new Size(909 + 22, 909 + 66);
                }
                else if (Model_Wath.model_gtr42)
                {
                    panel_Preview.Size = new Size(781, 781);
                    this.Size = new Size(781 + 22, 781 + 66);
                }
                else if (Model_Wath.model_gts)
                {
                    panel_Preview.Size = new Size(697, 885);
                    this.Size = new Size(697 + 22, 885 + 66);
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    panel_Preview.Size = new Size(721, 721);
                    this.Size = new Size(721 + 22, 721 + 66);
                }
                scale = 2f;
            }

            if (radioButton_xxlarge.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    panel_Preview.Size = new Size(1135, 1135);
                    this.Size = new Size(1135 + 22, 1135 + 66);
                }
                else if (Model_Wath.model_gtr42)
                {
                    panel_Preview.Size = new Size(975, 975);
                    this.Size = new Size(975 + 22, 975 + 66);
                }
                else if (Model_Wath.model_gts)
                {
                    panel_Preview.Size = new Size(1135, 1105);
                    this.Size = new Size(870 + 22, 1105 + 66);
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    panel_Preview.Size = new Size(900, 900);
                    this.Size = new Size(900 + 22, 900 + 66);
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

        private void panel_Preview_MouseMove(object sender, MouseEventArgs e)
        {
            int CursorX = (int)Math.Round(e.X / scale, 0);
            int CursorY = (int)Math.Round(e.Y / scale, 0);

            this.Text = Properties.FormStrings.Form_PreviewX + CursorX.ToString() +
                ";  Y=" + CursorY.ToString() + "]";
        }

        private void panel_Preview_MouseLeave(object sender, EventArgs e)
        {
            this.Text = Properties.FormStrings.Form_Preview;
        }
        private void panel_Preview_MouseDoubleClick(object sender, MouseEventArgs e)
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
