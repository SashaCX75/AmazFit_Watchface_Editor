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
                    panel_Preview.Size = new Size(227, 227);
                    this.Size = new Size(227 + 22, 227 + 66); 
                }
                else
                {
                    panel_Preview.Size = new Size(195, 195);
                    this.Size = new Size(195 + 22, 195 + 66);
                }
            }

            if (radioButton_normal.Checked)
            {
                if (Model_47.model_47)
                {
                    panel_Preview.Size = new Size(454, 454);
                    this.Size = new Size(454 + 22, 454 + 66);
                }
                else
                {
                    panel_Preview.Size = new Size(390, 390);
                    this.Size = new Size(390 + 22, 390 + 66);
                }
            }

            if (radioButton_large.Checked)
            {
                if (Model_47.model_47)
                {
                    panel_Preview.Size = new Size(681, 681);
                    this.Size = new Size(681 + 22, 681 + 66);
                }
                else
                {
                    panel_Preview.Size = new Size(585, 585);
                    this.Size = new Size(585 + 22, 585 + 66);
                }
            }

            if (radioButton_xlarge.Checked)
            {
                if (Model_47.model_47)
                {
                    panel_Preview.Size = new Size(908, 908);
                    this.Size = new Size(908 + 22, 908 + 66);
                }
                else
                {
                    panel_Preview.Size = new Size(780, 780);
                    this.Size = new Size(780 + 22, 780 + 66);
                }
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
                    this.Size = new Size(975 + 22, 975 + 66);
                }
            }
        }

        public class Model_47
        {
            public static bool model_47 { get; set; }
        }
    }
    
}
