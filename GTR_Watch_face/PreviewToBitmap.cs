using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace GTR_Watch_face
{
    public partial class Form1 : Form
    {

        /// <summary>формируем изображение на панедли Graphics</summary>
        /// <param name="gPanel">Поверхность для рисования</param>
        /// <param name="scale">Масштаб прорисовки</param>
        /// <param name="crop">Обрезать по форме экрана</param>
        /// <param name="WMesh">Рисовать белую сетку</param>
        /// <param name="BMesh">Рисовать черную сетку</param>
        /// <param name="BBorder">Рисовать рамку по координатам, вокруг элементов с выравниванием</param>
        private void PreviewToBitmap(Graphics gPanel, float scale, bool crop, bool WMesh, bool BMesh, bool BBorder)
        {
            var src = new Bitmap(1, 1);
            gPanel.ScaleTransform(scale, scale, MatrixOrder.Prepend);
            int i;
            //gPanel.SmoothingMode = SmoothingMode.AntiAlias;

            #region Background
            if (comboBox_Background.SelectedIndex >= 0)
            {
                i = comboBox_Background.SelectedIndex;
                src = new Bitmap(ListImagesFullName[i]);
                gPanel.DrawImage(src, new Rectangle(0, 0, src.Width, src.Height));
                src.Dispose();
            }
            #endregion
            if (scale == 0.5) gPanel.SmoothingMode = SmoothingMode.AntiAlias;

            #region Time
            if (checkBox_Time.Checked)
            {
                if (checkBox_AmPm.Checked)
                {
                    if (checkBox_Hours.Checked)
                    {
                        if (comboBox_Hours_Tens_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Hours_Tens_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.TimePm.Hours.Tens;
                            if (i < ListImagesFullName.Count)
                            {
                                src = new Bitmap(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Hours_Tens_X.Value,
                                    (int)numericUpDown_Hours_Tens_Y.Value, src.Width, src.Height));
                                src.Dispose();
                            }
                        }
                        if (comboBox_Hours_Ones_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Hours_Ones_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.TimePm.Hours.Ones;
                            if (i < ListImagesFullName.Count)
                            {
                                src = new Bitmap(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Hours_Ones_X.Value,
                                    (int)numericUpDown_Hours_Ones_Y.Value, src.Width, src.Height));
                                src.Dispose();
                            }
                        }
                    }

                    if (checkBox_Minutes.Checked)
                    {
                        if (comboBox_Min_Tens_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Min_Tens_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.TimePm.Minutes.Tens;
                            if (i < ListImagesFullName.Count)
                            {
                                src = new Bitmap(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Min_Tens_X.Value,
                                    (int)numericUpDown_Min_Tens_Y.Value, src.Width, src.Height));
                                src.Dispose();
                            }
                        }
                        if (comboBox_Min_Ones_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Min_Ones_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.TimePm.Minutes.Ones;
                            if (i < ListImagesFullName.Count)
                            {
                                src = new Bitmap(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Min_Ones_X.Value,
                                    (int)numericUpDown_Min_Ones_Y.Value, src.Width, src.Height));
                                src.Dispose();
                            }
                        }
                    }

                    if (checkBox_Seconds.Checked)
                    {
                        if (comboBox_Sec_Tens_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Sec_Tens_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.TimePm.Seconds.Tens;
                            if (i < ListImagesFullName.Count)
                            {
                                src = new Bitmap(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Sec_Tens_X.Value,
                                    (int)numericUpDown_Sec_Tens_Y.Value, src.Width, src.Height));
                                src.Dispose();
                            }
                        }
                        if (comboBox_Sec_Ones_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Sec_Ones_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.TimePm.Seconds.Ones;
                            if (i < ListImagesFullName.Count)
                            {
                                src = new Bitmap(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Sec_Ones_X.Value,
                                    (int)numericUpDown_Sec_Ones_Y.Value, src.Width, src.Height));
                                src.Dispose();
                            }
                        }
                    }

                    if (Watch_Face_Preview_TwoDigits.TimePm.Pm)
                    {
                        if (comboBox_Image_Pm.SelectedIndex >= 0)
                        {
                            i = comboBox_Image_Pm.SelectedIndex;
                            src = new Bitmap(ListImagesFullName[i]);
                            gPanel.DrawImage(src, new Rectangle((int)numericUpDown_AmPm_X.Value,
                                (int)numericUpDown_AmPm_Y.Value, src.Width, src.Height));
                            src.Dispose();
                        }
                    }
                    else
                    {
                        if (comboBox_Image_Am.SelectedIndex >= 0)
                        {
                            i = comboBox_Image_Am.SelectedIndex;
                            src = new Bitmap(ListImagesFullName[i]);
                            gPanel.DrawImage(src, new Rectangle((int)numericUpDown_AmPm_X.Value,
                                (int)numericUpDown_AmPm_Y.Value, src.Width, src.Height));
                            src.Dispose();
                        }
                    }

                    if (checkBox_Delimiter.Checked)
                    {
                        if (comboBox_Delimiter_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Delimiter_Image.SelectedIndex;
                            src = new Bitmap(ListImagesFullName[i]);
                            gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Delimiter_X.Value,
                                (int)numericUpDown_Delimiter_Y.Value, src.Width, src.Height));
                            src.Dispose();
                        }
                    }
                }
                else
                {
                    if (checkBox_Hours.Checked)
                    {
                        if (comboBox_Hours_Tens_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Hours_Tens_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Time.Hours.Tens;
                            if (i < ListImagesFullName.Count)
                            {
                                src = new Bitmap(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Hours_Tens_X.Value,
                                    (int)numericUpDown_Hours_Tens_Y.Value, src.Width, src.Height));
                                src.Dispose();
                            }
                        }
                        if (comboBox_Hours_Ones_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Hours_Ones_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Time.Hours.Ones;
                            if (i < ListImagesFullName.Count)
                            {
                                src = new Bitmap(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Hours_Ones_X.Value,
                                    (int)numericUpDown_Hours_Ones_Y.Value, src.Width, src.Height));
                                src.Dispose();
                            }
                        }
                    }

                    if (checkBox_Minutes.Checked)
                    {
                        if (comboBox_Min_Tens_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Min_Tens_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Time.Minutes.Tens;
                            if (i < ListImagesFullName.Count)
                            {
                                src = new Bitmap(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Min_Tens_X.Value,
                                    (int)numericUpDown_Min_Tens_Y.Value, src.Width, src.Height));
                                src.Dispose();
                            }
                        }
                        if (comboBox_Min_Ones_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Min_Ones_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Time.Minutes.Ones;
                            if (i < ListImagesFullName.Count)
                            {
                                src = new Bitmap(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Min_Ones_X.Value,
                                    (int)numericUpDown_Min_Ones_Y.Value, src.Width, src.Height));
                                src.Dispose();
                            }
                        }
                    }

                    if (checkBox_Seconds.Checked)
                    {
                        if (comboBox_Sec_Tens_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Sec_Tens_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Time.Seconds.Tens;
                            if (i < ListImagesFullName.Count)
                            {
                                src = new Bitmap(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Sec_Tens_X.Value,
                                    (int)numericUpDown_Sec_Tens_Y.Value, src.Width, src.Height));
                                src.Dispose();
                            }
                        }
                        if (comboBox_Sec_Ones_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Sec_Ones_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Time.Seconds.Ones;
                            if (i < ListImagesFullName.Count)
                            {
                                src = new Bitmap(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Sec_Ones_X.Value,
                                    (int)numericUpDown_Sec_Ones_Y.Value, src.Width, src.Height));
                                src.Dispose();
                            }
                        }
                    }

                    if (checkBox_Delimiter.Checked)
                    {
                        if (comboBox_Delimiter_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Delimiter_Image.SelectedIndex;
                            src = new Bitmap(ListImagesFullName[i]);
                            gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Delimiter_X.Value,
                                (int)numericUpDown_Delimiter_Y.Value, src.Width, src.Height));
                            src.Dispose();
                        }
                    }
                }
            }
            #endregion

            #region Date
            if (checkBox_Date.Checked)
            {
                if ((checkBox_MonthAndDayM.Checked) && (comboBox_MonthAndDayM_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_MonthAndDayM_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_MonthAndDayM_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_MonthAndDayM_EndCorner_X.Value + 1;
                    int y2 = (int)numericUpDown_MonthAndDayM_EndCorner_Y.Value + 1;
                    var Dagit = new Bitmap(ListImagesFullName[comboBox_MonthAndDayM_Image.SelectedIndex]);
                    int DateLenght = Dagit.Width;
                    int DateHeight = Dagit.Height;
                    if ((checkBox_TwoDigitsMonth.Checked) || (Watch_Face_Preview_TwoDigits.Date.Month.Tens > 0))
                        DateLenght = DateLenght + Dagit.Width + (int)numericUpDown_MonthAndDayM_Spacing.Value;
                    if (DateLenght < Dagit.Width) DateLenght = Dagit.Width;

                    int PointX = 0;
                    int PointY = 0;
                    switch (comboBox_MonthAndDayM_Alignment.SelectedIndex)
                    {
                        case 0:
                        case 1:
                        case 2:
                            PointY = y1;
                            break;
                        case 3:
                        case 4:
                        case 5:
                            PointY = (y1 + y2) / 2 - DateHeight / 2;
                            break;
                        case 6:
                        case 7:
                        case 8:
                            PointY = y2 - DateHeight;
                            break;
                    }
                    switch (comboBox_MonthAndDayM_Alignment.SelectedIndex)
                    {
                        case 0:
                        case 3:
                        case 6:
                            PointX = x1;
                            break;
                        case 1:
                        case 4:
                        case 7:
                            PointX = (x1 + x2) / 2 - DateLenght / 2;
                            break;
                        case 2:
                        case 5:
                        case 8:
                            PointX = x2 - DateLenght;
                            break;
                    }
                    if (PointX < x1) PointX = x1;
                    if (PointY < y1) PointY = y1;

                    if ((checkBox_TwoDigitsMonth.Checked) || (Watch_Face_Preview_TwoDigits.Date.Month.Tens > 0))
                    {
                        i = comboBox_MonthAndDayM_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Date.Month.Tens;
                        src = new Bitmap(ListImagesFullName[i]);
                        gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + Dagit.Width + (int)numericUpDown_MonthAndDayM_Spacing.Value;
                        src.Dispose();
                    }
                    i = comboBox_MonthAndDayM_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Date.Month.Ones;
                    src = new Bitmap(ListImagesFullName[i]);
                    gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                    src.Dispose();
                    Dagit.Dispose();

                    if (BBorder)
                    {
                        Rectangle rect = new Rectangle(x1, y1, x2 - x1 - 1, y2 - y1 - 1);
                        using (Pen pen1 = new Pen(Color.White, 1))
                        {
                            gPanel.DrawRectangle(pen1, rect);
                        }
                        using (Pen pen2 = new Pen(Color.Black, 1))
                        {
                            pen2.DashStyle = DashStyle.Dot;
                            gPanel.DrawRectangle(pen2, rect);
                        }
                    }
                }

                if ((checkBox_MonthAndDayD.Checked) && (comboBox_MonthAndDayD_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_MonthAndDayD_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_MonthAndDayD_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_MonthAndDayD_EndCorner_X.Value + 1;
                    int y2 = (int)numericUpDown_MonthAndDayD_EndCorner_Y.Value + 1;
                    var Dagit = new Bitmap(ListImagesFullName[comboBox_MonthAndDayD_Image.SelectedIndex]);
                    int DateLenght = Dagit.Width;
                    int DateHeight = Dagit.Height;
                    if ((checkBox_TwoDigitsDay.Checked) || (Watch_Face_Preview_TwoDigits.Date.Day.Tens > 0))
                        DateLenght = DateLenght + Dagit.Width + (int)numericUpDown_MonthAndDayD_Spacing.Value;
                    if (DateLenght < Dagit.Width) DateLenght = Dagit.Width;

                    int PointX = 0;
                    int PointY = 0;
                    switch (comboBox_MonthAndDayD_Alignment.SelectedIndex)
                    {
                        case 0:
                        case 1:
                        case 2:
                            PointY = y1;
                            break;
                        case 3:
                        case 4:
                        case 5:
                            PointY = (y1 + y2) / 2 - DateHeight / 2;
                            break;
                        case 6:
                        case 7:
                        case 8:
                            PointY = y2 - DateHeight;
                            break;
                    }
                    switch (comboBox_MonthAndDayD_Alignment.SelectedIndex)
                    {
                        case 0:
                        case 3:
                        case 6:
                            PointX = x1;
                            break;
                        case 1:
                        case 4:
                        case 7:
                            PointX = (x1 + x2) / 2 - DateLenght / 2;
                            break;
                        case 2:
                        case 5:
                        case 8:
                            PointX = x2 - DateLenght;
                            break;
                    }
                    if (PointX < x1) PointX = x1;
                    if (PointY < y1) PointY = y1;

                    if ((checkBox_TwoDigitsDay.Checked) || (Watch_Face_Preview_TwoDigits.Date.Day.Tens > 0))
                    {
                        i = comboBox_MonthAndDayD_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Date.Day.Tens;
                        src = new Bitmap(ListImagesFullName[i]);
                        gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + Dagit.Width + (int)numericUpDown_MonthAndDayD_Spacing.Value;
                        src.Dispose();
                    }
                    i = comboBox_MonthAndDayD_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Date.Day.Ones;
                    src = new Bitmap(ListImagesFullName[i]);
                    gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                    src.Dispose();
                    Dagit.Dispose();

                    if (BBorder)
                    {
                        Rectangle rect = new Rectangle(x1, y1, x2 - x1 - 1, y2 - y1 - 1);
                        using (Pen pen1 = new Pen(Color.White, 1))
                        {
                            gPanel.DrawRectangle(pen1, rect);
                        }
                        using (Pen pen2 = new Pen(Color.Black, 1))
                        {
                            pen2.DashStyle = DashStyle.Dot;
                            gPanel.DrawRectangle(pen2, rect);
                        }
                    }
                }

                if ((checkBox_MonthName.Checked) && (comboBox_MonthName_Image.SelectedIndex >= 0))
                {
                    i = comboBox_MonthName_Image.SelectedIndex + Watch_Face_Preview_Set.Date.Month - 1;
                    src = new Bitmap(ListImagesFullName[i]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_MonthName_X.Value,
                        (int)numericUpDown_MonthName_Y.Value, src.Width, src.Height));
                    src.Dispose();
                }

                if ((checkBox_OneLine.Checked) && (comboBox_OneLine_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_OneLine_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_OneLine_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_OneLine_EndCorner_X.Value + 1;
                    int y2 = (int)numericUpDown_OneLine_EndCorner_Y.Value + 1;
                    var Dagit = new Bitmap(ListImagesFullName[comboBox_OneLine_Image.SelectedIndex]);
                    var Delimit = new Bitmap(1, 1);
                    if (comboBox_OneLine_Delimiter.SelectedIndex >= 0)
                        Delimit = new Bitmap(ListImagesFullName[comboBox_OneLine_Delimiter.SelectedIndex]);
                    int DelimitW = Delimit.Width;
                    if (comboBox_OneLine_Delimiter.SelectedIndex < 0) DelimitW = 0;

                    int DateLenght = Dagit.Width * 4 + (int)numericUpDown_OneLine_Spacing.Value * 4 + DelimitW;
                    int DateHeight = Dagit.Height;
                    if (comboBox_OneLine_Delimiter.SelectedIndex < 0) DateLenght = DateLenght - DelimitW;
                    if ((!checkBox_TwoDigitsMonth.Checked) && (Watch_Face_Preview_TwoDigits.Date.Month.Tens == 0))
                        DateLenght = DateLenght - Dagit.Width - (int)numericUpDown_OneLine_Spacing.Value;
                    if ((!checkBox_TwoDigitsDay.Checked) && (Watch_Face_Preview_TwoDigits.Date.Day.Tens == 0))
                        DateLenght = DateLenght - Dagit.Width - (int)numericUpDown_OneLine_Spacing.Value;
                    if (DateLenght < Dagit.Width) DateLenght = Dagit.Width;

                    int PointX = 0;
                    int PointY = 0;
                    switch (comboBox_OneLine_Alignment.SelectedIndex)
                    {
                        case 0:
                        case 1:
                        case 2:
                            PointY = y1;
                            break;
                        case 3:
                        case 4:
                        case 5:
                            PointY = (y1 + y2) / 2 - DateHeight / 2;
                            break;
                        case 6:
                        case 7:
                        case 8:
                            PointY = y2 - DateHeight;
                            break;
                    }
                    switch (comboBox_OneLine_Alignment.SelectedIndex)
                    {
                        case 0:
                        case 3:
                        case 6:
                            PointX = x1;
                            break;
                        case 1:
                        case 4:
                        case 7:
                            PointX = (x1 + x2) / 2 - DateLenght / 2;
                            break;
                        case 2:
                        case 5:
                        case 8:
                            PointX = x2 - DateLenght;
                            break;
                    }
                    if (PointX < x1) PointX = x1;
                    if (PointY < y1) PointY = y1;

                    if ((checkBox_TwoDigitsMonth.Checked) || (Watch_Face_Preview_TwoDigits.Date.Month.Tens > 0))
                    {
                        i = comboBox_OneLine_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Date.Month.Tens;
                        src = new Bitmap(ListImagesFullName[i]);
                        gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + Dagit.Width + (int)numericUpDown_OneLine_Spacing.Value;
                        src.Dispose();
                    }
                    i = comboBox_OneLine_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Date.Month.Ones;
                    src = new Bitmap(ListImagesFullName[i]);
                    gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                    PointX = PointX + Dagit.Width + (int)numericUpDown_OneLine_Spacing.Value;
                    src.Dispose();

                    if (comboBox_OneLine_Delimiter.SelectedIndex >= 0)
                    {
                        i = comboBox_OneLine_Delimiter.SelectedIndex;
                        src = new Bitmap(ListImagesFullName[i]);
                        gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + DelimitW + (int)numericUpDown_OneLine_Spacing.Value;
                        src.Dispose();
                    }

                    if ((checkBox_TwoDigitsDay.Checked) || (Watch_Face_Preview_TwoDigits.Date.Day.Tens > 0))
                    {
                        i = comboBox_OneLine_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Date.Day.Tens;
                        src = new Bitmap(ListImagesFullName[i]);
                        gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + Dagit.Width + (int)numericUpDown_OneLine_Spacing.Value;
                    }
                    i = comboBox_OneLine_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Date.Day.Ones;
                    src = new Bitmap(ListImagesFullName[i]);
                    gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                    src.Dispose();
                    Dagit.Dispose();
                    Delimit.Dispose();

                    if (BBorder)
                    {
                        Rectangle rect = new Rectangle(x1, y1, x2 - x1 - 1, y2 - y1 - 1);
                        using (Pen pen1 = new Pen(Color.White, 1))
                        {
                            gPanel.DrawRectangle(pen1, rect);
                        }
                        using (Pen pen2 = new Pen(Color.Black, 1))
                        {
                            pen2.DashStyle = DashStyle.Dot;
                            gPanel.DrawRectangle(pen2, rect);
                        }
                    }
                }

                if ((checkBox_WeekDay.Checked) && (comboBox_WeekDay_Image.SelectedIndex >= 0))
                {
                    i = comboBox_WeekDay_Image.SelectedIndex + Watch_Face_Preview_Set.Date.WeekDay - 1;
                    src = new Bitmap(ListImagesFullName[i]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_WeekDay_X.Value,
                        (int)numericUpDown_WeekDay_Y.Value, src.Width, src.Height));
                    src.Dispose();
                }

                if ((checkBox_Year.Checked) && (comboBox_Year_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_Year_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_Year_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_Year_EndCorner_X.Value + 1;
                    int y2 = (int)numericUpDown_Year_EndCorner_Y.Value + 1;
                    var Dagit = new Bitmap(ListImagesFullName[comboBox_Year_Image.SelectedIndex]);
                    var Delimit = new Bitmap(1, 1);
                    if (comboBox_Year_Delimiter.SelectedIndex >= 0)
                        Delimit = new Bitmap(ListImagesFullName[comboBox_Year_Delimiter.SelectedIndex]);
                    int DelimitW = Delimit.Width;
                    if (comboBox_Year_Delimiter.SelectedIndex < 0) DelimitW = 0;

                    int DateLenght = Dagit.Width * 4 + (int)numericUpDown_Year_Spacing.Value * 4 + DelimitW;
                    int DateHeight = Dagit.Height;
                    if (comboBox_Year_Delimiter.SelectedIndex < 0) DateLenght = DateLenght - DelimitW;
                    if (DateLenght < Dagit.Width) DateLenght = Dagit.Width;

                    int PointX = 0;
                    int PointY = 0;
                    switch (comboBox_Year_Alignment.SelectedIndex)
                    {
                        case 0:
                        case 1:
                        case 2:
                            PointY = y1;
                            break;
                        case 3:
                        case 4:
                        case 5:
                            PointY = (y1 + y2) / 2 - DateHeight / 2;
                            break;
                        case 6:
                        case 7:
                        case 8:
                            PointY = y2 - DateHeight;
                            break;
                    }
                    switch (comboBox_Year_Alignment.SelectedIndex)
                    {
                        case 0:
                        case 3:
                        case 6:
                            PointX = x1;
                            break;
                        case 1:
                        case 4:
                        case 7:
                            PointX = (x1 + x2) / 2 - DateLenght / 2;
                            break;
                        case 2:
                        case 5:
                        case 8:
                            PointX = x2 - DateLenght;
                            break;
                    }
                    if (PointX < x1) PointX = x1;
                    if (PointY < y1) PointY = y1;
                    
                    i = comboBox_Year_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Year.Thousands;
                    src = new Bitmap(ListImagesFullName[i]);
                    gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                    PointX = PointX + Dagit.Width + (int)numericUpDown_Year_Spacing.Value;
                    src.Dispose();

                    i = comboBox_Year_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Year.Hundreds;
                    src = new Bitmap(ListImagesFullName[i]);
                    gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                    PointX = PointX + Dagit.Width + (int)numericUpDown_Year_Spacing.Value;
                    src.Dispose();

                    i = comboBox_Year_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Year.Tens;
                    src = new Bitmap(ListImagesFullName[i]);
                    gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                    PointX = PointX + Dagit.Width + (int)numericUpDown_Year_Spacing.Value;
                    src.Dispose();

                    i = comboBox_Year_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Year.Ones;
                    src = new Bitmap(ListImagesFullName[i]);
                    gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                    PointX = PointX + Dagit.Width + (int)numericUpDown_Year_Spacing.Value;
                    src.Dispose();

                    if (comboBox_Year_Delimiter.SelectedIndex >= 0)
                    {
                        i = comboBox_Year_Delimiter.SelectedIndex;
                        src = new Bitmap(ListImagesFullName[i]);
                        gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + DelimitW + (int)numericUpDown_Year_Spacing.Value;
                        src.Dispose();
                    }
                    
                    Dagit.Dispose();
                    Delimit.Dispose();

                    if (BBorder)
                    {
                        Rectangle rect = new Rectangle(x1, y1, x2 - x1 - 1, y2 - y1 - 1);
                        using (Pen pen1 = new Pen(Color.White, 1))
                        {
                            gPanel.DrawRectangle(pen1, rect);
                        }
                        using (Pen pen2 = new Pen(Color.Black, 1))
                        {
                            pen2.DashStyle = DashStyle.Dot;
                            gPanel.DrawRectangle(pen2, rect);
                        }
                    }
                }
            }
            #endregion

            #region DaysProgress
            // прогресс числа стрелкой
            if ((checkBox_ADDay_ClockHand.Checked) && (comboBox_ADDay_ClockHand_Image.SelectedIndex >= 0))
            {
                int x1 = (int)numericUpDown_ADDay_ClockHand_X.Value;
                int y1 = (int)numericUpDown_ADDay_ClockHand_Y.Value;
                int offsetX = (int)numericUpDown_ADDay_ClockHand_Offset_X.Value;
                int offsetY = (int)numericUpDown_ADDay_ClockHand_Offset_Y.Value;
                int image_index = comboBox_ADDay_ClockHand_Image.SelectedIndex;
                float startAngle = (float)(numericUpDown_ADDay_ClockHand_StartAngle.Value);
                float endAngle = (float)(numericUpDown_ADDay_ClockHand_EndAngle.Value);
                int Day = Watch_Face_Preview_Set.Date.Day;
                Day--;
                float angle = startAngle + Day * (endAngle - startAngle) / 30;
                DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
            }

            // прогресс дней недели стрелкой
            if ((checkBox_ADWeekDay_ClockHand.Checked) && (comboBox_ADWeekDay_ClockHand_Image.SelectedIndex >= 0))
            {
                int x1 = (int)numericUpDown_ADWeekDay_ClockHand_X.Value;
                int y1 = (int)numericUpDown_ADWeekDay_ClockHand_Y.Value;
                int offsetX = (int)numericUpDown_ADWeekDay_ClockHand_Offset_X.Value;
                int offsetY = (int)numericUpDown_ADWeekDay_ClockHand_Offset_Y.Value;
                int image_index = comboBox_ADWeekDay_ClockHand_Image.SelectedIndex;
                float startAngle = (float)(numericUpDown_ADWeekDay_ClockHand_StartAngle.Value);
                float endAngle = (float)(numericUpDown_ADWeekDay_ClockHand_EndAngle.Value);
                int WeekDay = Watch_Face_Preview_Set.Date.WeekDay;
                WeekDay--;
                if (WeekDay < 0) WeekDay = 6;
                float angle = startAngle + WeekDay * (endAngle - startAngle) / 6;
                DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
            }

            // прогресс месяца стрелкой
            if ((checkBox_ADMonth_ClockHand.Checked) && (comboBox_ADMonth_ClockHand_Image.SelectedIndex >= 0))
            {
                int x1 = (int)numericUpDown_ADMonth_ClockHand_X.Value;
                int y1 = (int)numericUpDown_ADMonth_ClockHand_Y.Value;
                int offsetX = (int)numericUpDown_ADMonth_ClockHand_Offset_X.Value;
                int offsetY = (int)numericUpDown_ADMonth_ClockHand_Offset_Y.Value;
                int image_index = comboBox_ADMonth_ClockHand_Image.SelectedIndex;
                float startAngle = (float)(numericUpDown_ADMonth_ClockHand_StartAngle.Value);
                float endAngle = (float)(numericUpDown_ADMonth_ClockHand_EndAngle.Value);
                int Month = Watch_Face_Preview_Set.Date.Month;
                Month--;
                float angle = startAngle + Month * (endAngle - startAngle) / 11;
                DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
            }
            #endregion

            #region Weather
            if (checkBox_Weather.Checked)
            {
                if ((checkBox_Weather_Icon.Checked) && (comboBox_Weather_Icon_Image.SelectedIndex >= 0))
                {
                    if (comboBox_WeatherSet_Icon.SelectedIndex >= 0)
                    {
                        i = comboBox_Weather_Icon_Image.SelectedIndex + comboBox_WeatherSet_Icon.SelectedIndex;
                        src = new Bitmap(ListImagesFullName[i]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Weather_Icon_X.Value,
                            (int)numericUpDown_Weather_Icon_Y.Value, src.Width, src.Height));
                        src.Dispose();
                    }
                    else
                    {
                        if (comboBox_Weather_Icon_NDImage.SelectedIndex >= 0)
                        {
                            src = new Bitmap(ListImagesFullName[comboBox_Weather_Icon_NDImage.SelectedIndex]);
                            gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Weather_Icon_X.Value,
                                (int)numericUpDown_Weather_Icon_Y.Value, src.Width, src.Height));
                            src.Dispose();
                        }
                    }
                }

                if ((checkBox_Weather_Text.Checked) && (comboBox_Weather_Text_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_Weather_Text_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_Weather_Text_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_Weather_Text_EndCorner_X.Value + 1;
                    int y2 = (int)numericUpDown_Weather_Text_EndCorner_Y.Value + 1;
                    int image_index = comboBox_Weather_Text_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_Weather_Text_Spacing.Value;
                    int alignment = comboBox_Weather_Text_Alignment.SelectedIndex;
                    int data_number = (int)numericUpDown_WeatherSet_Temp.Value;
                    int minus = comboBox_Weather_Text_MinusImage.SelectedIndex;
                    int degris = comboBox_Weather_Text_DegImage.SelectedIndex;
                    int error = comboBox_Weather_Text_NDImage.SelectedIndex;
                    bool ND = !checkBox_WeatherSet_Temp.Checked;
                    DrawWeather(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, minus, degris, error, ND, BBorder);
                    
                }

                if ((checkBox_Weather_Day.Checked) && (comboBox_Weather_Day_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_Weather_Day_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_Weather_Day_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_Weather_Day_EndCorner_X.Value + 1;
                    int y2 = (int)numericUpDown_Weather_Day_EndCorner_Y.Value + 1;
                    int image_index = comboBox_Weather_Day_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_Weather_Day_Spacing.Value;
                    int alignment = comboBox_Weather_Day_Alignment.SelectedIndex;

                    int data_number = (int)numericUpDown_WeatherSet_DayTemp.Value;
                    int minus = comboBox_Weather_Text_MinusImage.SelectedIndex;
                    bool ND = !checkBox_WeatherSet_DayTemp.Checked;

                    int degris = comboBox_Weather_Text_DegImage.SelectedIndex;
                    int error = comboBox_Weather_Text_NDImage.SelectedIndex;
                    DrawWeather(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number,
                        minus, degris, error, ND, BBorder);

                }
                if ((checkBox_Weather_Night.Checked) && (comboBox_Weather_Night_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_Weather_Night_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_Weather_Night_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_Weather_Night_EndCorner_X.Value + 1;
                    int y2 = (int)numericUpDown_Weather_Night_EndCorner_Y.Value + 1;
                    int image_index = comboBox_Weather_Night_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_Weather_Night_Spacing.Value;
                    int alignment = comboBox_Weather_Night_Alignment.SelectedIndex;

                    int data_number = (int)numericUpDown_WeatherSet_NightTemp.Value;
                    int minus = comboBox_Weather_Text_MinusImage.SelectedIndex;
                    bool ND = !checkBox_WeatherSet_DayTemp.Checked;

                    int degris = comboBox_Weather_Text_DegImage.SelectedIndex;
                    int error = comboBox_Weather_Text_NDImage.SelectedIndex;
                    DrawWeather(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number,
                        minus, degris, error, ND, BBorder);

                }
            }
            #endregion

            gPanel.SmoothingMode = SmoothingMode.AntiAlias;

            #region StepsProgress
            if (checkBox_StepsProgress.Checked)
            {
                Pen pen = new Pen(comboBox_StepsProgress_Color.BackColor,
                    (float)numericUpDown_StepsProgress_Width.Value);
                //switch (comboBox_StepsProgress_Flatness.Text)
                //{
                //    case "Треугольное":
                //        pen.EndCap = LineCap.Triangle;
                //        pen.StartCap = LineCap.Triangle;
                //        break;
                //    case "Плоское":
                //        pen.EndCap = LineCap.Flat;
                //        pen.StartCap = LineCap.Flat;
                //        break;
                //    default:
                //        pen.EndCap = LineCap.Round;
                //        pen.StartCap = LineCap.Round;
                //        break;
                //}
                switch (comboBox_StepsProgress_Flatness.SelectedIndex)
                {
                    case 1:
                        pen.EndCap = LineCap.Triangle;
                        pen.StartCap = LineCap.Triangle;
                        break;
                    case 2:
                        pen.EndCap = LineCap.Flat;
                        pen.StartCap = LineCap.Flat;
                        break;
                    default:
                        pen.EndCap = LineCap.Round;
                        pen.StartCap = LineCap.Round;
                        break;
                }

                int x = (int)numericUpDown_StepsProgress_Center_X.Value -
                    (int)numericUpDown_StepsProgress_Radius_X.Value;
                int y = (int)numericUpDown_StepsProgress_Center_Y.Value -
                    (int)numericUpDown_StepsProgress_Radius_Y.Value;
                int width = (int)numericUpDown_StepsProgress_Radius_X.Value * 2;
                //int height = (int)numericUpDown_StepsProgress_Radius_Y.Value * 2;
                int height = width;
                float StartAngle = (float)numericUpDown_StepsProgress_StartAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_StepsProgress_EndAngle.Value -
                    numericUpDown_StepsProgress_StartAngle.Value);
                float AngleScale = (float)Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal;
                if (AngleScale > 1) AngleScale = 1;
                EndAngle = EndAngle * AngleScale;
                try
                {
                    if ((width > 0) && (height > 0)) gPanel.DrawArc(pen, x, y, width, height, StartAngle, EndAngle);
                }
                catch (Exception)
                {

                }
            }

            if (checkBox_SPSliced.Checked)
            {
                if ((comboBox_SPSliced_Image.SelectedIndex >= 0) && (dataGridView_SPSliced.Rows.Count > 0))
                {
                    int x = 0;
                    int y = 0;
                    //int count = 0;
                    for (int count = 0; count < dataGridView_SPSliced.Rows.Count; count++)
                    {
                        if ((dataGridView_SPSliced.Rows[count].Cells[0].Value != null) &&
                            (dataGridView_SPSliced.Rows[count].Cells[1].Value != null))
                        {
                            //int x = Int32.Parse(dataGridView_SPSliced.Rows[count].Cells[0].Value.ToString());
                            //int y = Int32.Parse(dataGridView_SPSliced.Rows[count].Cells[1].Value.ToString());
                            if (Int32.TryParse(dataGridView_SPSliced.Rows[count].Cells[0].Value.ToString(), out x) &&
                                Int32.TryParse(dataGridView_SPSliced.Rows[count].Cells[1].Value.ToString(), out y))
                            {
                                i = comboBox_SPSliced_Image.SelectedIndex + count;
                                if (i < ListImagesFullName.Count)
                                {
                                    int value = (dataGridView_SPSliced.Rows.Count - 1) * Watch_Face_Preview_Set.Activity.Steps /
                                        Watch_Face_Preview_Set.Activity.StepsGoal;
                                    if (count < value)
                                    {
                                        src = new Bitmap(ListImagesFullName[i]);
                                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        //count++;
                                        src.Dispose();
                                    }
                                } 
                            }
                        }
                    }
                }
            }
            #endregion

            if (scale != 0.5) gPanel.SmoothingMode = SmoothingMode.Default;

            #region Activity
            if (checkBox_Activity.Checked)
            {
                if ((checkBox_ActivitySteps.Checked) && (comboBox_ActivitySteps_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_ActivitySteps_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_ActivitySteps_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_ActivitySteps_EndCorner_X.Value;
                    int y2 = (int)numericUpDown_ActivitySteps_EndCorner_Y.Value;
                    x2++;
                    y2++;
                    int image_index = comboBox_ActivitySteps_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_ActivitySteps_Spacing.Value;
                    int alignment = comboBox_ActivitySteps_Alignment.SelectedIndex;
                    int data_number = Watch_Face_Preview_Set.Activity.Steps;
                    if (numericUpDown_ActivitySteps_Count.Value == 10)
                        DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, BBorder);
                }

                // прогресс шагов стрелкой
                if ((checkBox_StProg_ClockHand.Checked) && (comboBox_StProg_ClockHand_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_StProg_ClockHand_X.Value;
                    int y1 = (int)numericUpDown_StProg_ClockHand_Y.Value;
                    int offsetX = (int)numericUpDown_StProg_ClockHand_Offset_X.Value;
                    int offsetY = (int)numericUpDown_StProg_ClockHand_Offset_Y.Value;
                    int image_index = comboBox_StProg_ClockHand_Image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_StProg_ClockHand_StartAngle.Value);
                    float endAngle = (float)(numericUpDown_StProg_ClockHand_EndAngle.Value);
                    float angle = startAngle + Watch_Face_Preview_Set.Activity.Steps *
                        (endAngle - startAngle) / Watch_Face_Preview_Set.Activity.StepsGoal;
                    if (Watch_Face_Preview_Set.Activity.Steps > Watch_Face_Preview_Set.Activity.StepsGoal) angle = endAngle;
                    DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
                }

                if ((checkBox_ActivityDistance.Checked) && (comboBox_ActivityDistance_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_ActivityDistance_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_ActivityDistance_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_ActivityDistance_EndCorner_X.Value;
                    int y2 = (int)numericUpDown_ActivityDistance_EndCorner_Y.Value;
                    x2++;
                    y2++;
                    int image_index = comboBox_ActivityDistance_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_ActivityDistance_Spacing.Value;
                    int alignment = comboBox_ActivityDistance_Alignment.SelectedIndex;
                    double data_number = Watch_Face_Preview_Set.Activity.Distance / 1000f;
                    int suffix = comboBox_ActivityDistance_Suffix.SelectedIndex;
                    int dec = comboBox_ActivityDistance_Decimal.SelectedIndex;
                    if (numericUpDown_ActivityDistance_Count.Value == 10)
                        DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, 
                            suffix, dec, 2, BBorder);
                }

                if ((checkBox_ActivityPuls.Checked) && (comboBox_ActivityPuls_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_ActivityPuls_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_ActivityPuls_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_ActivityPuls_EndCorner_X.Value;
                    int y2 = (int)numericUpDown_ActivityPuls_EndCorner_Y.Value;
                    x2++;
                    y2++;
                    int image_index = comboBox_ActivityPuls_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_ActivityPuls_Spacing.Value;
                    int alignment = comboBox_ActivityPuls_Alignment.SelectedIndex;
                    int data_number = Watch_Face_Preview_Set.Activity.Pulse;
                    if (numericUpDown_ActivityPuls_Count.Value == 10)
                        DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, BBorder);
                }

                // набор иконок
                if (checkBox_ActivityPuls_IconSet.Checked)
                {
                    if ((comboBox_ActivityPuls_IconSet_Image.SelectedIndex >= 0) && (dataGridView_ActivityPuls_IconSet.Rows.Count > 0))
                    {
                        int x = 0;
                        int y = 0;
                        //int count = 0;
                        for (int count = 0; count < dataGridView_ActivityPuls_IconSet.Rows.Count; count++)
                        {
                            if ((dataGridView_ActivityPuls_IconSet.Rows[count].Cells[0].Value != null) &&
                                (dataGridView_ActivityPuls_IconSet.Rows[count].Cells[1].Value != null))
                            {
                                if (Int32.TryParse(dataGridView_ActivityPuls_IconSet.Rows[count].Cells[0].Value.ToString(), out x) &&
                                    Int32.TryParse(dataGridView_ActivityPuls_IconSet.Rows[count].Cells[1].Value.ToString(), out y))
                                {
                                    i = comboBox_ActivityPuls_IconSet_Image.SelectedIndex + count;
                                    if (i < ListImagesFullName.Count)
                                    {
                                        int value = (dataGridView_ActivityPuls_IconSet.Rows.Count - 1) * Watch_Face_Preview_Set.Activity.Pulse / 200;
                                        value++;
                                        if (count < value)
                                        {
                                            src = new Bitmap(ListImagesFullName[i]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                            //count++;
                                            src.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // шкала пульса 
                if (checkBox_ActivityPulsScale.Checked)
                {
                    gPanel.SmoothingMode = SmoothingMode.AntiAlias;
                    Pen pen = new Pen(comboBox_ActivityPulsScale_Color.BackColor,
                        (float)numericUpDown_ActivityPulsScale_Width.Value);
                    
                    switch (comboBox_ActivityPulsScale_Flatness.SelectedIndex)
                    {
                        case 1:
                            pen.EndCap = LineCap.Triangle;
                            pen.StartCap = LineCap.Triangle;
                            break;
                        case 2:
                            pen.EndCap = LineCap.Flat;
                            pen.StartCap = LineCap.Flat;
                            break;
                        default:
                            pen.EndCap = LineCap.Round;
                            pen.StartCap = LineCap.Round;
                            break;
                    }

                    int x = (int)numericUpDown_ActivityPulsScale_Center_X.Value -
                        (int)numericUpDown_ActivityPulsScale_Radius_X.Value;
                    int y = (int)numericUpDown_ActivityPulsScale_Center_Y.Value -
                        (int)numericUpDown_ActivityPulsScale_Radius_Y.Value;
                    int width = (int)numericUpDown_ActivityPulsScale_Radius_X.Value * 2;
                    //int height = (int)numericUpDown_Battery_Scale_Radius_Y.Value * 2;
                    int height = width;
                    float StartAngle = (float)numericUpDown_ActivityPulsScale_StartAngle.Value - 90;
                    float EndAngle = (float)(numericUpDown_ActivityPulsScale_EndAngle.Value -
                        numericUpDown_ActivityPulsScale_StartAngle.Value);
                    float AngleScale = (float)Watch_Face_Preview_Set.Activity.Pulse / 200f;
                    if (AngleScale > 1) AngleScale = 1;
                    EndAngle = EndAngle * AngleScale;
                    try
                    {
                        if ((width > 0) && (height > 0)) gPanel.DrawArc(pen, x, y, width, height, StartAngle, EndAngle);
                    }
                    catch (Exception)
                    {

                    }
                }

                if ((checkBox_ActivityCalories.Checked) && (comboBox_ActivityCalories_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_ActivityCalories_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_ActivityCalories_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_ActivityCalories_EndCorner_X.Value;
                    int y2 = (int)numericUpDown_ActivityCalories_EndCorner_Y.Value;
                    x2++;
                    y2++;
                    int image_index = comboBox_ActivityCalories_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_ActivityCalories_Spacing.Value;
                    int alignment = comboBox_ActivityCalories_Alignment.SelectedIndex;
                    int data_number = Watch_Face_Preview_Set.Activity.Calories;
                    if (numericUpDown_ActivityCalories_Count.Value == 10)
                        DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, BBorder);
                }

                // шкала калорий 
                if (checkBox_ActivityCaloriesScale.Checked)
                {
                    gPanel.SmoothingMode = SmoothingMode.AntiAlias;
                    Pen pen = new Pen(comboBox_ActivityCaloriesScale_Color.BackColor,
                        (float)numericUpDown_ActivityCaloriesScale_Width.Value);

                    switch (comboBox_ActivityCaloriesScale_Flatness.SelectedIndex)
                    {
                        case 1:
                            pen.EndCap = LineCap.Triangle;
                            pen.StartCap = LineCap.Triangle;
                            break;
                        case 2:
                            pen.EndCap = LineCap.Flat;
                            pen.StartCap = LineCap.Flat;
                            break;
                        default:
                            pen.EndCap = LineCap.Round;
                            pen.StartCap = LineCap.Round;
                            break;
                    }

                    int x = (int)numericUpDown_ActivityCaloriesScale_Center_X.Value -
                        (int)numericUpDown_ActivityCaloriesScale_Radius_X.Value;
                    int y = (int)numericUpDown_ActivityCaloriesScale_Center_Y.Value -
                        (int)numericUpDown_ActivityCaloriesScale_Radius_Y.Value;
                    int width = (int)numericUpDown_ActivityCaloriesScale_Radius_X.Value * 2;
                    //int height = (int)numericUpDown_Battery_Scale_Radius_Y.Value * 2;
                    int height = width;
                    float StartAngle = (float)numericUpDown_ActivityCaloriesScale_StartAngle.Value - 90;
                    float EndAngle = (float)(numericUpDown_ActivityCaloriesScale_EndAngle.Value -
                        numericUpDown_ActivityCaloriesScale_StartAngle.Value);
                    float AngleScale = (float)(8f * Watch_Face_Preview_Set.Activity.Calories / 
                        Watch_Face_Preview_Set.Activity.StepsGoal);
                    if (AngleScale > 1) AngleScale = 1;
                    EndAngle = EndAngle * AngleScale;
                    try
                    {
                        if ((width > 0) && (height > 0)) gPanel.DrawArc(pen, x, y, width, height, StartAngle, EndAngle);
                    }
                    catch (Exception)
                    {

                    }
                }

                if ((checkBox_ActivityStar.Checked) && (comboBox_ActivityStar_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face_Preview_Set.Activity.Steps >= Watch_Face_Preview_Set.Activity.StepsGoal)
                    {
                        src = new Bitmap(ListImagesFullName[comboBox_ActivityStar_Image.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_ActivityStar_X.Value,
                            (int)numericUpDown_ActivityStar_Y.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region Status
            if (checkBox_Bluetooth.Checked)
            {
                if (Watch_Face_Preview_Set.Status.Bluetooth)
                {
                    if (comboBox_Bluetooth_On.SelectedIndex >= 0)
                    {
                        src = new Bitmap(ListImagesFullName[comboBox_Bluetooth_On.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Bluetooth_X.Value,
                            (int)numericUpDown_Bluetooth_Y.Value, src.Width, src.Height));
                        src.Dispose();
                    }
                }
                else
                {
                    if (comboBox_Bluetooth_Off.SelectedIndex >= 0)
                    {
                        src = new Bitmap(ListImagesFullName[comboBox_Bluetooth_Off.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Bluetooth_X.Value,
                            (int)numericUpDown_Bluetooth_Y.Value, src.Width, src.Height));
                        src.Dispose();
                    }
                }
            }

            if (checkBox_Alarm.Checked)
            {
                if (Watch_Face_Preview_Set.Status.Alarm)
                {
                    if (comboBox_Alarm_On.SelectedIndex >= 0)
                    {
                        src = new Bitmap(ListImagesFullName[comboBox_Alarm_On.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Alarm_X.Value,
                            (int)numericUpDown_Alarm_Y.Value, src.Width, src.Height));
                        src.Dispose();
                    }
                }
                else
                {
                    if (comboBox_Alarm_Off.SelectedIndex >= 0)
                    {
                        src = new Bitmap(ListImagesFullName[comboBox_Alarm_Off.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Alarm_X.Value,
                            (int)numericUpDown_Alarm_Y.Value, src.Width, src.Height));
                        src.Dispose();
                    }
                }
            }

            if (checkBox_Lock.Checked)
            {
                if (Watch_Face_Preview_Set.Status.Lock)
                {
                    if (comboBox_Lock_On.SelectedIndex >= 0)
                    {
                        src = new Bitmap(ListImagesFullName[comboBox_Lock_On.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Lock_X.Value,
                            (int)numericUpDown_Lock_Y.Value, src.Width, src.Height));
                        src.Dispose();
                    }
                }
                else
                {
                    if (comboBox_Lock_Off.SelectedIndex >= 0)
                    {
                        src = new Bitmap(ListImagesFullName[comboBox_Lock_Off.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Lock_X.Value,
                            (int)numericUpDown_Lock_Y.Value, src.Width, src.Height));
                        src.Dispose();
                    }
                }
            }

            if (checkBox_DND.Checked)
            {
                if (Watch_Face_Preview_Set.Status.DoNotDisturb)
                {
                    if (comboBox_DND_On.SelectedIndex >= 0)
                    {
                        src = new Bitmap(ListImagesFullName[comboBox_DND_On.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_DND_X.Value,
                            (int)numericUpDown_DND_Y.Value, src.Width, src.Height));
                        src.Dispose();
                    }
                }
                else
                {
                    if (comboBox_DND_Off.SelectedIndex >= 0)
                    {
                        src = new Bitmap(ListImagesFullName[comboBox_DND_Off.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_DND_X.Value,
                            (int)numericUpDown_DND_Y.Value, src.Width, src.Height));
                        src.Dispose();
                    }
                }
            }
            #endregion

            #region Battery
            if (checkBox_Battery.Checked)
            {
                if (checkBox_Battery.Checked)
                {
                    // заряд числом
                    if ((checkBox_Battery_Text.Checked) && (comboBox_Battery_Text_Image.SelectedIndex >= 0))
                    {
                        int x1 = (int)numericUpDown_Battery_Text_StartCorner_X.Value;
                        int y1 = (int)numericUpDown_Battery_Text_StartCorner_Y.Value;
                        int x2 = (int)numericUpDown_Battery_Text_EndCorner_X.Value;
                        int y2 = (int)numericUpDown_Battery_Text_EndCorner_Y.Value;
                        x2++;
                        y2++;
                        int image_index = comboBox_Battery_Text_Image.SelectedIndex;
                        int spacing = (int)numericUpDown_Battery_Text_Spacing.Value;
                        int alignment = comboBox_Battery_Text_Alignment.SelectedIndex;
                        int data_number = Watch_Face_Preview_Set.Battery;
                        if ((checkBox_Battery_Percent.Checked) && (comboBox_Battery_Percent_Image.SelectedIndex >= 0) &&
                            (numericUpDown_Battery_Percent_X.Value == 0) && (numericUpDown_Battery_Percent_Y.Value == 0))
                        {
                            if (numericUpDown_ActivityDistance_Count.Value == 10)
                                DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number,
                                    comboBox_Battery_Percent_Image.SelectedIndex, -1, 0, BBorder);
                        }
                        else if (numericUpDown_Battery_Text_Count.Value == 10)
                            DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, BBorder);
                    }

                    // заряд картинкой
                    if ((checkBox_Battery_Img.Checked) && (comboBox_Battery_Img_Image.SelectedIndex >= 0))
                    {
                        float count = (float)numericUpDown_Battery_Img_Count.Value - 1;
                        int offSet = (int)Math.Round(count * Watch_Face_Preview_Set.Battery / 100f, 0);
                        i = comboBox_Battery_Img_Image.SelectedIndex + offSet;
                        src = new Bitmap(ListImagesFullName[i]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Battery_Img_X.Value,
                            (int)numericUpDown_Battery_Img_Y.Value, src.Width, src.Height));
                        src.Dispose();
                    }

                    // значек процентов
                    //if ((checkBox_Battery_Percent.Checked) && (comboBox_Battery_Percent_Image.SelectedIndex >= 0))
                    if ((checkBox_Battery_Percent.Checked) && (comboBox_Battery_Percent_Image.SelectedIndex >= 0) &&
                            (numericUpDown_Battery_Percent_X.Value != 0) && (numericUpDown_Battery_Percent_Y.Value != 0))
                    {
                        src = new Bitmap(ListImagesFullName[comboBox_Battery_Percent_Image.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Battery_Percent_X.Value,
                            (int)numericUpDown_Battery_Percent_Y.Value, src.Width, src.Height));
                        src.Dispose();
                    }

                    // шкала
                    if (checkBox_Battery_Scale.Checked)
                    {
                        gPanel.SmoothingMode = SmoothingMode.AntiAlias;
                        Pen pen = new Pen(comboBox_Battery_Scale_Color.BackColor,
                            (float)numericUpDown_Battery_Scale_Width.Value);


                        //pen.EndCap = LineCap.Flat;
                        //pen.StartCap = LineCap.Triangle;
                        //switch (comboBox_Battery_Flatness.Text)
                        //{
                        //    case "Треугольное":
                        //        pen.EndCap = LineCap.Triangle;
                        //        pen.StartCap = LineCap.Triangle;
                        //        break;
                        //    case "Плоское":
                        //        pen.EndCap = LineCap.Flat;
                        //        pen.StartCap = LineCap.Flat;
                        //        break;
                        //    default:
                        //        pen.EndCap = LineCap.Round;
                        //        pen.StartCap = LineCap.Round;
                        //        break;
                        //}
                        switch (comboBox_Battery_Flatness.SelectedIndex)
                        {
                            case 1:
                                pen.EndCap = LineCap.Triangle;
                                pen.StartCap = LineCap.Triangle;
                                break;
                            case 2:
                                pen.EndCap = LineCap.Flat;
                                pen.StartCap = LineCap.Flat;
                                break;
                            default:
                                pen.EndCap = LineCap.Round;
                                pen.StartCap = LineCap.Round;
                                break;
                        }

                        int x = (int)numericUpDown_Battery_Scale_Center_X.Value -
                            (int)numericUpDown_Battery_Scale_Radius_X.Value;
                        int y = (int)numericUpDown_Battery_Scale_Center_Y.Value -
                            (int)numericUpDown_Battery_Scale_Radius_Y.Value;
                        int width = (int)numericUpDown_Battery_Scale_Radius_X.Value * 2;
                        //int height = (int)numericUpDown_Battery_Scale_Radius_Y.Value * 2;
                        int height = width;
                        float StartAngle = (float)numericUpDown_Battery_Scale_StartAngle.Value - 90;
                        float EndAngle = (float)(numericUpDown_Battery_Scale_EndAngle.Value -
                            numericUpDown_Battery_Scale_StartAngle.Value);
                        float AngleScale = (float)Watch_Face_Preview_Set.Battery / 100f;
                        if (AngleScale > 1) AngleScale = 1;
                        EndAngle = EndAngle * AngleScale;
                        try
                        {
                            if ((width > 0) && (height > 0)) gPanel.DrawArc(pen, x, y, width, height, StartAngle, EndAngle);
                        }
                        catch (Exception)
                        {

                        }
                    }

                    // заряд стрелочный индикатор
                    if ((checkBox_Battery_ClockHand.Checked) && (comboBox_Battery_ClockHand_Image.SelectedIndex >= 0))
                    {
                        int x1 = (int)numericUpDown_Battery_ClockHand_X.Value;
                        int y1 = (int)numericUpDown_Battery_ClockHand_Y.Value;
                        int offsetX = (int)numericUpDown_Battery_ClockHand_Offset_X.Value;
                        int offsetY = (int)numericUpDown_Battery_ClockHand_Offset_Y.Value;
                        int image_index = comboBox_Battery_ClockHand_Image.SelectedIndex;
                        float startAngle = (float)(numericUpDown_Battery_ClockHand_StartAngle.Value);
                        float endAngle = (float)(numericUpDown_Battery_ClockHand_EndAngle.Value);
                        float angle = startAngle + Watch_Face_Preview_Set.Battery * (endAngle - startAngle) / 100;
                        DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
                    }

                    // набор иконок
                    if (checkBox_Battery_IconSet.Checked)
                    {
                        if ((comboBox_Battery_IconSet_Image.SelectedIndex >= 0) && (dataGridView_Battery_IconSet.Rows.Count > 0))
                        {
                            int x = 0;
                            int y = 0;
                            //int count = 0;
                            for (int count = 0; count < dataGridView_Battery_IconSet.Rows.Count; count++)
                            {
                                if ((dataGridView_Battery_IconSet.Rows[count].Cells[0].Value != null) &&
                                    (dataGridView_Battery_IconSet.Rows[count].Cells[1].Value != null))
                                {
                                    //int x = Int32.Parse(dataGridView_SPSliced.Rows[count].Cells[0].Value.ToString());
                                    //int y = Int32.Parse(dataGridView_SPSliced.Rows[count].Cells[1].Value.ToString());
                                    if (Int32.TryParse(dataGridView_Battery_IconSet.Rows[count].Cells[0].Value.ToString(), out x) &&
                                        Int32.TryParse(dataGridView_Battery_IconSet.Rows[count].Cells[1].Value.ToString(), out y))
                                    {
                                        i = comboBox_Battery_IconSet_Image.SelectedIndex + count;
                                        if (i < ListImagesFullName.Count)
                                        {
                                            int value = (dataGridView_Battery_IconSet.Rows.Count - 1) * Watch_Face_Preview_Set.Battery / 100;
                                            value++;
                                            if (count < value)
                                            {
                                                src = new Bitmap(ListImagesFullName[i]);
                                                gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                                //count++;
                                                src.Dispose();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
            #endregion

            #region AnalogDialFace
            if (checkBox_AnalogClock.Checked)
            {
                bool SecondsOffSet = false;
                if ((numericUpDown_AnalogClock_Sec_Offset_X.Value != 0) ||
                    (numericUpDown_AnalogClock_Sec_Offset_Y.Value != 0)) SecondsOffSet = true;

                if (SecondsOffSet)
                {
                    // секунды
                    if ((checkBox_AnalogClock_Sec.Checked) && (comboBox_AnalogClock_Sec_Image.SelectedIndex >= 0))
                    {
                        int x1 = (int)numericUpDown_AnalogClock_Sec_X.Value;
                        int y1 = (int)numericUpDown_AnalogClock_Sec_Y.Value;
                        int offsetX = (int)numericUpDown_AnalogClock_Sec_Offset_X.Value;
                        int offsetY = (int)numericUpDown_AnalogClock_Sec_Offset_Y.Value;
                        int image_index = comboBox_AnalogClock_Sec_Image.SelectedIndex;
                        //int hour = Watch_Face_Preview_Set.TimeW.Hours;
                        //int min = Watch_Face_Preview_Set.TimeW.Minutes;
                        int sec = Watch_Face_Preview_Set.Time.Seconds;
                        //if (hour >= 12) hour = hour - 12;
                        float angle = 360 * sec / 60;
                        DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
                    }
                }

                // часы
                if ((checkBox_AnalogClock_Hour.Checked) && (comboBox_AnalogClock_Hour_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_AnalogClock_Hour_X.Value;
                    int y1 = (int)numericUpDown_AnalogClock_Hour_Y.Value;
                    int offsetX = (int)numericUpDown_AnalogClock_Hour_Offset_X.Value;
                    int offsetY = (int)numericUpDown_AnalogClock_Hour_Offset_Y.Value;
                    int image_index = comboBox_AnalogClock_Hour_Image.SelectedIndex;
                    int hour = Watch_Face_Preview_Set.Time.Hours;
                    int min = Watch_Face_Preview_Set.Time.Minutes;
                    //int sec = Watch_Face_Preview_Set.TimeW.Seconds;
                    if (hour >= 12) hour = hour - 12;
                    float angle = 360 * hour / 12 + 360 * min / (60 * 12);
                    DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
                }
                if ((checkBox_HourCenterImage.Checked) && (comboBox_HourCenterImage_Image.SelectedIndex >= 0))
                {
                    src = new Bitmap(ListImagesFullName[comboBox_HourCenterImage_Image.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_HourCenterImage_X.Value,
                        (int)numericUpDown_HourCenterImage_Y.Value, src.Width, src.Height));
                    src.Dispose();
                }

                // минуты
                if ((checkBox_AnalogClock_Min.Checked) && (comboBox_AnalogClock_Min_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_AnalogClock_Min_X.Value;
                    int y1 = (int)numericUpDown_AnalogClock_Min_Y.Value;
                    int offsetX = (int)numericUpDown_AnalogClock_Min_Offset_X.Value;
                    int offsetY = (int)numericUpDown_AnalogClock_Min_Offset_Y.Value;
                    int image_index = comboBox_AnalogClock_Min_Image.SelectedIndex;
                    //int hour = Watch_Face_Preview_Set.TimeW.Hours;
                    int min = Watch_Face_Preview_Set.Time.Minutes;
                    //int sec = Watch_Face_Preview_Set.TimeW.Seconds;
                    //if (hour >= 12) hour = hour - 12;
                    float angle = 360 * min / 60;
                    DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
                }
                if ((checkBox_MinCenterImage.Checked) && (comboBox_MinCenterImage_Image.SelectedIndex >= 0))
                {
                    src = new Bitmap(ListImagesFullName[comboBox_MinCenterImage_Image.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_MinCenterImage_X.Value,
                        (int)numericUpDown_MinCenterImage_Y.Value, src.Width, src.Height));
                    src.Dispose();
                }

                // секунды
                if (!SecondsOffSet)
                {
                    // секунды
                    if ((checkBox_AnalogClock_Sec.Checked) && (comboBox_AnalogClock_Sec_Image.SelectedIndex >= 0))
                    {
                        int x1 = (int)numericUpDown_AnalogClock_Sec_X.Value;
                        int y1 = (int)numericUpDown_AnalogClock_Sec_Y.Value;
                        int offsetX = (int)numericUpDown_AnalogClock_Sec_Offset_X.Value;
                        int offsetY = (int)numericUpDown_AnalogClock_Sec_Offset_Y.Value;
                        int image_index = comboBox_AnalogClock_Sec_Image.SelectedIndex;
                        //int hour = Watch_Face_Preview_Set.TimeW.Hours;
                        //int min = Watch_Face_Preview_Set.TimeW.Minutes;
                        int sec = Watch_Face_Preview_Set.Time.Seconds;
                        //if (hour >= 12) hour = hour - 12;
                        float angle = 360 * sec / 60;
                        DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
                    }
                }
                if ((checkBox_SecCenterImage.Checked) && (comboBox_SecCenterImage_Image.SelectedIndex >= 0))
                {
                    src = new Bitmap(ListImagesFullName[comboBox_SecCenterImage_Image.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_SecCenterImage_X.Value,
                        (int)numericUpDown_SecCenterImage_Y.Value, src.Width, src.Height));
                    src.Dispose();
                }
            }
            #endregion

            #region Mesh

            if (WMesh)
            {
                Pen pen = new Pen(Color.White, 1);
                int LineDistance = 30;
                if (panel_Preview.Height > 300) LineDistance = 15;
                for (i = 0; i < 30; i++)
                {
                    gPanel.DrawLine(pen, new Point(offSet_X + i * LineDistance, 0), new Point(offSet_X + i * LineDistance, 454));
                    gPanel.DrawLine(pen, new Point(offSet_X - i * LineDistance, 0), new Point(offSet_X - i * LineDistance, 454));

                    gPanel.DrawLine(pen, new Point(0, offSet_Y + i * LineDistance), new Point(454, offSet_Y + i * LineDistance));
                    gPanel.DrawLine(pen, new Point(0, offSet_Y - i * LineDistance), new Point(454, offSet_Y - i * LineDistance));
                }
            }

            if (BMesh)
            {
                Pen pen = new Pen(Color.Black, 1);
                int LineDistance = 30;
                if (panel_Preview.Height > 300) LineDistance = 15;
                for (i = 0; i < 30; i++)
                {
                    gPanel.DrawLine(pen, new Point(offSet_X + i * LineDistance, 0), new Point(offSet_X + i * LineDistance, 454));
                    gPanel.DrawLine(pen, new Point(offSet_X - i * LineDistance, 0), new Point(offSet_X - i * LineDistance, 454));

                    gPanel.DrawLine(pen, new Point(0, offSet_Y + i * LineDistance), new Point(454, offSet_Y + i * LineDistance));
                    gPanel.DrawLine(pen, new Point(0, offSet_Y - i * LineDistance), new Point(454, offSet_Y - i * LineDistance));
                }
            }
            #endregion

            src.Dispose();

            if (crop)
            {
                Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr47.png");
                if (radioButton_42.Checked)
                {
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr42.png");
                }
                if (radioButton_gts.Checked)
                {
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gts.png");
                }
                mask = FormColor(mask);
                gPanel.DrawImage(mask, new Rectangle(0, 0, mask.Width, mask.Height));
                mask.Dispose();
            }
        }

        /// <summary>Рисует число</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="x1">TopLeftX</param>
        /// <param name="y1">TopLefty</param>
        /// <param name="x2">BottomRightX</param>
        /// <param name="y2">BottomRightY</param>
        /// <param name="image_index">Номер изображения</param>
        /// <param name="spacing">Величина отступа</param>
        /// <param name="alignment">Новер выравнивания</param>
        /// <param name="data_number">Отображаемая величина</param>
        /// <param name="BBorder">Рисовать рамку по координатам, вокруг элементов с выравниванием</param>
        public void DrawNumber(Graphics graphics, int x1, int y1, int x2, int y2,
            int image_index, int spacing, int alignment, int data_number, bool BBorder)
        {
            var Dagit = new Bitmap(ListImagesFullName[image_index]);
            string data_numberS = data_number.ToString();
            char[] CH = data_numberS.ToCharArray();
            int _number;
            int i;
            var src = new Bitmap(1, 1);
            //int DateLenght = Dagit.Width * data_numberS.Length + spacing * (data_numberS.Length - 1);
            int DateLenght = 0;
            foreach (char ch in CH)
            {
                _number = 0;
                if (int.TryParse(ch.ToString(), out _number))
                {
                    i = image_index + _number;
                    if (i < ListImagesFullName.Count)
                    {
                        src = new Bitmap(ListImagesFullName[i]);
                        DateLenght = DateLenght + src.Width + spacing;
                        src.Dispose();
                    }
                }

            }
            DateLenght = DateLenght - spacing;
            src = new Bitmap(ListImagesFullName[image_index]);
            if (DateLenght < src.Width) DateLenght = src.Width;
            src.Dispose();

            int DateHeight = Dagit.Height;

            int PointX = 0;
            int PointY = 0;
            switch (alignment)
            {
                case 0:
                case 1:
                case 2:
                    PointY = y1;
                    break;
                case 3:
                case 4:
                case 5:
                    PointY = (y1 + y2) / 2 - DateHeight / 2;
                    break;
                case 6:
                case 7:
                case 8:
                    PointY = y2 - DateHeight;
                    break;
            }
            switch (alignment)
            {
                case 0:
                case 3:
                case 6:
                    PointX = x1;
                    break;
                case 1:
                case 4:
                case 7:
                    PointX = (x1 + x2) / 2 - DateLenght / 2;
                    break;
                case 2:
                case 5:
                case 8:
                    PointX = x2 - DateLenght;
                    break;
            }
            if (PointX < x1) PointX = x1;
            if (PointY < y1) PointY = y1;
            foreach (char ch in CH)
            {
                _number = 0;
                if (int.TryParse(ch.ToString(), out _number))
                {
                    i = image_index + _number;
                    if (i < ListImagesFullName.Count)
                    {
                        src = new Bitmap(ListImagesFullName[i]);
                        graphics.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + src.Width + spacing;
                        src.Dispose();
                    }
                }

            }
            Dagit.Dispose();

            if (BBorder)
            {
                Rectangle rect = new Rectangle(x1, y1, x2 - x1 - 1, y2 - y1 - 1);
                using (Pen pen1 = new Pen(Color.White, 1))
                {
                    graphics.DrawRectangle(pen1, rect);
                }
                using (Pen pen2 = new Pen(Color.Black, 1))
                {
                    pen2.DashStyle = DashStyle.Dot;
                    graphics.DrawRectangle(pen2, rect);
                }
            }
        }

        /// <summary>Рисует число</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="x1">TopLeftX</param>
        /// <param name="y1">TopLefty</param>
        /// <param name="x2">BottomRightX</param>
        /// <param name="y2">BottomRightY</param>
        /// <param name="image_index">Номер изображения</param>
        /// <param name="spacing">Величина отступа</param>
        /// <param name="alignment">Новер выравнивания</param>
        /// <param name="data_number">Отображаемая величина</param>
        /// <param name="suffix">Номер изображения суфикса</param>
        /// <param name="dec">Номер изображения разделителя</param>
        /// <param name="decCount">Число знаков после запятой</param>
        /// <param name="BBorder">Рисовать рамку по координатам, вокруг элементов с выравниванием</param>
        public void DrawNumber(Graphics graphics, int x1, int y1, int x2, int y2, int image_index, int spacing,
            int alignment, double data_number, int suffix, int dec, int decCount, bool BBorder)
        {
            data_number = Math.Round(data_number, 2);
            var Dagit = new Bitmap(ListImagesFullName[image_index]);
            //var Delimit = new Bitmap(1, 1);
            //if (dec >= 0) Delimit = new Bitmap(ListImagesFullName[dec]);
            string decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string data_numberS = data_number.ToString();
            if (decCount > 0)
            {
                if (data_numberS.IndexOf(decimalSeparator) < 0) data_numberS = data_numberS + decimalSeparator;
                while (data_numberS.IndexOf(decimalSeparator) > data_numberS.Length - decCount - 1)
                {
                    data_numberS = data_numberS + "0";
                } 
            }
            int DateLenght = 0;
            int _number;
            int i;
            var src = new Bitmap(1, 1);
            char[] CH = data_numberS.ToCharArray();

            foreach (char ch in CH)
            {
                _number = 0;
                if (int.TryParse(ch.ToString(), out _number))
                {
                    i = image_index + _number;
                    if (i < ListImagesFullName.Count)
                    {
                        src = new Bitmap(ListImagesFullName[i]);
                        DateLenght = DateLenght + src.Width + spacing;
                        src.Dispose();
                    }
                }
                else
                {
                    if (dec >= 0)
                    {
                        src = new Bitmap(ListImagesFullName[dec]);
                        DateLenght = DateLenght + src.Width + spacing;
                        src.Dispose();
                    }
                }

            }
            if (suffix >= 0)
            {
                src = new Bitmap(ListImagesFullName[suffix]);
                DateLenght = DateLenght + src.Width + spacing;
                src.Dispose();
            }
            DateLenght = DateLenght - spacing;
            src = new Bitmap(ListImagesFullName[image_index]);
            if (DateLenght < src.Width) DateLenght = src.Width;
            src.Dispose();
            //if ((data_number != (int)data_number) && (dec >= 0))
            //{
            //    DateLenght = Dagit.Width * (data_numberS.Length - 1) + spacing * (data_numberS.Length - 2);
            //}
            //else
            //{
            //    DateLenght = Dagit.Width * data_numberS.Length + spacing * (data_numberS.Length - 1);
            //}

            int DateHeight = Dagit.Height;

            int PointX = 0;
            int PointY = 0;
            switch (alignment)
            {
                case 0:
                case 1:
                case 2:
                    PointY = y1;
                    break;
                case 3:
                case 4:
                case 5:
                    PointY = (y1 + y2) / 2 - DateHeight / 2;
                    break;
                case 6:
                case 7:
                case 8:
                    PointY = y2 - DateHeight;
                    break;
            }
            switch (alignment)
            {
                case 0:
                case 3:
                case 6:
                    PointX = x1;
                    break;
                case 1:
                case 4:
                case 7:
                    PointX = (x1 + x2) / 2 - DateLenght / 2;
                    break;
                case 2:
                case 5:
                case 8:
                    PointX = x2 - DateLenght;
                    break;
            }
            if (PointX < x1) PointX = x1;
            if (PointY < y1) PointY = y1;
            foreach (char ch in CH)
            {
                _number = 0;
                if (int.TryParse(ch.ToString(), out _number))
                {
                    i = image_index + _number;
                    if (i < ListImagesFullName.Count)
                    {
                        src = new Bitmap(ListImagesFullName[i]);
                        graphics.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + src.Width + spacing;
                        src.Dispose();
                    }
                }
                else
                {
                    if (dec >= 0)
                    {
                        src = new Bitmap(ListImagesFullName[dec]);
                        graphics.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + src.Width + spacing;
                        src.Dispose();
                    }
                }

            }
            if (suffix >= 0)
            {
                src = new Bitmap(ListImagesFullName[suffix]);
                graphics.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                src.Dispose();

            }
            Dagit.Dispose();

            if (BBorder)
            {
                Rectangle rect = new Rectangle(x1, y1, x2 - x1 - 1, y2 - y1 - 1);
                using (Pen pen1 = new Pen(Color.White, 1))
                {
                    graphics.DrawRectangle(pen1, rect);
                }
                using (Pen pen2 = new Pen(Color.Black, 1))
                {
                    pen2.DashStyle = DashStyle.Dot;
                    graphics.DrawRectangle(pen2, rect);
                }
            }
        }

        /// <summary>Рисует погоду</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="x1">TopLeftX</param>
        /// <param name="y1">TopLefty</param>
        /// <param name="x2">BottomRightX</param>
        /// <param name="y2">BottomRightY</param>
        /// <param name="image_index">Номер изображения</param>
        /// <param name="spacing">Величина отступа</param>
        /// <param name="alignment">Новер выравнивания</param>
        /// <param name="data_number">Отображаемая величина</param>
        /// <param name="minus">Номер изображения минуса </param>
        /// <param name="degris">Номер изображения градуса</param>
        /// <param name="error">Номер изображения ошибки</param>
        /// <param name="ND">Показывать ошибку</param>
        /// <param name="BBorder">Рисовать рамку по координатам, вокруг элементов с выравниванием</param>
        public void DrawWeather(Graphics graphics, int x1, int y1, int x2, int y2, int image_index, int spacing,
            int alignment, int data_number, int minus, int degris, int error, bool ND, bool BBorder)
        {
            //data_number = Math.Round(data_number, 2);
            var Dagit = new Bitmap(ListImagesFullName[image_index]);
            //var Delimit = new Bitmap(1, 1);
            //if (dec >= 0) Delimit = new Bitmap(ListImagesFullName[dec]);
            string data_numberS = data_number.ToString();
            int DateLenght = 0;
            int _number;
            var src = new Bitmap(1, 1);
            char[] CH = data_numberS.ToCharArray();
            int i;
            if (!ND)
            {
                foreach (char ch in CH)
                {
                    _number = 0;
                    if (int.TryParse(ch.ToString(), out _number))
                    {
                        i = image_index + _number;
                        if (i < ListImagesFullName.Count)
                        {
                            src = new Bitmap(ListImagesFullName[i]);
                            DateLenght = DateLenght + src.Width + spacing;
                            src.Dispose();
                        }
                    }
                    else
                    {
                        if (minus >= 0)
                        {
                            src = new Bitmap(ListImagesFullName[minus]);
                            DateLenght = DateLenght + src.Width + spacing;
                            src.Dispose();
                        }
                    }

                }
                if (degris >= 0)
                {
                    src = new Bitmap(ListImagesFullName[degris]);
                    DateLenght = DateLenght + src.Width + spacing;
                    src.Dispose();
                }
                DateLenght = DateLenght - spacing;
            }
            else
            {
                if (error >= 0)
                {
                    src = new Bitmap(ListImagesFullName[error]);
                    DateLenght = DateLenght + src.Width;
                    src.Dispose();
                }
            }
            //if ((data_number != (int)data_number) && (dec >= 0))
            //{
            //    DateLenght = Dagit.Width * (data_numberS.Length - 1) + spacing * (data_numberS.Length - 2);
            //}
            //else
            //{
            //    DateLenght = Dagit.Width * data_numberS.Length + spacing * (data_numberS.Length - 1);
            //}

            int DateHeight = Dagit.Height;

            int PointX = 0;
            int PointY = 0;
            switch (alignment)
            {
                case 0:
                case 1:
                case 2:
                    PointY = y1;
                    break;
                case 3:
                case 4:
                case 5:
                    PointY = (y1 + y2) / 2 - DateHeight / 2;
                    break;
                case 6:
                case 7:
                case 8:
                    PointY = y2 - DateHeight;
                    break;
            }
            switch (alignment)
            {
                case 0:
                case 3:
                case 6:
                    PointX = x1;
                    break;
                case 1:
                case 4:
                case 7:
                    PointX = (x1 + x2) / 2 - DateLenght / 2;
                    break;
                case 2:
                case 5:
                case 8:
                    PointX = x2 - DateLenght;
                    break;
            }
            if (PointX < x1) PointX = x1;
            if (PointY < y1) PointY = y1;
            if (!ND)
            {
                foreach (char ch in CH)
                {
                    _number = 0;
                    if (int.TryParse(ch.ToString(), out _number))
                    {
                        i = image_index + _number;
                        if (i < ListImagesFullName.Count)
                        {
                            src = new Bitmap(ListImagesFullName[i]);
                            graphics.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                            PointX = PointX + src.Width + spacing;
                            src.Dispose();
                        }
                    }
                    else
                    {
                        if (minus >= 0)
                        {
                            src = new Bitmap(ListImagesFullName[minus]);
                            graphics.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                            PointX = PointX + src.Width + spacing;
                            src.Dispose();
                        }
                    }

                }
                if (degris >= 0)
                {
                    src = new Bitmap(ListImagesFullName[degris]);
                    graphics.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                    src.Dispose();
                }
            }
            else
            {
                if (error >= 0)
                {
                    src = new Bitmap(ListImagesFullName[error]);
                    graphics.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                    src.Dispose();
                }
            }
            Dagit.Dispose();

            if (BBorder)
            {
                Rectangle rect = new Rectangle(x1, y1, x2 - x1 - 1, y2 - y1 - 1);
                using (Pen pen1 = new Pen(Color.White, 1))
                {
                    graphics.DrawRectangle(pen1, rect);
                }
                using (Pen pen2 = new Pen(Color.Black, 1))
                {
                    pen2.DashStyle = DashStyle.Dot;
                    graphics.DrawRectangle(pen2, rect);
                }
            }
        }


        /// <summary>Рисует стрелки</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="x1">Центр стрелки X</param>
        /// <param name="y1">Центр стрелки Y</param>
        /// <param name="offsetX">Смещение от центра по X</param>
        /// <param name="offsetY">Смещение от центра по Y</param>
        /// <param name="image_index">Номер изображения</param>
        /// <param name="angle">Угол поворота стрелки в градусах</param>
        public void DrawAnalogClock(Graphics graphics, int x1, int y1, float offsetX, float offsetY, int image_index, float angle)
        {
            //graphics.RotateTransform(angle);
            var src = new Bitmap(ListImagesFullName[image_index]);
            //graphics.DrawImage(src, new Rectangle(227 - x1, 227 - y1, src.Width, src.Height));
            //if (!model_gtr47)
            //{
            //    offSet_X = 195;
            //    offSet_Y = 195;
            //}
            graphics.TranslateTransform(offSet_X + offsetX, offSet_Y + offsetY);
            graphics.RotateTransform(angle);
            graphics.DrawImage(src, new Rectangle(-x1, -y1, src.Width, src.Height));
            graphics.RotateTransform(-angle);
            graphics.TranslateTransform(-offSet_X - offsetX, -offSet_Y - offsetY);
            src.Dispose();
        }

        public Bitmap FormColor(Bitmap bitmap)
        {
            int[] bgColors = { 203, 255, 240 };
            Color color = panel_Preview.BackColor;
            ImageMagick.MagickImage image = new ImageMagick.MagickImage(bitmap);
            // меняем прозрачный цвет на цвет фона
            image.Opaque(ImageMagick.MagickColor.FromRgba((byte)0, (byte)0, (byte)0, (byte)0),
                ImageMagick.MagickColor.FromRgb(color.R, color.G, color.B));
            // меняем черный цвет на прозрачный
            image.Opaque(ImageMagick.MagickColor.FromRgb((byte)0, (byte)0, (byte)0),
                ImageMagick.MagickColor.FromRgba((byte)0, (byte)0, (byte)0, (byte)0));
            return image.ToBitmap();
        }
    }
}
