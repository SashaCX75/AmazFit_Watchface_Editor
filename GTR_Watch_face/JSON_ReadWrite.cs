using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTR_Watch_face
{
    public partial class Form1 : Form
    {
        /// <summary>заполняем поля с настройками из JSON файла</summary>
        private void JSON_read()
        {
            SettingsClear();
            comboBox_Background.Items.AddRange(ListImages.ToArray());
            comboBox_Preview.Items.AddRange(ListImages.ToArray());

            #region выпадающие списки с картинками
            comboBox_Hours_Tens_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Hours_Ones_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Min_Tens_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Min_Ones_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Sec_Tens_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Sec_Ones_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Delimiter_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Image_Am.Items.AddRange(ListImages.ToArray());
            comboBox_Image_Pm.Items.AddRange(ListImages.ToArray());

            comboBox_WeekDay_Image.Items.AddRange(ListImages.ToArray());
            comboBox_OneLine_Image.Items.AddRange(ListImages.ToArray());
            comboBox_OneLine_Delimiter.Items.AddRange(ListImages.ToArray());
            comboBox_MonthAndDayD_Image.Items.AddRange(ListImages.ToArray());
            comboBox_MonthAndDayM_Image.Items.AddRange(ListImages.ToArray());
            comboBox_MonthName_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Year_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Year_Delimiter.Items.AddRange(ListImages.ToArray());

            comboBox_ActivityGoal_Image.Items.AddRange(ListImages.ToArray());
            comboBox_ActivitySteps_Image.Items.AddRange(ListImages.ToArray());
            comboBox_ActivityDistance_Image.Items.AddRange(ListImages.ToArray());
            comboBox_ActivityDistance_Decimal.Items.AddRange(ListImages.ToArray());
            comboBox_ActivityDistance_Suffix.Items.AddRange(ListImages.ToArray());
            comboBox_ActivityPuls_Image.Items.AddRange(ListImages.ToArray());
            comboBox_ActivityCalories_Image.Items.AddRange(ListImages.ToArray());
            comboBox_ActivityStar_Image.Items.AddRange(ListImages.ToArray());

            comboBox_Bluetooth_On.Items.AddRange(ListImages.ToArray());
            comboBox_Bluetooth_Off.Items.AddRange(ListImages.ToArray());
            comboBox_Alarm_On.Items.AddRange(ListImages.ToArray());
            comboBox_Alarm_Off.Items.AddRange(ListImages.ToArray());
            comboBox_Lock_On.Items.AddRange(ListImages.ToArray());
            comboBox_Lock_Off.Items.AddRange(ListImages.ToArray());
            comboBox_DND_On.Items.AddRange(ListImages.ToArray());
            comboBox_DND_Off.Items.AddRange(ListImages.ToArray());

            comboBox_Battery_Text_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Battery_Img_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Battery_Percent_Image.Items.AddRange(ListImages.ToArray());

            comboBox_AnalogClock_Hour_Image.Items.AddRange(ListImages.ToArray());
            comboBox_AnalogClock_Min_Image.Items.AddRange(ListImages.ToArray());
            comboBox_AnalogClock_Sec_Image.Items.AddRange(ListImages.ToArray());

            comboBox_HourCenterImage_Image.Items.AddRange(ListImages.ToArray());
            comboBox_MinCenterImage_Image.Items.AddRange(ListImages.ToArray());
            comboBox_SecCenterImage_Image.Items.AddRange(ListImages.ToArray());

            comboBox_Weather_Text_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_Text_DegImage.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_Text_MinusImage.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_Text_NDImage.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_Icon_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_Icon_NDImage.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_Day_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_Night_Image.Items.AddRange(ListImages.ToArray());

            #endregion

            if (Watch_Face == null) return;
            if (Watch_Face.Background != null)
            {
                if (Watch_Face.Background.Image != null)
                    //comboBox_Background.Text = Watch_Face.Background.Image.ImageIndex.ToString();
                    checkBoxSetText(comboBox_Background, Watch_Face.Background.Image.ImageIndex);
                if (Watch_Face.Background.Preview != null)
                    //comboBox_Preview.Text comboBox_Preview Watch_Face.Background.Preview.ImageIndex.ToString();
                    checkBoxSetText(comboBox_Preview, Watch_Face.Background.Preview.ImageIndex);
            }

            #region Time
            if (Watch_Face.Time != null)
            {
                checkBox_Time.Checked = true;
                if (Watch_Face.Time.Hours != null)
                {
                    checkBox_Hours.Checked = true;
                    numericUpDown_Hours_Tens_X.Value = Watch_Face.Time.Hours.Tens.X;
                    numericUpDown_Hours_Tens_Y.Value = Watch_Face.Time.Hours.Tens.Y;
                    numericUpDown_Hours_Tens_Count.Value = Watch_Face.Time.Hours.Tens.ImagesCount;
                    //comboBox_Hours_Tens_Image.Text = Watch_Face.Time.Hours.Tens.ImageIndex.ToString();
                    checkBoxSetText(comboBox_Hours_Tens_Image, Watch_Face.Time.Hours.Tens.ImageIndex);

                    numericUpDown_Hours_Ones_X.Value = Watch_Face.Time.Hours.Ones.X;
                    numericUpDown_Hours_Ones_Y.Value = Watch_Face.Time.Hours.Ones.Y;
                    numericUpDown_Hours_Ones_Count.Value = Watch_Face.Time.Hours.Ones.ImagesCount;
                    //comboBox_Hours_Ones_Image.Text = Watch_Face.Time.Hours.Ones.ImageIndex.ToString();
                    checkBoxSetText(comboBox_Hours_Ones_Image, Watch_Face.Time.Hours.Ones.ImageIndex);
                }
                else checkBox_Hours.Checked = false;

                if (Watch_Face.Time.Minutes != null)
                {
                    checkBox_Minutes.Checked = true;
                    numericUpDown_Min_Tens_X.Value = Watch_Face.Time.Minutes.Tens.X;
                    numericUpDown_Min_Tens_Y.Value = Watch_Face.Time.Minutes.Tens.Y;
                    numericUpDown_Min_Tens_Count.Value = Watch_Face.Time.Minutes.Tens.ImagesCount;
                    //comboBox_Min_Tens_Image.Text = Watch_Face.Time.Minutes.Tens.ImageIndex.ToString();
                    checkBoxSetText(comboBox_Min_Tens_Image, Watch_Face.Time.Minutes.Tens.ImageIndex);

                    numericUpDown_Min_Ones_X.Value = Watch_Face.Time.Minutes.Ones.X;
                    numericUpDown_Min_Ones_Y.Value = Watch_Face.Time.Minutes.Ones.Y;
                    numericUpDown_Min_Ones_Count.Value = Watch_Face.Time.Minutes.Ones.ImagesCount;
                    //comboBox_Min_Ones_Image.Text = Watch_Face.Time.Minutes.Ones.ImageIndex.ToString();
                    checkBoxSetText(comboBox_Min_Ones_Image, Watch_Face.Time.Minutes.Ones.ImageIndex);
                }
                else checkBox_Minutes.Checked = false;

                if (Watch_Face.Time.Seconds != null)
                {
                    checkBox_Seconds.Checked = true;
                    numericUpDown_Sec_Tens_X.Value = Watch_Face.Time.Seconds.Tens.X;
                    numericUpDown_Sec_Tens_Y.Value = Watch_Face.Time.Seconds.Tens.Y;
                    numericUpDown_Sec_Tens_Count.Value = Watch_Face.Time.Seconds.Tens.ImagesCount;
                    //comboBox_Sec_Tens_Image.Text = Watch_Face.Time.Seconds.Tens.ImageIndex.ToString();
                    checkBoxSetText(comboBox_Sec_Tens_Image, Watch_Face.Time.Seconds.Tens.ImageIndex);

                    numericUpDown_Sec_Ones_X.Value = Watch_Face.Time.Seconds.Ones.X;
                    numericUpDown_Sec_Ones_Y.Value = Watch_Face.Time.Seconds.Ones.Y;
                    numericUpDown_Sec_Ones_Count.Value = Watch_Face.Time.Seconds.Ones.ImagesCount;
                    //comboBox_Sec_Ones_Image.Text = Watch_Face.Time.Seconds.Ones.ImageIndex.ToString();
                    checkBoxSetText(comboBox_Sec_Ones_Image, Watch_Face.Time.Seconds.Ones.ImageIndex);
                }
                else checkBox_Seconds.Checked = false;

                if (Watch_Face.Time.Delimiter != null)
                {
                    checkBox_Delimiter.Checked = true;
                    numericUpDown_Delimiter_X.Value = Watch_Face.Time.Delimiter.X;
                    numericUpDown_Delimiter_Y.Value = Watch_Face.Time.Delimiter.Y;
                    //comboBox_Delimiter_Image.Text = Watch_Face.Time.Delimiter.ImageIndex.ToString();
                    checkBoxSetText(comboBox_Delimiter_Image, Watch_Face.Time.Delimiter.ImageIndex);
                }
                else checkBox_Delimiter.Checked = false;

                if (Watch_Face.Time.AmPm != null)
                {
                    checkBox_AmPm.Checked = true;
                    numericUpDown_AmPm_X.Value = Watch_Face.Time.AmPm.X;
                    numericUpDown_AmPm_Y.Value = Watch_Face.Time.AmPm.Y;
                    if (Watch_Face.Time.AmPm.ImageIndexAMCN > 0)
                        //comboBox_Image_Am.Text = Watch_Face.Time.AmPm.ImageIndexAMCN.ToString();
                        checkBoxSetText(comboBox_Image_Am, Watch_Face.Time.AmPm.ImageIndexAMCN);
                    if (Watch_Face.Time.AmPm.ImageIndexAMEN > 0)
                        //comboBox_Image_Am.Text = Watch_Face.Time.AmPm.ImageIndexAMEN.ToString();
                        checkBoxSetText(comboBox_Image_Am, Watch_Face.Time.AmPm.ImageIndexAMEN);
                    if (Watch_Face.Time.AmPm.ImageIndexPMCN > 0)
                        //comboBox_Image_Pm.Text = Watch_Face.Time.AmPm.ImageIndexPMCN.ToString();
                        checkBoxSetText(comboBox_Image_Pm, Watch_Face.Time.AmPm.ImageIndexPMCN);
                    if (Watch_Face.Time.AmPm.ImageIndexPMEN > 0)
                        //comboBox_Image_Pm.Text = Watch_Face.Time.AmPm.ImageIndexPMEN.ToString();
                        checkBoxSetText(comboBox_Image_Pm, Watch_Face.Time.AmPm.ImageIndexPMEN);
                }
                else checkBox_AmPm.Checked = false;
            }
            else
            {
                checkBox_Time.Checked = false;
                checkBox_Hours.Checked = false;
                checkBox_Minutes.Checked = false;
                checkBox_Seconds.Checked = false;
                checkBox_Delimiter.Checked = false;
                checkBox_AmPm.Checked = false;
            }
            #endregion

            #region Date
            if (Watch_Face.Date != null)
            {
                checkBox_Date.Checked = true;
                if (Watch_Face.Date.WeekDay != null)
                {
                    checkBox_WeekDay.Checked = true;
                    numericUpDown_WeekDay_X.Value = Watch_Face.Date.WeekDay.X;
                    numericUpDown_WeekDay_Y.Value = Watch_Face.Date.WeekDay.Y;
                    numericUpDown_WeekDay_Count.Value = Watch_Face.Date.WeekDay.ImagesCount;
                    //comboBox_WeekDay_Image.Text = Watch_Face.Date.WeekDay.ImageIndex.ToString();
                    checkBoxSetText(comboBox_WeekDay_Image, Watch_Face.Date.WeekDay.ImageIndex);
                }
                else checkBox_WeekDay.Checked = false;

                if (Watch_Face.Date.MonthAndDay != null)
                {
                    checkBox_TwoDigitsDay.Checked = Watch_Face.Date.MonthAndDay.TwoDigitsDay;
                    checkBox_TwoDigitsMonth.Checked = Watch_Face.Date.MonthAndDay.TwoDigitsMonth;

                    if ((Watch_Face.Date.MonthAndDay.OneLine != null) && (Watch_Face.Date.MonthAndDay.OneLine.Number != null))
                    {
                        checkBox_OneLine.Checked = true;
                        numericUpDown_OneLine_StartCorner_X.Value = Watch_Face.Date.MonthAndDay.OneLine.Number.TopLeftX;
                        numericUpDown_OneLine_StartCorner_Y.Value = Watch_Face.Date.MonthAndDay.OneLine.Number.TopLeftY;
                        numericUpDown_OneLine_EndCorner_X.Value = Watch_Face.Date.MonthAndDay.OneLine.Number.BottomRightX;
                        numericUpDown_OneLine_EndCorner_Y.Value = Watch_Face.Date.MonthAndDay.OneLine.Number.BottomRightY;

                        numericUpDown_OneLine_Spacing.Value = Watch_Face.Date.MonthAndDay.OneLine.Number.Spacing;
                        numericUpDown_OneLine_Count.Value = Watch_Face.Date.MonthAndDay.OneLine.Number.ImagesCount;
                        //comboBox_OneLine_Image.Text = Watch_Face.Date.MonthAndDay.OneLine.Number.ImageIndex.ToString();
                        checkBoxSetText(comboBox_OneLine_Image, Watch_Face.Date.MonthAndDay.OneLine.Number.ImageIndex);
                        //comboBox_OneLine_Delimiter.Text = Watch_Face.Date.MonthAndDay.OneLine.DelimiterImageIndex.ToString();
                        if (Watch_Face.Date.MonthAndDay.OneLine.DelimiterImageIndex != null)
                            checkBoxSetText(comboBox_OneLine_Delimiter, (long)Watch_Face.Date.MonthAndDay.OneLine.DelimiterImageIndex);
                        string Alignment = AlignmentToString(Watch_Face.Date.MonthAndDay.OneLine.Number.Alignment);
                        comboBox_OneLine_Alignment.Text = Alignment;
                    }
                    else checkBox_OneLine.Checked = false;

                    if (Watch_Face.Date.MonthAndDay.Separate != null)
                    {
                        if (Watch_Face.Date.MonthAndDay.Separate.Day != null)
                        {
                            checkBox_MonthAndDayD.Checked = true;
                            numericUpDown_MonthAndDayD_StartCorner_X.Value = Watch_Face.Date.MonthAndDay.Separate.Day.TopLeftX;
                            numericUpDown_MonthAndDayD_StartCorner_Y.Value = Watch_Face.Date.MonthAndDay.Separate.Day.TopLeftY;
                            numericUpDown_MonthAndDayD_EndCorner_X.Value = Watch_Face.Date.MonthAndDay.Separate.Day.BottomRightX;
                            numericUpDown_MonthAndDayD_EndCorner_Y.Value = Watch_Face.Date.MonthAndDay.Separate.Day.BottomRightY;

                            numericUpDown_MonthAndDayD_Spacing.Value = Watch_Face.Date.MonthAndDay.Separate.Day.Spacing;
                            numericUpDown_MonthAndDayD_Count.Value = Watch_Face.Date.MonthAndDay.Separate.Day.ImagesCount;
                            //comboBox_MonthAndDayD_Image.Text = Watch_Face.Date.MonthAndDay.Separate.Day.ImageIndex.ToString();
                            checkBoxSetText(comboBox_MonthAndDayD_Image, Watch_Face.Date.MonthAndDay.Separate.Day.ImageIndex);
                            string Alignment = AlignmentToString(Watch_Face.Date.MonthAndDay.Separate.Day.Alignment);
                            comboBox_MonthAndDayD_Alignment.Text = Alignment;
                        }
                        else checkBox_MonthAndDayD.Checked = false;

                        if (Watch_Face.Date.MonthAndDay.Separate.Month != null)
                        {
                            checkBox_MonthAndDayM.Checked = true;
                            numericUpDown_MonthAndDayM_StartCorner_X.Value = Watch_Face.Date.MonthAndDay.Separate.Month.TopLeftX;
                            numericUpDown_MonthAndDayM_StartCorner_Y.Value = Watch_Face.Date.MonthAndDay.Separate.Month.TopLeftY;
                            numericUpDown_MonthAndDayM_EndCorner_X.Value = Watch_Face.Date.MonthAndDay.Separate.Month.BottomRightX;
                            numericUpDown_MonthAndDayM_EndCorner_Y.Value = Watch_Face.Date.MonthAndDay.Separate.Month.BottomRightY;

                            numericUpDown_MonthAndDayM_Spacing.Value = Watch_Face.Date.MonthAndDay.Separate.Month.Spacing;
                            numericUpDown_MonthAndDayM_Count.Value = Watch_Face.Date.MonthAndDay.Separate.Month.ImagesCount;
                            //comboBox_MonthAndDayM_Image.Text = Watch_Face.Date.MonthAndDay.Separate.Month.ImageIndex.ToString();
                            checkBoxSetText(comboBox_MonthAndDayM_Image, Watch_Face.Date.MonthAndDay.Separate.Month.ImageIndex);
                            string Alignment = AlignmentToString(Watch_Face.Date.MonthAndDay.Separate.Month.Alignment);
                            comboBox_MonthAndDayM_Alignment.Text = Alignment;
                        }
                        else checkBox_MonthAndDayM.Checked = false;

                        if (Watch_Face.Date.MonthAndDay.Separate.MonthName != null)
                        {
                            checkBox_MonthName.Checked = true;
                            numericUpDown_MonthName_X.Value = Watch_Face.Date.MonthAndDay.Separate.MonthName.X;
                            numericUpDown_MonthName_Y.Value = Watch_Face.Date.MonthAndDay.Separate.MonthName.Y;

                            numericUpDown_MonthName_Count.Value = Watch_Face.Date.MonthAndDay.Separate.MonthName.ImagesCount;
                            //comboBox_MonthName_Image.Text = Watch_Face.Date.MonthAndDay.Separate.MonthName.ImageIndex.ToString();
                            checkBoxSetText(comboBox_MonthName_Image, Watch_Face.Date.MonthAndDay.Separate.MonthName.ImageIndex);
                        }
                        else checkBox_MonthName.Checked = false;
                    }
                    else
                    {
                        checkBox_MonthAndDayD.Checked = false;
                        checkBox_MonthAndDayM.Checked = false;
                        checkBox_MonthName.Checked = false;
                    }
                    
                }

                if (Watch_Face.Date.Year != null)
                {
                    if ((Watch_Face.Date.Year.OneLine != null) && (Watch_Face.Date.Year.OneLine.Number != null))
                    {
                        checkBox_Year.Checked = true;
                        numericUpDown_Year_StartCorner_X.Value = Watch_Face.Date.Year.OneLine.Number.TopLeftX;
                        numericUpDown_Year_StartCorner_Y.Value = Watch_Face.Date.Year.OneLine.Number.TopLeftY;
                        numericUpDown_Year_EndCorner_X.Value = Watch_Face.Date.Year.OneLine.Number.BottomRightX;
                        numericUpDown_Year_EndCorner_Y.Value = Watch_Face.Date.Year.OneLine.Number.BottomRightY;

                        numericUpDown_Year_Spacing.Value = Watch_Face.Date.Year.OneLine.Number.Spacing;
                        numericUpDown_Year_Count.Value = Watch_Face.Date.Year.OneLine.Number.ImagesCount;
                        checkBoxSetText(comboBox_Year_Image, Watch_Face.Date.Year.OneLine.Number.ImageIndex);
                        if (Watch_Face.Date.Year.OneLine.DelimiterImageIndex != null)
                        checkBoxSetText(comboBox_Year_Delimiter, (long)Watch_Face.Date.Year.OneLine.DelimiterImageIndex);
                        string Alignment = AlignmentToString(Watch_Face.Date.Year.OneLine.Number.Alignment);
                        comboBox_Year_Alignment.Text = Alignment;
                    }
                    else checkBox_Year.Checked = false;
                }

            }
            else
            {
                checkBox_Date.Checked = false;
                checkBox_WeekDay.Checked = false;
                checkBox_OneLine.Checked = false;
                checkBox_MonthAndDayD.Checked = false;
                checkBox_MonthAndDayM.Checked = false;
                checkBox_MonthName.Checked = false;
                checkBox_Year.Checked = false;
            }
            #endregion

            #region StepsProgress
            if ((Watch_Face.StepsProgress != null) && (Watch_Face.StepsProgress.Circle != null))
            {
                checkBox_StepsProgress.Checked = true;
                numericUpDown_StepsProgress_Center_X.Value = Watch_Face.StepsProgress.Circle.CenterX;
                numericUpDown_StepsProgress_Center_Y.Value = Watch_Face.StepsProgress.Circle.CenterY;
                numericUpDown_StepsProgress_Radius_X.Value = Watch_Face.StepsProgress.Circle.RadiusX;
                numericUpDown_StepsProgress_Radius_Y.Value = Watch_Face.StepsProgress.Circle.RadiusY;

                numericUpDown_StepsProgress_StartAngle.Value = Watch_Face.StepsProgress.Circle.StartAngle;
                numericUpDown_StepsProgress_EndAngle.Value = Watch_Face.StepsProgress.Circle.EndAngle;
                numericUpDown_StepsProgress_Width.Value = Watch_Face.StepsProgress.Circle.Width;

                Color color = ColorTranslator.FromHtml(Watch_Face.StepsProgress.Circle.Color);
                Color new_color = Color.FromArgb(255, color.R, color.G, color.B);
                comboBox_StepsProgress_Color.BackColor = new_color;
                colorDialog1.Color = new_color;
                switch (Watch_Face.StepsProgress.Circle.Flatness)
                {
                    case 90:
                        comboBox_StepsProgress_Flatness.Text = "Треугольное";
                        break;
                    case 180:
                        comboBox_StepsProgress_Flatness.Text = "Плоское";
                        break;
                    default:
                        comboBox_StepsProgress_Flatness.Text = "Круглое";
                        break;
                }
            }
            else checkBox_StepsProgress.Checked = false;
            #endregion

            #region Activity
            if (Watch_Face.Activity != null)
            {
                checkBox_Activity.Checked = true;
                if (Watch_Face.Activity.StepsGoal != null)
                {
                    checkBox_ActivityGoal.Checked = true;
                    numericUpDown_ActivityGoal_StartCorner_X.Value = Watch_Face.Activity.StepsGoal.TopLeftX;
                    numericUpDown_ActivityGoal_StartCorner_Y.Value = Watch_Face.Activity.StepsGoal.TopLeftY;
                    numericUpDown_ActivityGoal_EndCorner_X.Value = Watch_Face.Activity.StepsGoal.BottomRightX;
                    numericUpDown_ActivityGoal_EndCorner_Y.Value = Watch_Face.Activity.StepsGoal.BottomRightY;

                    //comboBox_ActivityGoal_Image.Text = Watch_Face.Activity.StepsGoal.ImageIndex.ToString();
                    checkBoxSetText(comboBox_ActivityGoal_Image, Watch_Face.Activity.StepsGoal.ImageIndex);
                    numericUpDown_ActivityGoal_Count.Value = Watch_Face.Activity.StepsGoal.ImagesCount;
                    numericUpDown_ActivityGoal_Spacing.Value = Watch_Face.Activity.StepsGoal.Spacing;
                    string Alignment = AlignmentToString(Watch_Face.Activity.StepsGoal.Alignment);
                    comboBox_ActivityGoal_Alignment.Text = Alignment;
                }
                else checkBox_ActivityGoal.Checked = false;

                if ((Watch_Face.Activity.Steps != null) && (Watch_Face.Activity.Steps.Step != null))
                {
                    checkBox_ActivitySteps.Checked = true;
                    numericUpDown_ActivitySteps_StartCorner_X.Value = Watch_Face.Activity.Steps.Step.TopLeftX;
                    numericUpDown_ActivitySteps_StartCorner_Y.Value = Watch_Face.Activity.Steps.Step.TopLeftY;
                    numericUpDown_ActivitySteps_EndCorner_X.Value = Watch_Face.Activity.Steps.Step.BottomRightX;
                    numericUpDown_ActivitySteps_EndCorner_Y.Value = Watch_Face.Activity.Steps.Step.BottomRightY;

                    //comboBox_ActivitySteps_Image.Text = Watch_Face.Activity.Steps.Step.ImageIndex.ToString();
                    checkBoxSetText(comboBox_ActivitySteps_Image, Watch_Face.Activity.Steps.Step.ImageIndex);
                    numericUpDown_ActivitySteps_Count.Value = Watch_Face.Activity.Steps.Step.ImagesCount;
                    numericUpDown_ActivitySteps_Spacing.Value = Watch_Face.Activity.Steps.Step.Spacing;
                    string Alignment = AlignmentToString(Watch_Face.Activity.Steps.Step.Alignment);
                    comboBox_ActivitySteps_Alignment.Text = Alignment;
                }
                else checkBox_ActivitySteps.Checked = false;

                if ((Watch_Face.Activity.Distance != null) && (Watch_Face.Activity.Distance.Number != null))
                {
                    checkBox_ActivityDistance.Checked = true;
                    numericUpDown_ActivityDistance_StartCorner_X.Value = Watch_Face.Activity.Distance.Number.TopLeftX;
                    numericUpDown_ActivityDistance_StartCorner_Y.Value = Watch_Face.Activity.Distance.Number.TopLeftY;
                    numericUpDown_ActivityDistance_EndCorner_X.Value = Watch_Face.Activity.Distance.Number.BottomRightX;
                    numericUpDown_ActivityDistance_EndCorner_Y.Value = Watch_Face.Activity.Distance.Number.BottomRightY;

                    //comboBox_ActivityDistance_Image.Text = Watch_Face.Activity.Distance.Number.ImageIndex.ToString();
                    checkBoxSetText(comboBox_ActivityDistance_Image, Watch_Face.Activity.Distance.Number.ImageIndex);
                    numericUpDown_ActivityDistance_Count.Value = Watch_Face.Activity.Distance.Number.ImagesCount;
                    numericUpDown_ActivityDistance_Spacing.Value = Watch_Face.Activity.Distance.Number.Spacing;
                    string Alignment = AlignmentToString(Watch_Face.Activity.Distance.Number.Alignment);
                    comboBox_ActivityDistance_Alignment.Text = Alignment;

                    //comboBox_ActivityDistance_Suffix.Text = Watch_Face.Activity.Distance.SuffixImageIndex.ToString();
                    if (Watch_Face.Activity.Distance.SuffixImageIndex != null)
                        checkBoxSetText(comboBox_ActivityDistance_Suffix, (long)Watch_Face.Activity.Distance.SuffixImageIndex);
                    //comboBox_ActivityDistance_Decimal.Text = Watch_Face.Activity.Distance.DecimalPointImageIndex.ToString();
                    if (Watch_Face.Activity.Distance.DecimalPointImageIndex != null)
                        checkBoxSetText(comboBox_ActivityDistance_Decimal, (long)Watch_Face.Activity.Distance.DecimalPointImageIndex);
                }
                else checkBox_ActivityDistance.Checked = false;

                if (Watch_Face.Activity.Pulse != null)
                {
                    checkBox_ActivityPuls.Checked = true;
                    numericUpDown_ActivityPuls_StartCorner_X.Value = Watch_Face.Activity.Pulse.TopLeftX;
                    numericUpDown_ActivityPuls_StartCorner_Y.Value = Watch_Face.Activity.Pulse.TopLeftY;
                    numericUpDown_ActivityPuls_EndCorner_X.Value = Watch_Face.Activity.Pulse.BottomRightX;
                    numericUpDown_ActivityPuls_EndCorner_Y.Value = Watch_Face.Activity.Pulse.BottomRightY;

                    //comboBox_ActivityPuls_Image.Text = Watch_Face.Activity.Pulse.ImageIndex.ToString();
                    checkBoxSetText(comboBox_ActivityPuls_Image, Watch_Face.Activity.Pulse.ImageIndex);
                    numericUpDown_ActivityPuls_Count.Value = Watch_Face.Activity.Pulse.ImagesCount;
                    numericUpDown_ActivityPuls_Spacing.Value = Watch_Face.Activity.Pulse.Spacing;
                    string Alignment = AlignmentToString(Watch_Face.Activity.Pulse.Alignment);
                    comboBox_ActivityPuls_Alignment.Text = Alignment;
                }
                else checkBox_ActivityPuls.Checked = false;

                if (Watch_Face.Activity.Calories != null)
                {
                    checkBox_ActivityCalories.Checked = true;
                    numericUpDown_ActivityCalories_StartCorner_X.Value = Watch_Face.Activity.Calories.TopLeftX;
                    numericUpDown_ActivityCalories_StartCorner_Y.Value = Watch_Face.Activity.Calories.TopLeftY;
                    numericUpDown_ActivityCalories_EndCorner_X.Value = Watch_Face.Activity.Calories.BottomRightX;
                    numericUpDown_ActivityCalories_EndCorner_Y.Value = Watch_Face.Activity.Calories.BottomRightY;

                    //comboBox_ActivityCalories_Image.Text = Watch_Face.Activity.Calories.ImageIndex.ToString();
                    checkBoxSetText(comboBox_ActivityCalories_Image, Watch_Face.Activity.Calories.ImageIndex);
                    numericUpDown_ActivityCalories_Count.Value = Watch_Face.Activity.Calories.ImagesCount;
                    numericUpDown_ActivityCalories_Spacing.Value = Watch_Face.Activity.Calories.Spacing;
                    string Alignment = AlignmentToString(Watch_Face.Activity.Calories.Alignment);
                    comboBox_ActivityCalories_Alignment.Text = Alignment;
                }
                else checkBox_ActivityCalories.Checked = false;

                if (Watch_Face.Activity.StarImage != null)
                {
                    checkBox_ActivityStar.Checked = true;
                    numericUpDown_ActivityStar_X.Value = Watch_Face.Activity.StarImage.X;
                    numericUpDown_ActivityStar_Y.Value = Watch_Face.Activity.StarImage.Y;
                    //comboBox_ActivityStar_Image.Text = Watch_Face.Activity.StarImage.ImageIndex.ToString();
                    checkBoxSetText(comboBox_ActivityStar_Image, Watch_Face.Activity.StarImage.ImageIndex);
                }
                else checkBox_ActivityStar.Checked = false;

            }
            else
            {
                checkBox_Activity.Checked = false;
                checkBox_ActivityGoal.Checked = false;
                checkBox_ActivitySteps.Checked = false;
                checkBox_ActivityDistance.Checked = false;
                checkBox_ActivityPuls.Checked = false;
                checkBox_ActivityCalories.Checked = false;
                checkBox_ActivityStar.Checked = false;
            }
            #endregion

            #region Status
            if (Watch_Face.Status != null)
            {
                if (Watch_Face.Status.Bluetooth != null)
                {
                    checkBox_Bluetooth.Checked = true;
                    if (Watch_Face.Status.Bluetooth.Coordinates != null)
                    {
                        numericUpDown_Bluetooth_X.Value = Watch_Face.Status.Bluetooth.Coordinates.X;
                        numericUpDown_Bluetooth_Y.Value = Watch_Face.Status.Bluetooth.Coordinates.Y;
                    }
                    if (Watch_Face.Status.Bluetooth.ImageIndexOn != null)
                        //comboBox_Bluetooth_On.Text = Watch_Face.Status.Bluetooth.ImageIndexOn.Value.ToString();
                        checkBoxSetText(comboBox_Bluetooth_On, (long)Watch_Face.Status.Bluetooth.ImageIndexOn);
                    if (Watch_Face.Status.Bluetooth.ImageIndexOff != null)
                        //comboBox_Bluetooth_Off.Text = Watch_Face.Status.Bluetooth.ImageIndexOff.Value.ToString();
                        checkBoxSetText(comboBox_Bluetooth_Off, (long)Watch_Face.Status.Bluetooth.ImageIndexOff);
                }
                else checkBox_Bluetooth.Checked = false;

                if (Watch_Face.Status.Alarm != null)
                {
                    checkBox_Alarm.Checked = true;
                    if (Watch_Face.Status.Alarm.Coordinates != null)
                    {
                        numericUpDown_Alarm_X.Value = Watch_Face.Status.Alarm.Coordinates.X;
                        numericUpDown_Alarm_Y.Value = Watch_Face.Status.Alarm.Coordinates.Y;
                    }
                    if (Watch_Face.Status.Alarm.ImageIndexOn != null)
                        //comboBox_Alarm_On.Text = Watch_Face.Status.Alarm.ImageIndexOn.Value.ToString();
                        checkBoxSetText(comboBox_Alarm_On, (long)Watch_Face.Status.Alarm.ImageIndexOn);
                    if (Watch_Face.Status.Alarm.ImageIndexOff != null)
                        //comboBox_Alarm_Off.Text = Watch_Face.Status.Alarm.ImageIndexOff.Value.ToString();
                        checkBoxSetText(comboBox_ActivityStar_Image, (long)Watch_Face.Status.Alarm.ImageIndexOff);
                }
                else checkBox_Alarm.Checked = false;

                if (Watch_Face.Status.Lock != null)
                {
                    checkBox_Lock.Checked = true;
                    if (Watch_Face.Status.Lock.Coordinates != null)
                    {
                        numericUpDown_Lock_X.Value = Watch_Face.Status.Lock.Coordinates.X;
                        numericUpDown_Lock_Y.Value = Watch_Face.Status.Lock.Coordinates.Y;
                    }
                    if (Watch_Face.Status.Lock.ImageIndexOn != null)
                        //comboBox_Lock_On.Text = Watch_Face.Status.Lock.ImageIndexOn.Value.ToString();
                        checkBoxSetText(comboBox_Lock_On, (long)Watch_Face.Status.Lock.ImageIndexOn);
                    if (Watch_Face.Status.Lock.ImageIndexOff != null)
                        //comboBox_Lock_Off.Text = Watch_Face.Status.Lock.ImageIndexOff.Value.ToString();
                        checkBoxSetText(comboBox_Lock_Off, (long)Watch_Face.Status.Lock.ImageIndexOff);
                }
                else checkBox_Lock.Checked = false;

                if (Watch_Face.Status.DoNotDisturb != null)
                {
                    checkBox_DND.Checked = true;
                    if (Watch_Face.Status.DoNotDisturb.Coordinates != null)
                    {
                        numericUpDown_DND_X.Value = Watch_Face.Status.DoNotDisturb.Coordinates.X;
                        numericUpDown_DND_Y.Value = Watch_Face.Status.DoNotDisturb.Coordinates.Y;
                    }
                    if (Watch_Face.Status.DoNotDisturb.ImageIndexOn != null)
                        //comboBox_DND_On.Text = Watch_Face.Status.DoNotDisturb.ImageIndexOn.Value.ToString();
                        checkBoxSetText(comboBox_DND_On, (long)Watch_Face.Status.DoNotDisturb.ImageIndexOn);
                    if (Watch_Face.Status.DoNotDisturb.ImageIndexOff != null)
                        //comboBox_DND_Off.Text = Watch_Face.Status.DoNotDisturb.ImageIndexOff.Value.ToString();
                        checkBoxSetText(comboBox_DND_Off, (long)Watch_Face.Status.DoNotDisturb.ImageIndexOff);
                }
                else checkBox_DND.Checked = false;
            }
            #endregion

            #region Battery
            if (Watch_Face.Battery != null)
            {
                checkBox_Battery.Checked = true;
                if (Watch_Face.Battery.Text != null)
                {
                    checkBox_Battery_Text.Checked = true;
                    numericUpDown_Battery_Text_StartCorner_X.Value = Watch_Face.Battery.Text.TopLeftX;
                    numericUpDown_Battery_Text_StartCorner_Y.Value = Watch_Face.Battery.Text.TopLeftY;
                    numericUpDown_Battery_Text_EndCorner_X.Value = Watch_Face.Battery.Text.BottomRightX;
                    numericUpDown_Battery_Text_EndCorner_Y.Value = Watch_Face.Battery.Text.BottomRightY;
                    numericUpDown_Battery_Text_Spacing.Value = Watch_Face.Battery.Text.Spacing;
                    numericUpDown_Battery_Text_Count.Value = Watch_Face.Battery.Text.ImagesCount;
                    //comboBox_Battery_Text_Image.Text = Watch_Face.Battery.Text.ImageIndex.ToString();
                    checkBoxSetText(comboBox_Battery_Text_Image, Watch_Face.Battery.Text.ImageIndex);
                    string Alignment = AlignmentToString(Watch_Face.Battery.Text.Alignment);
                    comboBox_Battery_Text_Alignment.Text = Alignment;
                }
                else checkBox_Battery_Text.Checked = false;

                if (Watch_Face.Battery.Percent != null)
                {
                    checkBox_Battery_Percent.Checked = true;
                    numericUpDown_Battery_Percent_X.Value = Watch_Face.Battery.Percent.X;
                    numericUpDown_Battery_Percent_Y.Value = Watch_Face.Battery.Percent.Y;
                    //comboBox_Battery_Percent_Image.Text = Watch_Face.Battery.Percent.ImageIndex.ToString();
                    checkBoxSetText(comboBox_Battery_Percent_Image, Watch_Face.Battery.Percent.ImageIndex);
                }
                else checkBox_Battery_Percent.Checked = false;

                if (Watch_Face.Battery.Images != null)
                {
                    checkBox_Battery_Img.Checked = true;
                    numericUpDown_Battery_Img_X.Value = Watch_Face.Battery.Images.X;
                    numericUpDown_Battery_Img_Y.Value = Watch_Face.Battery.Images.Y;
                    numericUpDown_Battery_Img_Count.Value = Watch_Face.Battery.Images.ImagesCount;
                    //comboBox_Battery_Img_Image.Text = Watch_Face.Battery.Images.ImageIndex.ToString();
                    checkBoxSetText(comboBox_Battery_Img_Image, Watch_Face.Battery.Images.ImageIndex);
                }
                else checkBox_Battery_Img.Checked = false;

                if (Watch_Face.Battery.Scale != null)
                {
                    checkBox_Battery_Scale.Checked = true;
                    numericUpDown_Battery_Scale_Center_X.Value = Watch_Face.Battery.Scale.CenterX;
                    numericUpDown_Battery_Scale_Center_Y.Value = Watch_Face.Battery.Scale.CenterY;
                    numericUpDown_Battery_Scale_Radius_X.Value = Watch_Face.Battery.Scale.RadiusX;
                    numericUpDown_Battery_Scale_Radius_Y.Value = Watch_Face.Battery.Scale.RadiusY;

                    numericUpDown_Battery_Scale_StartAngle.Value = Watch_Face.Battery.Scale.StartAngle;
                    numericUpDown_Battery_Scale_EndAngle.Value = Watch_Face.Battery.Scale.EndAngle;
                    numericUpDown_Battery_Scale_Width.Value = Watch_Face.Battery.Scale.Width;

                    Color color = ColorTranslator.FromHtml(Watch_Face.Battery.Scale.Color);
                    Color new_color = Color.FromArgb(255, color.R, color.G, color.B);
                    comboBox_Battery_Scale_Color.BackColor = new_color;
                    colorDialog2.Color = new_color;

                    switch (Watch_Face.Battery.Scale.Flatness)
                    {
                        case 90:
                            comboBox_Battery_Flatness.Text = "Треугольное";
                            break;
                        case 180:
                            comboBox_Battery_Flatness.Text = "Плоское";
                            break;
                        default:
                            comboBox_Battery_Flatness.Text = "Круглое";
                            break;
                    }
                }
                else checkBox_Battery_Scale.Checked = false;
            }
            else
            {
                checkBox_Battery.Checked = false;
                checkBox_Battery_Text.Checked = false;
                checkBox_Battery_Img.Checked = false;
                checkBox_Battery_Scale.Checked = false;
            }
            #endregion

            #region AnalogDialFace
            if (Watch_Face.AnalogDialFace != null)
            {
                checkBox_AnalogClock.Checked = true;
                if ((Watch_Face.AnalogDialFace.Hours != null) && (Watch_Face.AnalogDialFace.Hours.Image != null))
                {
                    checkBox_AnalogClock_Hour.Checked = true;
                    numericUpDown_AnalogClock_Hour_X.Value = Watch_Face.AnalogDialFace.Hours.Image.X;
                    numericUpDown_AnalogClock_Hour_Y.Value = Watch_Face.AnalogDialFace.Hours.Image.Y;
                    //comboBox_AnalogClock_Hour_Image.Text = Watch_Face.AnalogDialFace.Hours.Image.ImageIndex.ToString();
                    checkBoxSetText(comboBox_AnalogClock_Hour_Image, Watch_Face.AnalogDialFace.Hours.Image.ImageIndex);

                    if (Watch_Face.AnalogDialFace.Hours.CenterOffset != null)
                    {
                        numericUpDown_AnalogClock_Hour_Offset_X.Value = Watch_Face.AnalogDialFace.Hours.CenterOffset.X;
                        numericUpDown_AnalogClock_Hour_Offset_Y.Value = Watch_Face.AnalogDialFace.Hours.CenterOffset.Y;

                    }
                }
                else checkBox_AnalogClock_Hour.Checked = false;

                if ((Watch_Face.AnalogDialFace.Minutes != null) && (Watch_Face.AnalogDialFace.Minutes.Image != null))
                {
                    checkBox_AnalogClock_Min.Checked = true;
                    numericUpDown_AnalogClock_Min_X.Value = Watch_Face.AnalogDialFace.Minutes.Image.X;
                    numericUpDown_AnalogClock_Min_Y.Value = Watch_Face.AnalogDialFace.Minutes.Image.Y;
                    //comboBox_AnalogClock_Min_Image.Text = Watch_Face.AnalogDialFace.Minutes.Image.ImageIndex.ToString();
                    checkBoxSetText(comboBox_AnalogClock_Min_Image, Watch_Face.AnalogDialFace.Minutes.Image.ImageIndex);

                    if (Watch_Face.AnalogDialFace.Minutes.CenterOffset != null)
                    {
                        numericUpDown_AnalogClock_Min_Offset_X.Value = Watch_Face.AnalogDialFace.Minutes.CenterOffset.X;
                        numericUpDown_AnalogClock_Min_Offset_Y.Value = Watch_Face.AnalogDialFace.Minutes.CenterOffset.Y;

                    }
                }
                else checkBox_AnalogClock_Min.Checked = false;

                if ((Watch_Face.AnalogDialFace.Seconds != null) && (Watch_Face.AnalogDialFace.Seconds.Image != null))
                {
                    checkBox_AnalogClock_Sec.Checked = true;
                    numericUpDown_AnalogClock_Sec_X.Value = Watch_Face.AnalogDialFace.Seconds.Image.X;
                    numericUpDown_AnalogClock_Sec_Y.Value = Watch_Face.AnalogDialFace.Seconds.Image.Y;
                    //comboBox_AnalogClock_Sec_Image.Text = Watch_Face.AnalogDialFace.Seconds.Image.ImageIndex.ToString();
                    checkBoxSetText(comboBox_AnalogClock_Sec_Image, Watch_Face.AnalogDialFace.Seconds.Image.ImageIndex);

                    if (Watch_Face.AnalogDialFace.Seconds.CenterOffset != null)
                    {
                        numericUpDown_AnalogClock_Sec_Offset_X.Value = Watch_Face.AnalogDialFace.Seconds.CenterOffset.X;
                        numericUpDown_AnalogClock_Sec_Offset_Y.Value = Watch_Face.AnalogDialFace.Seconds.CenterOffset.Y;

                    }
                }
                else checkBox_AnalogClock_Sec.Checked = false;

                if (Watch_Face.AnalogDialFace.HourCenterImage != null)
                {
                    checkBox_HourCenterImage.Checked = true;
                    numericUpDown_HourCenterImage_X.Value = Watch_Face.AnalogDialFace.HourCenterImage.X;
                    numericUpDown_HourCenterImage_Y.Value = Watch_Face.AnalogDialFace.HourCenterImage.Y;
                    //comboBox_HourCenterImage_Image.Text = Watch_Face.AnalogDialFace.HourCenterImage.ImageIndex.ToString();
                    checkBoxSetText(comboBox_HourCenterImage_Image, Watch_Face.AnalogDialFace.HourCenterImage.ImageIndex);
                }
                else checkBox_HourCenterImage.Checked = false;

                if (Watch_Face.AnalogDialFace.MinCenterImage != null)
                {
                    checkBox_MinCenterImage.Checked = true;
                    numericUpDown_MinCenterImage_X.Value = Watch_Face.AnalogDialFace.MinCenterImage.X;
                    numericUpDown_MinCenterImage_Y.Value = Watch_Face.AnalogDialFace.MinCenterImage.Y;
                    //comboBox_MinCenterImage_Image.Text = Watch_Face.AnalogDialFace.MinCenterImage.ImageIndex.ToString();
                    checkBoxSetText(comboBox_MinCenterImage_Image, Watch_Face.AnalogDialFace.MinCenterImage.ImageIndex);
                }
                else checkBox_MinCenterImage.Checked = false;

                if (Watch_Face.AnalogDialFace.SecCenterImage != null)
                {
                    checkBox_SecCenterImage.Checked = true;
                    numericUpDown_SecCenterImage_X.Value = Watch_Face.AnalogDialFace.SecCenterImage.X;
                    numericUpDown_SecCenterImage_Y.Value = Watch_Face.AnalogDialFace.SecCenterImage.Y;
                    //comboBox_SecCenterImage_Image.Text = Watch_Face.AnalogDialFace.SecCenterImage.ImageIndex.ToString();
                    checkBoxSetText(comboBox_SecCenterImage_Image, Watch_Face.AnalogDialFace.SecCenterImage.ImageIndex);
                }
                else checkBox_SecCenterImage.Checked = false;
            }
            else
            {
                checkBox_AnalogClock.Checked = false;
                checkBox_AnalogClock_Hour.Checked = false;
                checkBox_AnalogClock_Min.Checked = false;
                checkBox_AnalogClock_Sec.Checked = false;

                checkBox_HourCenterImage.Checked = false;
                checkBox_MinCenterImage.Checked = false;
                checkBox_SecCenterImage.Checked = false;
            }
            #endregion

            #region Weather
            if (Watch_Face.Weather != null)
            {
                checkBox_Weather.Checked = true;
                if ((Watch_Face.Weather.Temperature != null) && (Watch_Face.Weather.Temperature.Current != null))
                {
                    checkBox_Weather_Text.Checked = true;
                    numericUpDown_Weather_Text_StartCorner_X.Value = Watch_Face.Weather.Temperature.Current.TopLeftX;
                    numericUpDown_Weather_Text_StartCorner_Y.Value = Watch_Face.Weather.Temperature.Current.TopLeftY;
                    numericUpDown_Weather_Text_EndCorner_X.Value = Watch_Face.Weather.Temperature.Current.BottomRightX;
                    numericUpDown_Weather_Text_EndCorner_Y.Value = Watch_Face.Weather.Temperature.Current.BottomRightY;

                    numericUpDown_Weather_Text_Spacing.Value = Watch_Face.Weather.Temperature.Current.Spacing;
                    numericUpDown_Weather_Text_Count.Value = Watch_Face.Weather.Temperature.Current.ImagesCount;
                    //comboBox_Weather_Text_Image.Text = Watch_Face.Weather.Temperature.Current.ImageIndex.ToString();
                    checkBoxSetText(comboBox_Weather_Text_Image, Watch_Face.Weather.Temperature.Current.ImageIndex);
                    string Alignment = AlignmentToString(Watch_Face.Weather.Temperature.Current.Alignment);
                    comboBox_Weather_Text_Alignment.Text = Alignment;
                }
                else checkBox_Weather_Text.Checked = false;

                if ((Watch_Face.Weather.Temperature != null) && (Watch_Face.Weather.Temperature.Today != null))
                {
                    if ((Watch_Face.Weather.Temperature.Today.Separate != null) &&
                        (Watch_Face.Weather.Temperature.Today.Separate.Day != null))
                    {
                        checkBox_Weather_Day.Checked = true;
                        numericUpDown_Weather_Day_StartCorner_X.Value =
                            Watch_Face.Weather.Temperature.Today.Separate.Day.TopLeftX;
                        numericUpDown_Weather_Day_StartCorner_Y.Value =
                            Watch_Face.Weather.Temperature.Today.Separate.Day.TopLeftY;
                        numericUpDown_Weather_Day_EndCorner_X.Value =
                            Watch_Face.Weather.Temperature.Today.Separate.Day.BottomRightX;
                        numericUpDown_Weather_Day_EndCorner_Y.Value =
                            Watch_Face.Weather.Temperature.Today.Separate.Day.BottomRightY;

                        numericUpDown_Weather_Day_Spacing.Value =
                            Watch_Face.Weather.Temperature.Today.Separate.Day.Spacing;
                        numericUpDown_Weather_Day_Count.Value =
                            Watch_Face.Weather.Temperature.Today.Separate.Day.ImagesCount;
                        //comboBox_Weather_Day_Image.Text =
                        //Watch_Face.Weather.Temperature.Today.Separate.Day.ImageIndex.ToString();
                        checkBoxSetText(comboBox_Weather_Day_Image,
                            Watch_Face.Weather.Temperature.Today.Separate.Day.ImageIndex);
                        string Alignment = AlignmentToString(Watch_Face.Weather.Temperature.Today.Separate.Day.Alignment);
                        comboBox_Weather_Day_Alignment.Text = Alignment;
                    }
                    else checkBox_Weather_Day.Checked = false;

                    if ((Watch_Face.Weather.Temperature.Today.Separate != null) &&
                        (Watch_Face.Weather.Temperature.Today.Separate.Night != null))
                    {
                        checkBox_Weather_Night.Checked = true;
                        numericUpDown_Weather_Night_StartCorner_X.Value =
                            Watch_Face.Weather.Temperature.Today.Separate.Night.TopLeftX;
                        numericUpDown_Weather_Night_StartCorner_Y.Value =
                            Watch_Face.Weather.Temperature.Today.Separate.Night.TopLeftY;
                        numericUpDown_Weather_Night_EndCorner_X.Value =
                            Watch_Face.Weather.Temperature.Today.Separate.Night.BottomRightX;
                        numericUpDown_Weather_Night_EndCorner_Y.Value =
                            Watch_Face.Weather.Temperature.Today.Separate.Night.BottomRightY;

                        numericUpDown_Weather_Night_Spacing.Value =
                            Watch_Face.Weather.Temperature.Today.Separate.Night.Spacing;
                        numericUpDown_Weather_Night_Count.Value =
                            Watch_Face.Weather.Temperature.Today.Separate.Night.ImagesCount;
                        //comboBox_Weather_Night_Image.Text =
                        //    Watch_Face.Weather.Temperature.Today.Separate.Night.ImageIndex.ToString();
                        checkBoxSetText(comboBox_Weather_Night_Image,
                            Watch_Face.Weather.Temperature.Today.Separate.Night.ImageIndex);
                        string Alignment = AlignmentToString(Watch_Face.Weather.Temperature.Today.Separate.Night.Alignment);
                        comboBox_Weather_Night_Alignment.Text = Alignment;
                    }
                    else checkBox_Weather_Night.Checked = false;
                }
                else
                {
                    checkBox_Weather_Day.Checked = false;
                    checkBox_Weather_Night.Checked = false;
                }

                if ((Watch_Face.Weather.Temperature != null) && (Watch_Face.Weather.Temperature.Symbols != null))
                {
                    //comboBox_Weather_Text_MinusImage.Text = Watch_Face.Weather.Temperature.Symbols.MinusImageIndex.ToString();
                    checkBoxSetText(comboBox_Weather_Text_MinusImage, Watch_Face.Weather.Temperature.Symbols.MinusImageIndex);
                    //comboBox_Weather_Text_DegImage.Text = Watch_Face.Weather.Temperature.Symbols.DegreesImageIndex.ToString();
                    checkBoxSetText(comboBox_Weather_Text_DegImage, Watch_Face.Weather.Temperature.Symbols.DegreesImageIndex);
                    //comboBox_Weather_Text_NDImage.Text = Watch_Face.Weather.Temperature.Symbols.NoDataImageIndex.ToString();
                    checkBoxSetText(comboBox_Weather_Text_NDImage, Watch_Face.Weather.Temperature.Symbols.NoDataImageIndex);
                }

                if ((Watch_Face.Weather.Icon != null) && (Watch_Face.Weather.Icon.Images != null))
                {
                    checkBox_Weather_Icon.Checked = true;
                    numericUpDown_Weather_Icon_X.Value = Watch_Face.Weather.Icon.Images.X;
                    numericUpDown_Weather_Icon_Y.Value = Watch_Face.Weather.Icon.Images.Y;

                    numericUpDown_Weather_Icon_Count.Value = Watch_Face.Weather.Icon.Images.ImagesCount;
                    //comboBox_Weather_Icon_Image.Text = Watch_Face.Weather.Icon.Images.ImageIndex.ToString();
                    checkBoxSetText(comboBox_Weather_Icon_Image, Watch_Face.Weather.Icon.Images.ImageIndex);
                    //comboBox_Weather_Icon_NDImage.Text = Watch_Face.Weather.Icon.NoWeatherImageIndex.ToString();
                    checkBoxSetText(comboBox_Weather_Icon_NDImage, Watch_Face.Weather.Icon.NoWeatherImageIndex);
                }
                else checkBox_Weather_Icon.Checked = false;
            }
            else
            {
                checkBox_Weather_Text.Checked = false;
                checkBox_Weather_Day.Checked = false;
                checkBox_Weather_Night.Checked = false;
                checkBox_Weather_Icon.Checked = false;
                checkBox_Weather.Checked = false;
            }
            #endregion
        }

        // формируем JSON файл из настроек
        private void JSON_write()
        {
            if (!PreviewView) return;
            Watch_Face = new WATCH_FACE_JSON();
            if ((comboBox_Background.SelectedIndex >= 0) || (comboBox_Preview.SelectedIndex >= 0))
            {
                Watch_Face.Background = new Background();
                if (comboBox_Background.SelectedIndex >= 0)
                {
                    Watch_Face.Background.Image = new ImageW();
                    Watch_Face.Background.Image.ImageIndex = Int32.Parse(comboBox_Background.Text);
                }
                if (comboBox_Preview.SelectedIndex >= 0)
                {
                    Watch_Face.Background.Preview = new ImageW();
                    Watch_Face.Background.Preview.ImageIndex = Int32.Parse(comboBox_Preview.Text);
                }
            }

            // время 
            if (checkBox_Time.Checked)
            {
                if ((checkBox_Hours.Checked) && (comboBox_Hours_Tens_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Time == null) Watch_Face.Time = new TimeW();
                    if (Watch_Face.Time.Hours == null) Watch_Face.Time.Hours = new TwoDigits();

                    Watch_Face.Time.Hours.Tens = new ImageSet();
                    Watch_Face.Time.Hours.Tens.ImageIndex = Int32.Parse(comboBox_Hours_Tens_Image.Text);
                    Watch_Face.Time.Hours.Tens.ImagesCount = (int)numericUpDown_Hours_Tens_Count.Value;
                    Watch_Face.Time.Hours.Tens.X = (int)numericUpDown_Hours_Tens_X.Value;
                    Watch_Face.Time.Hours.Tens.Y = (int)numericUpDown_Hours_Tens_Y.Value;
                }
                if ((checkBox_Hours.Checked) && (comboBox_Hours_Ones_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Time == null) Watch_Face.Time = new TimeW();
                    if (Watch_Face.Time.Hours == null) Watch_Face.Time.Hours = new TwoDigits();

                    Watch_Face.Time.Hours.Ones = new ImageSet();
                    Watch_Face.Time.Hours.Ones.ImageIndex = Int32.Parse(comboBox_Hours_Ones_Image.Text);
                    Watch_Face.Time.Hours.Ones.ImagesCount = (int)numericUpDown_Hours_Ones_Count.Value;
                    Watch_Face.Time.Hours.Ones.X = (int)numericUpDown_Hours_Ones_X.Value;
                    Watch_Face.Time.Hours.Ones.Y = (int)numericUpDown_Hours_Ones_Y.Value;
                }

                if ((checkBox_Minutes.Checked) && (comboBox_Min_Tens_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Time == null) Watch_Face.Time = new TimeW();
                    if (Watch_Face.Time.Minutes == null) Watch_Face.Time.Minutes = new TwoDigits();

                    Watch_Face.Time.Minutes.Tens = new ImageSet();
                    Watch_Face.Time.Minutes.Tens.ImageIndex = Int32.Parse(comboBox_Min_Tens_Image.Text);
                    Watch_Face.Time.Minutes.Tens.ImagesCount = (int)numericUpDown_Min_Tens_Count.Value;
                    Watch_Face.Time.Minutes.Tens.X = (int)numericUpDown_Min_Tens_X.Value;
                    Watch_Face.Time.Minutes.Tens.Y = (int)numericUpDown_Min_Tens_Y.Value;
                }
                if ((checkBox_Minutes.Checked) && (comboBox_Min_Ones_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Time == null) Watch_Face.Time = new TimeW();
                    if (Watch_Face.Time.Minutes == null) Watch_Face.Time.Minutes = new TwoDigits();

                    Watch_Face.Time.Minutes.Ones = new ImageSet();
                    Watch_Face.Time.Minutes.Ones.ImageIndex = Int32.Parse(comboBox_Min_Ones_Image.Text);
                    Watch_Face.Time.Minutes.Ones.ImagesCount = (int)numericUpDown_Min_Ones_Count.Value;
                    Watch_Face.Time.Minutes.Ones.X = (int)numericUpDown_Min_Ones_X.Value;
                    Watch_Face.Time.Minutes.Ones.Y = (int)numericUpDown_Min_Ones_Y.Value;
                }

                if ((checkBox_Seconds.Checked) && (comboBox_Sec_Tens_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Time == null) Watch_Face.Time = new TimeW();
                    if (Watch_Face.Time.Seconds == null) Watch_Face.Time.Seconds = new TwoDigits();

                    Watch_Face.Time.Seconds.Tens = new ImageSet();
                    Watch_Face.Time.Seconds.Tens.ImageIndex = Int32.Parse(comboBox_Sec_Tens_Image.Text);
                    Watch_Face.Time.Seconds.Tens.ImagesCount = (int)numericUpDown_Sec_Tens_Count.Value;
                    Watch_Face.Time.Seconds.Tens.X = (int)numericUpDown_Sec_Tens_X.Value;
                    Watch_Face.Time.Seconds.Tens.Y = (int)numericUpDown_Sec_Tens_Y.Value;
                }
                if ((checkBox_Seconds.Checked) && (comboBox_Sec_Ones_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Time == null) Watch_Face.Time = new TimeW();
                    if (Watch_Face.Time.Seconds == null) Watch_Face.Time.Seconds = new TwoDigits();

                    Watch_Face.Time.Seconds.Ones = new ImageSet();
                    Watch_Face.Time.Seconds.Ones.ImageIndex = Int32.Parse(comboBox_Sec_Ones_Image.Text);
                    Watch_Face.Time.Seconds.Ones.ImagesCount = (int)numericUpDown_Sec_Ones_Count.Value;
                    Watch_Face.Time.Seconds.Ones.X = (int)numericUpDown_Sec_Ones_X.Value;
                    Watch_Face.Time.Seconds.Ones.Y = (int)numericUpDown_Sec_Ones_Y.Value;
                }

                if ((checkBox_AmPm.Checked) && (comboBox_Image_Am.SelectedIndex >= 0) &&
                    (comboBox_Image_Pm.SelectedIndex >= 0))
                {
                    if (Watch_Face.Time == null) Watch_Face.Time = new TimeW();
                    if (Watch_Face.Time.AmPm == null) Watch_Face.Time.AmPm = new AmPm();

                    Watch_Face.Time.AmPm.ImageIndexAMCN = Int32.Parse(comboBox_Image_Am.Text);
                    Watch_Face.Time.AmPm.ImageIndexAMEN = Int32.Parse(comboBox_Image_Am.Text);
                    Watch_Face.Time.AmPm.ImageIndexPMCN = Int32.Parse(comboBox_Image_Pm.Text);
                    Watch_Face.Time.AmPm.ImageIndexPMEN = Int32.Parse(comboBox_Image_Pm.Text);
                    Watch_Face.Time.AmPm.X = (int)numericUpDown_AmPm_X.Value;
                    Watch_Face.Time.AmPm.Y = (int)numericUpDown_AmPm_Y.Value;
                }

                if ((checkBox_Delimiter.Checked) && (comboBox_Delimiter_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Time == null) Watch_Face.Time = new TimeW();
                    if (Watch_Face.Time.Delimiter == null) Watch_Face.Time.Delimiter = new ImageW();

                    Watch_Face.Time.Delimiter.ImageIndex = Int32.Parse(comboBox_Delimiter_Image.Text);
                    Watch_Face.Time.Delimiter.X = (int)numericUpDown_Delimiter_X.Value;
                    Watch_Face.Time.Delimiter.Y = (int)numericUpDown_Delimiter_Y.Value;
                }
            }

            // активити
            if (checkBox_Activity.Checked)
            {
                if ((checkBox_ActivityGoal.Checked) && (comboBox_ActivityGoal_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
                    if (Watch_Face.Activity.StepsGoal == null) Watch_Face.Activity.StepsGoal = new Number();

                    Watch_Face.Activity.StepsGoal.ImageIndex = Int32.Parse(comboBox_ActivityGoal_Image.Text);
                    Watch_Face.Activity.StepsGoal.ImagesCount = (int)numericUpDown_ActivityGoal_Count.Value;
                    Watch_Face.Activity.StepsGoal.TopLeftX = (int)numericUpDown_ActivityGoal_StartCorner_X.Value;
                    Watch_Face.Activity.StepsGoal.TopLeftY = (int)numericUpDown_ActivityGoal_StartCorner_Y.Value;
                    Watch_Face.Activity.StepsGoal.BottomRightX = (int)numericUpDown_ActivityGoal_EndCorner_X.Value;
                    Watch_Face.Activity.StepsGoal.BottomRightY = (int)numericUpDown_ActivityGoal_EndCorner_Y.Value;

                    Watch_Face.Activity.StepsGoal.Spacing = (int)numericUpDown_ActivityGoal_Spacing.Value;
                    string Alignment = StringToAlignment(comboBox_ActivityGoal_Alignment.Text);
                    Watch_Face.Activity.StepsGoal.Alignment = Alignment;
                }

                if ((checkBox_ActivitySteps.Checked) && (comboBox_ActivitySteps_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
                    if (Watch_Face.Activity.Steps == null) Watch_Face.Activity.Steps = new FormattedNumber();
                    if (Watch_Face.Activity.Steps.Step == null) Watch_Face.Activity.Steps.Step = new Number();

                    Watch_Face.Activity.Steps.Step.ImageIndex = Int32.Parse(comboBox_ActivitySteps_Image.Text);
                    Watch_Face.Activity.Steps.Step.ImagesCount = (int)numericUpDown_ActivitySteps_Count.Value;
                    Watch_Face.Activity.Steps.Step.TopLeftX = (int)numericUpDown_ActivitySteps_StartCorner_X.Value;
                    Watch_Face.Activity.Steps.Step.TopLeftY = (int)numericUpDown_ActivitySteps_StartCorner_Y.Value;
                    Watch_Face.Activity.Steps.Step.BottomRightX = (int)numericUpDown_ActivitySteps_EndCorner_X.Value;
                    Watch_Face.Activity.Steps.Step.BottomRightY = (int)numericUpDown_ActivitySteps_EndCorner_Y.Value;

                    Watch_Face.Activity.Steps.Step.Spacing = (int)numericUpDown_ActivitySteps_Spacing.Value;
                    string Alignment = StringToAlignment(comboBox_ActivitySteps_Alignment.Text);
                    Watch_Face.Activity.Steps.Step.Alignment = Alignment;
                }

                if ((checkBox_ActivityDistance.Checked) && (comboBox_ActivityDistance_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
                    if (Watch_Face.Activity.Distance == null) Watch_Face.Activity.Distance = new Distance();
                    if (Watch_Face.Activity.Distance.Number == null) Watch_Face.Activity.Distance.Number = new Number();

                    Watch_Face.Activity.Distance.Number.ImageIndex = Int32.Parse(comboBox_ActivityDistance_Image.Text);
                    Watch_Face.Activity.Distance.Number.ImagesCount = (int)numericUpDown_ActivityDistance_Count.Value;
                    Watch_Face.Activity.Distance.Number.TopLeftX = (int)numericUpDown_ActivityDistance_StartCorner_X.Value;
                    Watch_Face.Activity.Distance.Number.TopLeftY = (int)numericUpDown_ActivityDistance_StartCorner_Y.Value;
                    Watch_Face.Activity.Distance.Number.BottomRightX = (int)numericUpDown_ActivityDistance_EndCorner_X.Value;
                    Watch_Face.Activity.Distance.Number.BottomRightY = (int)numericUpDown_ActivityDistance_EndCorner_Y.Value;

                    Watch_Face.Activity.Distance.Number.Spacing = (int)numericUpDown_ActivityDistance_Spacing.Value;
                    string Alignment = StringToAlignment(comboBox_ActivityDistance_Alignment.Text);
                    Watch_Face.Activity.Distance.Number.Alignment = Alignment;

                    if ((comboBox_ActivityDistance_Suffix.SelectedIndex >= 0) &&
                        (comboBox_ActivityDistance_Suffix.Text.Length > 0))
                        Watch_Face.Activity.Distance.SuffixImageIndex = Int32.Parse(comboBox_ActivityDistance_Suffix.Text);
                    if ((comboBox_ActivityDistance_Decimal.SelectedIndex >= 0) &&
                        (comboBox_ActivityDistance_Decimal.Text.Length > 0))
                        Watch_Face.Activity.Distance.DecimalPointImageIndex = Int32.Parse(comboBox_ActivityDistance_Decimal.Text);
                }

                if ((checkBox_ActivityPuls.Checked) && (comboBox_ActivityPuls_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
                    if (Watch_Face.Activity.Pulse == null) Watch_Face.Activity.Pulse = new Number();

                    Watch_Face.Activity.Pulse.ImageIndex = Int32.Parse(comboBox_ActivityPuls_Image.Text);
                    Watch_Face.Activity.Pulse.ImagesCount = (int)numericUpDown_ActivityPuls_Count.Value;
                    Watch_Face.Activity.Pulse.TopLeftX = (int)numericUpDown_ActivityPuls_StartCorner_X.Value;
                    Watch_Face.Activity.Pulse.TopLeftY = (int)numericUpDown_ActivityPuls_StartCorner_Y.Value;
                    Watch_Face.Activity.Pulse.BottomRightX = (int)numericUpDown_ActivityPuls_EndCorner_X.Value;
                    Watch_Face.Activity.Pulse.BottomRightY = (int)numericUpDown_ActivityPuls_EndCorner_Y.Value;

                    Watch_Face.Activity.Pulse.Spacing = (int)numericUpDown_ActivityPuls_Spacing.Value;
                    string Alignment = StringToAlignment(comboBox_ActivityPuls_Alignment.Text);
                    Watch_Face.Activity.Pulse.Alignment = Alignment;
                }

                if ((checkBox_ActivityCalories.Checked) && (comboBox_ActivityCalories_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
                    if (Watch_Face.Activity.Calories == null) Watch_Face.Activity.Calories = new Number();

                    Watch_Face.Activity.Calories.ImageIndex = Int32.Parse(comboBox_ActivityCalories_Image.Text);
                    Watch_Face.Activity.Calories.ImagesCount = (int)numericUpDown_ActivityCalories_Count.Value;
                    Watch_Face.Activity.Calories.TopLeftX = (int)numericUpDown_ActivityCalories_StartCorner_X.Value;
                    Watch_Face.Activity.Calories.TopLeftY = (int)numericUpDown_ActivityCalories_StartCorner_Y.Value;
                    Watch_Face.Activity.Calories.BottomRightX = (int)numericUpDown_ActivityCalories_EndCorner_X.Value;
                    Watch_Face.Activity.Calories.BottomRightY = (int)numericUpDown_ActivityCalories_EndCorner_Y.Value;

                    Watch_Face.Activity.Calories.Spacing = (int)numericUpDown_ActivityCalories_Spacing.Value;
                    string Alignment = StringToAlignment(comboBox_ActivityCalories_Alignment.Text);
                    Watch_Face.Activity.Calories.Alignment = Alignment;
                }

                if ((checkBox_ActivityStar.Checked) && (comboBox_ActivityStar_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
                    if (Watch_Face.Activity.StarImage == null) Watch_Face.Activity.StarImage = new ImageW();

                    Watch_Face.Activity.StarImage.ImageIndex = Int32.Parse(comboBox_ActivityStar_Image.Text);
                    Watch_Face.Activity.StarImage.X = (int)numericUpDown_ActivityStar_X.Value;
                    Watch_Face.Activity.StarImage.Y = (int)numericUpDown_ActivityStar_Y.Value;
                }
            }

            // дата 
            if (checkBox_Date.Checked)
            {
                if ((checkBox_WeekDay.Checked) && (comboBox_WeekDay_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Date == null) Watch_Face.Date = new Date();
                    if (Watch_Face.Date.WeekDay == null) Watch_Face.Date.WeekDay = new ImageSet();

                    Watch_Face.Date.WeekDay.ImageIndex = Int32.Parse(comboBox_WeekDay_Image.Text);
                    Watch_Face.Date.WeekDay.ImagesCount = (int)numericUpDown_WeekDay_Count.Value;
                    Watch_Face.Date.WeekDay.X = (int)numericUpDown_WeekDay_X.Value;
                    Watch_Face.Date.WeekDay.Y = (int)numericUpDown_WeekDay_Y.Value;
                }

                if ((checkBox_MonthName.Checked) && (comboBox_MonthName_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Date == null) Watch_Face.Date = new Date();
                    if (Watch_Face.Date.MonthAndDay == null) Watch_Face.Date.MonthAndDay = new MonthAndDay();
                    if (Watch_Face.Date.MonthAndDay.Separate == null)
                        Watch_Face.Date.MonthAndDay.Separate = new SeparateMonthAndDay();
                    if (Watch_Face.Date.MonthAndDay.Separate.MonthName == null)
                        Watch_Face.Date.MonthAndDay.Separate.MonthName = new ImageSet();

                    Watch_Face.Date.MonthAndDay.Separate.MonthName.ImageIndex = Int32.Parse(comboBox_MonthName_Image.Text);
                    Watch_Face.Date.MonthAndDay.Separate.MonthName.ImagesCount = (int)numericUpDown_MonthName_Count.Value;
                    Watch_Face.Date.MonthAndDay.Separate.MonthName.X = (int)numericUpDown_MonthName_X.Value;
                    Watch_Face.Date.MonthAndDay.Separate.MonthName.Y = (int)numericUpDown_MonthName_Y.Value;
                }
                if ((checkBox_MonthAndDayD.Checked) && (comboBox_MonthAndDayD_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Date == null) Watch_Face.Date = new Date();
                    if (Watch_Face.Date.MonthAndDay == null) Watch_Face.Date.MonthAndDay = new MonthAndDay();
                    if (Watch_Face.Date.MonthAndDay.Separate == null)
                        Watch_Face.Date.MonthAndDay.Separate = new SeparateMonthAndDay();
                    if (Watch_Face.Date.MonthAndDay.Separate.Day == null)
                        Watch_Face.Date.MonthAndDay.Separate.Day = new Number();

                    Watch_Face.Date.MonthAndDay.Separate.Day.ImageIndex = Int32.Parse(comboBox_MonthAndDayD_Image.Text);
                    Watch_Face.Date.MonthAndDay.Separate.Day.ImagesCount = (int)numericUpDown_MonthAndDayD_Count.Value;
                    Watch_Face.Date.MonthAndDay.Separate.Day.TopLeftX = (int)numericUpDown_MonthAndDayD_StartCorner_X.Value;
                    Watch_Face.Date.MonthAndDay.Separate.Day.TopLeftY = (int)numericUpDown_MonthAndDayD_StartCorner_Y.Value;
                    Watch_Face.Date.MonthAndDay.Separate.Day.BottomRightX = (int)numericUpDown_MonthAndDayD_EndCorner_X.Value;
                    Watch_Face.Date.MonthAndDay.Separate.Day.BottomRightY = (int)numericUpDown_MonthAndDayD_EndCorner_Y.Value;

                    Watch_Face.Date.MonthAndDay.Separate.Day.Spacing = (int)numericUpDown_MonthAndDayD_Spacing.Value;
                    string Alignment = StringToAlignment(comboBox_MonthAndDayD_Alignment.Text);
                    Watch_Face.Date.MonthAndDay.Separate.Day.Alignment = Alignment;
                }
                if ((checkBox_MonthAndDayM.Checked) && (comboBox_MonthAndDayM_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Date == null) Watch_Face.Date = new Date();
                    if (Watch_Face.Date.MonthAndDay == null) Watch_Face.Date.MonthAndDay = new MonthAndDay();
                    if (Watch_Face.Date.MonthAndDay.Separate == null)
                        Watch_Face.Date.MonthAndDay.Separate = new SeparateMonthAndDay();
                    if (Watch_Face.Date.MonthAndDay.Separate.Month == null)
                        Watch_Face.Date.MonthAndDay.Separate.Month = new Number();

                    Watch_Face.Date.MonthAndDay.Separate.Month.ImageIndex = Int32.Parse(comboBox_MonthAndDayM_Image.Text);
                    Watch_Face.Date.MonthAndDay.Separate.Month.ImagesCount = (int)numericUpDown_MonthAndDayM_Count.Value;
                    Watch_Face.Date.MonthAndDay.Separate.Month.TopLeftX = (int)numericUpDown_MonthAndDayM_StartCorner_X.Value;
                    Watch_Face.Date.MonthAndDay.Separate.Month.TopLeftY = (int)numericUpDown_MonthAndDayM_StartCorner_Y.Value;
                    Watch_Face.Date.MonthAndDay.Separate.Month.BottomRightX = (int)numericUpDown_MonthAndDayM_EndCorner_X.Value;
                    Watch_Face.Date.MonthAndDay.Separate.Month.BottomRightY = (int)numericUpDown_MonthAndDayM_EndCorner_Y.Value;

                    Watch_Face.Date.MonthAndDay.Separate.Month.Spacing = (int)numericUpDown_MonthAndDayM_Spacing.Value;
                    string Alignment = StringToAlignment(comboBox_MonthAndDayM_Alignment.Text);
                    Watch_Face.Date.MonthAndDay.Separate.Month.Alignment = Alignment;
                }

                if ((checkBox_OneLine.Checked) && (comboBox_OneLine_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Date == null) Watch_Face.Date = new Date();
                    if (Watch_Face.Date.MonthAndDay == null) Watch_Face.Date.MonthAndDay = new MonthAndDay();
                    if (Watch_Face.Date.MonthAndDay.OneLine == null)
                        Watch_Face.Date.MonthAndDay.OneLine = new OneLineMonthAndDay();
                    if (Watch_Face.Date.MonthAndDay.OneLine.Number == null)
                        Watch_Face.Date.MonthAndDay.OneLine.Number = new Number();

                    Watch_Face.Date.MonthAndDay.OneLine.Number.ImageIndex = Int32.Parse(comboBox_OneLine_Image.Text);
                    Watch_Face.Date.MonthAndDay.OneLine.Number.ImagesCount = (int)numericUpDown_OneLine_Count.Value;
                    Watch_Face.Date.MonthAndDay.OneLine.Number.TopLeftX = (int)numericUpDown_OneLine_StartCorner_X.Value;
                    Watch_Face.Date.MonthAndDay.OneLine.Number.TopLeftY = (int)numericUpDown_OneLine_StartCorner_Y.Value;
                    Watch_Face.Date.MonthAndDay.OneLine.Number.BottomRightX = (int)numericUpDown_OneLine_EndCorner_X.Value;
                    Watch_Face.Date.MonthAndDay.OneLine.Number.BottomRightY = (int)numericUpDown_OneLine_EndCorner_Y.Value;

                    Watch_Face.Date.MonthAndDay.OneLine.Number.Spacing = (int)numericUpDown_OneLine_Spacing.Value;
                    string Alignment = StringToAlignment(comboBox_OneLine_Alignment.Text);
                    Watch_Face.Date.MonthAndDay.OneLine.Number.Alignment = Alignment;

                    if (comboBox_OneLine_Delimiter.SelectedIndex >= 0)
                        Watch_Face.Date.MonthAndDay.OneLine.DelimiterImageIndex = Int32.Parse(comboBox_OneLine_Delimiter.Text);
                }

                if ((Watch_Face.Date != null) && (Watch_Face.Date.MonthAndDay != null))
                {
                    Watch_Face.Date.MonthAndDay.TwoDigitsMonth = checkBox_TwoDigitsMonth.Checked;
                    Watch_Face.Date.MonthAndDay.TwoDigitsDay = checkBox_TwoDigitsDay.Checked;
                }

                if ((checkBox_Year.Checked) && (comboBox_Year_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Date == null) Watch_Face.Date = new Date();
                    if (Watch_Face.Date.Year == null) Watch_Face.Date.Year = new Year();
                    if (Watch_Face.Date.Year.OneLine == null)
                        Watch_Face.Date.Year.OneLine = new OneLineYear();
                    if (Watch_Face.Date.Year.OneLine.Number == null)
                        Watch_Face.Date.Year.OneLine.Number = new Number();

                    Watch_Face.Date.Year.OneLine.Number.ImageIndex = Int32.Parse(comboBox_Year_Image.Text);
                    Watch_Face.Date.Year.OneLine.Number.ImagesCount = (int)numericUpDown_Year_Count.Value;
                    Watch_Face.Date.Year.OneLine.Number.TopLeftX = (int)numericUpDown_Year_StartCorner_X.Value;
                    Watch_Face.Date.Year.OneLine.Number.TopLeftY = (int)numericUpDown_Year_StartCorner_Y.Value;
                    Watch_Face.Date.Year.OneLine.Number.BottomRightX = (int)numericUpDown_Year_EndCorner_X.Value;
                    Watch_Face.Date.Year.OneLine.Number.BottomRightY = (int)numericUpDown_Year_EndCorner_Y.Value;

                    Watch_Face.Date.Year.OneLine.Number.Spacing = (int)numericUpDown_Year_Spacing.Value;
                    string Alignment = StringToAlignment(comboBox_Year_Alignment.Text);
                    Watch_Face.Date.Year.OneLine.Number.Alignment = Alignment;

                    if (comboBox_Year_Delimiter.SelectedIndex >= 0)
                        Watch_Face.Date.Year.OneLine.DelimiterImageIndex = Int32.Parse(comboBox_Year_Delimiter.Text);
                }
            }

            // прогресc шагов
            if (checkBox_StepsProgress.Checked)
            {
                if (Watch_Face.StepsProgress == null) Watch_Face.StepsProgress = new StepsProgress();
                if (Watch_Face.StepsProgress.Circle == null) Watch_Face.StepsProgress.Circle = new CircleScale();

                Watch_Face.StepsProgress.Circle.CenterX = (int)numericUpDown_StepsProgress_Center_X.Value;
                Watch_Face.StepsProgress.Circle.CenterY = (int)numericUpDown_StepsProgress_Center_Y.Value;
                Watch_Face.StepsProgress.Circle.RadiusX = (int)numericUpDown_StepsProgress_Radius_X.Value;
                Watch_Face.StepsProgress.Circle.RadiusY = (int)numericUpDown_StepsProgress_Radius_Y.Value;

                Watch_Face.StepsProgress.Circle.StartAngle = (int)numericUpDown_StepsProgress_StartAngle.Value;
                Watch_Face.StepsProgress.Circle.EndAngle = (int)numericUpDown_StepsProgress_EndAngle.Value;
                Watch_Face.StepsProgress.Circle.Width = (int)numericUpDown_StepsProgress_Width.Value;

                Color color = comboBox_StepsProgress_Color.BackColor;
                Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
                string colorStr = ColorTranslator.ToHtml(new_color);
                colorStr = colorStr.Replace("#", "0x00");
                Watch_Face.StepsProgress.Circle.Color = colorStr;

                switch (comboBox_StepsProgress_Flatness.Text)
                {
                    case "Треугольное":
                        Watch_Face.StepsProgress.Circle.Flatness = 90;
                        break;
                    case "Плоское":
                        Watch_Face.StepsProgress.Circle.Flatness = 180;
                        break;
                    default:
                        Watch_Face.StepsProgress.Circle.Flatness = 0;
                        break;
                }
            }

            // статусы
            if ((checkBox_Bluetooth.Checked) &&
                ((comboBox_Bluetooth_On.SelectedIndex >= 0) || (comboBox_Bluetooth_Off.SelectedIndex >= 0)))
            {
                if (Watch_Face.Status == null) Watch_Face.Status = new Status();
                if (Watch_Face.Status.Bluetooth == null) Watch_Face.Status.Bluetooth = new SwitchW();
                if (Watch_Face.Status.Bluetooth.Coordinates == null)
                    Watch_Face.Status.Bluetooth.Coordinates = new Coordinates();

                if (comboBox_Bluetooth_On.SelectedIndex >= 0)
                    Watch_Face.Status.Bluetooth.ImageIndexOn = Int32.Parse(comboBox_Bluetooth_On.Text);
                if (comboBox_Bluetooth_Off.SelectedIndex >= 0)
                    Watch_Face.Status.Bluetooth.ImageIndexOff = Int32.Parse(comboBox_Bluetooth_Off.Text);
                Watch_Face.Status.Bluetooth.Coordinates.X = (int)numericUpDown_Bluetooth_X.Value;
                Watch_Face.Status.Bluetooth.Coordinates.Y = (int)numericUpDown_Bluetooth_Y.Value;
            }
            if ((checkBox_Alarm.Checked) &&
                ((comboBox_Alarm_On.SelectedIndex >= 0) || (comboBox_Alarm_Off.SelectedIndex >= 0)))
            {
                if (Watch_Face.Status == null) Watch_Face.Status = new Status();
                if (Watch_Face.Status.Alarm == null) Watch_Face.Status.Alarm = new SwitchW();
                if (Watch_Face.Status.Alarm.Coordinates == null)
                    Watch_Face.Status.Alarm.Coordinates = new Coordinates();
                Watch_Face.Status.Alarm.Coordinates = new Coordinates();

                if (comboBox_Alarm_On.SelectedIndex >= 0)
                    Watch_Face.Status.Alarm.ImageIndexOn = Int32.Parse(comboBox_Alarm_On.Text);
                if (comboBox_Alarm_Off.SelectedIndex >= 0)
                    Watch_Face.Status.Alarm.ImageIndexOff = Int32.Parse(comboBox_Alarm_Off.Text);
                Watch_Face.Status.Alarm.Coordinates.X = (int)numericUpDown_Alarm_X.Value;
                Watch_Face.Status.Alarm.Coordinates.Y = (int)numericUpDown_Alarm_Y.Value;
            }
            if ((checkBox_Lock.Checked) &&
                ((comboBox_Lock_On.SelectedIndex >= 0) || (comboBox_Lock_Off.SelectedIndex >= 0)))
            {
                if (Watch_Face.Status == null) Watch_Face.Status = new Status();
                if (Watch_Face.Status.Lock == null) Watch_Face.Status.Lock = new SwitchW();
                if (Watch_Face.Status.Lock.Coordinates == null)
                    Watch_Face.Status.Lock.Coordinates = new Coordinates();

                if (comboBox_Lock_On.SelectedIndex >= 0)
                    Watch_Face.Status.Lock.ImageIndexOn = Int32.Parse(comboBox_Lock_On.Text);
                if (comboBox_Lock_Off.SelectedIndex >= 0)
                    Watch_Face.Status.Lock.ImageIndexOff = Int32.Parse(comboBox_Lock_Off.Text);
                Watch_Face.Status.Lock.Coordinates.X = (int)numericUpDown_Lock_X.Value;
                Watch_Face.Status.Lock.Coordinates.Y = (int)numericUpDown_Lock_Y.Value;
            }
            if ((checkBox_DND.Checked) &&
                ((comboBox_DND_On.SelectedIndex >= 0) || (comboBox_DND_Off.SelectedIndex >= 0)))
            {
                if (Watch_Face.Status == null) Watch_Face.Status = new Status();
                if (Watch_Face.Status.DoNotDisturb == null) Watch_Face.Status.DoNotDisturb = new SwitchW();
                if (Watch_Face.Status.DoNotDisturb.Coordinates == null)
                    Watch_Face.Status.DoNotDisturb.Coordinates = new Coordinates();

                if (comboBox_DND_On.SelectedIndex >= 0)
                    Watch_Face.Status.DoNotDisturb.ImageIndexOn = Int32.Parse(comboBox_DND_On.Text);
                if (comboBox_DND_Off.SelectedIndex >= 0)
                    Watch_Face.Status.DoNotDisturb.ImageIndexOff = Int32.Parse(comboBox_DND_Off.Text);
                Watch_Face.Status.DoNotDisturb.Coordinates.X = (int)numericUpDown_DND_X.Value;
                Watch_Face.Status.DoNotDisturb.Coordinates.Y = (int)numericUpDown_DND_Y.Value;
            }

            // батарея
            if (checkBox_Battery.Checked)
            {
                if ((checkBox_Battery_Text.Checked) && (comboBox_Battery_Text_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Battery == null) Watch_Face.Battery = new Battery();
                    if (Watch_Face.Battery.Text == null) Watch_Face.Battery.Text = new Number();

                    Watch_Face.Battery.Text.ImageIndex = Int32.Parse(comboBox_Battery_Text_Image.Text);
                    Watch_Face.Battery.Text.ImagesCount = (int)numericUpDown_Battery_Text_Count.Value;
                    Watch_Face.Battery.Text.TopLeftX = (int)numericUpDown_Battery_Text_StartCorner_X.Value;
                    Watch_Face.Battery.Text.TopLeftY = (int)numericUpDown_Battery_Text_StartCorner_Y.Value;
                    Watch_Face.Battery.Text.BottomRightX = (int)numericUpDown_Battery_Text_EndCorner_X.Value;
                    Watch_Face.Battery.Text.BottomRightY = (int)numericUpDown_Battery_Text_EndCorner_Y.Value;

                    Watch_Face.Battery.Text.Spacing = (int)numericUpDown_Battery_Text_Spacing.Value;
                    string Alignment = StringToAlignment(comboBox_Battery_Text_Alignment.Text);
                    Watch_Face.Battery.Text.Alignment = Alignment;
                }

                if ((checkBox_Battery_Percent.Checked) && (comboBox_Battery_Percent_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Battery == null) Watch_Face.Battery = new Battery();
                    if (Watch_Face.Battery.Percent == null) Watch_Face.Battery.Percent = new ImageW();

                    Watch_Face.Battery.Percent.ImageIndex = Int32.Parse(comboBox_Battery_Percent_Image.Text);
                    Watch_Face.Battery.Percent.X = (int)numericUpDown_Battery_Percent_X.Value;
                    Watch_Face.Battery.Percent.Y = (int)numericUpDown_Battery_Percent_Y.Value;
                }

                if ((checkBox_Battery_Img.Checked) && (comboBox_Battery_Img_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Battery == null) Watch_Face.Battery = new Battery();
                    if (Watch_Face.Battery.Images == null) Watch_Face.Battery.Images = new ImageSet();

                    Watch_Face.Battery.Images.ImageIndex = Int32.Parse(comboBox_Battery_Img_Image.Text);
                    Watch_Face.Battery.Images.ImagesCount = (int)numericUpDown_Battery_Img_Count.Value;
                    Watch_Face.Battery.Images.X = (int)numericUpDown_Battery_Img_X.Value;
                    Watch_Face.Battery.Images.Y = (int)numericUpDown_Battery_Img_Y.Value;
                }

                if (checkBox_Battery_Scale.Checked)
                {
                    if (Watch_Face.Battery == null) Watch_Face.Battery = new Battery();
                    if (Watch_Face.Battery.Scale == null) Watch_Face.Battery.Scale = new CircleScale();

                    Watch_Face.Battery.Scale.CenterX = (int)numericUpDown_Battery_Scale_Center_X.Value;
                    Watch_Face.Battery.Scale.CenterY = (int)numericUpDown_Battery_Scale_Center_Y.Value;
                    Watch_Face.Battery.Scale.RadiusX = (int)numericUpDown_Battery_Scale_Radius_X.Value;
                    Watch_Face.Battery.Scale.RadiusY = (int)numericUpDown_Battery_Scale_Radius_Y.Value;

                    Watch_Face.Battery.Scale.StartAngle = (int)numericUpDown_Battery_Scale_StartAngle.Value;
                    Watch_Face.Battery.Scale.EndAngle = (int)numericUpDown_Battery_Scale_EndAngle.Value;
                    Watch_Face.Battery.Scale.Width = (int)numericUpDown_Battery_Scale_Width.Value;

                    Color color = comboBox_Battery_Scale_Color.BackColor;
                    Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
                    string colorStr = ColorTranslator.ToHtml(new_color);
                    colorStr = colorStr.Replace("#", "0x00");
                    Watch_Face.Battery.Scale.Color = colorStr;

                    switch (comboBox_Battery_Flatness.Text)
                    {
                        case "Треугольное":
                            Watch_Face.Battery.Scale.Flatness = 90;
                            break;
                        case "Плоское":
                            Watch_Face.Battery.Scale.Flatness = 180;
                            break;
                        default:
                            Watch_Face.Battery.Scale.Flatness = 0;
                            break;
                    }
                }
            }

            // стрелки
            if (checkBox_AnalogClock.Checked)
            {
                if ((checkBox_AnalogClock_Hour.Checked) && (comboBox_AnalogClock_Hour_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.AnalogDialFace == null) Watch_Face.AnalogDialFace = new Analogdialface();
                    if (Watch_Face.AnalogDialFace.Hours == null) Watch_Face.AnalogDialFace.Hours = new ClockHand();
                    if (Watch_Face.AnalogDialFace.Hours.CenterOffset == null)
                        Watch_Face.AnalogDialFace.Hours.CenterOffset = new Coordinates();
                    //if (Watch_Face.AnalogDialFace.Hours.Shape == null)
                    //Watch_Face.AnalogDialFace.Hours.Shape = new Coordinates();
                    if (Watch_Face.AnalogDialFace.Hours.Image == null)
                        Watch_Face.AnalogDialFace.Hours.Image = new ImageW();

                    Watch_Face.AnalogDialFace.Hours.Image.ImageIndex = Int32.Parse(comboBox_AnalogClock_Hour_Image.Text);
                    Watch_Face.AnalogDialFace.Hours.Image.X = (int)numericUpDown_AnalogClock_Hour_X.Value;
                    Watch_Face.AnalogDialFace.Hours.Image.Y = (int)numericUpDown_AnalogClock_Hour_Y.Value;

                    Watch_Face.AnalogDialFace.Hours.Color = "0x00000000";
                    Watch_Face.AnalogDialFace.Hours.OnlyBorder = false;

                    Watch_Face.AnalogDialFace.Hours.CenterOffset.X = (int)numericUpDown_AnalogClock_Hour_Offset_X.Value;
                    Watch_Face.AnalogDialFace.Hours.CenterOffset.Y = (int)numericUpDown_AnalogClock_Hour_Offset_Y.Value;
                }

                if ((checkBox_AnalogClock_Min.Checked) && (comboBox_AnalogClock_Min_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.AnalogDialFace == null) Watch_Face.AnalogDialFace = new Analogdialface();
                    if (Watch_Face.AnalogDialFace.Minutes == null) Watch_Face.AnalogDialFace.Minutes = new ClockHand();
                    if (Watch_Face.AnalogDialFace.Minutes.CenterOffset == null)
                        Watch_Face.AnalogDialFace.Minutes.CenterOffset = new Coordinates();
                    //if (Watch_Face.AnalogDialFace.Minutes.Shape == null)
                    //    Watch_Face.AnalogDialFace.Minutes.Shape = new Coordinates();
                    if (Watch_Face.AnalogDialFace.Minutes.Image == null)
                        Watch_Face.AnalogDialFace.Minutes.Image = new ImageW();

                    Watch_Face.AnalogDialFace.Minutes.Image.ImageIndex = Int32.Parse(comboBox_AnalogClock_Min_Image.Text);
                    Watch_Face.AnalogDialFace.Minutes.Image.X = (int)numericUpDown_AnalogClock_Min_X.Value;
                    Watch_Face.AnalogDialFace.Minutes.Image.Y = (int)numericUpDown_AnalogClock_Min_Y.Value;

                    Watch_Face.AnalogDialFace.Minutes.Color = "0x00000000";
                    Watch_Face.AnalogDialFace.Minutes.OnlyBorder = false;

                    Watch_Face.AnalogDialFace.Minutes.CenterOffset.X = (int)numericUpDown_AnalogClock_Min_Offset_X.Value;
                    Watch_Face.AnalogDialFace.Minutes.CenterOffset.Y = (int)numericUpDown_AnalogClock_Min_Offset_Y.Value;
                }

                if ((checkBox_AnalogClock_Sec.Checked) && (comboBox_AnalogClock_Sec_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.AnalogDialFace == null) Watch_Face.AnalogDialFace = new Analogdialface();
                    if (Watch_Face.AnalogDialFace.Seconds == null) Watch_Face.AnalogDialFace.Seconds = new ClockHand();
                    if (Watch_Face.AnalogDialFace.Seconds.CenterOffset == null)
                        Watch_Face.AnalogDialFace.Seconds.CenterOffset = new Coordinates();
                    //if (Watch_Face.AnalogDialFace.Seconds.Shape == null)
                    //    Watch_Face.AnalogDialFace.Seconds.Shape = new Coordinates();
                    if (Watch_Face.AnalogDialFace.Seconds.Image == null)
                        Watch_Face.AnalogDialFace.Seconds.Image = new ImageW();

                    Watch_Face.AnalogDialFace.Seconds.Image.ImageIndex = Int32.Parse(comboBox_AnalogClock_Sec_Image.Text);
                    Watch_Face.AnalogDialFace.Seconds.Image.X = (int)numericUpDown_AnalogClock_Sec_X.Value;
                    Watch_Face.AnalogDialFace.Seconds.Image.Y = (int)numericUpDown_AnalogClock_Sec_Y.Value;

                    Watch_Face.AnalogDialFace.Seconds.Color = "0x00000000";
                    Watch_Face.AnalogDialFace.Seconds.OnlyBorder = false;

                    Watch_Face.AnalogDialFace.Seconds.CenterOffset.X = (int)numericUpDown_AnalogClock_Sec_Offset_X.Value;
                    Watch_Face.AnalogDialFace.Seconds.CenterOffset.Y = (int)numericUpDown_AnalogClock_Sec_Offset_Y.Value;
                }

                if ((checkBox_HourCenterImage.Checked) && (comboBox_HourCenterImage_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.AnalogDialFace == null) Watch_Face.AnalogDialFace = new Analogdialface();
                    if (Watch_Face.AnalogDialFace.HourCenterImage == null)
                        Watch_Face.AnalogDialFace.HourCenterImage = new ImageW();

                    Watch_Face.AnalogDialFace.HourCenterImage.ImageIndex = Int32.Parse(comboBox_HourCenterImage_Image.Text);
                    Watch_Face.AnalogDialFace.HourCenterImage.X = (int)numericUpDown_HourCenterImage_X.Value;
                    Watch_Face.AnalogDialFace.HourCenterImage.Y = (int)numericUpDown_HourCenterImage_Y.Value;
                }

                if ((checkBox_MinCenterImage.Checked) && (comboBox_MinCenterImage_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.AnalogDialFace == null) Watch_Face.AnalogDialFace = new Analogdialface();
                    if (Watch_Face.AnalogDialFace.MinCenterImage == null)
                        Watch_Face.AnalogDialFace.MinCenterImage = new ImageW();

                    Watch_Face.AnalogDialFace.MinCenterImage.ImageIndex = Int32.Parse(comboBox_MinCenterImage_Image.Text);
                    Watch_Face.AnalogDialFace.MinCenterImage.X = (int)numericUpDown_MinCenterImage_X.Value;
                    Watch_Face.AnalogDialFace.MinCenterImage.Y = (int)numericUpDown_MinCenterImage_Y.Value;
                }

                if ((checkBox_SecCenterImage.Checked) && (comboBox_SecCenterImage_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.AnalogDialFace == null) Watch_Face.AnalogDialFace = new Analogdialface();
                    if (Watch_Face.AnalogDialFace.SecCenterImage == null)
                        Watch_Face.AnalogDialFace.SecCenterImage = new ImageW();

                    Watch_Face.AnalogDialFace.SecCenterImage.ImageIndex = Int32.Parse(comboBox_SecCenterImage_Image.Text);
                    Watch_Face.AnalogDialFace.SecCenterImage.X = (int)numericUpDown_SecCenterImage_X.Value;
                    Watch_Face.AnalogDialFace.SecCenterImage.Y = (int)numericUpDown_SecCenterImage_Y.Value;
                }
            }

            // погода 
            if (checkBox_Weather.Checked)
            {
                if ((checkBox_Weather_Text.Checked) && (comboBox_Weather_Text_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Weather == null) Watch_Face.Weather = new Weather();
                    if (Watch_Face.Weather.Temperature == null) Watch_Face.Weather.Temperature = new Temperature();
                    if (Watch_Face.Weather.Temperature.Current == null)
                        Watch_Face.Weather.Temperature.Current = new Number();

                    Watch_Face.Weather.Temperature.Current.ImageIndex = Int32.Parse(comboBox_Weather_Text_Image.Text);
                    Watch_Face.Weather.Temperature.Current.ImagesCount = (int)numericUpDown_Weather_Text_Count.Value;
                    Watch_Face.Weather.Temperature.Current.TopLeftX = (int)numericUpDown_Weather_Text_StartCorner_X.Value;
                    Watch_Face.Weather.Temperature.Current.TopLeftY = (int)numericUpDown_Weather_Text_StartCorner_Y.Value;
                    Watch_Face.Weather.Temperature.Current.BottomRightX = (int)numericUpDown_Weather_Text_EndCorner_X.Value;
                    Watch_Face.Weather.Temperature.Current.BottomRightY = (int)numericUpDown_Weather_Text_EndCorner_Y.Value;

                    Watch_Face.Weather.Temperature.Current.Spacing = (int)numericUpDown_Weather_Text_Spacing.Value;
                    string Alignment = StringToAlignment(comboBox_Weather_Text_Alignment.Text);
                    Watch_Face.Weather.Temperature.Current.Alignment = Alignment;

                    if ((comboBox_Weather_Text_MinusImage.SelectedIndex >= 0) ||
                        (comboBox_Weather_Text_DegImage.SelectedIndex >= 0) ||
                        (comboBox_Weather_Text_NDImage.SelectedIndex >= 0))
                    {
                        if (Watch_Face.Weather.Temperature.Symbols == null)
                            Watch_Face.Weather.Temperature.Symbols = new Symbols();
                        Watch_Face.Weather.Temperature.Symbols.Unknown0800 = 0;
                        if (comboBox_Weather_Text_MinusImage.SelectedIndex >= 0)
                            Watch_Face.Weather.Temperature.Symbols.MinusImageIndex =
                                    Int32.Parse(comboBox_Weather_Text_MinusImage.Text);
                        if (comboBox_Weather_Text_DegImage.SelectedIndex >= 0)
                            Watch_Face.Weather.Temperature.Symbols.DegreesImageIndex =
                                    Int32.Parse(comboBox_Weather_Text_DegImage.Text);
                        if (comboBox_Weather_Text_NDImage.SelectedIndex >= 0)
                            Watch_Face.Weather.Temperature.Symbols.NoDataImageIndex =
                                    Int32.Parse(comboBox_Weather_Text_NDImage.Text);
                    }
                }

                if ((checkBox_Weather_Day.Checked) && (comboBox_Weather_Day_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Weather == null) Watch_Face.Weather = new Weather();
                    if (Watch_Face.Weather.Temperature == null) Watch_Face.Weather.Temperature = new Temperature();
                    if (Watch_Face.Weather.Temperature.Today == null)
                        Watch_Face.Weather.Temperature.Today = new Today();
                    if (Watch_Face.Weather.Temperature.Today.Separate == null)
                        Watch_Face.Weather.Temperature.Today.Separate = new Separate();
                    if (Watch_Face.Weather.Temperature.Today.Separate.Day == null)
                        Watch_Face.Weather.Temperature.Today.Separate.Day = new Number();

                    Watch_Face.Weather.Temperature.Today.Separate.Day.ImageIndex =
                        Int32.Parse(comboBox_Weather_Day_Image.Text);
                    Watch_Face.Weather.Temperature.Today.Separate.Day.ImagesCount =
                        (int)numericUpDown_Weather_Day_Count.Value;
                    Watch_Face.Weather.Temperature.Today.Separate.Day.TopLeftX =
                        (int)numericUpDown_Weather_Day_StartCorner_X.Value;
                    Watch_Face.Weather.Temperature.Today.Separate.Day.TopLeftY =
                        (int)numericUpDown_Weather_Day_StartCorner_Y.Value;
                    Watch_Face.Weather.Temperature.Today.Separate.Day.BottomRightX =
                        (int)numericUpDown_Weather_Day_EndCorner_X.Value;
                    Watch_Face.Weather.Temperature.Today.Separate.Day.BottomRightY =
                        (int)numericUpDown_Weather_Day_EndCorner_Y.Value;

                    Watch_Face.Weather.Temperature.Today.Separate.Day.Spacing =
                        (int)numericUpDown_Weather_Day_Spacing.Value;
                    string Alignment = StringToAlignment(comboBox_Weather_Day_Alignment.Text);
                    Watch_Face.Weather.Temperature.Today.Separate.Day.Alignment = Alignment;
                    Watch_Face.Weather.Temperature.Today.AppendDegreesForBoth = true;

                    if ((comboBox_Weather_Text_MinusImage.SelectedIndex >= 0) ||
                        (comboBox_Weather_Text_DegImage.SelectedIndex >= 0) ||
                        (comboBox_Weather_Text_NDImage.SelectedIndex >= 0))
                    {
                        if (Watch_Face.Weather.Temperature.Symbols == null)
                            Watch_Face.Weather.Temperature.Symbols = new Symbols();
                        Watch_Face.Weather.Temperature.Symbols.Unknown0800 = 0;
                        if (comboBox_Weather_Text_MinusImage.SelectedIndex >= 0)
                            Watch_Face.Weather.Temperature.Symbols.MinusImageIndex =
                                    Int32.Parse(comboBox_Weather_Text_MinusImage.Text);
                        if (comboBox_Weather_Text_DegImage.SelectedIndex >= 0)
                            Watch_Face.Weather.Temperature.Symbols.DegreesImageIndex =
                                    Int32.Parse(comboBox_Weather_Text_DegImage.Text);
                        if (comboBox_Weather_Text_NDImage.SelectedIndex >= 0)
                            Watch_Face.Weather.Temperature.Symbols.NoDataImageIndex =
                                    Int32.Parse(comboBox_Weather_Text_NDImage.Text);
                    }
                }

                if ((checkBox_Weather_Night.Checked) && (comboBox_Weather_Night_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Weather == null) Watch_Face.Weather = new Weather();
                    if (Watch_Face.Weather.Temperature == null) Watch_Face.Weather.Temperature = new Temperature();
                    if (Watch_Face.Weather.Temperature.Today == null)
                        Watch_Face.Weather.Temperature.Today = new Today();
                    if (Watch_Face.Weather.Temperature.Today.Separate == null)
                        Watch_Face.Weather.Temperature.Today.Separate = new Separate();
                    if (Watch_Face.Weather.Temperature.Today.Separate.Night == null)
                        Watch_Face.Weather.Temperature.Today.Separate.Night = new Number();

                    Watch_Face.Weather.Temperature.Today.Separate.Night.ImageIndex =
                        Int32.Parse(comboBox_Weather_Night_Image.Text);
                    Watch_Face.Weather.Temperature.Today.Separate.Night.ImagesCount =
                        (int)numericUpDown_Weather_Night_Count.Value;
                    Watch_Face.Weather.Temperature.Today.Separate.Night.TopLeftX =
                        (int)numericUpDown_Weather_Night_StartCorner_X.Value;
                    Watch_Face.Weather.Temperature.Today.Separate.Night.TopLeftY =
                        (int)numericUpDown_Weather_Night_StartCorner_Y.Value;
                    Watch_Face.Weather.Temperature.Today.Separate.Night.BottomRightX =
                        (int)numericUpDown_Weather_Night_EndCorner_X.Value;
                    Watch_Face.Weather.Temperature.Today.Separate.Night.BottomRightY =
                        (int)numericUpDown_Weather_Night_EndCorner_Y.Value;

                    Watch_Face.Weather.Temperature.Today.Separate.Night.Spacing =
                        (int)numericUpDown_Weather_Night_Spacing.Value;
                    string Alignment = StringToAlignment(comboBox_Weather_Night_Alignment.Text);
                    Watch_Face.Weather.Temperature.Today.Separate.Night.Alignment = Alignment;
                    Watch_Face.Weather.Temperature.Today.AppendDegreesForBoth = true;

                    if ((comboBox_Weather_Text_MinusImage.SelectedIndex >= 0) ||
                        (comboBox_Weather_Text_DegImage.SelectedIndex >= 0) ||
                        (comboBox_Weather_Text_NDImage.SelectedIndex >= 0))
                    {
                        if (Watch_Face.Weather.Temperature.Symbols == null)
                            Watch_Face.Weather.Temperature.Symbols = new Symbols();
                        Watch_Face.Weather.Temperature.Symbols.Unknown0800 = 0;
                        if (comboBox_Weather_Text_MinusImage.SelectedIndex >= 0)
                            Watch_Face.Weather.Temperature.Symbols.MinusImageIndex =
                                    Int32.Parse(comboBox_Weather_Text_MinusImage.Text);
                        if (comboBox_Weather_Text_DegImage.SelectedIndex >= 0)
                            Watch_Face.Weather.Temperature.Symbols.DegreesImageIndex =
                                    Int32.Parse(comboBox_Weather_Text_DegImage.Text);
                        if (comboBox_Weather_Text_NDImage.SelectedIndex >= 0)
                            Watch_Face.Weather.Temperature.Symbols.NoDataImageIndex =
                                    Int32.Parse(comboBox_Weather_Text_NDImage.Text);
                    }
                }

                if ((checkBox_Weather_Icon.Checked) && (comboBox_Weather_Icon_Image.SelectedIndex >= 0)
                    && (comboBox_Weather_Icon_NDImage.SelectedIndex >= 0))
                {
                    // numericUpDown_Weather_Icon_X
                    if (Watch_Face.Weather == null) Watch_Face.Weather = new Weather();
                    if (Watch_Face.Weather.Icon == null) Watch_Face.Weather.Icon = new IconW();
                    if (Watch_Face.Weather.Icon.Images == null) Watch_Face.Weather.Icon.Images = new ImageSet();

                    Watch_Face.Weather.Icon.Images.X = (int)numericUpDown_Weather_Icon_X.Value;
                    Watch_Face.Weather.Icon.Images.Y = (int)numericUpDown_Weather_Icon_Y.Value;
                    Watch_Face.Weather.Icon.Images.ImagesCount = (int)numericUpDown_Weather_Icon_Count.Value;
                    Watch_Face.Weather.Icon.Images.ImageIndex = Int32.Parse(comboBox_Weather_Icon_Image.Text);
                    Watch_Face.Weather.Icon.NoWeatherImageIndex = Int32.Parse(comboBox_Weather_Icon_NDImage.Text);
                }
            }


            richTextBox_JSON.Text = JsonConvert.SerializeObject(Watch_Face, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        private string AlignmentToString(string Alignment)
        {
            string result = "Середина по центру";
            switch (Alignment)
            {
                case "TopLeft":
                    result = "Вверх влево";
                    break;
                case "TopCenter":
                    result = "Вверх по центру";
                    break;
                case "TopRight":
                    result = "Вверх вправо";
                    break;

                case "CenterLeft":
                    result = "Середина влево";
                    break;
                case "Center":
                    result = "Середина по центру";
                    break;
                case "CenterRight":
                    result = "Середина вправо";
                    break;

                case "BottomLeft":
                    result = "Вниз влево";
                    break;
                case "BottomCenter":
                    result = "Вниз по центру";
                    break;
                case "BottomRight":
                    result = "Вниз вправо";
                    break;

                case "Left":
                    result = "Середина влево";
                    break;
                case "Right":
                    result = "Середина вправо";
                    break;
                case "Top":
                    result = "Вверх по центру";
                    break;
                case "Bottom":
                    result = "Вниз по центру";
                    break;

                default:
                    result = "Середина по центру";
                    break;

            }
            return result;
        }

        private string StringToAlignment(string Alignment)
        {
            string result = "Center";
            switch (Alignment)
            {
                case "Вверх влево":
                    result = "TopLeft";
                    break;
                case "Вверх по центру":
                    result = "TopCenter";
                    break;
                case "Вверх вправо":
                    result = "TopRight";
                    break;

                case "Середина влево":
                    result = "CenterLeft";
                    break;
                case "Center":
                    result = "CenterLeft";
                    break;
                case "Середина вправо":
                    result = "CenterRight";
                    break;

                case "Вниз влево":
                    result = "BottomLeft";
                    break;
                case "Вниз по центру":
                    result = "BottomCenter";
                    break;
                case "Вниз вправо":
                    result = "BottomRight";
                    break;

                default:
                    result = "Center";
                    break;

            }
            return result;
        }

        private void checkBoxSetText(ComboBox comboBox, long value)
        {
            //for (int i = 0; i < comboBox.Items.Count; i++)
            //{
            //    if(value.ToString().Equals(comboBox.Items[i].ToString()))
            //    {
            //        //comboBox.SelectedIndex = i;
            //        comboBox.Text = value.ToString();
            //        break;
            //    }
            //}
            comboBox.Text = value.ToString();
            if (comboBox.SelectedIndex < 0) comboBox.Text = "";
        }




    }
}
