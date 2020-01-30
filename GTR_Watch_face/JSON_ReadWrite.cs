using Newtonsoft.Json;
using System;
using System.Drawing;
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
            comboBox_DOW_IconSet_Image.Items.AddRange(ListImages.ToArray());
            comboBox_OneLine_Image.Items.AddRange(ListImages.ToArray());
            comboBox_OneLine_Delimiter.Items.AddRange(ListImages.ToArray());
            comboBox_MonthAndDayD_Image.Items.AddRange(ListImages.ToArray());
            comboBox_MonthAndDayM_Image.Items.AddRange(ListImages.ToArray());
            comboBox_MonthName_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Year_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Year_Delimiter.Items.AddRange(ListImages.ToArray());
            comboBox_ADDay_ClockHand_Image.Items.AddRange(ListImages.ToArray());
            comboBox_ADWeekDay_ClockHand_Image.Items.AddRange(ListImages.ToArray());
            comboBox_ADMonth_ClockHand_Image.Items.AddRange(ListImages.ToArray());

            comboBox_StProg_ClockHand_Image.Items.AddRange(ListImages.ToArray());
            comboBox_SPSliced_Image.Items.AddRange(ListImages.ToArray());
            comboBox_StepsProgress_Image.Items.AddRange(ListImages.ToArray());

            comboBox_ActivityCaloriesScale_Image.Items.AddRange(ListImages.ToArray());
            comboBox_ActivitySteps_Image.Items.AddRange(ListImages.ToArray());
            comboBox_ActivityDistance_Image.Items.AddRange(ListImages.ToArray());
            comboBox_ActivityDistance_Decimal.Items.AddRange(ListImages.ToArray());
            comboBox_ActivityDistance_Suffix.Items.AddRange(ListImages.ToArray());
            comboBox_ActivityPulsScale_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Pulse_ClockHand_Image.Items.AddRange(ListImages.ToArray());
            comboBox_ActivityPuls_Image.Items.AddRange(ListImages.ToArray());
            comboBox_ActivityPuls_IconSet_Image.Items.AddRange(ListImages.ToArray());
            comboBox_ActivityCalories_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Calories_ClockHand_Image.Items.AddRange(ListImages.ToArray());
            comboBox_ActivityStar_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Activity_NDImage.Items.AddRange(ListImages.ToArray());

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
            comboBox_Battery_ClockHand_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Battery_IconSet_Image.Items.AddRange(ListImages.ToArray());
            comboBox_Battery_Scale_Image.Items.AddRange(ListImages.ToArray());

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
            if (Watch_Face.Info != null) ReadDeviceId();
            if (Watch_Face.Background != null)
            {
                if (Watch_Face.Background.Image != null)
                    //comboBox_Background.Text = Watch_Face.Background.Image.ImageIndex.ToString();
                    comboBoxSetText(comboBox_Background, Watch_Face.Background.Image.ImageIndex);
                if (Watch_Face.Background.Preview != null)
                    //comboBox_Preview.Text comboBox_Preview Watch_Face.Background.Preview.ImageIndex.ToString();
                    comboBoxSetText(comboBox_Preview, Watch_Face.Background.Preview.ImageIndex);
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
                    comboBoxSetText(comboBox_Hours_Tens_Image, Watch_Face.Time.Hours.Tens.ImageIndex);

                    numericUpDown_Hours_Ones_X.Value = Watch_Face.Time.Hours.Ones.X;
                    numericUpDown_Hours_Ones_Y.Value = Watch_Face.Time.Hours.Ones.Y;
                    numericUpDown_Hours_Ones_Count.Value = Watch_Face.Time.Hours.Ones.ImagesCount;
                    //comboBox_Hours_Ones_Image.Text = Watch_Face.Time.Hours.Ones.ImageIndex.ToString();
                    comboBoxSetText(comboBox_Hours_Ones_Image, Watch_Face.Time.Hours.Ones.ImageIndex);
                }
                else checkBox_Hours.Checked = false;

                if (Watch_Face.Time.Minutes != null)
                {
                    checkBox_Minutes.Checked = true;
                    numericUpDown_Min_Tens_X.Value = Watch_Face.Time.Minutes.Tens.X;
                    numericUpDown_Min_Tens_Y.Value = Watch_Face.Time.Minutes.Tens.Y;
                    numericUpDown_Min_Tens_Count.Value = Watch_Face.Time.Minutes.Tens.ImagesCount;
                    //comboBox_Min_Tens_Image.Text = Watch_Face.Time.Minutes.Tens.ImageIndex.ToString();
                    comboBoxSetText(comboBox_Min_Tens_Image, Watch_Face.Time.Minutes.Tens.ImageIndex);

                    numericUpDown_Min_Ones_X.Value = Watch_Face.Time.Minutes.Ones.X;
                    numericUpDown_Min_Ones_Y.Value = Watch_Face.Time.Minutes.Ones.Y;
                    numericUpDown_Min_Ones_Count.Value = Watch_Face.Time.Minutes.Ones.ImagesCount;
                    //comboBox_Min_Ones_Image.Text = Watch_Face.Time.Minutes.Ones.ImageIndex.ToString();
                    comboBoxSetText(comboBox_Min_Ones_Image, Watch_Face.Time.Minutes.Ones.ImageIndex);
                }
                else checkBox_Minutes.Checked = false;

                if (Watch_Face.Time.Seconds != null)
                {
                    checkBox_Seconds.Checked = true;
                    numericUpDown_Sec_Tens_X.Value = Watch_Face.Time.Seconds.Tens.X;
                    numericUpDown_Sec_Tens_Y.Value = Watch_Face.Time.Seconds.Tens.Y;
                    numericUpDown_Sec_Tens_Count.Value = Watch_Face.Time.Seconds.Tens.ImagesCount;
                    //comboBox_Sec_Tens_Image.Text = Watch_Face.Time.Seconds.Tens.ImageIndex.ToString();
                    comboBoxSetText(comboBox_Sec_Tens_Image, Watch_Face.Time.Seconds.Tens.ImageIndex);

                    numericUpDown_Sec_Ones_X.Value = Watch_Face.Time.Seconds.Ones.X;
                    numericUpDown_Sec_Ones_Y.Value = Watch_Face.Time.Seconds.Ones.Y;
                    numericUpDown_Sec_Ones_Count.Value = Watch_Face.Time.Seconds.Ones.ImagesCount;
                    //comboBox_Sec_Ones_Image.Text = Watch_Face.Time.Seconds.Ones.ImageIndex.ToString();
                    comboBoxSetText(comboBox_Sec_Ones_Image, Watch_Face.Time.Seconds.Ones.ImageIndex);
                }
                else checkBox_Seconds.Checked = false;

                if (Watch_Face.Time.Delimiter != null)
                {
                    checkBox_Delimiter.Checked = true;
                    numericUpDown_Delimiter_X.Value = Watch_Face.Time.Delimiter.X;
                    numericUpDown_Delimiter_Y.Value = Watch_Face.Time.Delimiter.Y;
                    //comboBox_Delimiter_Image.Text = Watch_Face.Time.Delimiter.ImageIndex.ToString();
                    comboBoxSetText(comboBox_Delimiter_Image, Watch_Face.Time.Delimiter.ImageIndex);
                }
                else checkBox_Delimiter.Checked = false;

                if (Watch_Face.Time.AmPm != null)
                {
                    checkBox_AmPm.Checked = true;
                    numericUpDown_AmPm_X.Value = Watch_Face.Time.AmPm.X;
                    numericUpDown_AmPm_Y.Value = Watch_Face.Time.AmPm.Y;
                    if (Watch_Face.Time.AmPm.ImageIndexAMCN > 0)
                        //comboBox_Image_Am.Text = Watch_Face.Time.AmPm.ImageIndexAMCN.ToString();
                        comboBoxSetText(comboBox_Image_Am, Watch_Face.Time.AmPm.ImageIndexAMCN);
                    if (Watch_Face.Time.AmPm.ImageIndexAMEN > 0)
                        //comboBox_Image_Am.Text = Watch_Face.Time.AmPm.ImageIndexAMEN.ToString();
                        comboBoxSetText(comboBox_Image_Am, Watch_Face.Time.AmPm.ImageIndexAMEN);
                    if (Watch_Face.Time.AmPm.ImageIndexPMCN > 0)
                        //comboBox_Image_Pm.Text = Watch_Face.Time.AmPm.ImageIndexPMCN.ToString();
                        comboBoxSetText(comboBox_Image_Pm, Watch_Face.Time.AmPm.ImageIndexPMCN);
                    if (Watch_Face.Time.AmPm.ImageIndexPMEN > 0)
                        //comboBox_Image_Pm.Text = Watch_Face.Time.AmPm.ImageIndexPMEN.ToString();
                        comboBoxSetText(comboBox_Image_Pm, Watch_Face.Time.AmPm.ImageIndexPMEN);
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
                    comboBoxSetText(comboBox_WeekDay_Image, Watch_Face.Date.WeekDay.ImageIndex);
                }
                else checkBox_WeekDay.Checked = false;

                if ((Watch_Face.Date.WeekDayProgress != null) && (Watch_Face.Date.WeekDayProgress.Coordinates != null))
                {
                    checkBox_DOW_IconSet.Checked = true;
                    dataGridView_DOW_IconSet.Rows.Clear();
                    comboBoxSetText(comboBox_DOW_IconSet_Image, Watch_Face.Date.WeekDayProgress.ImageIndex);
                    foreach (Coordinates coordinates in Watch_Face.Date.WeekDayProgress.Coordinates)
                    {
                        var RowNew = new DataGridViewRow();
                        dataGridView_DOW_IconSet.Rows.Add(coordinates.X, coordinates.Y);
                    }
                }
                else checkBox_DOW_IconSet.Checked = false;

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
                        comboBoxSetText(comboBox_OneLine_Image, Watch_Face.Date.MonthAndDay.OneLine.Number.ImageIndex);
                        //comboBox_OneLine_Delimiter.Text = Watch_Face.Date.MonthAndDay.OneLine.DelimiterImageIndex.ToString();
                        if (Watch_Face.Date.MonthAndDay.OneLine.DelimiterImageIndex != null)
                            comboBoxSetText(comboBox_OneLine_Delimiter, (long)Watch_Face.Date.MonthAndDay.OneLine.DelimiterImageIndex);
                        AlignmentToString(comboBox_OneLine_Alignment, Watch_Face.Date.MonthAndDay.OneLine.Number.Alignment);
                        //comboBox_OneLine_Alignment.Text = Alignment;
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
                            comboBoxSetText(comboBox_MonthAndDayD_Image, Watch_Face.Date.MonthAndDay.Separate.Day.ImageIndex);
                            AlignmentToString(comboBox_MonthAndDayD_Alignment, Watch_Face.Date.MonthAndDay.Separate.Day.Alignment);
                            //comboBox_MonthAndDayD_Alignment.Text = Alignment;
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
                            comboBoxSetText(comboBox_MonthAndDayM_Image, Watch_Face.Date.MonthAndDay.Separate.Month.ImageIndex);
                            AlignmentToString(comboBox_MonthAndDayM_Alignment,Watch_Face.Date.MonthAndDay.Separate.Month.Alignment);
                            //comboBox_MonthAndDayM_Alignment.Text = Alignment;
                        }
                        else checkBox_MonthAndDayM.Checked = false;

                        if (Watch_Face.Date.MonthAndDay.Separate.MonthName != null)
                        {
                            checkBox_MonthName.Checked = true;
                            numericUpDown_MonthName_X.Value = Watch_Face.Date.MonthAndDay.Separate.MonthName.X;
                            numericUpDown_MonthName_Y.Value = Watch_Face.Date.MonthAndDay.Separate.MonthName.Y;

                            numericUpDown_MonthName_Count.Value = Watch_Face.Date.MonthAndDay.Separate.MonthName.ImagesCount;
                            //comboBox_MonthName_Image.Text = Watch_Face.Date.MonthAndDay.Separate.MonthName.ImageIndex.ToString();
                            comboBoxSetText(comboBox_MonthName_Image, Watch_Face.Date.MonthAndDay.Separate.MonthName.ImageIndex);
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
                        comboBoxSetText(comboBox_Year_Image, Watch_Face.Date.Year.OneLine.Number.ImageIndex);
                        if (Watch_Face.Date.Year.OneLine.DelimiterImageIndex != null)
                        comboBoxSetText(comboBox_Year_Delimiter, (long)Watch_Face.Date.Year.OneLine.DelimiterImageIndex);
                        AlignmentToString(comboBox_Year_Alignment, Watch_Face.Date.Year.OneLine.Number.Alignment);
                        //comboBox_Year_Alignment.Text = Alignment;
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

            #region AnalogDate
            if (Watch_Face.DaysProgress != null)
            {
                if ((Watch_Face.DaysProgress.UnknownField2 != null) && (Watch_Face.DaysProgress.UnknownField2.Image != null))
                {
                    checkBox_ADDay_ClockHand.Checked = true;
                    numericUpDown_ADDay_ClockHand_X.Value = Watch_Face.DaysProgress.UnknownField2.Image.X;
                    numericUpDown_ADDay_ClockHand_Y.Value = Watch_Face.DaysProgress.UnknownField2.Image.Y;
                    comboBoxSetText(comboBox_ADDay_ClockHand_Image, Watch_Face.DaysProgress.UnknownField2.Image.ImageIndex);
                    if (Watch_Face.DaysProgress.UnknownField2.CenterOffset != null)
                    {
                        numericUpDown_ADDay_ClockHand_Offset_X.Value = Watch_Face.DaysProgress.UnknownField2.CenterOffset.X;
                        numericUpDown_ADDay_ClockHand_Offset_Y.Value = Watch_Face.DaysProgress.UnknownField2.CenterOffset.Y;

                    }
                    if (Watch_Face.DaysProgress.UnknownField2.Sector != null)
                    {
                        numericUpDown_ADDay_ClockHand_StartAngle.Value = (int)(Watch_Face.DaysProgress.UnknownField2.Sector.StartAngle / 100);
                        numericUpDown_ADDay_ClockHand_EndAngle.Value = (int)(Watch_Face.DaysProgress.UnknownField2.Sector.EndAngle / 100);

                    }
                }
                else checkBox_ADDay_ClockHand.Checked = false;

                if ((Watch_Face.DaysProgress.AnalogDOW != null) && (Watch_Face.DaysProgress.AnalogDOW.Image != null))
                {
                    checkBox_ADWeekDay_ClockHand.Checked = true;
                    numericUpDown_ADWeekDay_ClockHand_X.Value = Watch_Face.DaysProgress.AnalogDOW.Image.X;
                    numericUpDown_ADWeekDay_ClockHand_Y.Value = Watch_Face.DaysProgress.AnalogDOW.Image.Y;
                    comboBoxSetText(comboBox_ADWeekDay_ClockHand_Image, Watch_Face.DaysProgress.AnalogDOW.Image.ImageIndex);
                    if (Watch_Face.DaysProgress.AnalogDOW.CenterOffset != null)
                    {
                        numericUpDown_ADWeekDay_ClockHand_Offset_X.Value = Watch_Face.DaysProgress.AnalogDOW.CenterOffset.X;
                        numericUpDown_ADWeekDay_ClockHand_Offset_Y.Value = Watch_Face.DaysProgress.AnalogDOW.CenterOffset.Y;

                    }
                    if (Watch_Face.DaysProgress.AnalogDOW.Sector != null)
                    {
                        numericUpDown_ADWeekDay_ClockHand_StartAngle.Value = (int)(Watch_Face.DaysProgress.AnalogDOW.Sector.StartAngle / 100);
                        numericUpDown_ADWeekDay_ClockHand_EndAngle.Value = (int)(Watch_Face.DaysProgress.AnalogDOW.Sector.EndAngle / 100);

                    }
                }
                else checkBox_ADWeekDay_ClockHand.Checked = false;

                if ((Watch_Face.DaysProgress.AnalogMonth != null) && (Watch_Face.DaysProgress.AnalogMonth.Image != null))
                {
                    checkBox_ADMonth_ClockHand.Checked = true;
                    numericUpDown_ADMonth_ClockHand_X.Value = Watch_Face.DaysProgress.AnalogMonth.Image.X;
                    numericUpDown_ADMonth_ClockHand_Y.Value = Watch_Face.DaysProgress.AnalogMonth.Image.Y;
                    comboBoxSetText(comboBox_ADMonth_ClockHand_Image, Watch_Face.DaysProgress.AnalogMonth.Image.ImageIndex);
                    if (Watch_Face.DaysProgress.AnalogMonth.CenterOffset != null)
                    {
                        numericUpDown_ADMonth_ClockHand_Offset_X.Value = Watch_Face.DaysProgress.AnalogMonth.CenterOffset.X;
                        numericUpDown_ADMonth_ClockHand_Offset_Y.Value = Watch_Face.DaysProgress.AnalogMonth.CenterOffset.Y;

                    }
                    if (Watch_Face.DaysProgress.AnalogMonth.Sector != null)
                    {
                        numericUpDown_ADMonth_ClockHand_StartAngle.Value = (int)(Watch_Face.DaysProgress.AnalogMonth.Sector.StartAngle / 100);
                        numericUpDown_ADMonth_ClockHand_EndAngle.Value = (int)(Watch_Face.DaysProgress.AnalogMonth.Sector.EndAngle / 100);

                    }
                }
                else checkBox_ADMonth_ClockHand.Checked = false;

            }
            else
            {
                checkBox_ADDay_ClockHand.Checked = false;
                checkBox_ADWeekDay_ClockHand.Checked = false;
                checkBox_ADMonth_ClockHand.Checked = false;
            }
            #endregion

            #region StepsProgress
            if (Watch_Face.StepsProgress != null)
            {
                if (Watch_Face.StepsProgress.Circle != null)
                {
                    checkBox_StepsProgress.Checked = true;
                    numericUpDown_StepsProgress_Center_X.Value = Watch_Face.StepsProgress.Circle.CenterX;
                    numericUpDown_StepsProgress_Center_Y.Value = Watch_Face.StepsProgress.Circle.CenterY;
                    numericUpDown_StepsProgress_Radius_X.Value = Watch_Face.StepsProgress.Circle.RadiusX;
                    numericUpDown_StepsProgress_Radius_Y.Value = Watch_Face.StepsProgress.Circle.RadiusY;

                    numericUpDown_StepsProgress_StartAngle.Value = Watch_Face.StepsProgress.Circle.StartAngle;
                    numericUpDown_StepsProgress_EndAngle.Value = Watch_Face.StepsProgress.Circle.EndAngle;
                    numericUpDown_StepsProgress_Width.Value = Watch_Face.StepsProgress.Circle.Width;
                    
                    Color new_color = ColorRead(Watch_Face.StepsProgress.Circle.Color);
                    comboBox_StepsProgress_Color.BackColor = new_color;
                    colorDialog_StepsProgress.Color = new_color;
                    switch (Watch_Face.StepsProgress.Circle.Flatness)
                    {
                        case 90:
                            //comboBox_StepsProgress_Flatness.Text = "Треугольное";
                            comboBox_StepsProgress_Flatness.SelectedIndex = 1;
                            break;
                        case 180:
                            //comboBox_StepsProgress_Flatness.Text = "Плоское";
                            comboBox_StepsProgress_Flatness.SelectedIndex = 2;
                            break;
                        default:
                            //comboBox_StepsProgress_Flatness.Text = "Круглое";
                            comboBox_StepsProgress_Flatness.SelectedIndex = 0;
                            break;
                    }

                    if (Watch_Face.StepsProgress.Circle.ImageIndex != null)
                    {
                        comboBoxSetText(comboBox_StepsProgress_Image, (long)Watch_Face.StepsProgress.Circle.ImageIndex);
                        ColorToCoodinates(new_color, numericUpDown_StepsProgress_ImageX,
                            numericUpDown_StepsProgress_ImageY);
                        checkBox_StepsProgress_Image.Checked = true;
                    }
                    else checkBox_StepsProgress_Image.Checked = false;
                }
                else checkBox_StepsProgress.Checked = false;

                if ((Watch_Face.StepsProgress.ClockHand != null) && (Watch_Face.StepsProgress.ClockHand.Image != null))
                {
                    checkBox_StProg_ClockHand.Checked = true;
                    numericUpDown_StProg_ClockHand_X.Value = Watch_Face.StepsProgress.ClockHand.Image.X;
                    numericUpDown_StProg_ClockHand_Y.Value = Watch_Face.StepsProgress.ClockHand.Image.Y;
                    comboBoxSetText(comboBox_StProg_ClockHand_Image, Watch_Face.StepsProgress.ClockHand.Image.ImageIndex);
                    if (Watch_Face.StepsProgress.ClockHand.CenterOffset != null)
                    {
                        numericUpDown_StProg_ClockHand_Offset_X.Value = Watch_Face.StepsProgress.ClockHand.CenterOffset.X;
                        numericUpDown_StProg_ClockHand_Offset_Y.Value = Watch_Face.StepsProgress.ClockHand.CenterOffset.Y;

                    }
                    if (Watch_Face.StepsProgress.ClockHand.Sector != null)
                    {
                        numericUpDown_StProg_ClockHand_StartAngle.Value = (int)(Watch_Face.StepsProgress.ClockHand.Sector.StartAngle / 100);
                        numericUpDown_StProg_ClockHand_EndAngle.Value = (int)(Watch_Face.StepsProgress.ClockHand.Sector.EndAngle / 100);

                    }
                }
                else checkBox_StProg_ClockHand.Checked = false;

                if ((Watch_Face.StepsProgress.Sliced != null) && (Watch_Face.StepsProgress.Sliced.Coordinates != null))
                {
                    checkBox_SPSliced.Checked = true;
                    dataGridView_SPSliced.Rows.Clear();
                    comboBoxSetText(comboBox_SPSliced_Image, Watch_Face.StepsProgress.Sliced.ImageIndex);
                    foreach (Coordinates coordinates in Watch_Face.StepsProgress.Sliced.Coordinates)
                    {
                        var RowNew = new DataGridViewRow();
                        dataGridView_SPSliced.Rows.Add(coordinates.X, coordinates.Y);
                    }
                }
                else checkBox_SPSliced.Checked = false;

            }
            else
            {
                checkBox_StepsProgress.Checked = false;
                checkBox_StepsProgress_Image.Checked = false;
                checkBox_StProg_ClockHand.Checked = false;
                checkBox_SPSliced.Checked = false;
            }
            #endregion

            #region Activity
            if (Watch_Face.Activity != null)
            {
                checkBox_Activity.Checked = true;
                
                if ((Watch_Face.Activity.Steps != null) && (Watch_Face.Activity.Steps.Step != null))
                {
                    checkBox_ActivitySteps.Checked = true;
                    numericUpDown_ActivitySteps_StartCorner_X.Value = Watch_Face.Activity.Steps.Step.TopLeftX;
                    numericUpDown_ActivitySteps_StartCorner_Y.Value = Watch_Face.Activity.Steps.Step.TopLeftY;
                    numericUpDown_ActivitySteps_EndCorner_X.Value = Watch_Face.Activity.Steps.Step.BottomRightX;
                    numericUpDown_ActivitySteps_EndCorner_Y.Value = Watch_Face.Activity.Steps.Step.BottomRightY;

                    //comboBox_ActivitySteps_Image.Text = Watch_Face.Activity.Steps.Step.ImageIndex.ToString();
                    comboBoxSetText(comboBox_ActivitySteps_Image, Watch_Face.Activity.Steps.Step.ImageIndex);
                    numericUpDown_ActivitySteps_Count.Value = Watch_Face.Activity.Steps.Step.ImagesCount;
                    numericUpDown_ActivitySteps_Spacing.Value = Watch_Face.Activity.Steps.Step.Spacing;
                    AlignmentToString(comboBox_ActivitySteps_Alignment, Watch_Face.Activity.Steps.Step.Alignment);
                    //comboBox_ActivitySteps_Alignment.Text = Alignment;
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
                    comboBoxSetText(comboBox_ActivityDistance_Image, Watch_Face.Activity.Distance.Number.ImageIndex);
                    numericUpDown_ActivityDistance_Count.Value = Watch_Face.Activity.Distance.Number.ImagesCount;
                    numericUpDown_ActivityDistance_Spacing.Value = Watch_Face.Activity.Distance.Number.Spacing;
                    AlignmentToString(comboBox_ActivityDistance_Alignment, Watch_Face.Activity.Distance.Number.Alignment);
                    //comboBox_ActivityDistance_Alignment.Text = Alignment;

                    //comboBox_ActivityDistance_Suffix.Text = Watch_Face.Activity.Distance.SuffixImageIndex.ToString();
                    if (Watch_Face.Activity.Distance.SuffixImageIndex != null)
                        comboBoxSetText(comboBox_ActivityDistance_Suffix, (long)Watch_Face.Activity.Distance.SuffixImageIndex);
                    //comboBox_ActivityDistance_Decimal.Text = Watch_Face.Activity.Distance.DecimalPointImageIndex.ToString();
                    if (Watch_Face.Activity.Distance.DecimalPointImageIndex != null)
                        comboBoxSetText(comboBox_ActivityDistance_Decimal, (long)Watch_Face.Activity.Distance.DecimalPointImageIndex);
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
                    comboBoxSetText(comboBox_ActivityPuls_Image, Watch_Face.Activity.Pulse.ImageIndex);
                    numericUpDown_ActivityPuls_Count.Value = Watch_Face.Activity.Pulse.ImagesCount;
                    numericUpDown_ActivityPuls_Spacing.Value = Watch_Face.Activity.Pulse.Spacing;
                    AlignmentToString(comboBox_ActivityPuls_Alignment, Watch_Face.Activity.Pulse.Alignment);
                    //comboBox_ActivityPuls_Alignment.Text = Alignment;
                }
                else checkBox_ActivityPuls.Checked = false;

                if (Watch_Face.Activity.PulseMeter != null)
                {
                    checkBox_ActivityPulsScale.Checked = true;
                    numericUpDown_ActivityPulsScale_Center_X.Value = Watch_Face.Activity.PulseMeter.CenterX;
                    numericUpDown_ActivityPulsScale_Center_Y.Value = Watch_Face.Activity.PulseMeter.CenterY;
                    numericUpDown_ActivityPulsScale_Radius_X.Value = Watch_Face.Activity.PulseMeter.RadiusX;
                    numericUpDown_ActivityPulsScale_Radius_Y.Value = Watch_Face.Activity.PulseMeter.RadiusY;

                    numericUpDown_ActivityPulsScale_StartAngle.Value = Watch_Face.Activity.PulseMeter.StartAngle;
                    numericUpDown_ActivityPulsScale_EndAngle.Value = Watch_Face.Activity.PulseMeter.EndAngle;
                    numericUpDown_ActivityPulsScale_Width.Value = Watch_Face.Activity.PulseMeter.Width;

                    Color new_color = ColorRead(Watch_Face.Activity.PulseMeter.Color);
                    comboBox_ActivityPulsScale_Color.BackColor = new_color;
                    colorDialog_Pulse.Color = new_color;

                    switch (Watch_Face.Activity.PulseMeter.Flatness)
                    {
                        case 90:
                            //comboBox_Battery_Flatness.Text = "Треугольное";
                            comboBox_ActivityPulsScale_Flatness.SelectedIndex = 1;
                            break;
                        case 180:
                            //comboBox_Battery_Flatness.Text = "Плоское";
                            comboBox_ActivityPulsScale_Flatness.SelectedIndex = 2;
                            break;
                        default:
                            //comboBox_Battery_Flatness.Text = "Круглое";
                            comboBox_ActivityPulsScale_Flatness.SelectedIndex = 0;
                            break;
                    }

                    if (Watch_Face.Activity.PulseMeter.ImageIndex != null)
                    {
                        comboBoxSetText(comboBox_ActivityPulsScale_Image, (long)Watch_Face.Activity.PulseMeter.ImageIndex);
                        ColorToCoodinates(new_color, numericUpDown_ActivityPulsScale_ImageX,
                            numericUpDown_ActivityPulsScale_ImageY);
                        checkBox_ActivityPulsScale_Image.Checked = true;
                    }
                    else checkBox_ActivityPulsScale_Image.Checked = false;
                }
                else checkBox_ActivityPulsScale.Checked = false;

                if ((Watch_Face.Activity.PulseGraph != null) && 
                    (Watch_Face.Activity.PulseGraph.ClockHand != null) &&
                    (Watch_Face.Activity.PulseGraph.ClockHand.Image != null))
                {
                    checkBox_Pulse_ClockHand.Checked = true;
                    numericUpDown_Pulse_ClockHand_X.Value = Watch_Face.Activity.PulseGraph.ClockHand.Image.X;
                    numericUpDown_Pulse_ClockHand_Y.Value = Watch_Face.Activity.PulseGraph.ClockHand.Image.Y;
                    comboBoxSetText(comboBox_Pulse_ClockHand_Image, Watch_Face.Activity.PulseGraph.ClockHand.Image.ImageIndex);
                    if (Watch_Face.Activity.PulseGraph.ClockHand.CenterOffset != null)
                    {
                        numericUpDown_Pulse_ClockHand_Offset_X.Value = Watch_Face.Activity.PulseGraph.ClockHand.CenterOffset.X;
                        numericUpDown_Pulse_ClockHand_Offset_Y.Value = Watch_Face.Activity.PulseGraph.ClockHand.CenterOffset.Y;

                    }
                    if (Watch_Face.Activity.PulseGraph.ClockHand.Sector != null)
                    {
                        numericUpDown_Pulse_ClockHand_StartAngle.Value = (int)(Watch_Face.Activity.PulseGraph.ClockHand.Sector.StartAngle / 100);
                        numericUpDown_Pulse_ClockHand_EndAngle.Value = (int)(Watch_Face.Activity.PulseGraph.ClockHand.Sector.EndAngle / 100);

                    }
                }
                else checkBox_Pulse_ClockHand.Checked = false;

                if ((Watch_Face.Activity.ColouredSquares != null) && 
                    (Watch_Face.Activity.ColouredSquares.Coordinates != null))
                {
                    checkBox_ActivityPuls_IconSet.Checked = true;
                    dataGridView_ActivityPuls_IconSet.Rows.Clear();
                    comboBoxSetText(comboBox_ActivityPuls_IconSet_Image, Watch_Face.Activity.ColouredSquares.ImageIndex);
                    foreach (Coordinates coordinates in Watch_Face.Activity.ColouredSquares.Coordinates)
                    {
                        var RowNew = new DataGridViewRow();
                        dataGridView_ActivityPuls_IconSet.Rows.Add(coordinates.X, coordinates.Y);
                    }
                }
                else checkBox_ActivityPuls_IconSet.Checked = false;

                if (Watch_Face.Activity.Calories != null)
                {
                    checkBox_ActivityCalories.Checked = true;
                    numericUpDown_ActivityCalories_StartCorner_X.Value = Watch_Face.Activity.Calories.TopLeftX;
                    numericUpDown_ActivityCalories_StartCorner_Y.Value = Watch_Face.Activity.Calories.TopLeftY;
                    numericUpDown_ActivityCalories_EndCorner_X.Value = Watch_Face.Activity.Calories.BottomRightX;
                    numericUpDown_ActivityCalories_EndCorner_Y.Value = Watch_Face.Activity.Calories.BottomRightY;

                    //comboBox_ActivityCalories_Image.Text = Watch_Face.Activity.Calories.ImageIndex.ToString();
                    comboBoxSetText(comboBox_ActivityCalories_Image, Watch_Face.Activity.Calories.ImageIndex);
                    numericUpDown_ActivityCalories_Count.Value = Watch_Face.Activity.Calories.ImagesCount;
                    numericUpDown_ActivityCalories_Spacing.Value = Watch_Face.Activity.Calories.Spacing;
                    AlignmentToString(comboBox_ActivityCalories_Alignment, Watch_Face.Activity.Calories.Alignment);
                    //comboBox_ActivityCalories_Alignment.Text = Alignment;
                }
                else checkBox_ActivityCalories.Checked = false;

                //if (Watch_Face.Activity.StepsGoal != null)
                //{
                //    checkBox_ActivityCaloriesScale.Checked = true;
                //    numericUpDown_ActivityCaloriesScale_Center_X.Value = Watch_Face.Activity.StepsGoal.CenterX;
                //    numericUpDown_ActivityCaloriesScale_Center_Y.Value = Watch_Face.Activity.StepsGoal.CenterY;
                //    numericUpDown_ActivityCaloriesScale_Radius_X.Value = Watch_Face.Activity.StepsGoal.RadiusX;
                //    numericUpDown_ActivityCaloriesScale_Radius_Y.Value = Watch_Face.Activity.StepsGoal.RadiusY;

                //    numericUpDown_ActivityCaloriesScale_StartAngle.Value = Watch_Face.Activity.StepsGoal.StartAngle;
                //    numericUpDown_ActivityCaloriesScale_EndAngle.Value = Watch_Face.Activity.StepsGoal.EndAngle;
                //    numericUpDown_ActivityCaloriesScale_Width.Value = Watch_Face.Activity.StepsGoal.Width;

                //    Color new_color = ColorRead(Watch_Face.Activity.StepsGoal.Color);
                //    comboBox_ActivityCaloriesScale_Color.BackColor = new_color;
                //    colorDialog_Calories.Color = new_color;

                //    switch (Watch_Face.Activity.StepsGoal.Flatness)
                //    {
                //        case 90:
                //            //comboBox_Battery_Flatness.Text = "Треугольное";
                //            comboBox_ActivityCaloriesScale_Flatness.SelectedIndex = 1;
                //            break;
                //        case 180:
                //            //comboBox_Battery_Flatness.Text = "Плоское";
                //            comboBox_ActivityCaloriesScale_Flatness.SelectedIndex = 2;
                //            break;
                //        default:
                //            //comboBox_Battery_Flatness.Text = "Круглое";
                //            comboBox_ActivityCaloriesScale_Flatness.SelectedIndex = 0;
                //            break;
                //    }

                //    if(Watch_Face.Activity.StepsGoal.ImageIndex != null)
                //    {
                //        comboBoxSetText(comboBox_ActivityCaloriesScale_Image, (long)Watch_Face.Activity.StepsGoal.ImageIndex);
                //        ColorToCoodinates(new_color, numericUpDown_ActivityCaloriesScale_ImageX,
                //            numericUpDown_ActivityCaloriesScale_ImageY);
                //        checkBox_ActivityCaloriesScale_Image.Checked = true;
                //    }
                //    else checkBox_ActivityCaloriesScale_Image.Checked = false;
                //}
                //else checkBox_ActivityCaloriesScale.Checked = false;

                if (Watch_Face.Activity.CaloriesGraph != null && Watch_Face.Activity.CaloriesGraph.Circle != null)
                {
                    checkBox_ActivityCaloriesScale.Checked = true;
                    numericUpDown_ActivityCaloriesScale_Center_X.Value = Watch_Face.Activity.CaloriesGraph.Circle.CenterX;
                    numericUpDown_ActivityCaloriesScale_Center_Y.Value = Watch_Face.Activity.CaloriesGraph.Circle.CenterY;
                    numericUpDown_ActivityCaloriesScale_Radius_X.Value = Watch_Face.Activity.CaloriesGraph.Circle.RadiusX;
                    numericUpDown_ActivityCaloriesScale_Radius_Y.Value = Watch_Face.Activity.CaloriesGraph.Circle.RadiusY;

                    numericUpDown_ActivityCaloriesScale_StartAngle.Value = Watch_Face.Activity.CaloriesGraph.Circle.StartAngle;
                    numericUpDown_ActivityCaloriesScale_EndAngle.Value = Watch_Face.Activity.CaloriesGraph.Circle.EndAngle;
                    numericUpDown_ActivityCaloriesScale_Width.Value = Watch_Face.Activity.CaloriesGraph.Circle.Width;

                    Color new_color = ColorRead(Watch_Face.Activity.CaloriesGraph.Circle.Color);
                    comboBox_ActivityCaloriesScale_Color.BackColor = new_color;
                    colorDialog_Calories.Color = new_color;

                    switch (Watch_Face.Activity.CaloriesGraph.Circle.Flatness)
                    {
                        case 90:
                            //comboBox_Battery_Flatness.Text = "Треугольное";
                            comboBox_ActivityCaloriesScale_Flatness.SelectedIndex = 1;
                            break;
                        case 180:
                            //comboBox_Battery_Flatness.Text = "Плоское";
                            comboBox_ActivityCaloriesScale_Flatness.SelectedIndex = 2;
                            break;
                        default:
                            //comboBox_Battery_Flatness.Text = "Круглое";
                            comboBox_ActivityCaloriesScale_Flatness.SelectedIndex = 0;
                            break;
                    }

                    if (Watch_Face.Activity.CaloriesGraph.Circle.ImageIndex != null)
                    {
                        comboBoxSetText(comboBox_ActivityCaloriesScale_Image, 
                            (long)Watch_Face.Activity.CaloriesGraph.Circle.ImageIndex);
                        ColorToCoodinates(new_color, numericUpDown_ActivityCaloriesScale_ImageX,
                            numericUpDown_ActivityCaloriesScale_ImageY);
                        checkBox_ActivityCaloriesScale_Image.Checked = true;
                    }
                    else checkBox_ActivityCaloriesScale_Image.Checked = false;
                }
                else checkBox_ActivityCaloriesScale.Checked = false;

                if ((Watch_Face.Activity.CaloriesGraph != null) &&
                    (Watch_Face.Activity.CaloriesGraph.ClockHand != null) &&
                    (Watch_Face.Activity.CaloriesGraph.ClockHand.Image != null))
                {
                    checkBox_Calories_ClockHand.Checked = true;
                    numericUpDown_Calories_ClockHand_X.Value = Watch_Face.Activity.CaloriesGraph.ClockHand.Image.X;
                    numericUpDown_Calories_ClockHand_Y.Value = Watch_Face.Activity.CaloriesGraph.ClockHand.Image.Y;
                    comboBoxSetText(comboBox_Calories_ClockHand_Image, Watch_Face.Activity.CaloriesGraph.ClockHand.Image.ImageIndex);
                    if (Watch_Face.Activity.CaloriesGraph.ClockHand.CenterOffset != null)
                    {
                        numericUpDown_Calories_ClockHand_Offset_X.Value = Watch_Face.Activity.CaloriesGraph.ClockHand.CenterOffset.X;
                        numericUpDown_Calories_ClockHand_Offset_Y.Value = Watch_Face.Activity.CaloriesGraph.ClockHand.CenterOffset.Y;

                    }
                    if (Watch_Face.Activity.CaloriesGraph.ClockHand.Sector != null)
                    {
                        numericUpDown_Calories_ClockHand_StartAngle.Value = (int)(Watch_Face.Activity.CaloriesGraph.ClockHand.Sector.StartAngle / 100);
                        numericUpDown_Calories_ClockHand_EndAngle.Value = (int)(Watch_Face.Activity.CaloriesGraph.ClockHand.Sector.EndAngle / 100);

                    }
                }
                else checkBox_Calories_ClockHand.Checked = false;

                if (Watch_Face.Activity.StarImage != null)
                {
                    checkBox_ActivityStar.Checked = true;
                    numericUpDown_ActivityStar_X.Value = Watch_Face.Activity.StarImage.X;
                    numericUpDown_ActivityStar_Y.Value = Watch_Face.Activity.StarImage.Y;
                    //comboBox_ActivityStar_Image.Text = Watch_Face.Activity.StarImage.ImageIndex.ToString();
                    comboBoxSetText(comboBox_ActivityStar_Image, Watch_Face.Activity.StarImage.ImageIndex);
                }
                else checkBox_ActivityStar.Checked = false;

                if (Watch_Face.Activity.NoDataImageIndex != null)
                {
                    comboBoxSetText(comboBox_Activity_NDImage, (long)Watch_Face.Activity.NoDataImageIndex);
                }

            }
            else
            {
                checkBox_Activity.Checked = false;
                checkBox_ActivitySteps.Checked = false;
                checkBox_ActivityDistance.Checked = false;
                checkBox_ActivityPuls.Checked = false;
                checkBox_ActivityPulsScale.Checked = false;
                checkBox_ActivityPulsScale_Image.Checked = false;
                checkBox_ActivityPuls_IconSet.Checked = false;
                checkBox_ActivityCalories.Checked = false;
                checkBox_ActivityCaloriesScale.Checked = false;
                checkBox_ActivityCaloriesScale_Image.Checked = false;
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
                        comboBoxSetText(comboBox_Bluetooth_On, (long)Watch_Face.Status.Bluetooth.ImageIndexOn);
                    if (Watch_Face.Status.Bluetooth.ImageIndexOff != null)
                        //comboBox_Bluetooth_Off.Text = Watch_Face.Status.Bluetooth.ImageIndexOff.Value.ToString();
                        comboBoxSetText(comboBox_Bluetooth_Off, (long)Watch_Face.Status.Bluetooth.ImageIndexOff);
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
                        comboBoxSetText(comboBox_Alarm_On, (long)Watch_Face.Status.Alarm.ImageIndexOn);
                    if (Watch_Face.Status.Alarm.ImageIndexOff != null)
                        //comboBox_Alarm_Off.Text = Watch_Face.Status.Alarm.ImageIndexOff.Value.ToString();
                        comboBoxSetText(comboBox_Alarm_Off, (long)Watch_Face.Status.Alarm.ImageIndexOff);
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
                        comboBoxSetText(comboBox_Lock_On, (long)Watch_Face.Status.Lock.ImageIndexOn);
                    if (Watch_Face.Status.Lock.ImageIndexOff != null)
                        //comboBox_Lock_Off.Text = Watch_Face.Status.Lock.ImageIndexOff.Value.ToString();
                        comboBoxSetText(comboBox_Lock_Off, (long)Watch_Face.Status.Lock.ImageIndexOff);
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
                        comboBoxSetText(comboBox_DND_On, (long)Watch_Face.Status.DoNotDisturb.ImageIndexOn);
                    if (Watch_Face.Status.DoNotDisturb.ImageIndexOff != null)
                        //comboBox_DND_Off.Text = Watch_Face.Status.DoNotDisturb.ImageIndexOff.Value.ToString();
                        comboBoxSetText(comboBox_DND_Off, (long)Watch_Face.Status.DoNotDisturb.ImageIndexOff);
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
                    comboBoxSetText(comboBox_Battery_Text_Image, Watch_Face.Battery.Text.ImageIndex);
                    AlignmentToString(comboBox_Battery_Text_Alignment, Watch_Face.Battery.Text.Alignment);
                    //comboBox_Battery_Text_Alignment.Text = Alignment;
                }
                else checkBox_Battery_Text.Checked = false;

                if (Watch_Face.Battery.Images != null)
                {
                    checkBox_Battery_Img.Checked = true;
                    numericUpDown_Battery_Img_X.Value = Watch_Face.Battery.Images.X;
                    numericUpDown_Battery_Img_Y.Value = Watch_Face.Battery.Images.Y;
                    numericUpDown_Battery_Img_Count.Value = Watch_Face.Battery.Images.ImagesCount;
                    //comboBox_Battery_Img_Image.Text = Watch_Face.Battery.Images.ImageIndex.ToString();
                    comboBoxSetText(comboBox_Battery_Img_Image, Watch_Face.Battery.Images.ImageIndex);
                }
                else checkBox_Battery_Img.Checked = false;

                if ((Watch_Face.Battery.Unknown4 != null) && (Watch_Face.Battery.Unknown4.Image != null))
                {
                    checkBox_Battery_ClockHand.Checked = true;
                    numericUpDown_Battery_ClockHand_X.Value = Watch_Face.Battery.Unknown4.Image.X;
                    numericUpDown_Battery_ClockHand_Y.Value = Watch_Face.Battery.Unknown4.Image.Y;
                    comboBoxSetText(comboBox_Battery_ClockHand_Image, Watch_Face.Battery.Unknown4.Image.ImageIndex);
                    if (Watch_Face.Battery.Unknown4.CenterOffset != null)
                    {
                        numericUpDown_Battery_ClockHand_Offset_X.Value = Watch_Face.Battery.Unknown4.CenterOffset.X;
                        numericUpDown_Battery_ClockHand_Offset_Y.Value = Watch_Face.Battery.Unknown4.CenterOffset.Y;

                    }
                    if (Watch_Face.Battery.Unknown4.Sector != null)
                    {
                        numericUpDown_Battery_ClockHand_StartAngle.Value = (int)(Watch_Face.Battery.Unknown4.Sector.StartAngle/100);
                        numericUpDown_Battery_ClockHand_EndAngle.Value = (int)(Watch_Face.Battery.Unknown4.Sector.EndAngle/100);

                    }
                }
                else checkBox_Battery_ClockHand.Checked = false;

                if (Watch_Face.Battery.Percent != null)
                {
                    checkBox_Battery_Percent.Checked = true;
                    numericUpDown_Battery_Percent_X.Value = Watch_Face.Battery.Percent.X;
                    numericUpDown_Battery_Percent_Y.Value = Watch_Face.Battery.Percent.Y;
                    //comboBox_Battery_Percent_Image.Text = Watch_Face.Battery.Percent.ImageIndex.ToString();
                    comboBoxSetText(comboBox_Battery_Percent_Image, Watch_Face.Battery.Percent.ImageIndex);
                }
                else checkBox_Battery_Percent.Checked = false;

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
                    
                    Color new_color = ColorRead(Watch_Face.Battery.Scale.Color);
                    comboBox_Battery_Scale_Color.BackColor = new_color;
                    colorDialog_Battery.Color = new_color;

                    switch (Watch_Face.Battery.Scale.Flatness)
                    {
                        case 90:
                            //comboBox_Battery_Flatness.Text = "Треугольное";
                            comboBox_Battery_Flatness.SelectedIndex = 1;
                            break;
                        case 180:
                            //comboBox_Battery_Flatness.Text = "Плоское";
                            comboBox_Battery_Flatness.SelectedIndex = 2;
                            break;
                        default:
                            //comboBox_Battery_Flatness.Text = "Круглое";
                            comboBox_Battery_Flatness.SelectedIndex = 0;
                            break;
                    }

                    if (Watch_Face.Battery.Scale.ImageIndex != null)
                    {
                        comboBoxSetText(comboBox_Battery_Scale_Image, (long)Watch_Face.Battery.Scale.ImageIndex);
                        ColorToCoodinates(new_color, numericUpDown_Battery_Scale_ImageX,
                            numericUpDown_Battery_Scale_ImageY);
                        checkBox_Battery_Scale_Image.Checked = true;
                    }
                    else checkBox_Battery_Scale_Image.Checked = false;
                }
                else checkBox_Battery_Scale.Checked = false;

                if ((Watch_Face.Battery.Icons != null) && (Watch_Face.Battery.Icons.Coordinates != null))
                {
                    checkBox_Battery_IconSet.Checked = true;
                    dataGridView_Battery_IconSet.Rows.Clear();
                    comboBoxSetText(comboBox_Battery_IconSet_Image, Watch_Face.Battery.Icons.ImageIndex);
                    foreach (Coordinates coordinates in Watch_Face.Battery.Icons.Coordinates)
                    {
                        var RowNew = new DataGridViewRow();
                        dataGridView_Battery_IconSet.Rows.Add(coordinates.X, coordinates.Y);
                    }
                }
                else checkBox_Battery_IconSet.Checked = false;
            }
            else
            {
                checkBox_Battery.Checked = false;
                checkBox_Battery_Text.Checked = false;
                checkBox_Battery_Img.Checked = false;
                checkBox_Battery_ClockHand.Checked = false;
                checkBox_Battery_Percent.Checked = false;
                checkBox_Battery_Scale.Checked = false;
                checkBox_Battery_Scale_Image.Checked = false;
                checkBox_Battery_IconSet.Checked = false;
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
                    comboBoxSetText(comboBox_AnalogClock_Hour_Image, Watch_Face.AnalogDialFace.Hours.Image.ImageIndex);

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
                    comboBoxSetText(comboBox_AnalogClock_Min_Image, Watch_Face.AnalogDialFace.Minutes.Image.ImageIndex);

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
                    comboBoxSetText(comboBox_AnalogClock_Sec_Image, Watch_Face.AnalogDialFace.Seconds.Image.ImageIndex);

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
                    comboBoxSetText(comboBox_HourCenterImage_Image, Watch_Face.AnalogDialFace.HourCenterImage.ImageIndex);
                }
                else checkBox_HourCenterImage.Checked = false;

                if (Watch_Face.AnalogDialFace.MinCenterImage != null)
                {
                    checkBox_MinCenterImage.Checked = true;
                    numericUpDown_MinCenterImage_X.Value = Watch_Face.AnalogDialFace.MinCenterImage.X;
                    numericUpDown_MinCenterImage_Y.Value = Watch_Face.AnalogDialFace.MinCenterImage.Y;
                    //comboBox_MinCenterImage_Image.Text = Watch_Face.AnalogDialFace.MinCenterImage.ImageIndex.ToString();
                    comboBoxSetText(comboBox_MinCenterImage_Image, Watch_Face.AnalogDialFace.MinCenterImage.ImageIndex);
                }
                else checkBox_MinCenterImage.Checked = false;

                if (Watch_Face.AnalogDialFace.SecCenterImage != null)
                {
                    checkBox_SecCenterImage.Checked = true;
                    numericUpDown_SecCenterImage_X.Value = Watch_Face.AnalogDialFace.SecCenterImage.X;
                    numericUpDown_SecCenterImage_Y.Value = Watch_Face.AnalogDialFace.SecCenterImage.Y;
                    //comboBox_SecCenterImage_Image.Text = Watch_Face.AnalogDialFace.SecCenterImage.ImageIndex.ToString();
                    comboBoxSetText(comboBox_SecCenterImage_Image, Watch_Face.AnalogDialFace.SecCenterImage.ImageIndex);
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
                    comboBoxSetText(comboBox_Weather_Text_Image, Watch_Face.Weather.Temperature.Current.ImageIndex);
                    AlignmentToString(comboBox_Weather_Text_Alignment, Watch_Face.Weather.Temperature.Current.Alignment);
                    //comboBox_Weather_Text_Alignment.Text = Alignment;
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
                        comboBoxSetText(comboBox_Weather_Day_Image,
                            Watch_Face.Weather.Temperature.Today.Separate.Day.ImageIndex);
                        AlignmentToString(comboBox_Weather_Day_Alignment, Watch_Face.Weather.Temperature.Today.Separate.Day.Alignment);
                        //comboBox_Weather_Day_Alignment.Text = Alignment;
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
                        comboBoxSetText(comboBox_Weather_Night_Image,
                            Watch_Face.Weather.Temperature.Today.Separate.Night.ImageIndex);
                        AlignmentToString(comboBox_Weather_Night_Alignment, Watch_Face.Weather.Temperature.Today.Separate.Night.Alignment);
                        //comboBox_Weather_Night_Alignment.Text = Alignment;
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
                    comboBoxSetText(comboBox_Weather_Text_MinusImage, Watch_Face.Weather.Temperature.Symbols.MinusImageIndex);
                    //comboBox_Weather_Text_DegImage.Text = Watch_Face.Weather.Temperature.Symbols.DegreesImageIndex.ToString();
                    comboBoxSetText(comboBox_Weather_Text_DegImage, Watch_Face.Weather.Temperature.Symbols.DegreesImageIndex);
                    //comboBox_Weather_Text_NDImage.Text = Watch_Face.Weather.Temperature.Symbols.NoDataImageIndex.ToString();
                    comboBoxSetText(comboBox_Weather_Text_NDImage, Watch_Face.Weather.Temperature.Symbols.NoDataImageIndex);
                }

                if ((Watch_Face.Weather.Icon != null) && (Watch_Face.Weather.Icon.Images != null))
                {
                    checkBox_Weather_Icon.Checked = true;
                    numericUpDown_Weather_Icon_X.Value = Watch_Face.Weather.Icon.Images.X;
                    numericUpDown_Weather_Icon_Y.Value = Watch_Face.Weather.Icon.Images.Y;

                    numericUpDown_Weather_Icon_Count.Value = Watch_Face.Weather.Icon.Images.ImagesCount;
                    //comboBox_Weather_Icon_Image.Text = Watch_Face.Weather.Icon.Images.ImageIndex.ToString();
                    comboBoxSetText(comboBox_Weather_Icon_Image, Watch_Face.Weather.Icon.Images.ImageIndex);
                    //comboBox_Weather_Icon_NDImage.Text = Watch_Face.Weather.Icon.NoWeatherImageIndex.ToString();
                    comboBoxSetText(comboBox_Weather_Icon_NDImage, Watch_Face.Weather.Icon.NoWeatherImageIndex);
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

            #region Shortcuts
            if(Watch_Face.Shortcuts != null)
            {
                checkBox_Shortcuts.Checked = true;
                if (Watch_Face.Shortcuts.State != null && Watch_Face.Shortcuts.State.Element != null)
                {
                    checkBox_Shortcuts_Steps.Checked = true;
                    numericUpDown_Shortcuts_Steps_X.Value = Watch_Face.Shortcuts.State.Element.TopLeftX;
                    numericUpDown_Shortcuts_Steps_Y.Value = Watch_Face.Shortcuts.State.Element.TopLeftY;
                    numericUpDown_Shortcuts_Steps_Width.Value = Watch_Face.Shortcuts.State.Element.Width;
                    numericUpDown_Shortcuts_Steps_Height.Value = Watch_Face.Shortcuts.State.Element.Height;
                }
                else checkBox_Shortcuts_Steps.Checked = false;

                if (Watch_Face.Shortcuts.Pulse != null && Watch_Face.Shortcuts.Pulse.Element != null)
                {
                    checkBox_Shortcuts_Puls.Checked = true;
                    numericUpDown_Shortcuts_Puls_X.Value = Watch_Face.Shortcuts.Pulse.Element.TopLeftX;
                    numericUpDown_Shortcuts_Puls_Y.Value = Watch_Face.Shortcuts.Pulse.Element.TopLeftY;
                    numericUpDown_Shortcuts_Puls_Width.Value = Watch_Face.Shortcuts.Pulse.Element.Width;
                    numericUpDown_Shortcuts_Puls_Height.Value = Watch_Face.Shortcuts.Pulse.Element.Height;
                }
                else checkBox_Shortcuts_Puls.Checked = false;

                if (Watch_Face.Shortcuts.Weather != null && Watch_Face.Shortcuts.Weather.Element != null)
                {
                    checkBox_Shortcuts_Weather.Checked = true;
                    numericUpDown_Shortcuts_Weather_X.Value = Watch_Face.Shortcuts.Weather.Element.TopLeftX;
                    numericUpDown_Shortcuts_Weather_Y.Value = Watch_Face.Shortcuts.Weather.Element.TopLeftY;
                    numericUpDown_Shortcuts_Weather_Width.Value = Watch_Face.Shortcuts.Weather.Element.Width;
                    numericUpDown_Shortcuts_Weather_Height.Value = Watch_Face.Shortcuts.Weather.Element.Height;
                }
                else checkBox_Shortcuts_Weather.Checked = false;

                if (Watch_Face.Shortcuts.Unknown4 != null && Watch_Face.Shortcuts.Unknown4.Element != null)
                {
                    checkBox_Shortcuts_Energy.Checked = true;
                    numericUpDown_Shortcuts_Energy_X.Value = Watch_Face.Shortcuts.Unknown4.Element.TopLeftX;
                    numericUpDown_Shortcuts_Energy_Y.Value = Watch_Face.Shortcuts.Unknown4.Element.TopLeftY;
                    numericUpDown_Shortcuts_Energy_Width.Value = Watch_Face.Shortcuts.Unknown4.Element.Width;
                    numericUpDown_Shortcuts_Energy_Height.Value = Watch_Face.Shortcuts.Unknown4.Element.Height;
                }
                else checkBox_Shortcuts_Energy.Checked = false;
            }
            else
            {
                checkBox_Shortcuts.Checked = false;
                checkBox_Shortcuts_Steps.Checked = false;
                checkBox_Shortcuts_Puls.Checked = false;
                checkBox_Shortcuts_Weather.Checked = false;
                checkBox_Shortcuts_Energy.Checked = false;
            }
            #endregion
        }

        // формируем JSON файл из настроек
        private void JSON_write()
        {
            if (!PreviewView) return;
            Watch_Face = new WATCH_FACE_JSON();

            if (radioButton_47.Checked)
            {
                Watch_Face.Info = new Device_Id();
                Watch_Face.Info.DeviceId = 40;
            }
            if (radioButton_42.Checked)
            {
                Watch_Face.Info = new Device_Id();
                Watch_Face.Info.DeviceId = 42;
            }
            if (radioButton_gts.Checked)
            {
                Watch_Face.Info = new Device_Id();
                Watch_Face.Info.DeviceId = 46;
            }

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
                    string Alignment = StringToAlignment(comboBox_ActivitySteps_Alignment.SelectedIndex);
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
                    string Alignment = StringToAlignment(comboBox_ActivityDistance_Alignment.SelectedIndex);
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
                    string Alignment = StringToAlignment(comboBox_ActivityPuls_Alignment.SelectedIndex);
                    Watch_Face.Activity.Pulse.Alignment = Alignment;
                }

                // пульс набором иконок
                if (checkBox_ActivityPuls_IconSet.Checked)
                {
                    if ((comboBox_ActivityPuls_IconSet_Image.SelectedIndex >= 0) && (dataGridView_ActivityPuls_IconSet.Rows.Count > 1))
                    {
                        if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
                        if (Watch_Face.Activity.ColouredSquares == null) Watch_Face.Activity.ColouredSquares = new IconSet();

                        Watch_Face.Activity.ColouredSquares.ImageIndex = Int32.Parse(comboBox_ActivityPuls_IconSet_Image.Text);
                        Coordinates[] coordinates = new Coordinates[0];
                        int count = 0;

                        foreach (DataGridViewRow row in dataGridView_ActivityPuls_IconSet.Rows)
                        {
                            //whatever you are currently doing
                            //Coordinates coordinates = new Coordinates();
                            int x = 0;
                            int y = 0;
                            if ((row.Cells[0].Value != null) && (row.Cells[1].Value != null))
                            {
                                if ((Int32.TryParse(row.Cells[0].Value.ToString(), out x)) && (Int32.TryParse(row.Cells[1].Value.ToString(), out y)))
                                {

                                    //Array.Resize(ref objson, objson.Length + 1);
                                    Array.Resize(ref coordinates, coordinates.Length + 1);
                                    //objson[count] = coordinates;
                                    coordinates[count] = new Coordinates();
                                    coordinates[count].X = x;
                                    coordinates[count].Y = y;
                                    count++;
                                }
                            }
                            Watch_Face.Activity.ColouredSquares.Coordinates = coordinates;
                        }
                    }
                }

                if (checkBox_ActivityPulsScale.Checked)
                {
                    if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
                    if (Watch_Face.Activity.PulseMeter == null) Watch_Face.Activity.PulseMeter = new CircleScale();

                    Watch_Face.Activity.PulseMeter.CenterX = (int)numericUpDown_ActivityPulsScale_Center_X.Value;
                    Watch_Face.Activity.PulseMeter.CenterY = (int)numericUpDown_ActivityPulsScale_Center_Y.Value;
                    Watch_Face.Activity.PulseMeter.RadiusX = (int)numericUpDown_ActivityPulsScale_Radius_X.Value;
                    Watch_Face.Activity.PulseMeter.RadiusY = (int)numericUpDown_ActivityPulsScale_Radius_Y.Value;

                    Watch_Face.Activity.PulseMeter.StartAngle = (int)numericUpDown_ActivityPulsScale_StartAngle.Value;
                    Watch_Face.Activity.PulseMeter.EndAngle = (int)numericUpDown_ActivityPulsScale_EndAngle.Value;
                    Watch_Face.Activity.PulseMeter.Width = (int)numericUpDown_ActivityPulsScale_Width.Value;

                    Color color = comboBox_ActivityPulsScale_Color.BackColor;
                    Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
                    string colorStr = ColorTranslator.ToHtml(new_color);
                    colorStr = colorStr.Replace("#", "0x00");
                    Watch_Face.Activity.PulseMeter.Color = colorStr;

                    //switch (comboBox_Battery_Flatness.Text)
                    //{
                    //    case "Треугольное":
                    //        Watch_Face.Battery.Scale.Flatness = 90;
                    //        break;
                    //    case "Плоское":
                    //        Watch_Face.Battery.Scale.Flatness = 180;
                    //        break;
                    //    default:
                    //        Watch_Face.Battery.Scale.Flatness = 0;
                    //        break;
                    //}
                    switch (comboBox_ActivityPulsScale_Flatness.SelectedIndex)
                    {
                        case 1:
                            Watch_Face.Activity.PulseMeter.Flatness = 90;
                            break;
                        case 2:
                            Watch_Face.Activity.PulseMeter.Flatness = 180;
                            break;
                        default:
                            Watch_Face.Activity.PulseMeter.Flatness = 0;
                            break;
                    }

                    if (checkBox_ActivityPulsScale_Image.Checked &&
                        comboBox_ActivityPulsScale_Image.SelectedIndex >= 0)
                    {
                        int imageX = (int)numericUpDown_ActivityPulsScale_ImageX.Value;
                        int imageY = (int)numericUpDown_ActivityPulsScale_ImageY.Value;
                        int imageIndex = comboBox_ActivityPulsScale_Image.SelectedIndex;
                        colorStr = CoodinatesToColor(imageX, imageY);
                        Bitmap src = new Bitmap(ListImagesFullName[imageIndex]);
                        Watch_Face.Activity.PulseMeter.CenterX = imageX + src.Width / 2;
                        Watch_Face.Activity.PulseMeter.CenterY = imageY + src.Height / 2;
                        Watch_Face.Activity.PulseMeter.Color = colorStr;
                        Watch_Face.Activity.PulseMeter.ImageIndex = imageIndex;
                    }
                }

                if ((checkBox_Pulse_ClockHand.Checked) && (comboBox_Pulse_ClockHand_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
                    if (Watch_Face.Activity.PulseGraph == null) Watch_Face.Activity.PulseGraph = new PulseContainer();
                    if (Watch_Face.Activity.PulseGraph.ClockHand == null) Watch_Face.Activity.PulseGraph.ClockHand = new ClockHand();
                    if (Watch_Face.Activity.PulseGraph.ClockHand.CenterOffset == null)
                        Watch_Face.Activity.PulseGraph.ClockHand.CenterOffset = new Coordinates();
                    if (Watch_Face.Activity.PulseGraph.ClockHand.Sector == null)
                        Watch_Face.Activity.PulseGraph.ClockHand.Sector = new Sector();
                    //if (Watch_Face.AnalogDialFace.Hours.Shape == null)
                    //Watch_Face.AnalogDialFace.Hours.Shape = new Coordinates();
                    if (Watch_Face.Activity.PulseGraph.ClockHand.Image == null)
                        Watch_Face.Activity.PulseGraph.ClockHand.Image = new ImageW();

                    Watch_Face.Activity.PulseGraph.ClockHand.Image.ImageIndex = Int32.Parse(comboBox_Pulse_ClockHand_Image.Text);
                    Watch_Face.Activity.PulseGraph.ClockHand.Image.X = (int)numericUpDown_Pulse_ClockHand_X.Value;
                    Watch_Face.Activity.PulseGraph.ClockHand.Image.Y = (int)numericUpDown_Pulse_ClockHand_Y.Value;

                    Watch_Face.Activity.PulseGraph.ClockHand.Color = "0x00000000";
                    Watch_Face.Activity.PulseGraph.ClockHand.OnlyBorder = false;

                    Watch_Face.Activity.PulseGraph.ClockHand.CenterOffset.X = (int)numericUpDown_Pulse_ClockHand_Offset_X.Value;
                    Watch_Face.Activity.PulseGraph.ClockHand.CenterOffset.Y = (int)numericUpDown_Pulse_ClockHand_Offset_Y.Value;

                    Watch_Face.Activity.PulseGraph.ClockHand.Sector.StartAngle =
                        (int)(numericUpDown_Pulse_ClockHand_StartAngle.Value * 100);
                    Watch_Face.Activity.PulseGraph.ClockHand.Sector.EndAngle =
                        (int)(numericUpDown_Pulse_ClockHand_EndAngle.Value * 100);
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
                    string Alignment = StringToAlignment(comboBox_ActivityCalories_Alignment.SelectedIndex);
                    Watch_Face.Activity.Calories.Alignment = Alignment;
                }

                //if (checkBox_ActivityCaloriesScale.Checked)
                //{
                //    if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
                //    if (Watch_Face.Activity.StepsGoal == null) Watch_Face.Activity.StepsGoal = new CircleScale();

                //    Watch_Face.Activity.StepsGoal.CenterX = (int)numericUpDown_ActivityCaloriesScale_Center_X.Value;
                //    Watch_Face.Activity.StepsGoal.CenterY = (int)numericUpDown_ActivityCaloriesScale_Center_Y.Value;
                //    Watch_Face.Activity.StepsGoal.RadiusX = (int)numericUpDown_ActivityCaloriesScale_Radius_X.Value;
                //    Watch_Face.Activity.StepsGoal.RadiusY = (int)numericUpDown_ActivityCaloriesScale_Radius_Y.Value;

                //    Watch_Face.Activity.StepsGoal.StartAngle = (int)numericUpDown_ActivityCaloriesScale_StartAngle.Value;
                //    Watch_Face.Activity.StepsGoal.EndAngle = (int)numericUpDown_ActivityCaloriesScale_EndAngle.Value;
                //    Watch_Face.Activity.StepsGoal.Width = (int)numericUpDown_ActivityCaloriesScale_Width.Value;

                //    Color color = comboBox_ActivityCaloriesScale_Color.BackColor;
                //    Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
                //    string colorStr = ColorTranslator.ToHtml(new_color);
                //    colorStr = colorStr.Replace("#", "0x00");
                //    Watch_Face.Activity.StepsGoal.Color = colorStr;

                //    switch (comboBox_ActivityCaloriesScale_Flatness.SelectedIndex)
                //    {
                //        case 1:
                //            Watch_Face.Activity.StepsGoal.Flatness = 90;
                //            break;
                //        case 2:
                //            Watch_Face.Activity.StepsGoal.Flatness = 180;
                //            break;
                //        default:
                //            Watch_Face.Activity.StepsGoal.Flatness = 0;
                //            break;
                //    }

                //    if (checkBox_ActivityCaloriesScale_Image.Checked &&
                //        comboBox_ActivityCaloriesScale_Image.SelectedIndex >= 0)
                //    {
                //        int imageX = (int)numericUpDown_ActivityCaloriesScale_ImageX.Value;
                //        int imageY = (int)numericUpDown_ActivityCaloriesScale_ImageY.Value;
                //        int imageIndex = comboBox_ActivityCaloriesScale_Image.SelectedIndex;
                //        colorStr = CoodinatesToColor(imageX, imageY);
                //        Bitmap src = new Bitmap(ListImagesFullName[imageIndex]);
                //        Watch_Face.Activity.StepsGoal.CenterX = imageX + src.Width / 2;
                //        Watch_Face.Activity.StepsGoal.CenterY = imageY + src.Height / 2;
                //        Watch_Face.Activity.StepsGoal.Color = colorStr;
                //        Watch_Face.Activity.StepsGoal.ImageIndex = imageIndex;
                //    }
                //}
                if (checkBox_ActivityCaloriesScale.Checked)
                {
                    if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
                    if (Watch_Face.Activity.CaloriesGraph == null) Watch_Face.Activity.CaloriesGraph = new CaloriesContainer();
                    if (Watch_Face.Activity.CaloriesGraph.Circle == null)
                        Watch_Face.Activity.CaloriesGraph.Circle = new CircleScale();

                    Watch_Face.Activity.CaloriesGraph.Circle.CenterX = (int)numericUpDown_ActivityCaloriesScale_Center_X.Value;
                    Watch_Face.Activity.CaloriesGraph.Circle.CenterY = (int)numericUpDown_ActivityCaloriesScale_Center_Y.Value;
                    Watch_Face.Activity.CaloriesGraph.Circle.RadiusX = (int)numericUpDown_ActivityCaloriesScale_Radius_X.Value;
                    Watch_Face.Activity.CaloriesGraph.Circle.RadiusY = (int)numericUpDown_ActivityCaloriesScale_Radius_Y.Value;

                    Watch_Face.Activity.CaloriesGraph.Circle.StartAngle = (int)numericUpDown_ActivityCaloriesScale_StartAngle.Value;
                    Watch_Face.Activity.CaloriesGraph.Circle.EndAngle = (int)numericUpDown_ActivityCaloriesScale_EndAngle.Value;
                    Watch_Face.Activity.CaloriesGraph.Circle.Width = (int)numericUpDown_ActivityCaloriesScale_Width.Value;

                    Color color = comboBox_ActivityCaloriesScale_Color.BackColor;
                    Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
                    string colorStr = ColorTranslator.ToHtml(new_color);
                    colorStr = colorStr.Replace("#", "0x00");
                    Watch_Face.Activity.CaloriesGraph.Circle.Color = colorStr;

                    switch (comboBox_ActivityCaloriesScale_Flatness.SelectedIndex)
                    {
                        case 1:
                            Watch_Face.Activity.CaloriesGraph.Circle.Flatness = 90;
                            break;
                        case 2:
                            Watch_Face.Activity.CaloriesGraph.Circle.Flatness = 180;
                            break;
                        default:
                            Watch_Face.Activity.CaloriesGraph.Circle.Flatness = 0;
                            break;
                    }

                    if (checkBox_ActivityCaloriesScale_Image.Checked &&
                        comboBox_ActivityCaloriesScale_Image.SelectedIndex >= 0)
                    {
                        int imageX = (int)numericUpDown_ActivityCaloriesScale_ImageX.Value;
                        int imageY = (int)numericUpDown_ActivityCaloriesScale_ImageY.Value;
                        int imageIndex = comboBox_ActivityCaloriesScale_Image.SelectedIndex;
                        colorStr = CoodinatesToColor(imageX, imageY);
                        Bitmap src = new Bitmap(ListImagesFullName[imageIndex]);
                        Watch_Face.Activity.CaloriesGraph.Circle.CenterX = imageX + src.Width / 2;
                        Watch_Face.Activity.CaloriesGraph.Circle.CenterY = imageY + src.Height / 2;
                        Watch_Face.Activity.CaloriesGraph.Circle.Color = colorStr;
                        Watch_Face.Activity.CaloriesGraph.Circle.ImageIndex = imageIndex;
                    }
                }

                if ((checkBox_Calories_ClockHand.Checked) && (comboBox_Calories_ClockHand_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
                    if (Watch_Face.Activity.CaloriesGraph == null) Watch_Face.Activity.CaloriesGraph = new CaloriesContainer();
                    if (Watch_Face.Activity.CaloriesGraph.ClockHand == null) Watch_Face.Activity.CaloriesGraph.ClockHand = new ClockHand();
                    if (Watch_Face.Activity.CaloriesGraph.ClockHand.CenterOffset == null)
                        Watch_Face.Activity.CaloriesGraph.ClockHand.CenterOffset = new Coordinates();
                    if (Watch_Face.Activity.CaloriesGraph.ClockHand.Sector == null)
                        Watch_Face.Activity.CaloriesGraph.ClockHand.Sector = new Sector();
                    //if (Watch_Face.AnalogDialFace.Hours.Shape == null)
                    //Watch_Face.AnalogDialFace.Hours.Shape = new Coordinates();
                    if (Watch_Face.Activity.CaloriesGraph.ClockHand.Image == null)
                        Watch_Face.Activity.CaloriesGraph.ClockHand.Image = new ImageW();

                    Watch_Face.Activity.CaloriesGraph.ClockHand.Image.ImageIndex = Int32.Parse(comboBox_Calories_ClockHand_Image.Text);
                    Watch_Face.Activity.CaloriesGraph.ClockHand.Image.X = (int)numericUpDown_Calories_ClockHand_X.Value;
                    Watch_Face.Activity.CaloriesGraph.ClockHand.Image.Y = (int)numericUpDown_Calories_ClockHand_Y.Value;

                    Watch_Face.Activity.CaloriesGraph.ClockHand.Color = "0x00000000";
                    Watch_Face.Activity.CaloriesGraph.ClockHand.OnlyBorder = false;

                    Watch_Face.Activity.CaloriesGraph.ClockHand.CenterOffset.X = (int)numericUpDown_Calories_ClockHand_Offset_X.Value;
                    Watch_Face.Activity.CaloriesGraph.ClockHand.CenterOffset.Y = (int)numericUpDown_Calories_ClockHand_Offset_Y.Value;

                    Watch_Face.Activity.CaloriesGraph.ClockHand.Sector.StartAngle =
                        (int)(numericUpDown_Calories_ClockHand_StartAngle.Value * 100);
                    Watch_Face.Activity.CaloriesGraph.ClockHand.Sector.EndAngle =
                        (int)(numericUpDown_Calories_ClockHand_EndAngle.Value * 100);
                }

                if ((checkBox_ActivityStar.Checked) && (comboBox_ActivityStar_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
                    if (Watch_Face.Activity.StarImage == null) Watch_Face.Activity.StarImage = new ImageW();

                    Watch_Face.Activity.StarImage.ImageIndex = Int32.Parse(comboBox_ActivityStar_Image.Text);
                    Watch_Face.Activity.StarImage.X = (int)numericUpDown_ActivityStar_X.Value;
                    Watch_Face.Activity.StarImage.Y = (int)numericUpDown_ActivityStar_Y.Value;
                }

                if (comboBox_Activity_NDImage.SelectedIndex >= 0)
                {
                    if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
                    Watch_Face.Activity.NoDataImageIndex = Int32.Parse(comboBox_Activity_NDImage.Text);
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

                // день недели набором иконок
                if (checkBox_DOW_IconSet.Checked)
                {
                    if ((comboBox_DOW_IconSet_Image.SelectedIndex >= 0) && (dataGridView_DOW_IconSet.Rows.Count > 1))
                    {
                        if (Watch_Face.Date == null) Watch_Face.Date = new Date();
                        if (Watch_Face.Date.WeekDayProgress == null) Watch_Face.Date.WeekDayProgress = new IconSet();

                        Watch_Face.Date.WeekDayProgress.ImageIndex = Int32.Parse(comboBox_DOW_IconSet_Image.Text);
                        Coordinates[] coordinates = new Coordinates[0];
                        int count = 0;

                        foreach (DataGridViewRow row in dataGridView_DOW_IconSet.Rows)
                        {
                            //whatever you are currently doing
                            //Coordinates coordinates = new Coordinates();
                            int x = 0;
                            int y = 0;
                            if ((row.Cells[0].Value != null) && (row.Cells[1].Value != null))
                            {
                                if ((Int32.TryParse(row.Cells[0].Value.ToString(), out x)) && (Int32.TryParse(row.Cells[1].Value.ToString(), out y)))
                                {

                                    //Array.Resize(ref objson, objson.Length + 1);
                                    Array.Resize(ref coordinates, coordinates.Length + 1);
                                    //objson[count] = coordinates;
                                    coordinates[count] = new Coordinates();
                                    coordinates[count].X = x;
                                    coordinates[count].Y = y;
                                    count++;
                                }
                            }
                            Watch_Face.Date.WeekDayProgress.Coordinates = coordinates;
                        }
                    }
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
                    string Alignment = StringToAlignment(comboBox_MonthAndDayD_Alignment.SelectedIndex);
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
                    string Alignment = StringToAlignment(comboBox_MonthAndDayM_Alignment.SelectedIndex);
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
                    string Alignment = StringToAlignment(comboBox_OneLine_Alignment.SelectedIndex);
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
                    string Alignment = StringToAlignment(comboBox_Year_Alignment.SelectedIndex);
                    Watch_Face.Date.Year.OneLine.Number.Alignment = Alignment;

                    if (comboBox_Year_Delimiter.SelectedIndex >= 0)
                        Watch_Face.Date.Year.OneLine.DelimiterImageIndex = Int32.Parse(comboBox_Year_Delimiter.Text);
                }
            }

            // число стрелкой
            if ((checkBox_ADDay_ClockHand.Checked) && (comboBox_ADDay_ClockHand_Image.SelectedIndex >= 0))
            {
                if (Watch_Face.DaysProgress == null) Watch_Face.DaysProgress = new DaysProgress();
                if (Watch_Face.DaysProgress.UnknownField2 == null) Watch_Face.DaysProgress.UnknownField2 = new ClockHand();
                if (Watch_Face.DaysProgress.UnknownField2.CenterOffset == null)
                    Watch_Face.DaysProgress.UnknownField2.CenterOffset = new Coordinates();
                if (Watch_Face.DaysProgress.UnknownField2.Sector == null)
                    Watch_Face.DaysProgress.UnknownField2.Sector = new Sector();
                if (Watch_Face.DaysProgress.UnknownField2.Image == null)
                    Watch_Face.DaysProgress.UnknownField2.Image = new ImageW();

                Watch_Face.DaysProgress.UnknownField2.Image.ImageIndex = Int32.Parse(comboBox_ADDay_ClockHand_Image.Text);
                Watch_Face.DaysProgress.UnknownField2.Image.X = (int)numericUpDown_ADDay_ClockHand_X.Value;
                Watch_Face.DaysProgress.UnknownField2.Image.Y = (int)numericUpDown_ADDay_ClockHand_Y.Value;

                Watch_Face.DaysProgress.UnknownField2.Color = "0x00000000";
                Watch_Face.DaysProgress.UnknownField2.OnlyBorder = false;

                Watch_Face.DaysProgress.UnknownField2.CenterOffset.X = (int)numericUpDown_ADDay_ClockHand_Offset_X.Value;
                Watch_Face.DaysProgress.UnknownField2.CenterOffset.Y = (int)numericUpDown_ADDay_ClockHand_Offset_Y.Value;

                Watch_Face.DaysProgress.UnknownField2.Sector.StartAngle =
                    (int)(numericUpDown_ADDay_ClockHand_StartAngle.Value * 100);
                Watch_Face.DaysProgress.UnknownField2.Sector.EndAngle =
                    (int)(numericUpDown_ADDay_ClockHand_EndAngle.Value * 100);
            }

            // день недели стрелкой
            if ((checkBox_ADWeekDay_ClockHand.Checked) && (comboBox_ADWeekDay_ClockHand_Image.SelectedIndex >= 0))
            {
                if (Watch_Face.DaysProgress == null) Watch_Face.DaysProgress = new DaysProgress();
                if (Watch_Face.DaysProgress.AnalogDOW == null) Watch_Face.DaysProgress.AnalogDOW = new ClockHand();
                if (Watch_Face.DaysProgress.AnalogDOW.CenterOffset == null)
                    Watch_Face.DaysProgress.AnalogDOW.CenterOffset = new Coordinates();
                if (Watch_Face.DaysProgress.AnalogDOW.Sector == null)
                    Watch_Face.DaysProgress.AnalogDOW.Sector = new Sector();
                if (Watch_Face.DaysProgress.AnalogDOW.Image == null)
                    Watch_Face.DaysProgress.AnalogDOW.Image = new ImageW();

                Watch_Face.DaysProgress.AnalogDOW.Image.ImageIndex = Int32.Parse(comboBox_ADWeekDay_ClockHand_Image.Text);
                Watch_Face.DaysProgress.AnalogDOW.Image.X = (int)numericUpDown_ADWeekDay_ClockHand_X.Value;
                Watch_Face.DaysProgress.AnalogDOW.Image.Y = (int)numericUpDown_ADWeekDay_ClockHand_Y.Value;

                Watch_Face.DaysProgress.AnalogDOW.Color = "0x00000000";
                Watch_Face.DaysProgress.AnalogDOW.OnlyBorder = false;

                Watch_Face.DaysProgress.AnalogDOW.CenterOffset.X = (int)numericUpDown_ADWeekDay_ClockHand_Offset_X.Value;
                Watch_Face.DaysProgress.AnalogDOW.CenterOffset.Y = (int)numericUpDown_ADWeekDay_ClockHand_Offset_Y.Value;

                Watch_Face.DaysProgress.AnalogDOW.Sector.StartAngle =
                    (int)(numericUpDown_ADWeekDay_ClockHand_StartAngle.Value * 100);
                Watch_Face.DaysProgress.AnalogDOW.Sector.EndAngle =
                    (int)(numericUpDown_ADWeekDay_ClockHand_EndAngle.Value * 100);
            }

            // месяц стрелкой
            if ((checkBox_ADMonth_ClockHand.Checked) && (comboBox_ADMonth_ClockHand_Image.SelectedIndex >= 0))
            {
                if (Watch_Face.DaysProgress == null) Watch_Face.DaysProgress = new DaysProgress();
                if (Watch_Face.DaysProgress.AnalogMonth == null) Watch_Face.DaysProgress.AnalogMonth = new ClockHand();
                if (Watch_Face.DaysProgress.AnalogMonth.CenterOffset == null)
                    Watch_Face.DaysProgress.AnalogMonth.CenterOffset = new Coordinates();
                if (Watch_Face.DaysProgress.AnalogMonth.Sector == null)
                    Watch_Face.DaysProgress.AnalogMonth.Sector = new Sector();
                if (Watch_Face.DaysProgress.AnalogMonth.Image == null)
                    Watch_Face.DaysProgress.AnalogMonth.Image = new ImageW();

                Watch_Face.DaysProgress.AnalogMonth.Image.ImageIndex = Int32.Parse(comboBox_ADMonth_ClockHand_Image.Text);
                Watch_Face.DaysProgress.AnalogMonth.Image.X = (int)numericUpDown_ADMonth_ClockHand_X.Value;
                Watch_Face.DaysProgress.AnalogMonth.Image.Y = (int)numericUpDown_ADMonth_ClockHand_Y.Value;

                Watch_Face.DaysProgress.AnalogMonth.Color = "0x00000000";
                Watch_Face.DaysProgress.AnalogMonth.OnlyBorder = false;

                Watch_Face.DaysProgress.AnalogMonth.CenterOffset.X = (int)numericUpDown_ADMonth_ClockHand_Offset_X.Value;
                Watch_Face.DaysProgress.AnalogMonth.CenterOffset.Y = (int)numericUpDown_ADMonth_ClockHand_Offset_Y.Value;

                Watch_Face.DaysProgress.AnalogMonth.Sector.StartAngle =
                    (int)(numericUpDown_ADMonth_ClockHand_StartAngle.Value * 100);
                Watch_Face.DaysProgress.AnalogMonth.Sector.EndAngle =
                    (int)(numericUpDown_ADMonth_ClockHand_EndAngle.Value * 100);
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

                //switch (comboBox_StepsProgress_Flatness.Text)
                //{
                //    case "Треугольное":
                //        Watch_Face.StepsProgress.Circle.Flatness = 90;
                //        break;
                //    case "Плоское":
                //        Watch_Face.StepsProgress.Circle.Flatness = 180;
                //        break;
                //    default:
                //        Watch_Face.StepsProgress.Circle.Flatness = 0;
                //        break;
                //}
                switch (comboBox_StepsProgress_Flatness.SelectedIndex)
                {
                    case 1:
                        Watch_Face.StepsProgress.Circle.Flatness = 90;
                        break;
                    case 2:
                        Watch_Face.StepsProgress.Circle.Flatness = 180;
                        break;
                    default:
                        Watch_Face.StepsProgress.Circle.Flatness = 0;
                        break;
                }

                if (checkBox_StepsProgress_Image.Checked &&
                    comboBox_StepsProgress_Image.SelectedIndex >= 0)
                {
                    int imageX = (int)numericUpDown_StepsProgress_ImageX.Value;
                    int imageY = (int)numericUpDown_StepsProgress_ImageY.Value;
                    int imageIndex = comboBox_StepsProgress_Image.SelectedIndex;
                    colorStr = CoodinatesToColor(imageX, imageY);
                    Bitmap src = new Bitmap(ListImagesFullName[imageIndex]);
                    Watch_Face.StepsProgress.Circle.CenterX = imageX + src.Width / 2;
                    Watch_Face.StepsProgress.Circle.CenterY = imageY + src.Height / 2;
                    Watch_Face.StepsProgress.Circle.Color = colorStr;
                    Watch_Face.StepsProgress.Circle.ImageIndex = imageIndex;
                }
            }

            // прогресc шагов стрелкой
            if ((checkBox_StProg_ClockHand.Checked) && (comboBox_StProg_ClockHand_Image.SelectedIndex >= 0))
            {
                if (Watch_Face.StepsProgress == null) Watch_Face.StepsProgress = new StepsProgress();
                if (Watch_Face.StepsProgress.ClockHand == null) Watch_Face.StepsProgress.ClockHand = new ClockHand();
                if (Watch_Face.StepsProgress.ClockHand.CenterOffset == null)
                    Watch_Face.StepsProgress.ClockHand.CenterOffset = new Coordinates();
                if (Watch_Face.StepsProgress.ClockHand.Sector == null)
                    Watch_Face.StepsProgress.ClockHand.Sector = new Sector();
                //if (Watch_Face.AnalogDialFace.Hours.Shape == null)
                //Watch_Face.AnalogDialFace.Hours.Shape = new Coordinates();
                if (Watch_Face.StepsProgress.ClockHand.Image == null)
                    Watch_Face.StepsProgress.ClockHand.Image = new ImageW();

                Watch_Face.StepsProgress.ClockHand.Image.ImageIndex = Int32.Parse(comboBox_StProg_ClockHand_Image.Text);
                Watch_Face.StepsProgress.ClockHand.Image.X = (int)numericUpDown_StProg_ClockHand_X.Value;
                Watch_Face.StepsProgress.ClockHand.Image.Y = (int)numericUpDown_StProg_ClockHand_Y.Value;

                Watch_Face.StepsProgress.ClockHand.Color = "0x00000000";
                Watch_Face.StepsProgress.ClockHand.OnlyBorder = false;

                Watch_Face.StepsProgress.ClockHand.CenterOffset.X = (int)numericUpDown_StProg_ClockHand_Offset_X.Value;
                Watch_Face.StepsProgress.ClockHand.CenterOffset.Y = (int)numericUpDown_StProg_ClockHand_Offset_Y.Value;

                Watch_Face.StepsProgress.ClockHand.Sector.StartAngle =
                    (int)(numericUpDown_StProg_ClockHand_StartAngle.Value * 100);
                Watch_Face.StepsProgress.ClockHand.Sector.EndAngle =
                    (int)(numericUpDown_StProg_ClockHand_EndAngle.Value * 100);
            }

            // прогресc шагов набором иконок
            if (checkBox_SPSliced.Checked)
            {
                if ((comboBox_SPSliced_Image.SelectedIndex >= 0) && (dataGridView_SPSliced.Rows.Count > 1))
                {
                    if (Watch_Face.StepsProgress == null) Watch_Face.StepsProgress = new StepsProgress();
                    if (Watch_Face.StepsProgress.Sliced == null) Watch_Face.StepsProgress.Sliced = new IconSet();

                    Watch_Face.StepsProgress.Sliced.ImageIndex = Int32.Parse(comboBox_SPSliced_Image.Text);
                    Coordinates[] coordinates = new Coordinates[0];
                    int count = 0;

                    foreach (DataGridViewRow row in dataGridView_SPSliced.Rows)
                    {
                        //whatever you are currently doing
                        //Coordinates coordinates = new Coordinates();
                        int x = 0;
                        int y = 0;
                        if ((row.Cells[0].Value != null) && (row.Cells[1].Value != null))
                        {
                            if ((Int32.TryParse(row.Cells[0].Value.ToString(), out x)) && (Int32.TryParse(row.Cells[1].Value.ToString(), out y)))
                            {

                                //Array.Resize(ref objson, objson.Length + 1);
                                Array.Resize(ref coordinates, coordinates.Length + 1);
                                //objson[count] = coordinates;
                                coordinates[count] = new Coordinates();
                                coordinates[count].X = x;
                                coordinates[count].Y = y;
                                count++;
                            }
                        }
                        Watch_Face.StepsProgress.Sliced.Coordinates = coordinates;
                    }
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
                    string Alignment = StringToAlignment(comboBox_Battery_Text_Alignment.SelectedIndex);
                    Watch_Face.Battery.Text.Alignment = Alignment;
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

                if ((checkBox_Battery_ClockHand.Checked) && (comboBox_Battery_ClockHand_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Battery == null) Watch_Face.Battery = new Battery();
                    if (Watch_Face.Battery.Unknown4 == null) Watch_Face.Battery.Unknown4 = new ClockHand();
                    if (Watch_Face.Battery.Unknown4.CenterOffset == null)
                        Watch_Face.Battery.Unknown4.CenterOffset = new Coordinates();
                    if (Watch_Face.Battery.Unknown4.Sector == null)
                        Watch_Face.Battery.Unknown4.Sector = new Sector();
                    //if (Watch_Face.AnalogDialFace.Hours.Shape == null)
                    //Watch_Face.AnalogDialFace.Hours.Shape = new Coordinates();
                    if (Watch_Face.Battery.Unknown4.Image == null)
                        Watch_Face.Battery.Unknown4.Image = new ImageW();

                    Watch_Face.Battery.Unknown4.Image.ImageIndex = Int32.Parse(comboBox_Battery_ClockHand_Image.Text);
                    Watch_Face.Battery.Unknown4.Image.X = (int)numericUpDown_Battery_ClockHand_X.Value;
                    Watch_Face.Battery.Unknown4.Image.Y = (int)numericUpDown_Battery_ClockHand_Y.Value;

                    Watch_Face.Battery.Unknown4.Color = "0x00000000";
                    Watch_Face.Battery.Unknown4.OnlyBorder = false;

                    Watch_Face.Battery.Unknown4.CenterOffset.X = (int)numericUpDown_Battery_ClockHand_Offset_X.Value;
                    Watch_Face.Battery.Unknown4.CenterOffset.Y = (int)numericUpDown_Battery_ClockHand_Offset_Y.Value;

                    Watch_Face.Battery.Unknown4.Sector.StartAngle =
                        (int)(numericUpDown_Battery_ClockHand_StartAngle.Value * 100);
                    Watch_Face.Battery.Unknown4.Sector.EndAngle =
                        (int)(numericUpDown_Battery_ClockHand_EndAngle.Value * 100);
                }

                if ((checkBox_Battery_Percent.Checked) && (comboBox_Battery_Percent_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face.Battery == null) Watch_Face.Battery = new Battery();
                    if (Watch_Face.Battery.Percent == null) Watch_Face.Battery.Percent = new ImageW();

                    Watch_Face.Battery.Percent.ImageIndex = Int32.Parse(comboBox_Battery_Percent_Image.Text);
                    Watch_Face.Battery.Percent.X = (int)numericUpDown_Battery_Percent_X.Value;
                    Watch_Face.Battery.Percent.Y = (int)numericUpDown_Battery_Percent_Y.Value;
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

                    //switch (comboBox_Battery_Flatness.Text)
                    //{
                    //    case "Треугольное":
                    //        Watch_Face.Battery.Scale.Flatness = 90;
                    //        break;
                    //    case "Плоское":
                    //        Watch_Face.Battery.Scale.Flatness = 180;
                    //        break;
                    //    default:
                    //        Watch_Face.Battery.Scale.Flatness = 0;
                    //        break;
                    //}
                    switch (comboBox_Battery_Flatness.SelectedIndex)
                    {
                        case 1:
                            Watch_Face.Battery.Scale.Flatness = 90;
                            break;
                        case 2:
                            Watch_Face.Battery.Scale.Flatness = 180;
                            break;
                        default:
                            Watch_Face.Battery.Scale.Flatness = 0;
                            break;
                    }

                    if (checkBox_Battery_Scale_Image.Checked &&
                        comboBox_Battery_Scale_Image.SelectedIndex >= 0)
                    {
                        int imageX = (int)numericUpDown_Battery_Scale_ImageX.Value;
                        int imageY = (int)numericUpDown_Battery_Scale_ImageY.Value;
                        int imageIndex = comboBox_Battery_Scale_Image.SelectedIndex;
                        colorStr = CoodinatesToColor(imageX, imageY);
                        Bitmap src = new Bitmap(ListImagesFullName[imageIndex]);
                        Watch_Face.Battery.Scale.CenterX = imageX + src.Width / 2;
                        Watch_Face.Battery.Scale.CenterY = imageY + src.Height / 2;
                        Watch_Face.Battery.Scale.Color = colorStr;
                        Watch_Face.Battery.Scale.ImageIndex = imageIndex;
                    }
                }

                // батарея набором иконок
                if (checkBox_Battery_IconSet.Checked)
                {
                    if ((comboBox_Battery_IconSet_Image.SelectedIndex >= 0) && (dataGridView_Battery_IconSet.Rows.Count > 1))
                    {
                        if (Watch_Face.Battery == null) Watch_Face.Battery = new Battery();
                        if (Watch_Face.Battery.Icons == null) Watch_Face.Battery.Icons = new IconSet();

                        Watch_Face.Battery.Icons.ImageIndex = Int32.Parse(comboBox_Battery_IconSet_Image.Text);
                        Coordinates[] coordinates = new Coordinates[0];
                        int count = 0;

                        foreach (DataGridViewRow row in dataGridView_Battery_IconSet.Rows)
                        {
                            //whatever you are currently doing
                            //Coordinates coordinates = new Coordinates();
                            int x = 0;
                            int y = 0;
                            if ((row.Cells[0].Value != null) && (row.Cells[1].Value != null))
                            {
                                if ((Int32.TryParse(row.Cells[0].Value.ToString(), out x)) && (Int32.TryParse(row.Cells[1].Value.ToString(), out y)))
                                {

                                    //Array.Resize(ref objson, objson.Length + 1);
                                    Array.Resize(ref coordinates, coordinates.Length + 1);
                                    //objson[count] = coordinates;
                                    coordinates[count] = new Coordinates();
                                    coordinates[count].X = x;
                                    coordinates[count].Y = y;
                                    count++;
                                }
                            }
                            Watch_Face.Battery.Icons.Coordinates = coordinates;
                        }
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
                    string Alignment = StringToAlignment(comboBox_Weather_Text_Alignment.SelectedIndex);
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
                    string Alignment = StringToAlignment(comboBox_Weather_Day_Alignment.SelectedIndex);
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
                    string Alignment = StringToAlignment(comboBox_Weather_Night_Alignment.SelectedIndex);
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

            // ярлыки
            if (checkBox_Shortcuts.Checked)
            {
                if (checkBox_Shortcuts_Steps.Checked)
                {
                    if (Watch_Face.Shortcuts == null) Watch_Face.Shortcuts = new Shortcuts();
                    if (Watch_Face.Shortcuts.State == null) Watch_Face.Shortcuts.State = new Shortcut();
                    Watch_Face.Shortcuts.State.Element = new Element();
                    Watch_Face.Shortcuts.State.Element.TopLeftX = (int)numericUpDown_Shortcuts_Steps_X.Value;
                    Watch_Face.Shortcuts.State.Element.TopLeftY = (int)numericUpDown_Shortcuts_Steps_Y.Value;
                    Watch_Face.Shortcuts.State.Element.Width = (int)numericUpDown_Shortcuts_Steps_Width.Value;
                    Watch_Face.Shortcuts.State.Element.Height = (int)numericUpDown_Shortcuts_Steps_Height.Value;
                }

                if (checkBox_Shortcuts_Puls.Checked)
                {
                    if (Watch_Face.Shortcuts == null) Watch_Face.Shortcuts = new Shortcuts();
                    if (Watch_Face.Shortcuts.Pulse == null) Watch_Face.Shortcuts.Pulse = new Shortcut();
                    Watch_Face.Shortcuts.Pulse.Element = new Element();
                    Watch_Face.Shortcuts.Pulse.Element.TopLeftX = (int)numericUpDown_Shortcuts_Puls_X.Value;
                    Watch_Face.Shortcuts.Pulse.Element.TopLeftY = (int)numericUpDown_Shortcuts_Puls_Y.Value;
                    Watch_Face.Shortcuts.Pulse.Element.Width = (int)numericUpDown_Shortcuts_Puls_Width.Value;
                    Watch_Face.Shortcuts.Pulse.Element.Height = (int)numericUpDown_Shortcuts_Puls_Height.Value;
                }

                if (checkBox_Shortcuts_Weather.Checked)
                {
                    if (Watch_Face.Shortcuts == null) Watch_Face.Shortcuts = new Shortcuts();
                    if (Watch_Face.Shortcuts.Weather == null) Watch_Face.Shortcuts.Weather = new Shortcut();
                    Watch_Face.Shortcuts.Weather.Element = new Element();
                    Watch_Face.Shortcuts.Weather.Element.TopLeftX = (int)numericUpDown_Shortcuts_Weather_X.Value;
                    Watch_Face.Shortcuts.Weather.Element.TopLeftY = (int)numericUpDown_Shortcuts_Weather_Y.Value;
                    Watch_Face.Shortcuts.Weather.Element.Width = (int)numericUpDown_Shortcuts_Weather_Width.Value;
                    Watch_Face.Shortcuts.Weather.Element.Height = (int)numericUpDown_Shortcuts_Weather_Height.Value;
                }

                if (checkBox_Shortcuts_Energy.Checked)
                {
                    if (Watch_Face.Shortcuts == null) Watch_Face.Shortcuts = new Shortcuts();
                    if (Watch_Face.Shortcuts.Unknown4 == null) Watch_Face.Shortcuts.Unknown4 = new Shortcut();
                    Watch_Face.Shortcuts.Unknown4.Element = new Element();
                    Watch_Face.Shortcuts.Unknown4.Element.TopLeftX = (int)numericUpDown_Shortcuts_Energy_X.Value;
                    Watch_Face.Shortcuts.Unknown4.Element.TopLeftY = (int)numericUpDown_Shortcuts_Energy_Y.Value;
                    Watch_Face.Shortcuts.Unknown4.Element.Width = (int)numericUpDown_Shortcuts_Energy_Width.Value;
                    Watch_Face.Shortcuts.Unknown4.Element.Height = (int)numericUpDown_Shortcuts_Energy_Height.Value;
                }
            }

            richTextBox_JSON.Text = JsonConvert.SerializeObject(Watch_Face, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        private void AlignmentToString(ComboBox comboBoxAlignment, string Alignment)
        {
            int result = 0;
            switch (Alignment)
            {
                case "TopLeft":
                    //result = "Вверх влево";
                    result = 0;
                    break;
                case "TopCenter":
                    //result = "Вверх по центру";
                    result = 1;
                    break;
                case "TopRight":
                    //result = "Вверх вправо";
                    result = 2;
                    break;

                case "CenterLeft":
                    //result = "Середина влево";
                    result = 3;
                    break;
                case "Center":
                    //result = "Середина по центру";
                    result = 4;
                    break;
                case "CenterRight":
                    //result = "Середина вправо";
                    result = 5;
                    break;

                case "BottomLeft":
                    //result = "Вниз влево";
                    result = 6;
                    break;
                case "BottomCenter":
                    //result = "Вниз по центру";
                    result = 7;
                    break;
                case "BottomRight":
                    //result = "Вниз вправо";
                    result = 8;
                    break;

                case "Left":
                    //result = "Середина влево";
                    result = 3;
                    break;
                case "Right":
                    //result = "Середина вправо";
                    result = 5;
                    break;
                case "Top":
                    //result = "Вверх по центру";
                    result = 1;
                    break;
                case "Bottom":
                    //result = "Вниз по центру";
                    result = 7;
                    break;

                default:
                    //result = "Середина по центру";
                    result = 4;
                    break;

            }
            //return result;
            comboBoxAlignment.SelectedIndex = result;
        }

        private string StringToAlignment(int Alignment)
        {
            string result = "Center";
            switch (Alignment)
            {
                case 0:
                    result = "TopLeft";
                    break;
                case 1:
                    result = "TopCenter";
                    break;
                case 2:
                    result = "TopRight";
                    break;

                case 3:
                    result = "CenterLeft";
                    break;
                case 4:
                    result = "Center";
                    break;
                case 5:
                    result = "CenterRight";
                    break;

                case 6:
                    result = "BottomLeft";
                    break;
                case 7:
                    result = "BottomCenter";
                    break;
                case 8:
                    result = "BottomRight";
                    break;

                default:
                    result = "Center";
                    break;

            }
            return result;
        }

        private Color ColorRead(string color)
        {
            if(color.Length==18)  color = color.Remove(2, 8);
            Color old_color = ColorTranslator.FromHtml(color);
            Color new_color = Color.FromArgb(255, old_color.R, old_color.G, old_color.B);
            return new_color;
        }

        private void ColorToCoodinates(Color color, NumericUpDown numericUpDown_X, NumericUpDown numericUpDown_Y)
        {
            //string sColor = ColorTranslator.ToHtml(color);
            //string sColor = color.A.ToString("X") + color.R.ToString("X") + color.G.ToString("X") + color.B.ToString("X");
            string sColor = color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

            //sColor = "X123456";
            //sColor = sColor.Remove(0, 1);
            int sColorLenght = sColor.Length;
            string colorX = sColor.Remove(3, sColorLenght - 3);
            string colorY = sColor.Remove(0, sColorLenght - 3);

            //int myInt = 2934;
            //string myHex = myInt.ToString("X");  // Gives you hexadecimal
            //int myNewInt = Convert.ToInt32(myHex, 16);  // Back to int again.

            int X = Convert.ToInt32(colorX, 16);
            int Y = Convert.ToInt32(colorY, 16);
            numericUpDown_X.Value = X;
            numericUpDown_Y.Value = Y;
        }

        private string CoodinatesToColor(int X, int Y)
        {
            string colorX = X.ToString("X3");
            string colorY = Y.ToString("X3");
            string color = "0xFF" + colorX + colorY;

            //int myInt = 2934;
            //string myHex = myInt.ToString("X");  // Gives you hexadecimal
            //int myNewInt = Convert.ToInt32(myHex, 16);  // Back to int again.
            
            return color;
        }

        private void comboBoxSetText(ComboBox comboBox, long value)
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

        // сбрасываем все настройки отображения
        private void SettingsClear()
        {
            comboBox_Background.Items.Clear();
            comboBox_Background.Text = "";
            comboBox_Preview.Items.Clear();
            comboBox_Preview.Text = "";

            comboBox_Hours_Tens_Image.Text = "";
            comboBox_Hours_Tens_Image.Items.Clear();
            comboBox_Hours_Ones_Image.Text = "";
            comboBox_Hours_Ones_Image.Items.Clear();

            comboBox_Min_Tens_Image.Text = "";
            comboBox_Min_Tens_Image.Items.Clear();
            comboBox_Min_Ones_Image.Text = "";
            comboBox_Min_Ones_Image.Items.Clear();

            comboBox_Sec_Tens_Image.Text = "";
            comboBox_Sec_Tens_Image.Items.Clear();
            comboBox_Sec_Ones_Image.Text = "";
            comboBox_Sec_Ones_Image.Items.Clear();

            comboBox_Image_Am.Text = "";
            comboBox_Image_Am.Items.Clear();
            comboBox_Image_Pm.Text = "";
            comboBox_Image_Pm.Items.Clear();
            comboBox_Delimiter_Image.Text = "";
            comboBox_Delimiter_Image.Items.Clear();


            comboBox_WeekDay_Image.Text = "";
            comboBox_WeekDay_Image.Items.Clear();
            comboBox_DOW_IconSet_Image.Text = "";
            comboBox_DOW_IconSet_Image.Items.Clear();
            comboBox_OneLine_Delimiter.Text = "";
            comboBox_OneLine_Delimiter.Items.Clear();
            comboBox_OneLine_Image.Text = "";
            comboBox_OneLine_Image.Items.Clear();
            comboBox_MonthName_Image.Text = "";
            comboBox_MonthName_Image.Items.Clear();
            comboBox_MonthAndDayM_Image.Text = "";
            comboBox_MonthAndDayM_Image.Items.Clear();
            comboBox_MonthAndDayD_Image.Text = "";
            comboBox_MonthAndDayD_Image.Items.Clear();
            comboBox_Year_Image.Text = "";
            comboBox_Year_Image.Items.Clear();
            comboBox_Year_Delimiter.Text = "";
            comboBox_Year_Delimiter.Items.Clear();

            comboBox_ADDay_ClockHand_Image.Text = "";
            comboBox_ADDay_ClockHand_Image.Items.Clear();
            comboBox_ADWeekDay_ClockHand_Image.Text = "";
            comboBox_ADWeekDay_ClockHand_Image.Items.Clear();
            comboBox_ADMonth_ClockHand_Image.Text = "";
            comboBox_ADMonth_ClockHand_Image.Items.Clear();

            comboBox_StProg_ClockHand_Image.Text = "";
            comboBox_StProg_ClockHand_Image.Items.Clear();
            comboBox_SPSliced_Image.Text = "";
            comboBox_SPSliced_Image.Items.Clear();
            comboBox_StepsProgress_Image.Text = "";
            comboBox_StepsProgress_Image.Items.Clear();

            comboBox_ActivityCaloriesScale_Image.Text = "";
            comboBox_ActivityCaloriesScale_Image.Items.Clear();
            comboBox_ActivitySteps_Image.Text = "";
            comboBox_ActivitySteps_Image.Items.Clear();
            comboBox_ActivityDistance_Image.Text = "";
            comboBox_ActivityDistance_Image.Items.Clear();
            comboBox_ActivityDistance_Decimal.Text = "";
            comboBox_ActivityDistance_Decimal.Items.Clear();
            comboBox_ActivityDistance_Suffix.Text = "";
            comboBox_ActivityDistance_Suffix.Items.Clear();
            comboBox_ActivityPuls_Image.Text = "";
            comboBox_ActivityPuls_Image.Items.Clear();
            comboBox_ActivityPulsScale_Image.Text = "";
            comboBox_ActivityPulsScale_Image.Items.Clear();
            comboBox_Pulse_ClockHand_Image.Text = "";
            comboBox_Pulse_ClockHand_Image.Items.Clear();
            comboBox_ActivityPuls_IconSet_Image.Text = "";
            comboBox_ActivityPuls_IconSet_Image.Items.Clear();
            comboBox_ActivityCalories_Image.Text = "";
            comboBox_ActivityCalories_Image.Items.Clear();
            comboBox_Calories_ClockHand_Image.Text = "";
            comboBox_Calories_ClockHand_Image.Items.Clear();
            comboBox_ActivityStar_Image.Text = "";
            comboBox_ActivityStar_Image.Items.Clear();
            comboBox_Activity_NDImage.Text = "";
            comboBox_Activity_NDImage.Items.Clear();

            comboBox_Bluetooth_On.Text = "";
            comboBox_Bluetooth_On.Items.Clear();
            comboBox_Bluetooth_Off.Text = "";
            comboBox_Bluetooth_Off.Items.Clear();
            comboBox_Alarm_On.Text = "";
            comboBox_Alarm_On.Items.Clear();
            comboBox_Alarm_Off.Text = "";
            comboBox_Alarm_Off.Items.Clear();
            comboBox_Lock_On.Text = "";
            comboBox_Lock_On.Items.Clear();
            comboBox_Lock_Off.Text = "";
            comboBox_Lock_Off.Items.Clear();
            comboBox_DND_On.Text = "";
            comboBox_DND_On.Items.Clear();
            comboBox_DND_Off.Text = "";
            comboBox_DND_Off.Items.Clear();


            comboBox_Battery_Text_Image.Text = "";
            comboBox_Battery_Text_Image.Items.Clear();
            comboBox_Battery_Img_Image.Text = "";
            comboBox_Battery_Img_Image.Items.Clear();
            comboBox_Battery_Percent_Image.Text = "";
            comboBox_Battery_Percent_Image.Items.Clear();
            comboBox_Battery_ClockHand_Image.Text = "";
            comboBox_Battery_ClockHand_Image.Items.Clear();
            comboBox_Battery_IconSet_Image.Text = "";
            comboBox_Battery_IconSet_Image.Items.Clear();
            comboBox_Battery_Scale_Image.Text = "";
            comboBox_Battery_Scale_Image.Items.Clear();

            comboBox_AnalogClock_Hour_Image.Text = "";
            comboBox_AnalogClock_Hour_Image.Items.Clear();
            comboBox_AnalogClock_Min_Image.Text = "";
            comboBox_AnalogClock_Min_Image.Items.Clear();
            comboBox_AnalogClock_Sec_Image.Text = "";
            comboBox_AnalogClock_Sec_Image.Items.Clear();

            comboBox_HourCenterImage_Image.Text = "";
            comboBox_HourCenterImage_Image.Items.Clear();
            comboBox_MinCenterImage_Image.Text = "";
            comboBox_MinCenterImage_Image.Items.Clear();
            comboBox_SecCenterImage_Image.Text = "";
            comboBox_SecCenterImage_Image.Items.Clear();

            comboBox_Weather_Text_Image.Text = "";
            comboBox_Weather_Text_Image.Items.Clear();
            comboBox_Weather_Text_DegImage.Text = "";
            comboBox_Weather_Text_DegImage.Items.Clear();
            comboBox_Weather_Text_MinusImage.Text = "";
            comboBox_Weather_Text_MinusImage.Items.Clear();
            comboBox_Weather_Text_NDImage.Text = "";
            comboBox_Weather_Text_NDImage.Items.Clear();
            comboBox_Weather_Icon_Image.Text = "";
            comboBox_Weather_Icon_Image.Items.Clear();
            comboBox_Weather_Icon_NDImage.Text = "";
            comboBox_Weather_Icon_NDImage.Items.Clear();
            comboBox_Weather_Day_Image.Text = "";
            comboBox_Weather_Day_Image.Items.Clear();
            comboBox_Weather_Night_Image.Text = "";
            comboBox_Weather_Night_Image.Items.Clear();

        }

        // устанавливаем тип циферблата исходя из DeviceId
        private void ReadDeviceId()
        {
            if (Watch_Face.Info != null)
            {
                switch (Watch_Face.Info.DeviceId)
                {
                    case 40:
                        radioButton_47.Checked = true;
                        break;
                    case 42:
                        radioButton_42.Checked = true;
                        break;
                    case 46:
                        radioButton_gts.Checked = true;
                        break;
                    default:
                        return;
                }

                if (radioButton_47.Checked)
                {
                    this.Text = "GTR watch face editor";
                    panel_Preview.Height = 230;
                    panel_Preview.Width = 230;
                    offSet_X = 227;
                    offSet_Y = 227;

                    //Properties.Settings.Default.unpack_command_GTR42 = textBox_unpack_command.Text;
                    //Properties.Settings.Default.pack_command_GTR42 = textBox_pack_command.Text;
                    //Properties.Settings.Default.Save();
                    //Program_Settings.unpack_command_GTR42 = textBox_unpack_command.Text;
                    //Program_Settings.pack_command_GTR42 = textBox_pack_command.Text;

                    //textBox_unpack_command.Text = "--gtr 47 --file";
                    //textBox_pack_command.Text = "--gtr 47 --file";
                    //if (Properties.Settings.Default.unpack_command.Length > 1)
                    //    textBox_unpack_command.Text = Properties.Settings.Default.unpack_command;
                    //if (Properties.Settings.Default.pack_command.Length > 1)
                    //    textBox_pack_command.Text = Properties.Settings.Default.pack_command;
                    textBox_unpack_command.Text = Program_Settings.unpack_command_GTR47;
                    textBox_pack_command.Text = Program_Settings.pack_command_GTR47;

                    button_unpack.Enabled = true;
                    button_pack.Enabled = true;
                    button_zip.Enabled = true;
                }
                else if (radioButton_42.Checked)
                {
                    this.Text = "GTR watch face editor";
                    panel_Preview.Height = 198;
                    panel_Preview.Width = 198;
                    offSet_X = 195;
                    offSet_Y = 195;

                    textBox_unpack_command.Text = Program_Settings.unpack_command_GTR42;
                    textBox_pack_command.Text = Program_Settings.pack_command_GTR42;

                    button_unpack.Enabled = true;
                    button_pack.Enabled = true;
                    button_zip.Enabled = true;
                }
                else
                {
                    this.Text = "GTS watch face editor";
                    panel_Preview.Height = 223;
                    panel_Preview.Width = 176;
                    offSet_X = 174;
                    offSet_Y = 221;

                    textBox_unpack_command.Text = Program_Settings.unpack_command_GTS;
                    textBox_pack_command.Text = Program_Settings.pack_command_GTS;

                    button_unpack.Enabled = false;
                    button_pack.Enabled = false;
                    button_zip.Enabled = false;
                }

                if ((formPreview != null) && (formPreview.Visible))
                {
                    Form_Preview.Model_Wath.model_gtr47 = radioButton_47.Checked;
                    Form_Preview.Model_Wath.model_gtr42 = radioButton_42.Checked;
                    Form_Preview.Model_Wath.model_gts = radioButton_gts.Checked;
                }

                Program_Settings.Model_GTR47 = radioButton_47.Checked;
                Program_Settings.Model_GTR42 = radioButton_42.Checked;
                Program_Settings.Model_GTS = radioButton_gts.Checked;
                string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                System.IO.File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, 
                    System.Text.Encoding.UTF8);
            }
        }



    }
}
