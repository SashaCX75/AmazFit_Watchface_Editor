using ImageMagick;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using LineCap = System.Drawing.Drawing2D.LineCap;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace GTR_Watch_face
{
    public partial class Form1 : Form
    {
        WATCH_FACE_JSON Watch_Face;
        WATCH_FACE_PREWIEV_TwoDigits Watch_Face_Preview_TwoDigits;
        WATCH_FACE_PREWIEV_SET Watch_Face_Preview_Set;
        List<string> ListImages = new List<string>(); // перечень имен файлов с картинками
        List<string> ListImagesFullName = new List<string>(); // перечень путей к файлам с картинками
        bool PreviewView; // включает прорисовку предпросмотра
        bool Settings_Load; // включать при обновлении настроек длу выключения перерисовки
        string FileName; // Запоминает имя для диалогов
        string FullFileDir; // Запоминает папку для диалогов
        string StartFileNameJson; // имя файла из параметров запуска
        string StartFileNameBin; // имя файла из параметров запуска
        float currentDPI; // масштаб экрана
        Form_Preview formPreview;
        PROGRAM_SETTINGS Program_Settings;
        
        int offSet_X = 227; // координаты центра циферблата
        int offSet_Y = 227;


        public Form1(string[] args)
        {
            //Logger.WriteLine("Form1");
            //if (File.Exists(Application.StartupPath + "\\log.txt")) File.Delete(Application.StartupPath + @"\log.txt");
            Program_Settings = new PROGRAM_SETTINGS();
            try
            {
                if (File.Exists(Application.StartupPath + @"\Settings.json"))
                {
                    Program_Settings = JsonConvert.DeserializeObject<PROGRAM_SETTINGS>
                                (File.ReadAllText(Application.StartupPath + @"\Settings.json"), new JsonSerializerSettings
                                {
                        //DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                                });
                    //Logger.WriteLine("Чтение Settings.json");
                }


                if ((Program_Settings.language == null) || (Program_Settings.language.Length < 2))
                {
                    string language = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                    //int language = System.Globalization.CultureInfo.CurrentCulture.LCID;
                    //Program_Settings.language = "Русский";
                    Program_Settings.language = "English";
                    if (language == "ru")
                    {
                        Program_Settings.language = "Русский";
                    }
                    if (language == "en")
                    {
                        Program_Settings.language = "English";
                    }
                    if (language == "es")
                    {
                        Program_Settings.language = "Español";
                    }
                    if (language == "pt")
                    {
                        Program_Settings.language = "Português";
                    }
                    if (language == "cs")
                    {
                        Program_Settings.language = "Čeština";
                    }
                    if (language == "hu")
                    {
                        Program_Settings.language = "Magyar";
                    }
                    if (language == "sk")
                    {
                        Program_Settings.language = "Slovenčina";
                    }
                }
                //Logger.WriteLine("Определили язык");
                SetLanguage();
                //if (Program_Settings.language == "English")
                //{
                //    Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");
                //    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                //}
                //else if(Program_Settings.language == "Español")
                //{
                //    Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es");
                //    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("es");
                //}
                //else
                //{
                //    Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru");
                //    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ru");
                //}
                //Logger.WriteLine("Применили язык");
            }
            catch (Exception)
            {
                //Logger.WriteLine("Ошибка чтения настроек " + ex);
            }
            
            InitializeComponent();

            Watch_Face_Preview_Set = new WATCH_FACE_PREWIEV_SET();
            Watch_Face_Preview_Set.Activity = new ActivityS();
            Watch_Face_Preview_Set.Date = new DateS();
            Watch_Face_Preview_Set.Status = new StatusS();
            Watch_Face_Preview_Set.Time = new TimeS();

            Watch_Face_Preview_TwoDigits = new WATCH_FACE_PREWIEV_TwoDigits();
            Watch_Face_Preview_TwoDigits.Date = new DateP();
            Watch_Face_Preview_TwoDigits.Date.Day = new TwoDigitsP();
            Watch_Face_Preview_TwoDigits.Date.Month = new TwoDigitsP();

            Watch_Face_Preview_TwoDigits.Year = new YearP();

            Watch_Face_Preview_TwoDigits.Time = new TimeP();
            Watch_Face_Preview_TwoDigits.Time.Hours = new TwoDigitsP();
            Watch_Face_Preview_TwoDigits.Time.Minutes = new TwoDigitsP();
            Watch_Face_Preview_TwoDigits.Time.Seconds = new TwoDigitsP();

            Watch_Face_Preview_TwoDigits.TimePm = new TimePmP();
            Watch_Face_Preview_TwoDigits.TimePm.Hours = new TwoDigitsP();
            Watch_Face_Preview_TwoDigits.TimePm.Minutes = new TwoDigitsP();
            Watch_Face_Preview_TwoDigits.TimePm.Seconds = new TwoDigitsP();
            

            PreviewView = true;
            Settings_Load = false;
            currentDPI = (int)Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "LogPixels", 96)/96f;
            //Logger.WriteLine("Создали переменные");

            if (args.Length == 1)
            {
                string fileName = args[0].ToString();
                if ((File.Exists(fileName)) && (Path.GetExtension(fileName) == ".json"))
                {
                    StartFileNameJson = fileName;
                    //Logger.WriteLine("Программа запущена с аргументом: " + fileName);
                }
                if ((File.Exists(fileName)) && (Path.GetExtension(fileName) == ".bin"))
                {
                    StartFileNameBin = fileName;
                    //Logger.WriteLine("Программа запущена с аргументом: " + fileName);
                }
            }

        }
        
        private void SetLanguage()
        {
            if (Program_Settings.language == "English")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
            }
            else if (Program_Settings.language == "Español")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("es");
            }
            else if (Program_Settings.language == "Português")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("pt");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("pt");
            }
            else if (Program_Settings.language == "Čeština")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("cs");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("cs");
            }
            else if (Program_Settings.language == "Magyar")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("hu");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("hu");
            }
            else if (Program_Settings.language == "Slovenčina")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("sk");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("sk");
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ru");
            }
        }
        //private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    Properties.Settings.Default.pack_unpack_dir = textBox_pack_unpack_dir.Text;
        //    if (radioButton_47.Checked)
        //    {
        //        Properties.Settings.Default.unpack_command = textBox_unpack_command.Text;
        //        Properties.Settings.Default.pack_command = textBox_pack_command.Text;
        //    }
        //    else
        //    {
        //        Properties.Settings.Default.unpack_command_GTR42 = textBox_unpack_command.Text;
        //        Properties.Settings.Default.pack_command_GTR42 = textBox_pack_command.Text;
        //    }
        //    Properties.Settings.Default.Save();
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            //Logger.WriteLine("Form1_Load");
            helpProvider1.HelpNamespace = Application.StartupPath + Properties.FormStrings.File_ReadMy;
#if Puthon
            string subPath = Application.StartupPath + @"\py_amazfit_tools-fix_array\main.py";
#else
            string subPath = Application.StartupPath + @"\main_v0.2-beta\main.exe";
            //string subPath = Application.StartupPath + @"\main\main.exe";
#endif

            if (Program_Settings.pack_unpack_dir == null)
            {
                Program_Settings.pack_unpack_dir = subPath;
                string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
            }
            textBox_pack_unpack_dir.Text = Program_Settings.pack_unpack_dir;
            if (Program_Settings.Model_GTS)
            {
                radioButton_gts.Checked = Program_Settings.Model_GTS;
                textBox_unpack_command.Text = Program_Settings.unpack_command_GTS;
                textBox_pack_command.Text = Program_Settings.pack_command_GTS;
            }
            else if (Program_Settings.Model_GTR42)
            {
                radioButton_42.Checked = Program_Settings.Model_GTR42;
                textBox_unpack_command.Text = Program_Settings.unpack_command_GTR42;
                textBox_pack_command.Text = Program_Settings.pack_command_GTR42;
            }
            else
            {
                radioButton_47.Checked = Program_Settings.Model_GTR47;
                textBox_unpack_command.Text = Program_Settings.unpack_command_GTR47;
                textBox_pack_command.Text = Program_Settings.pack_command_GTR47;
            }

            PreviewView = false;
            //checkBox_border.Checked = Program_Settings.ShowBorder;
            //comboBox_MonthAndDayD_Alignment.Text = "Вверх влево";
            //comboBox_MonthAndDayM_Alignment.Text = "Вверх влево";
            //comboBox_OneLine_Alignment.Text = "Вверх влево";
            //comboBox_Year_Alignment.Text = "Вверх влево";

            //comboBox_ActivityGoal_Alignment.Text = "Вверх влево";
            //comboBox_ActivitySteps_Alignment.Text = "Вверх влево";
            //comboBox_ActivityDistance_Alignment.Text = "Вверх влево";
            //comboBox_ActivityPuls_Alignment.Text = "Вверх влево";
            //comboBox_ActivityCalories_Alignment.Text = "Вверх влево";
            //comboBox_Battery_Text_Alignment.Text = "Вверх влево";

            //comboBox_Weather_Text_Alignment.Text = "Вверх влево";
            //comboBox_Weather_Day_Alignment.Text = "Вверх влево";
            //comboBox_Weather_Night_Alignment.Text = "Вверх влево";

            //comboBox_Battery_Flatness.Text = "Круглое";
            //comboBox_StepsProgress_Flatness.Text = "Круглое";
            checkBox_border.Checked = Program_Settings.ShowBorder;
            checkBox_crop.Checked = Program_Settings.Crop;
            checkBox_Show_Shortcuts.Checked = Program_Settings.Show_Shortcuts;
            comboBox_MonthAndDayD_Alignment.SelectedIndex = 0;
            comboBox_MonthAndDayM_Alignment.SelectedIndex = 0;
            comboBox_OneLine_Alignment.SelectedIndex = 0;
            comboBox_Year_Alignment.SelectedIndex = 0;
            
            comboBox_ActivitySteps_Alignment.SelectedIndex = 0;
            comboBox_ActivityDistance_Alignment.SelectedIndex = 0;
            comboBox_ActivityPuls_Alignment.SelectedIndex = 0;
            comboBox_ActivityCalories_Alignment.SelectedIndex = 0;
            comboBox_Battery_Text_Alignment.SelectedIndex = 0;

            comboBox_Weather_Text_Alignment.SelectedIndex = 0;
            comboBox_Weather_Day_Alignment.SelectedIndex = 0;
            comboBox_Weather_Night_Alignment.SelectedIndex = 0;

            comboBox_Battery_Flatness.SelectedIndex = 0;
            comboBox_StepsProgress_Flatness.SelectedIndex = 0;

            label_version.Text = "v " +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
            label_version_help.Text =
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();

            Settings_Load = true;
            radioButton_Settings_AfterUnpack_Dialog.Checked = Program_Settings.Settings_AfterUnpack_Dialog;
            radioButton_Settings_AfterUnpack_DoNothing.Checked = Program_Settings.Settings_AfterUnpack_DoNothing;
            radioButton_Settings_AfterUnpack_Download.Checked = Program_Settings.Settings_AfterUnpack_Download;

            radioButton_Settings_Open_Dialog.Checked = Program_Settings.Settings_Open_Dialog;
            radioButton_Settings_Open_DoNotning.Checked = Program_Settings.Settings_Open_DoNotning;
            radioButton_Settings_Open_Download.Checked = Program_Settings.Settings_Open_Download;

            radioButton_Settings_Pack_Copy.Checked = Program_Settings.Settings_Pack_Copy;
            radioButton_Settings_Pack_Dialog.Checked = Program_Settings.Settings_Pack_Dialog;
            radioButton_Settings_Pack_DoNotning.Checked = Program_Settings.Settings_Pack_DoNotning;
            radioButton_Settings_Pack_GoToFile.Checked = Program_Settings.Settings_Pack_GoToFile;

            radioButton_Settings_Unpack_Dialog.Checked = Program_Settings.Settings_Unpack_Dialog;
            radioButton_Settings_Unpack_Replace.Checked = Program_Settings.Settings_Unpack_Replace;
            radioButton_Settings_Unpack_Save.Checked = Program_Settings.Settings_Unpack_Save;
            numericUpDown_Gif_Speed.Value = (decimal)Program_Settings.Gif_Speed;

            checkBox_Shortcuts_Area.Checked = Program_Settings.Shortcuts_Area;
            checkBox_Shortcuts_Border.Checked = Program_Settings.Shortcuts_Border;

            if (Program_Settings.language.Length>1) comboBox_Language.Text = Program_Settings.language;

            Settings_Load = false;

            SetPreferences1();
            PreviewView = true;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

            //Logger.WriteLine("Form1_Shown");
            //Logger.WriteLine("Загружаем файл из значения аргумента " + StartFileNameJson);
            if ((StartFileNameJson != null) && (StartFileNameJson.Length > 0))
            {
                LoadJsonAndImage(StartFileNameJson);
                StartFileNameJson = "";
            }
            else if ((StartFileNameBin != null) && (StartFileNameBin.Length > 0))
            {
                zip_unpack_bin(StartFileNameBin);
                StartFileNameBin = "";
            }
            //Logger.WriteLine("Загрузили файл из значения аргумента " + StartFileNameJson);
        }

        private void button_pack_unpack_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = textBox_pack_unpack_dir.Text;
            //openFileDialog.InitialDirectory = @"C:\main_gtr\remake_by_kolomnych_045_ru-65473-787ef4d01c";
            //openFileDialog.Filter = "Json files (*.json) | *.json";
            openFileDialog.Filter = "";
            ////openFileDialog1.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_PackUnpack;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox_pack_unpack_dir.Text = openFileDialog.FileName;

                Program_Settings.pack_unpack_dir = openFileDialog.FileName;
                string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
            }
        }

        private void Form1_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            //FormFileExists f = new FormFileExists();
            //f.ShowDialog();
            //SendKeys.Send("{F1}");
            Help.ShowHelp(this, Application.StartupPath + Properties.FormStrings.File_ReadMy);
            e.Cancel = true;
        }

        private void button_zip_unpack_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Json files (*.json) | *.json";
            openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            ////openFileDialog1.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_Unpack;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullfilename = openFileDialog.FileName;
                zip_unpack_bin(fullfilename);
            }
        }

        private void zip_unpack_bin(string fullfilename)
        {
            string subPath = Application.StartupPath + @"\Watch_face\";
            if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);
            string respackerPath = Application.StartupPath + @"\Res_PackerUnpacker\";
            if (Is64Bit()) respackerPath = respackerPath + @"x64\resunpacker.exe";
            else respackerPath = respackerPath + @"x86\resunpacker.exe";

#if !DEBUG
            if (!File.Exists(respackerPath))
            {
                MessageBox.Show(Properties.FormStrings.Message_error_respackerPath_Text1 + Environment.NewLine +
                    Properties.FormStrings.Message_error_respackerPath_Text2 + respackerPath + "].\r\n\r\n" +
                    Properties.FormStrings.Message_error_respackerPath_Text3,
                    Properties.FormStrings.Message_error_pack_unpack_dir_Caption, 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
#endif

            string filename = Path.GetFileName(fullfilename);
            filename = filename.Replace(" ", "_");
            string fullPath = subPath + filename;
            // если файл существует
            if (File.Exists(fullPath))
            {
                FormFileExists f = new FormFileExists();
                f.ShowDialog();
                int dialogResult = f.Data;

                string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
                string extension = Path.GetExtension(fullPath);
                string path = Path.GetDirectoryName(fullPath);
                string newFullPath = fullPath;
                switch (dialogResult)
                {
                    case 0:
                        return;
                    //break;
                    case 1:
                        File.Copy(fullfilename, fullPath, true);
                        newFullPath = Path.Combine(path, fileNameOnly);
                        if (Directory.Exists(newFullPath)) Directory.Delete(newFullPath, true);
                        break;
                    case 2:
                        int count = 1;

                        while (File.Exists(newFullPath))
                        {
                            string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                            newFullPath = Path.Combine(path, tempFileName + extension);
                        }
                        File.Copy(fullfilename, newFullPath);
                        fullPath = newFullPath;
                        fileNameOnly = Path.GetFileNameWithoutExtension(newFullPath);
                        path = Path.GetDirectoryName(newFullPath);
                        newFullPath = Path.Combine(path, fileNameOnly);
                        if (Directory.Exists(newFullPath)) Directory.Delete(newFullPath, true);
                        break;
                }
            }
            else File.Copy(fullfilename, fullPath);

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = respackerPath;
                startInfo.Arguments = fullPath;
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();//ждем 
                };
                // этот блок закончится только после окончания работы программы 
                //сюда писать команды после успешного завершения программы

                string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
                //string extension = Path.GetExtension(fullPath);
                string path = Path.GetDirectoryName(fullPath);
                //path = Path.Combine(path, fileNameOnly);
                string newFullName_unp = Path.Combine(path, fileNameOnly + ".bin.unp");
                //string newFullName_bin = Path.Combine(path, fileNameOnly + ".unp.bin");
                string newFullName_bin = Path.Combine(path, fileNameOnly + ".bin");

                if (File.Exists(newFullName_unp))
                {
                    File.Copy(newFullName_unp, newFullName_bin, true);
                    this.BringToFront();
                    //после декомпресии bin файла
                    if (File.Exists(newFullName_bin))
                    {
                        File.Delete(newFullName_unp);
#if !DEBUG
                            if (!File.Exists(textBox_pack_unpack_dir.Text))
                            {
                                MessageBox.Show(Properties.FormStrings.Message_error_pack_unpack_dir_Text1 +
                                    textBox_pack_unpack_dir.Text + Properties.FormStrings.Message_error_pack_unpack_dir_Text2 +
                                    Environment.NewLine + Environment.NewLine + Properties.FormStrings.Message_error_pack_unpack_dir_Text3,
                                    Properties.FormStrings.Message_error_pack_unpack_dir_Caption,
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
#endif
                        startInfo.FileName = textBox_pack_unpack_dir.Text;
                        startInfo.Arguments = textBox_unpack_command.Text + "   " + newFullName_bin;
                        using (Process exeProcess = Process.Start(startInfo))
                        {
                            exeProcess.WaitForExit();//ждем 
                        };
                        // этот блок закончится только после окончания работы программы 
                        //сюда писать команды после успешного завершения программы
                        fileNameOnly = Path.GetFileNameWithoutExtension(newFullName_bin);
                        //string extension = Path.GetExtension(fullPath);
                        path = Path.GetDirectoryName(newFullName_bin);
                        path = Path.Combine(path, fileNameOnly);
                        string newFullName = Path.Combine(path, fileNameOnly + ".json");

                        //MessageBox.Show(newFullName);
                        if (Program_Settings.Settings_AfterUnpack_Dialog)
                        {
                            if (File.Exists(newFullName))
                            {
                                this.BringToFront();
                                if (MessageBox.Show(Properties.FormStrings.Message_openProject_Text, Properties.FormStrings.
                                    Message_openProject_Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    LoadJsonAndImage(newFullName);
                                    //newFullName = Path.Combine(path, "PreviewStates.json");
                                    //if (File.Exists(newFullName))
                                    //{
                                    //    JsonPreview_Read(newFullName);
                                    //}
                                }
                            }
                        }
                        else if (Program_Settings.Settings_AfterUnpack_Download)
                        {
                            LoadJsonAndImage(newFullName);
                        }
                    }
                }
            }
            catch
            {
                // сюда писать команды при ошибке вызова 
            }
        }

        private void button_unpack_Click(object sender, EventArgs e)
        {
            string subPath = Application.StartupPath + @"\Watch_face\";
            if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Json files (*.json) | *.json";
            openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            ////openFileDialog1.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_Unpack;

#if !DEBUG
            if (!File.Exists(textBox_pack_unpack_dir.Text))
            {
                MessageBox.Show(Properties.FormStrings.Message_error_pack_unpack_dir_Text1 +
                    textBox_pack_unpack_dir.Text + Properties.FormStrings.Message_error_pack_unpack_dir_Text2 +
                    Environment.NewLine + Environment.NewLine + Properties.FormStrings.Message_error_pack_unpack_dir_Text3,
                    Properties.FormStrings.Message_error_pack_unpack_dir_Caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
#endif

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullfilename = openFileDialog.FileName;
                string filename = Path.GetFileName(fullfilename);
                filename = filename.Replace(" ", "_");
                string fullPath = subPath + filename;
                if (File.Exists(fullPath))
                {
                    string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
                    string extension = Path.GetExtension(fullPath);
                    string path = Path.GetDirectoryName(fullPath);
                    string newFullPath = fullPath;
                    //if (radioButton_Settings_Unpack_Dialog.Checked)
                    if (Program_Settings.Settings_Unpack_Dialog)
                    {
                        FormFileExists f = new FormFileExists();
                        f.ShowDialog();
                        int dialogResult = f.Data;
                        switch (dialogResult)
                        {
                            case 0:
                                return;
                                break;
                            case 1:
                                File.Copy(fullfilename, fullPath, true);
                                newFullPath = Path.Combine(path, fileNameOnly);
                                if (Directory.Exists(newFullPath)) Directory.Delete(newFullPath, true);
                                break;
                            case 2:
                                int count = 1;

                                while (File.Exists(newFullPath))
                                {
                                    string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                                    newFullPath = Path.Combine(path, tempFileName + extension);
                                }
                                File.Copy(fullfilename, newFullPath);
                                fullPath = newFullPath;
                                fileNameOnly = Path.GetFileNameWithoutExtension(newFullPath);
                                path = Path.GetDirectoryName(newFullPath);
                                newFullPath = Path.Combine(path, fileNameOnly);
                                if (Directory.Exists(newFullPath)) Directory.Delete(newFullPath, true);
                                break;
                        } 
                    }
                    //else if (radioButton_Settings_Unpack_Save.Checked)
                    else if (Program_Settings.Settings_Unpack_Save)
                    {
                        int count = 1;

                        while (File.Exists(newFullPath))
                        {
                            string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                            newFullPath = Path.Combine(path, tempFileName + extension);
                        }
                        File.Copy(fullfilename, newFullPath);
                        fullPath = newFullPath;
                        fileNameOnly = Path.GetFileNameWithoutExtension(newFullPath);
                        path = Path.GetDirectoryName(newFullPath);
                        newFullPath = Path.Combine(path, fileNameOnly);
                        if (Directory.Exists(newFullPath)) Directory.Delete(newFullPath, true);
                    }
                    //else if (radioButton_Settings_Unpack_Replace.Checked)
                    else if (Program_Settings.Settings_Unpack_Replace)
                    {
                        File.Copy(fullfilename, fullPath, true);
                        newFullPath = Path.Combine(path, fileNameOnly);
                        if (Directory.Exists(newFullPath)) Directory.Delete(newFullPath, true);
                    }
                }
                else File.Copy(fullfilename, fullPath);

#if !DEBUG

                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = textBox_pack_unpack_dir.Text;
                    startInfo.Arguments = textBox_unpack_command.Text + "   " + fullPath;
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();//ждем 
                    };
                    // этот блок закончится только после окончания работы программы 
                    //сюда писать команды после успешного завершения программы
                    string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
                    //string extension = Path.GetExtension(fullPath);
                    string path = Path.GetDirectoryName(fullPath);
                    path = Path.Combine(path, fileNameOnly);
                    string newFullName = Path.Combine(path, fileNameOnly + ".json");

                    //MessageBox.Show(newFullName);
                    if (Program_Settings.Settings_AfterUnpack_Dialog)
                    {
                        if (File.Exists(newFullName))
                        {
                            this.BringToFront();
                            if (MessageBox.Show(Properties.FormStrings.Message_openProject_Text,
                                Properties.FormStrings.Message_openProject_Caption, 
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                LoadJsonAndImage(newFullName);
                                //newFullName = Path.Combine(path, "PreviewStates.json");
                                //if (File.Exists(newFullName))
                                //{
                                //    JsonPreview_Read(newFullName);
                                //}
                            }
                        } 
                    }
                    else if (Program_Settings.Settings_AfterUnpack_Download)
                    {
                        LoadJsonAndImage(newFullName);
                    }
                }
                catch
                {
                    // сюда писать команды при ошибке вызова 
                }

                /* try
                {
                    //Process _process = new Process();
                    //ProcessStartInfo startInfo = new ProcessStartInfo();
                    //startInfo.FileName = textBox_pack_unpack_dir.Text;
                    //startInfo.Arguments = textBox_unpack_command.Text + "   " + fullPath;
                    //_process.StartInfo = startInfo;
                    //_process.Start();

                    do
                    {
                        if (!myProcess.HasExited)
                        {
                            if (myProcess.Responding)
                            {
                                Console.WriteLine("Status = Running");
                            }
                            else
                            {
                                Console.WriteLine("Status = Not Responding");
                            }
                        }
                    }
                    while (!myProcess.WaitForExit(1000));
                }
                finally
                {
                    if (_process != null)
                    {
                        _process.Close();
                    }
                }
                */
#endif
            }
        }

        private void button_pack_Click(object sender, EventArgs e)
        {
            string subPath = Application.StartupPath + @"\Watch_face\";
            if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = FullFileDir;
            openFileDialog.FileName = FileName;
            openFileDialog.Filter = "Json files (*.json) | *.json";
            //openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            ////openFileDialog1.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_Pack;

#if !DEBUG
            if (!File.Exists(textBox_pack_unpack_dir.Text))
            {
                MessageBox.Show(Properties.FormStrings.Message_error_pack_unpack_dir_Text1 +
                    textBox_pack_unpack_dir.Text + Properties.FormStrings.Message_error_pack_unpack_dir_Text2 +
                    Environment.NewLine + Environment.NewLine + Properties.FormStrings.Message_error_pack_unpack_dir_Text3,
                    Properties.FormStrings.Message_error_pack_unpack_dir_Caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fullfilename = openFileDialog.FileName;
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = textBox_pack_unpack_dir.Text;
                    startInfo.Arguments = textBox_pack_command.Text + "   " + fullfilename;
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();//ждем 
                    };
                    // этот блок закончится только после окончания работы программы 
                    //сюда писать команды после успешного завершения программы
                    string fileNameOnly = Path.GetFileNameWithoutExtension(fullfilename);
                    //string extension = Path.GetExtension(fullPath);
                    string path = Path.GetDirectoryName(fullfilename);
                    string newFullName = Path.Combine(path, fileNameOnly + "_packed.bin");

                    //MessageBox.Show(newFullName);
                    if (File.Exists(newFullName))
                    {
                        this.BringToFront();
                        double fileSize = (GetFileSizeMB(new FileInfo(newFullName)));
                        if (fileSize > 1.95) MessageBox.Show(Properties.FormStrings.Message_bigFile_Text1 + Environment.NewLine + Environment.NewLine +
                            Properties.FormStrings.Message_bigFile_Text2, Properties.FormStrings.Message_bigFile_Caption,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //if (radioButton_Settings_Pack_Dialog.Checked)
                        if (Program_Settings.Settings_Pack_Dialog)
                        {
                            if (MessageBox.Show(Properties.FormStrings.Message_GoToFile_Text,
                                Properties.FormStrings.Message_GoToFile_Caption,
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName));
                                //StringCollection paths = new StringCollection();
                                //paths.Add(newFullName);
                                //Clipboard.SetFileDropList(paths);

                                //FileStream fs = new FileStream(newFullName, FileMode.Open);
                                //Clipboard.SetDataObject(fs, true);
                                //fs.Close();
                            }
                        }
                        else if (Program_Settings.Settings_Pack_Copy)
                        {
                            StringCollection paths = new StringCollection();
                            paths.Add(newFullName);
                            Clipboard.SetFileDropList(paths);
                        }
                        else if (Program_Settings.Settings_Pack_GoToFile)
                        {
                            Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName));
                        }
                    }
                }
                catch
                {
                    // сюда писать команды при ошибке вызова 
                }

                /*string fullfilename = openFileDialog.FileName;
                Process _process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = textBox_pack_unpack_dir.Text;
                startInfo.Arguments = textBox_pack_command.Text + "   " + fullfilename;
                _process.StartInfo = startInfo;
                _process.Start();*/
            }
#endif
        }

        private void button_zip_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Json files (*.json) | *.json";
            openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            openFileDialog.FileName = Path.GetFileNameWithoutExtension(FileName) + "_packed";
            ////openFileDialog1.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.InitialDirectory = FullFileDir;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_Unpack;
            string respackerPath = Application.StartupPath + @"\Res_PackerUnpacker\";
            if (Is64Bit()) respackerPath = respackerPath + @"x64\respacker.exe";
            else respackerPath = respackerPath + @"x86\respacker.exe";

#if !DEBUG
            if (!File.Exists(respackerPath))
            {
                MessageBox.Show(Properties.FormStrings.Message_error_respackerPath_Text1 + Environment.NewLine +
                    Properties.FormStrings.Message_error_respackerPath_Text2 + respackerPath + "].\r\n\r\n" +
                    Properties.FormStrings.Message_error_respackerPath_Text3,
                    Properties.FormStrings.Message_error_pack_unpack_dir_Caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
#endif
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullfilename = openFileDialog.FileName;
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = respackerPath;
                    startInfo.Arguments = fullfilename;
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();//ждем 
                    };
                    // этот блок закончится только после окончания работы программы 
                    //сюда писать команды после успешного завершения программы
                    string fileNameOnly = Path.GetFileNameWithoutExtension(fullfilename);
                    //string extension = Path.GetExtension(fullPath);
                    string path = Path.GetDirectoryName(fullfilename);
                    //path = Path.Combine(path, fileNameOnly);
                    string newFullName_cmp = Path.Combine(path, fileNameOnly + ".bin.cmp");
                    string newFullName_bin = Path.Combine(path, fileNameOnly + "_zip.bin");
                    File.Copy(newFullName_cmp, newFullName_bin, true);
                    if (File.Exists(newFullName_bin))
                    {
                        File.Delete(newFullName_cmp);
                        this.BringToFront();
                        //if (radioButton_Settings_Pack_Dialog.Checked)
                        //MessageBox.Show(GetFileSize(new FileInfo(newFullName_bin)));
                        if (Program_Settings.Settings_Pack_Dialog)
                        {
                            if (MessageBox.Show(Properties.FormStrings.Message_GoToFile_Text,
                                Properties.FormStrings.Message_GoToFile_Caption,
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName_bin));
                            }
                        }
                        else if (Program_Settings.Settings_Pack_Copy)
                        {
                            StringCollection paths = new StringCollection();
                            paths.Add(newFullName_bin);
                            Clipboard.SetFileDropList(paths);
                        }
                        else if (Program_Settings.Settings_Pack_GoToFile)
                        {
                            Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName_bin));
                        }
                    }
                }
                catch
                {
                }
            }

        }

        private void button_pack_zip_Click(object sender, EventArgs e)
        {
            string subPath = Application.StartupPath + @"\Watch_face\";
            if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = FullFileDir;
            openFileDialog.FileName = FileName;
            openFileDialog.Filter = "Json files (*.json) | *.json";
            //openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            ////openFileDialog1.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_Pack;

#if !DEBUG
            if (!File.Exists(textBox_pack_unpack_dir.Text))
            {
                MessageBox.Show(Properties.FormStrings.Message_error_pack_unpack_dir_Text1 +
                    textBox_pack_unpack_dir.Text + Properties.FormStrings.Message_error_pack_unpack_dir_Text2 +
                    Environment.NewLine + Environment.NewLine + Properties.FormStrings.Message_error_pack_unpack_dir_Text3,
                    Properties.FormStrings.Message_error_pack_unpack_dir_Caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fullfilename = openFileDialog.FileName;
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = textBox_pack_unpack_dir.Text;
                    startInfo.Arguments = textBox_pack_command.Text + "   " + fullfilename;
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();//ждем 
                    };
                    // этот блок закончится только после окончания работы программы 
                    //сюда писать команды после успешного завершения программы
                    string fileNameOnly = Path.GetFileNameWithoutExtension(fullfilename);
                    //string extension = Path.GetExtension(fullPath);
                    string path = Path.GetDirectoryName(fullfilename);
                    string newFullName = Path.Combine(path, fileNameOnly + "_packed.bin");

                    //MessageBox.Show(newFullName);
                    if (File.Exists(newFullName))
                    {
                        double fileSize = (GetFileSizeMB(new FileInfo(newFullName)));
                        if ((fileSize > 1.5) && (!radioButton_47.Checked))
                        {
                            MessageBox.Show(Properties.FormStrings.Message_bigFile_Text1_gts + Environment.NewLine + Environment.NewLine +
                            Properties.FormStrings.Message_bigFile_Text2, Properties.FormStrings.Message_bigFile_Caption,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (fileSize > 1.95) MessageBox.Show(Properties.FormStrings.Message_bigFile_Text1 + Environment.NewLine + Environment.NewLine +
                        Properties.FormStrings.Message_bigFile_Text2, Properties.FormStrings.Message_bigFile_Caption,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //MessageBox.Show(fileSize.ToString());
                        //MessageBox.Show(GetFileSize(new FileInfo(newFullName)));
                        string respackerPath = Application.StartupPath + @"\Res_PackerUnpacker\";
                        if (Is64Bit()) respackerPath = respackerPath + @"x64\respacker.exe";
                        else respackerPath = respackerPath + @"x86\respacker.exe";

                        if (!File.Exists(respackerPath))
                        {
                            MessageBox.Show(Properties.FormStrings.Message_error_respackerPath_Text1 + Environment.NewLine +
                                Properties.FormStrings.Message_error_respackerPath_Text2 + respackerPath + "].\r\n\r\n" +
                                Properties.FormStrings.Message_error_respackerPath_Text3,
                                Properties.FormStrings.Message_error_pack_unpack_dir_Caption,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        startInfo = new ProcessStartInfo();
                        startInfo.FileName = respackerPath;
                        startInfo.Arguments = newFullName;
                        using (Process exeProcess = Process.Start(startInfo))
                        {
                            exeProcess.WaitForExit();//ждем 
                        };
                        // этот блок закончится только после окончания работы программы 
                        //сюда писать команды после успешного завершения программы
                        fileNameOnly = Path.GetFileNameWithoutExtension(newFullName);
                        //string extension = Path.GetExtension(fullPath);
                        path = Path.GetDirectoryName(newFullName);
                        //path = Path.Combine(path, fileNameOnly);
                        string newFullName_cmp = Path.Combine(path, fileNameOnly + ".bin.cmp");
                        string newFullName_bin = Path.Combine(path, fileNameOnly + "_zip.bin");
                        File.Copy(newFullName_cmp, newFullName_bin, true);
                        if (File.Exists(newFullName_bin))
                        {
                            File.Delete(newFullName_cmp);
                            this.BringToFront();
                            //if (radioButton_Settings_Pack_Dialog.Checked)
                            if (Program_Settings.Settings_Pack_Dialog)
                            {
                                if (MessageBox.Show(Properties.FormStrings.Message_GoToFile_Text,
                                Properties.FormStrings.Message_GoToFile_Caption,
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName_bin));
                                }
                            }
                            else if (Program_Settings.Settings_Pack_Copy)
                            {
                                StringCollection paths = new StringCollection();
                                paths.Add(newFullName_bin);
                                Clipboard.SetFileDropList(paths);
                            }
                            else if (Program_Settings.Settings_Pack_GoToFile)
                            {
                                Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName_bin));
                            }
                        }
                    }
                }
                catch
                {
                    // сюда писать команды при ошибке вызова 
                }
            }
#endif
        }

        // загружаем перечень картинок
        private void button_images_Click(object sender, EventArgs e)
        {
            string subPath = Application.StartupPath + @"\Watch_face\";
            if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = subPath;
            openFileDialog.Filter = "PNG Files: (*.png)|*.png";
            //openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            ////openFileDialog1.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_Image;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                dataGridView_ImagesList.Rows.Clear();
                ListImages.Clear();
                ListImagesFullName.Clear();
                int i;
                Image loadedImage = null;
                foreach (String file in openFileDialog.FileNames)
                {
                    try
                    {
                        string fileNameOnly = Path.GetFileNameWithoutExtension(file);
                        //string fileNameOnly = Path.GetFileName(file);
                        if (int.TryParse(fileNameOnly, out i))
                        {
                            //Image loadedImage = Image.FromFile(file);
                            using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                            {
                                loadedImage = Image.FromStream(stream);
                            }
                            var RowNew = new DataGridViewRow();
                            DataGridViewImageCellLayout ZoomType = DataGridViewImageCellLayout.Zoom;
                            if ((loadedImage.Height < 45) && (loadedImage.Width < 110))
                                ZoomType = DataGridViewImageCellLayout.Normal;
                            RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = i.ToString() });
                            RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = fileNameOnly });
                            //RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = file });
                            RowNew.Cells.Add(new DataGridViewImageCell()
                            {
                                Value = loadedImage,
                                ImageLayout = ZoomType
                            });
                            RowNew.Height = 45;
                            dataGridView_ImagesList.Rows.Add(RowNew);
                            ListImages.Add(i.ToString());
                            ListImagesFullName.Add(file);
                        }

                    }
                    catch
                    {
                        // Could not load the image - probably related to Windows file system permissions.
                        MessageBox.Show(Properties.FormStrings.Message_error_Image_Text1 + file.Substring(file.LastIndexOf('\\')+1)
                            + Properties.FormStrings.Message_error_Image_Text2);
                    }
                }
                //loadedImage.Dispose();
                PreviewView = false;
                JSON_read();
                PreviewView = true;
                PreviewImage();
            }
        }

        // загружаем JSON файл с настройками
        private void button_JSON_Click(object sender, EventArgs e)
        {
            string subPath = Application.StartupPath + @"\Watch_face\";
            if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = FullFileDir;
            openFileDialog.FileName = FileName;
            openFileDialog.Filter = "Json files (*.json) | *.json";
            //openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            ////openFileDialog1.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_Pack;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //string fullfilename = openFileDialog.FileName;
                LoadJsonAndImage(openFileDialog.FileName);
            }
        }

        private void LoadJsonAndImage(string fullfilename)
        {
            //Logger.WriteLine("LoadJsonAndImage");
            FileName = Path.GetFileName(fullfilename);
            FullFileDir = Path.GetDirectoryName(fullfilename);
            string text = File.ReadAllText(fullfilename);
            //richTextBox_JSON.Text = text;
            PreviewView = false;
            ListImages.Clear();
            ListImagesFullName.Clear();
            dataGridView_ImagesList.Rows.Clear();
            dataGridView_Battery_IconSet.Rows.Clear();
            dataGridView_SPSliced.Rows.Clear();
            //Logger.WriteLine("Прочитали текст из json файла " + fullfilename);

            DirectoryInfo Folder;
            FileInfo[] Images;
            Folder = new DirectoryInfo(FullFileDir);
            Images = Folder.GetFiles("*.png");
            int count = 0;
            Image loadedImage = null;
            foreach (FileInfo file in Images)
            {
                try
                {
                    string fileNameOnly = Path.GetFileNameWithoutExtension(file.Name);
                    //string fileNameOnly = Path.GetFileName(file);
                    int i;
                    if (int.TryParse(fileNameOnly, out i))
                    {
                        //loadedImage = Image.FromFile(file.FullName);
                        using (FileStream stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                        {
                            loadedImage = Image.FromStream(stream);
                        }
                        var RowNew = new DataGridViewRow();
                        DataGridViewImageCellLayout ZoomType = DataGridViewImageCellLayout.Zoom;
                        if ((loadedImage.Height < 45) && (loadedImage.Width < 110))
                            ZoomType = DataGridViewImageCellLayout.Normal;
                        RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = i.ToString() });
                        RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = fileNameOnly });
                        //RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = file });
                        RowNew.Cells.Add(new DataGridViewImageCell()
                        {
                            Value = loadedImage,
                            ImageLayout = ZoomType
                        });
                        //loadedImage.Dispose();
                        RowNew.Height = 45;
                        dataGridView_ImagesList.Rows.Add(RowNew);
                        count++;
                        ListImages.Add(i.ToString());
                        ListImagesFullName.Add(file.FullName);
                    }
                }
                catch
                {
                    // Could not load the image - probably related to Windows file system permissions.
                    MessageBox.Show(Properties.FormStrings.Message_error_Image_Text1 + 
                        file.FullName.Substring(file.FullName.LastIndexOf('\\')+1) + Properties.FormStrings.Message_error_Image_Text2);
                }
            }
            //Logger.WriteLine("Загрузили все файлы изображений");

            //loadedImage.Dispose();
            int LastImage = 0;
            Int32.TryParse(ListImages.Last(), out LastImage);
            LastImage++;
#if !DEBUG
            if (count != LastImage) MessageBox.Show(Properties.FormStrings.Message_PNGmissing_Text1 + Environment.NewLine +
                 Properties.FormStrings.Message_PNGmissing_Text2, Properties.FormStrings.Message_Error_Caption, 
                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif

            //Logger.WriteLine("Не верный json формат");
            try
            {
                Watch_Face = JsonConvert.DeserializeObject<WATCH_FACE_JSON>(text, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex, 
                    Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //Logger.WriteLine("Распознали json формат");

            richTextBox_JSON.Text = JsonConvert.SerializeObject(Watch_Face, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });

            
            
            JSON_read();
            //Logger.WriteLine("Установили значения в соответствии с json файлом");

            string path = Path.GetDirectoryName(fullfilename);
            string newFullName = Path.Combine(path, "PreviewStates.json");
            if (File.Exists(newFullName))
            {
                if (Program_Settings.Settings_Open_Download)
                {
                    JsonPreview_Read(newFullName); 
                }
                else if (Program_Settings.Settings_Open_Dialog)
                {
                    if (MessageBox.Show(Properties.FormStrings.Message_LoadPreviewStates_Text,
                        Properties.FormStrings.Message_openProject_Caption, 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        JsonPreview_Read(newFullName);
                    }
                }
            }
            PreviewView = true;
            PreviewImage();
        }
      
        // формируем изображение для предпросмотра
        private void PreviewImage()
        {
            if (!PreviewView) return;
            Graphics gPanel = panel_Preview.CreateGraphics();
            gPanel.Clear(panel_Preview.BackColor);
            float scale = 1.0f;
            if (panel_Preview.Height < 300) scale = 0.5f;
            PreviewToBitmap(gPanel, scale, checkBox_crop.Checked, checkBox_WebW.Checked, checkBox_WebB.Checked, 
                checkBox_border.Checked, checkBox_Show_Shortcuts.Checked, checkBox_Shortcuts_Area.Checked, checkBox_Shortcuts_Border.Checked);
            gPanel.Dispose();

            if ((formPreview != null) && (formPreview.Visible))
            {
                Graphics gPanelPreview = formPreview.panel_Preview.CreateGraphics();
                gPanelPreview.Clear(panel_Preview.BackColor);
                float scalePreview = 1.0f;
                if (formPreview.radioButton_small.Checked) scalePreview = 0.5f;
                if (formPreview.radioButton_large.Checked) scalePreview = 1.5f;
                if (formPreview.radioButton_xlarge.Checked) scalePreview = 2.0f;
                if (formPreview.radioButton_xxlarge.Checked) scalePreview = 2.5f;
                PreviewToBitmap(gPanelPreview, scalePreview, checkBox_crop.Checked, checkBox_WebW.Checked, 
                    checkBox_WebB.Checked, checkBox_border.Checked, checkBox_Show_Shortcuts.Checked, 
                    checkBox_Shortcuts_Area.Checked, checkBox_Shortcuts_Border.Checked);
                gPanelPreview.Dispose();
                
            }
        }
        

#region выбираем данные для предпросмотра
        private void SetPreferences1()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set1.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set1.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set1.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set1.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set1.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set1.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set1.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set1.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set1.Value;
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set1.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set1.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set1.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set1.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set1.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set1.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set1.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set1.Checked;

            SetDigitForPrewiev();
        }

        private void SetPreferences2()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set2.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set2.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set2.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set2.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set2.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set2.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set2.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set2.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set2.Value;
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set2.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set2.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set2.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set2.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set2.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set2.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set2.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set2.Checked;

            SetDigitForPrewiev();
        }

        private void SetPreferences3()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set3.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set3.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set3.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set3.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set3.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set3.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set3.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set3.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set3.Value;
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set3.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set3.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set3.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set3.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set3.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set3.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set3.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set3.Checked;

            SetDigitForPrewiev();
        }

        private void SetPreferences4()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set4.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set4.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set4.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set4.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set4.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set4.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set4.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set4.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set4.Value;
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set4.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set4.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set4.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set4.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set4.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set4.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set4.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set4.Checked;

            SetDigitForPrewiev();
        }

        private void SetPreferences5()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set5.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set5.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set5.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set5.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set5.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set5.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set5.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set5.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set5.Value;
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set5.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set5.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set5.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set5.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set5.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set5.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set5.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set5.Checked;

            SetDigitForPrewiev();
        }

        private void SetPreferences6()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set6.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set6.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set6.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set6.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set6.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set6.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set6.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set6.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set6.Value;
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set6.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set6.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set6.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set6.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set6.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set6.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set6.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set6.Checked;

            SetDigitForPrewiev();
        }

        private void SetPreferences7()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set7.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set7.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set7.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set7.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set7.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set7.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set7.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set7.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set7.Value;
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set7.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set7.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set7.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set7.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set7.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set7.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set7.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set7.Checked;

            SetDigitForPrewiev();
        }

        private void SetPreferences8()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set8.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set8.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set8.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set8.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set8.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set8.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set8.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set8.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set8.Value;
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set8.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set8.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set8.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set8.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set8.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set8.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set8.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set8.Checked;

            SetDigitForPrewiev();
        }

        private void SetPreferences9()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set9.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set9.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set9.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set9.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set9.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set9.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set9.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set9.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set9.Value;
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set9.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set9.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set9.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set9.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set9.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set9.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set9.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set9.Checked;

            SetDigitForPrewiev();
        }

        private void SetPreferences10()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set10.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set10.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set10.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set10.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set10.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set10.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set10.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set10.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set10.Value;
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set10.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set10.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set10.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set10.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set10.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set10.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set10.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set10.Checked;

            SetDigitForPrewiev();
        }

        private void SetPreferences11()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set11.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set11.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set11.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set11.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set11.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set11.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set11.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set11.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set11.Value;
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set11.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set11.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set11.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set11.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set11.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set11.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set11.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set11.Checked;

            SetDigitForPrewiev();
        }

        private void SetPreferences12()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set12.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set12.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set12.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set12.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set12.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set12.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set12.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set12.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set12.Value;
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set12.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set12.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set12.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set12.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set12.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set12.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set12.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set12.Checked;

            SetDigitForPrewiev();
        }

        private void SetPreferences13()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set13.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set13.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set13.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set13.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set13.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set13.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set13.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set13.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set13.Value;
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set13.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set13.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set13.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set13.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set13.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set13.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set13.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set13.Checked;

            SetDigitForPrewiev();
        }
#endregion

        // определяем отдельные цифры для даты и времени
        private void SetDigitForPrewiev()
        {
            Watch_Face_Preview_TwoDigits.Date.Month.Tens = (int)Watch_Face_Preview_Set.Date.Month / 10;
            Watch_Face_Preview_TwoDigits.Date.Month.Ones = Watch_Face_Preview_Set.Date.Month -
                Watch_Face_Preview_TwoDigits.Date.Month.Tens * 10;
            Watch_Face_Preview_TwoDigits.Date.Day.Tens = (int)Watch_Face_Preview_Set.Date.Day / 10;
            Watch_Face_Preview_TwoDigits.Date.Day.Ones = Watch_Face_Preview_Set.Date.Day -
                Watch_Face_Preview_TwoDigits.Date.Day.Tens * 10;

            Watch_Face_Preview_TwoDigits.Year.Thousands = (int)Watch_Face_Preview_Set.Date.Year / 1000;
            Watch_Face_Preview_TwoDigits.Year.Hundreds = (int)(Watch_Face_Preview_Set.Date.Year -
                Watch_Face_Preview_TwoDigits.Year.Thousands * 1000)/100;
            Watch_Face_Preview_TwoDigits.Year.Tens = (int)(Watch_Face_Preview_Set.Date.Year -
                Watch_Face_Preview_TwoDigits.Year.Thousands * 1000 - Watch_Face_Preview_TwoDigits.Year.Hundreds * 100)/10;
            Watch_Face_Preview_TwoDigits.Year.Ones = Watch_Face_Preview_Set.Date.Year -
                Watch_Face_Preview_TwoDigits.Year.Thousands * 1000 - Watch_Face_Preview_TwoDigits.Year.Hundreds * 100 - 
                Watch_Face_Preview_TwoDigits.Year.Tens * 10;

            Watch_Face_Preview_TwoDigits.Time.Hours.Tens = (int)Watch_Face_Preview_Set.Time.Hours / 10;
            Watch_Face_Preview_TwoDigits.Time.Hours.Ones = Watch_Face_Preview_Set.Time.Hours -
                Watch_Face_Preview_TwoDigits.Time.Hours.Tens * 10;
            Watch_Face_Preview_TwoDigits.Time.Minutes.Tens = (int)Watch_Face_Preview_Set.Time.Minutes / 10;
            Watch_Face_Preview_TwoDigits.Time.Minutes.Ones = Watch_Face_Preview_Set.Time.Minutes -
                Watch_Face_Preview_TwoDigits.Time.Minutes.Tens * 10;
            Watch_Face_Preview_TwoDigits.Time.Seconds.Tens = (int)Watch_Face_Preview_Set.Time.Seconds / 10;
            Watch_Face_Preview_TwoDigits.Time.Seconds.Ones = Watch_Face_Preview_Set.Time.Seconds -
                Watch_Face_Preview_TwoDigits.Time.Seconds.Tens * 10;

            int hour = Watch_Face_Preview_Set.Time.Hours;
            if (Watch_Face_Preview_Set.Time.Hours > 12)
            {
                hour = hour - 12;
                Watch_Face_Preview_TwoDigits.TimePm.Pm = true;
            }
            else
            {
                Watch_Face_Preview_TwoDigits.TimePm.Pm = false;
            }
            Watch_Face_Preview_TwoDigits.TimePm.Hours.Tens = hour / 10;
            Watch_Face_Preview_TwoDigits.TimePm.Hours.Ones = hour - (int)Watch_Face_Preview_TwoDigits.TimePm.Hours.Tens * 10;
            Watch_Face_Preview_TwoDigits.TimePm.Minutes.Tens = (int)Watch_Face_Preview_Set.Time.Minutes / 10;
            Watch_Face_Preview_TwoDigits.TimePm.Minutes.Ones = (int)Watch_Face_Preview_Set.Time.Minutes -
                (int)Watch_Face_Preview_TwoDigits.TimePm.Minutes.Tens * 10;
            Watch_Face_Preview_TwoDigits.TimePm.Seconds.Tens = (int)Watch_Face_Preview_Set.Time.Seconds / 10;
            Watch_Face_Preview_TwoDigits.TimePm.Seconds.Ones = (int)Watch_Face_Preview_Set.Time.Seconds -
                Watch_Face_Preview_TwoDigits.TimePm.Seconds.Tens * 10;
        }

        // меняем цвет текста и рамки для groupBox
        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.Black, Color.DarkGray);
        }
        private void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                               box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                               box.ClientRectangle.Width - 1,
                                               box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

                // Clear text and border
                g.Clear(this.BackColor);

                // Draw text
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                // Drawing Border
                //Left
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                //Right
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Bottom
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Top1
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                //Top2
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }
        

        private void checkBox_Time_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Time.Checked)
            {
                tabControl_Time.Enabled = true;
                groupBox_AmPm.Enabled = true;
                groupBox_Delimiter.Enabled = true;
            }
            else
            {
                tabControl_Time.Enabled = false;
                groupBox_AmPm.Enabled = false;
                groupBox_Delimiter.Enabled = false;
            }

        }

        private void checkBox_AmPm_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AmPm.Checked)
            {
                numericUpDown_AmPm_X.Enabled = true;
                numericUpDown_AmPm_Y.Enabled = true;
                comboBox_Image_Am.Enabled = true;
                comboBox_Image_Pm.Enabled = true;

                label32.Enabled = true;
                label33.Enabled = true;
                label34.Enabled = true;
                label35.Enabled = true;
            }
            else
            {
                numericUpDown_AmPm_X.Enabled = false;
                numericUpDown_AmPm_Y.Enabled = false;
                comboBox_Image_Am.Enabled = false;
                comboBox_Image_Pm.Enabled = false;

                label32.Enabled = false;
                label33.Enabled = false;
                label34.Enabled = false;
                label35.Enabled = false;
            }
        }

        private void checkBox_Hours_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Hours.Checked)
            {
                groupBox_Hours_Tens.Enabled = true;
                groupBox_Hours_Ones.Enabled = true;
            }
            else
            {
                groupBox_Hours_Tens.Enabled = false;
                groupBox_Hours_Ones.Enabled = false;
            }
        }

        #region сворачиваем и разварачиваем панели с настройками

        private void button_Background_Click(object sender, EventArgs e)
        {
            if (panel_Background.Height == 1)
            {
                panel_Background.Height = (int)(30 * currentDPI);
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
            }
            else panel_Background.Height = 1;
        }

        private void button_Time_Click(object sender, EventArgs e)
        {
            if (panel_Time.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = (int)(330 * currentDPI);
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
            }
            else panel_Time.Height = 1;
        }

        private void button_Date_Click(object sender, EventArgs e)
        {
            if (panel_Date.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = (int)(240 * currentDPI);
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
            }
            else panel_Date.Height = 1;
        }

        private void button_AnalogDate_Click(object sender, EventArgs e)
        {
            if (panel_AnalogDate.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = (int)(145 * currentDPI);
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
            }
            else panel_AnalogDate.Height = 1;
        }

        private void button_StepsProgress_Click(object sender, EventArgs e)
        {
            if (panel_StepsProgress.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = (int)(165 * currentDPI);
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
            }
            else panel_StepsProgress.Height = 1;
        }

        private void button_Activity_Click(object sender, EventArgs e)
        {
            if (panel_Activity.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = (int)(215 * currentDPI);
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
            }
            else panel_Activity.Height = 1;
        }

        private void button_Status_Click(object sender, EventArgs e)
        {
            if (panel_Status.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = (int)(82 * currentDPI);
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
            }
            else panel_Status.Height = 1;
        }

        private void button_Battery_Click(object sender, EventArgs e)
        {
            if (panel_Battery.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = (int)(185 * currentDPI);
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
            }
            else panel_Battery.Height = 1;
        }

        private void button_AnalogClock_Click(object sender, EventArgs e)
        {
            if (panel_AnalogClock.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = (int)(193 * currentDPI);
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
            }
            else panel_AnalogClock.Height = 1;
        }

        private void button_Weather_Click(object sender, EventArgs e)
        {
            if (panel_Weather.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = (int)(230 * currentDPI);
                panel_Shortcuts.Height = 1;
            }
            else panel_Weather.Height = 1;
        }

        private void button_Shortcuts_Click(object sender, EventArgs e)
        {
            if (panel_Shortcuts.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = (int)(145 * currentDPI);
            }
            else panel_Shortcuts.Height = 1;
        }
        #endregion

        #region активируем и деактивируем настройки
        private void checkBox_Minutes_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Minutes.Checked;
            groupBox_Minutes_Tens.Enabled = b;
            groupBox_Minutes_Ones.Enabled = b;
        }

        private void checkBox_Seconds_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Seconds.Checked;
            groupBox_Seconds_Tens.Enabled = b;
            groupBox_Seconds_Ones.Enabled = b;
        }

        private void checkBox_WeekDay_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_WeekDay.Checked;
            numericUpDown_WeekDay_X.Enabled = b;
            numericUpDown_WeekDay_Y.Enabled = b;
            comboBox_WeekDay_Image.Enabled = b;
            numericUpDown_WeekDay_Count.Enabled = b;

            label55.Enabled = b;
            label56.Enabled = b;
            label57.Enabled = b;
            label58.Enabled = b;
        }

        private void checkBox_DOW_IconSet_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_DOW_IconSet.Checked;
            comboBox_DOW_IconSet_Image.Enabled = b;
            dataGridView_DOW_IconSet.Enabled = b;
            label460.Enabled = b;
        }

        private void checkBox_MonthName_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_MonthName.Checked;
            numericUpDown_MonthName_X.Enabled = b;
            numericUpDown_MonthName_Y.Enabled = b;
            comboBox_MonthName_Image.Enabled = b;
            numericUpDown_MonthName_Count.Enabled = b;

            label59.Enabled = b;
            label60.Enabled = b;
            label61.Enabled = b;
            label62.Enabled = b;
        }

        private void checkBox_Date_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Date.Checked;
            tabControl_Date.Enabled = b;
            groupBox__WeekDay.Enabled = b;
            groupBox_WeekDayProgress.Enabled = b;
            checkBox_TwoDigitsMonth.Enabled = b;
            checkBox_TwoDigitsDay.Enabled = b;
        }

        private void checkBox_MonthAndDayD_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_MonthAndDayD.Checked;
            numericUpDown_MonthAndDayD_StartCorner_X.Enabled = b;
            numericUpDown_MonthAndDayD_StartCorner_Y.Enabled = b;
            numericUpDown_MonthAndDayD_EndCorner_X.Enabled = b;
            numericUpDown_MonthAndDayD_EndCorner_Y.Enabled = b;
            comboBox_MonthAndDayD_Alignment.Enabled = b;
            numericUpDown_MonthAndDayD_Spacing.Enabled = b;
            comboBox_MonthAndDayD_Image.Enabled = b;
            numericUpDown_MonthAndDayD_Count.Enabled = b;

            label63.Enabled = b;
            label64.Enabled = b;
            label65.Enabled = b;
            label66.Enabled = b;
            label67.Enabled = b;
            label68.Enabled = b;
            label69.Enabled = b;
            label70.Enabled = b;
            label71.Enabled = b;
            label72.Enabled = b;
        }

        private void checkBox_MonthAndDayM_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_MonthAndDayM.Checked;
            numericUpDown_MonthAndDayM_StartCorner_X.Enabled = b;
            numericUpDown_MonthAndDayM_StartCorner_Y.Enabled = b;
            numericUpDown_MonthAndDayM_EndCorner_X.Enabled = b;
            numericUpDown_MonthAndDayM_EndCorner_Y.Enabled = b;
            comboBox_MonthAndDayM_Alignment.Enabled = b;
            numericUpDown_MonthAndDayM_Spacing.Enabled = b;
            comboBox_MonthAndDayM_Image.Enabled = b;
            numericUpDown_MonthAndDayM_Count.Enabled = b;

            label73.Enabled = b;
            label74.Enabled = b;
            label75.Enabled = b;
            label76.Enabled = b;
            label77.Enabled = b;
            label78.Enabled = b;
            label79.Enabled = b;
            label80.Enabled = b;
            label81.Enabled = b;
            label82.Enabled = b;
        }

        private void checkBox_OneLine_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_OneLine.Checked;
            numericUpDown_OneLine_StartCorner_X.Enabled = b;
            numericUpDown_OneLine_StartCorner_Y.Enabled = b;
            numericUpDown_OneLine_EndCorner_X.Enabled = b;
            numericUpDown_OneLine_EndCorner_Y.Enabled = b;
            comboBox_OneLine_Alignment.Enabled = b;
            numericUpDown_OneLine_Spacing.Enabled = b;
            comboBox_OneLine_Image.Enabled = b;
            numericUpDown_OneLine_Count.Enabled = b;
            comboBox_OneLine_Delimiter.Enabled = b;

            label93.Enabled = b;
            label94.Enabled = b;
            label95.Enabled = b;
            label96.Enabled = b;
            label97.Enabled = b;
            label98.Enabled = b;
            label99.Enabled = b;
            label100.Enabled = b;
            label101.Enabled = b;
            label102.Enabled = b;
            label103.Enabled = b;
        }

        private void checkBox_Year_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Year.Checked;
            numericUpDown_Year_StartCorner_X.Enabled = b;
            numericUpDown_Year_StartCorner_Y.Enabled = b;
            numericUpDown_Year_EndCorner_X.Enabled = b;
            numericUpDown_Year_EndCorner_Y.Enabled = b;
            comboBox_Year_Alignment.Enabled = b;
            numericUpDown_Year_Spacing.Enabled = b;
            comboBox_Year_Image.Enabled = b;
            numericUpDown_Year_Count.Enabled = b;
            comboBox_Year_Delimiter.Enabled = b;

            label357.Enabled = b;
            label358.Enabled = b;
            label359.Enabled = b;
            label360.Enabled = b;
            label361.Enabled = b;
            label362.Enabled = b;
            label363.Enabled = b;
            label364.Enabled = b;
            label365.Enabled = b;
            label366.Enabled = b;
            label367.Enabled = b;
        }

        private void checkBox_ADDay_ClockHand_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ADDay_ClockHand.Checked;
            numericUpDown_ADDay_ClockHand_X.Enabled = b;
            numericUpDown_ADDay_ClockHand_Y.Enabled = b;
            numericUpDown_ADDay_ClockHand_Offset_X.Enabled = b;
            numericUpDown_ADDay_ClockHand_Offset_Y.Enabled = b;
            comboBox_ADDay_ClockHand_Image.Enabled = b;
            numericUpDown_ADDay_ClockHand_StartAngle.Enabled = b;
            numericUpDown_ADDay_ClockHand_EndAngle.Enabled = b;
            label396.Enabled = b;
            label397.Enabled = b;
            label398.Enabled = b;
            label399.Enabled = b;
            label400.Enabled = b;
            label401.Enabled = b;
            label402.Enabled = b;
        }

        private void checkBox_ADWeekDay_ClockHand_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ADWeekDay_ClockHand.Checked;
            numericUpDown_ADWeekDay_ClockHand_X.Enabled = b;
            numericUpDown_ADWeekDay_ClockHand_Y.Enabled = b;
            numericUpDown_ADWeekDay_ClockHand_Offset_X.Enabled = b;
            numericUpDown_ADWeekDay_ClockHand_Offset_Y.Enabled = b;
            comboBox_ADWeekDay_ClockHand_Image.Enabled = b;
            numericUpDown_ADWeekDay_ClockHand_StartAngle.Enabled = b;
            numericUpDown_ADWeekDay_ClockHand_EndAngle.Enabled = b;
            label389.Enabled = b;
            label390.Enabled = b;
            label391.Enabled = b;
            label392.Enabled = b;
            label393.Enabled = b;
            label394.Enabled = b;
            label395.Enabled = b;
        }

        private void checkBox_ADMonth_ClockHand_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ADMonth_ClockHand.Checked;
            numericUpDown_ADMonth_ClockHand_X.Enabled = b;
            numericUpDown_ADMonth_ClockHand_Y.Enabled = b;
            numericUpDown_ADMonth_ClockHand_Offset_X.Enabled = b;
            numericUpDown_ADMonth_ClockHand_Offset_Y.Enabled = b;
            comboBox_ADMonth_ClockHand_Image.Enabled = b;
            numericUpDown_ADMonth_ClockHand_StartAngle.Enabled = b;
            numericUpDown_ADMonth_ClockHand_EndAngle.Enabled = b;
            label382.Enabled = b;
            label383.Enabled = b;
            label384.Enabled = b;
            label385.Enabled = b;
            label386.Enabled = b;
            label387.Enabled = b;
            label388.Enabled = b;
        }

        private void checkBox_Activity_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Activity.Checked;
            tabControl_Activity.Enabled = b;
            comboBox_Activity_NDImage.Enabled = b;
            label403.Enabled = b;
        }

        private void checkBox_CircleScale_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_StepsProgress.Checked;
            numericUpDown_StepsProgress_Center_X.Enabled = b;
            numericUpDown_StepsProgress_Center_Y.Enabled = b;
            numericUpDown_StepsProgress_Radius_X.Enabled = b;
            checkBox_StepsProgress_Image.Enabled = b;
            comboBox_StepsProgress_Image.Enabled = b;
            numericUpDown_StepsProgress_ImageX.Enabled = b;
            numericUpDown_StepsProgress_ImageY.Enabled = b;

            numericUpDown_StepsProgress_Width.Enabled = b;
            comboBox_StepsProgress_Color.Enabled = b;
            numericUpDown_StepsProgress_StartAngle.Enabled = b;
            numericUpDown_StepsProgress_EndAngle.Enabled = b;
            comboBox_StepsProgress_Flatness.Enabled = b;

            label104.Enabled = b;
            label105.Enabled = b;
            label106.Enabled = b;
            label107.Enabled = b;
            label108.Enabled = b;
            label109.Enabled = b;
            label110.Enabled = b;
            label111.Enabled = b;
            label348.Enabled = b;
            label432.Enabled = b;
            label433.Enabled = b;

            if (b)
            {
                if (checkBox_StepsProgress_Image.Checked)
                {
                    comboBox_StepsProgress_Image.Enabled = true;
                    numericUpDown_StepsProgress_ImageX.Enabled = true;
                    numericUpDown_StepsProgress_ImageY.Enabled = true;
                    label432.Enabled = true;
                    label433.Enabled = true;
                    comboBox_StepsProgress_Color.Enabled = false;
                    numericUpDown_StepsProgress_Center_X.Enabled = false;
                    numericUpDown_StepsProgress_Center_Y.Enabled = false;
                    label104.Enabled = false;
                    label105.Enabled = false;
                    label109.Enabled = false;
                }
                else
                {
                    comboBox_StepsProgress_Image.Enabled = false;
                    numericUpDown_StepsProgress_ImageX.Enabled = false;
                    numericUpDown_StepsProgress_ImageY.Enabled = false;
                    label432.Enabled = false;
                    label433.Enabled = false;
                    comboBox_StepsProgress_Color.Enabled = true;
                    numericUpDown_StepsProgress_Center_X.Enabled = true;
                    numericUpDown_StepsProgress_Center_Y.Enabled = true;
                    label104.Enabled = true;
                    label105.Enabled = true;
                    label109.Enabled = true;
                }
            }
        }

        private void checkBox_StepsProgress_Image_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_StepsProgress_Image.Checked)
            {
                comboBox_StepsProgress_Image.Enabled = true;
                numericUpDown_StepsProgress_ImageX.Enabled = true;
                numericUpDown_StepsProgress_ImageY.Enabled = true;
                label432.Enabled = true;
                label433.Enabled = true;
                comboBox_StepsProgress_Color.Enabled = false;
                numericUpDown_StepsProgress_Center_X.Enabled = false;
                numericUpDown_StepsProgress_Center_Y.Enabled = false;
                label104.Enabled = false;
                label105.Enabled = false;
                label109.Enabled = false;
            }
            else
            {
                comboBox_StepsProgress_Image.Enabled = false;
                numericUpDown_StepsProgress_ImageX.Enabled = false;
                numericUpDown_StepsProgress_ImageY.Enabled = false;
                label432.Enabled = false;
                label433.Enabled = false;
                comboBox_StepsProgress_Color.Enabled = true;
                numericUpDown_StepsProgress_Center_X.Enabled = true;
                numericUpDown_StepsProgress_Center_Y.Enabled = true;
                label104.Enabled = true;
                label105.Enabled = true;
                label109.Enabled = true;
            }
        }

        private void checkBox_StProg_ClockHand_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_StProg_ClockHand.Checked;
            numericUpDown_StProg_ClockHand_X.Enabled = b;
            numericUpDown_StProg_ClockHand_Y.Enabled = b;
            numericUpDown_StProg_ClockHand_Offset_X.Enabled = b;
            numericUpDown_StProg_ClockHand_Offset_Y.Enabled = b;
            comboBox_StProg_ClockHand_Image.Enabled = b;
            numericUpDown_StProg_ClockHand_StartAngle.Enabled = b;
            numericUpDown_StProg_ClockHand_EndAngle.Enabled = b;

            label375.Enabled = b;
            label376.Enabled = b;
            label377.Enabled = b;
            label378.Enabled = b;
            label379.Enabled = b;
            label380.Enabled = b;
            label381.Enabled = b;
        }

        private void checkBox_SPSliced_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_SPSliced.Checked;
            comboBox_SPSliced_Image.Enabled = b;
            dataGridView_SPSliced.Enabled = b;
            label404.Enabled = b;
        }

        private void checkBox_ActivityStar_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivityStar.Checked;
            numericUpDown_ActivityStar_X.Enabled = b;
            numericUpDown_ActivityStar_Y.Enabled = b;
            comboBox_ActivityStar_Image.Enabled = b;

            label163.Enabled = b;
            label164.Enabled = b;
            label166.Enabled = b;
            label171.Enabled = b;
        }

        private void checkBox_ActivitySteps_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivitySteps.Checked;
            numericUpDown_ActivitySteps_StartCorner_X.Enabled = b;
            numericUpDown_ActivitySteps_StartCorner_Y.Enabled = b;
            numericUpDown_ActivitySteps_EndCorner_X.Enabled = b;
            numericUpDown_ActivitySteps_EndCorner_Y.Enabled = b;

            comboBox_ActivitySteps_Image.Enabled = b;
            numericUpDown_ActivitySteps_Count.Enabled = b;
            numericUpDown_ActivitySteps_Spacing.Enabled = b;
            comboBox_ActivitySteps_Alignment.Enabled = b;

            label122.Enabled = b;
            label123.Enabled = b;
            label124.Enabled = b;
            label125.Enabled = b;
            label126.Enabled = b;
            label127.Enabled = b;
            label128.Enabled = b;
            label129.Enabled = b;
            label130.Enabled = b;
            label131.Enabled = b;
        }

        private void checkBox_ActivityDistance_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivityDistance.Checked;
            numericUpDown_ActivityDistance_StartCorner_X.Enabled = b;
            numericUpDown_ActivityDistance_StartCorner_Y.Enabled = b;
            numericUpDown_ActivityDistance_EndCorner_X.Enabled = b;
            numericUpDown_ActivityDistance_EndCorner_Y.Enabled = b;

            comboBox_ActivityDistance_Image.Enabled = b;
            numericUpDown_ActivityDistance_Count.Enabled = b;
            numericUpDown_ActivityDistance_Spacing.Enabled = b;
            comboBox_ActivityDistance_Alignment.Enabled = b;
            comboBox_ActivityDistance_Suffix.Enabled = b;
            comboBox_ActivityDistance_Decimal.Enabled = b;

            label132.Enabled = b;
            label133.Enabled = b;
            label134.Enabled = b;
            label135.Enabled = b;
            label136.Enabled = b;
            label137.Enabled = b;
            label138.Enabled = b;
            label139.Enabled = b;
            label140.Enabled = b;
            label141.Enabled = b;
            label172.Enabled = b;
            label173.Enabled = b;
        }

        private void checkBox_ActivityPuls_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivityPuls.Checked;
            numericUpDown_ActivityPuls_StartCorner_X.Enabled = b;
            numericUpDown_ActivityPuls_StartCorner_Y.Enabled = b;
            numericUpDown_ActivityPuls_EndCorner_X.Enabled = b;
            numericUpDown_ActivityPuls_EndCorner_Y.Enabled = b;

            comboBox_ActivityPuls_Image.Enabled = b;
            numericUpDown_ActivityPuls_Count.Enabled = b;
            numericUpDown_ActivityPuls_Spacing.Enabled = b;
            comboBox_ActivityPuls_Alignment.Enabled = b;

            label142.Enabled = b;
            label143.Enabled = b;
            label144.Enabled = b;
            label145.Enabled = b;
            label146.Enabled = b;
            label147.Enabled = b;
            label148.Enabled = b;
            label149.Enabled = b;
            label150.Enabled = b;
            label151.Enabled = b;
        }

        private void checkBox_ActivityPulse_IconSet_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivityPuls_IconSet.Checked;
            comboBox_ActivityPuls_IconSet_Image.Enabled = b;
            dataGridView_ActivityPuls_IconSet.Enabled = b;
            label121.Enabled = b;
        }

        private void checkBox_ActivityPulsScale_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivityPulsScale.Checked;
            numericUpDown_ActivityPulsScale_Center_X.Enabled = b;
            numericUpDown_ActivityPulsScale_Center_Y.Enabled = b;
            numericUpDown_ActivityPulsScale_Radius_X.Enabled = b;
            checkBox_ActivityPulsScale_Image.Enabled = b;
            comboBox_ActivityPulsScale_Image.Enabled = b;
            numericUpDown_ActivityPulsScale_ImageX.Enabled = b;
            numericUpDown_ActivityPulsScale_ImageY.Enabled = b;

            numericUpDown_ActivityPulsScale_Width.Enabled = b;
            comboBox_ActivityPulsScale_Color.Enabled = b;
            numericUpDown_ActivityPulsScale_EndAngle.Enabled = b;
            numericUpDown_ActivityPulsScale_StartAngle.Enabled = b;
            comboBox_ActivityPulsScale_Flatness.Enabled = b;

            label417.Enabled = b;
            label418.Enabled = b;
            label419.Enabled = b;
            label420.Enabled = b;
            label421.Enabled = b;
            label422.Enabled = b;
            label423.Enabled = b;
            label424.Enabled = b;
            label425.Enabled = b;
            label430.Enabled = b;
            label431.Enabled = b;

            if (b)
            {
                if (checkBox_ActivityPulsScale_Image.Checked)
                {
                    comboBox_ActivityPulsScale_Image.Enabled = true;
                    numericUpDown_ActivityPulsScale_ImageX.Enabled = true;
                    numericUpDown_ActivityPulsScale_ImageY.Enabled = true;
                    label430.Enabled = true;
                    label431.Enabled = true;
                    comboBox_ActivityPulsScale_Color.Enabled = false;
                    numericUpDown_ActivityPulsScale_Center_X.Enabled = false;
                    numericUpDown_ActivityPulsScale_Center_Y.Enabled = false;
                    label420.Enabled = false;
                    label424.Enabled = false;
                    label425.Enabled = false;
                }
                else
                {
                    comboBox_ActivityPulsScale_Image.Enabled = false;
                    numericUpDown_ActivityPulsScale_ImageX.Enabled = false;
                    numericUpDown_ActivityPulsScale_ImageY.Enabled = false;
                    label430.Enabled = false;
                    label431.Enabled = false;
                    comboBox_ActivityPulsScale_Color.Enabled = true;
                    numericUpDown_ActivityPulsScale_Center_X.Enabled = true;
                    numericUpDown_ActivityPulsScale_Center_Y.Enabled = true;
                    label420.Enabled = true;
                    label424.Enabled = true;
                    label425.Enabled = true;
                }
            }
        }

        private void checkBox_Pulse_ClockHand_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Pulse_ClockHand.Checked;
            numericUpDown_Pulse_ClockHand_X.Enabled = b;
            numericUpDown_Pulse_ClockHand_Y.Enabled = b;
            numericUpDown_Pulse_ClockHand_Offset_X.Enabled = b;
            numericUpDown_Pulse_ClockHand_Offset_Y.Enabled = b;
            comboBox_Pulse_ClockHand_Image.Enabled = b;
            numericUpDown_Pulse_ClockHand_StartAngle.Enabled = b;
            numericUpDown_Pulse_ClockHand_EndAngle.Enabled = b;
            label466.Enabled = b;
            label467.Enabled = b;
            label468.Enabled = b;
            label469.Enabled = b;
            label470.Enabled = b;
            label471.Enabled = b;
            label472.Enabled = b;
        }

        private void checkBox_ActivityPulsScale_Image_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ActivityPulsScale_Image.Checked)
            {
                comboBox_ActivityPulsScale_Image.Enabled = true;
                numericUpDown_ActivityPulsScale_ImageX.Enabled = true;
                numericUpDown_ActivityPulsScale_ImageY.Enabled = true;
                label430.Enabled = true;
                label431.Enabled = true;
                comboBox_ActivityPulsScale_Color.Enabled = false;
                numericUpDown_ActivityPulsScale_Center_X.Enabled = false;
                numericUpDown_ActivityPulsScale_Center_Y.Enabled = false;
                label420.Enabled = false;
                label424.Enabled = false;
                label425.Enabled = false;
            }
            else
            {
                comboBox_ActivityPulsScale_Image.Enabled = false;
                numericUpDown_ActivityPulsScale_ImageX.Enabled = false;
                numericUpDown_ActivityPulsScale_ImageY.Enabled = false;
                label430.Enabled = false;
                label431.Enabled = false;
                comboBox_ActivityPulsScale_Color.Enabled = true;
                numericUpDown_ActivityPulsScale_Center_X.Enabled = true;
                numericUpDown_ActivityPulsScale_Center_Y.Enabled = true;
                label420.Enabled = true;
                label424.Enabled = true;
                label425.Enabled = true;
            }
        }

        private void checkBox_ActivityCalories_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivityCalories.Checked;
            numericUpDown_ActivityCalories_StartCorner_X.Enabled = b;
            numericUpDown_ActivityCalories_StartCorner_Y.Enabled = b;
            numericUpDown_ActivityCalories_EndCorner_X.Enabled = b;
            numericUpDown_ActivityCalories_EndCorner_Y.Enabled = b;

            comboBox_ActivityCalories_Image.Enabled = b;
            numericUpDown_ActivityCalories_Count.Enabled = b;
            numericUpDown_ActivityCalories_Spacing.Enabled = b;
            comboBox_ActivityCalories_Alignment.Enabled = b;

            label152.Enabled = b;
            label153.Enabled = b;
            label154.Enabled = b;
            label155.Enabled = b;
            label156.Enabled = b;
            label157.Enabled = b;
            label158.Enabled = b;
            label159.Enabled = b;
            label160.Enabled = b;
            label161.Enabled = b;
        }

        private void checkBox_ActivityCaloriesScale_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivityCaloriesScale.Checked;
            numericUpDown_ActivityCaloriesScale_Center_X.Enabled = b;
            numericUpDown_ActivityCaloriesScale_Center_Y.Enabled = b;
            numericUpDown_ActivityCaloriesScale_Radius_X.Enabled = b;
            checkBox_ActivityCaloriesScale_Image.Enabled = b;
            comboBox_ActivityCaloriesScale_Image.Enabled = b;
            numericUpDown_ActivityCaloriesScale_ImageX.Enabled = b;
            numericUpDown_ActivityCaloriesScale_ImageY.Enabled = b;

            numericUpDown_ActivityCaloriesScale_Width.Enabled = b;
            comboBox_ActivityCaloriesScale_Color.Enabled = b;
            numericUpDown_ActivityCaloriesScale_EndAngle.Enabled = b;
            numericUpDown_ActivityCaloriesScale_StartAngle.Enabled = b;
            comboBox_ActivityCaloriesScale_Flatness.Enabled = b;

            label112.Enabled = b;
            label113.Enabled = b;
            label114.Enabled = b;
            label115.Enabled = b;
            label116.Enabled = b;
            label117.Enabled = b;
            label118.Enabled = b;
            label119.Enabled = b;
            label120.Enabled = b;
            label426.Enabled = b;
            label427.Enabled = b;

            if (b)
            {
                if (checkBox_ActivityCaloriesScale_Image.Checked)
                {
                    comboBox_ActivityCaloriesScale_Image.Enabled = true;
                    numericUpDown_ActivityCaloriesScale_ImageX.Enabled = true;
                    numericUpDown_ActivityCaloriesScale_ImageY.Enabled = true;
                    label426.Enabled = true;
                    label427.Enabled = true;
                    comboBox_ActivityCaloriesScale_Color.Enabled = false;
                    numericUpDown_ActivityCaloriesScale_Center_X.Enabled = false;
                    numericUpDown_ActivityCaloriesScale_Center_Y.Enabled = false;
                    label115.Enabled = false;
                    label119.Enabled = false;
                    label120.Enabled = false;
                }
                else
                {
                    comboBox_ActivityCaloriesScale_Image.Enabled = false;
                    numericUpDown_ActivityCaloriesScale_ImageX.Enabled = false;
                    numericUpDown_ActivityCaloriesScale_ImageY.Enabled = false;
                    label426.Enabled = false;
                    label427.Enabled = false;
                    comboBox_ActivityCaloriesScale_Color.Enabled = true;
                    numericUpDown_ActivityCaloriesScale_Center_X.Enabled = true;
                    numericUpDown_ActivityCaloriesScale_Center_Y.Enabled = true;
                    label115.Enabled = true;
                    label119.Enabled = true;
                    label120.Enabled = true;
                }
            }
        }

        private void checkBox_Calories_ClockHand_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Calories_ClockHand.Checked;
            numericUpDown_Calories_ClockHand_X.Enabled = b;
            numericUpDown_Calories_ClockHand_Y.Enabled = b;
            numericUpDown_Calories_ClockHand_Offset_X.Enabled = b;
            numericUpDown_Calories_ClockHand_Offset_Y.Enabled = b;
            comboBox_Calories_ClockHand_Image.Enabled = b;
            numericUpDown_Calories_ClockHand_StartAngle.Enabled = b;
            numericUpDown_Calories_ClockHand_EndAngle.Enabled = b;
            label458.Enabled = b;
            label459.Enabled = b;
            label461.Enabled = b;
            label462.Enabled = b;
            label463.Enabled = b;
            label464.Enabled = b;
            label465.Enabled = b;
        }

        private void checkBox_ActivityCaloriesScale_Image_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ActivityCaloriesScale_Image.Checked)
            {
                comboBox_ActivityCaloriesScale_Image.Enabled = true;
                numericUpDown_ActivityCaloriesScale_ImageX.Enabled = true;
                numericUpDown_ActivityCaloriesScale_ImageY.Enabled = true;
                label426.Enabled = true;
                label427.Enabled = true;
                comboBox_ActivityCaloriesScale_Color.Enabled = false;
                numericUpDown_ActivityCaloriesScale_Center_X.Enabled = false;
                numericUpDown_ActivityCaloriesScale_Center_Y.Enabled = false;
                label115.Enabled = false;
                label119.Enabled = false;
                label120.Enabled = false;
            }
            else
            {
                comboBox_ActivityCaloriesScale_Image.Enabled = false;
                numericUpDown_ActivityCaloriesScale_ImageX.Enabled = false;
                numericUpDown_ActivityCaloriesScale_ImageY.Enabled = false;
                label426.Enabled = false;
                label427.Enabled = false;
                comboBox_ActivityCaloriesScale_Color.Enabled = true;
                numericUpDown_ActivityCaloriesScale_Center_X.Enabled = true;
                numericUpDown_ActivityCaloriesScale_Center_Y.Enabled = true;
                label115.Enabled = true;
                label119.Enabled = true;
                label120.Enabled = true;
            }
        }

        private void checkBox_DND_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_DND.Checked;
            numericUpDown_DND_X.Enabled = b;
            numericUpDown_DND_Y.Enabled = b;
            comboBox_DND_On.Enabled = b;
            comboBox_DND_Off.Enabled = b;

            label180.Enabled = b;
            label181.Enabled = b;
            label182.Enabled = b;
            label183.Enabled = b;
        }

        private void checkBox_Lock_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Lock.Checked;
            numericUpDown_Lock_X.Enabled = b;
            numericUpDown_Lock_Y.Enabled = b;
            comboBox_Lock_On.Enabled = b;
            comboBox_Lock_Off.Enabled = b;

            label176.Enabled = b;
            label177.Enabled = b;
            label178.Enabled = b;
            label179.Enabled = b;
        }

        private void checkBox_Alarm_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Alarm.Checked;
            numericUpDown_Alarm_X.Enabled = b;
            numericUpDown_Alarm_Y.Enabled = b;
            comboBox_Alarm_On.Enabled = b;
            comboBox_Alarm_Off.Enabled = b;

            label170.Enabled = b;
            label175.Enabled = b;
            label174.Enabled = b;
            label169.Enabled = b;
        }

        private void checkBox_Bluetooth_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Bluetooth.Checked;
            numericUpDown_Bluetooth_X.Enabled = b;
            numericUpDown_Bluetooth_Y.Enabled = b;
            comboBox_Bluetooth_On.Enabled = b;
            comboBox_Bluetooth_Off.Enabled = b;

            label162.Enabled = b;
            label165.Enabled = b;
            label167.Enabled = b;
            label168.Enabled = b;
        }

        private void checkBox_Battery_CheckedChanged(object sender, EventArgs e)
        {
            tabControl4.Enabled = checkBox_Battery.Checked;
        }

        private void checkBox_Battery_Text_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Battery_Text.Checked;
            numericUpDown_Battery_Text_StartCorner_X.Enabled = b;
            numericUpDown_Battery_Text_StartCorner_Y.Enabled = b;
            numericUpDown_Battery_Text_EndCorner_X.Enabled = b;
            numericUpDown_Battery_Text_EndCorner_Y.Enabled = b;
            comboBox_Battery_Text_Image.Enabled = b;
            numericUpDown_Battery_Text_Count.Enabled = b;
            numericUpDown_Battery_Text_Spacing.Enabled = b;
            comboBox_Battery_Text_Alignment.Enabled = b;

            label184.Enabled = b;
            label185.Enabled = b;
            label186.Enabled = b;
            label187.Enabled = b;
            label188.Enabled = b;
            label189.Enabled = b;
            label190.Enabled = b;
            label191.Enabled = b;
            label192.Enabled = b;
            label193.Enabled = b;
        }

        private void checkBox_Battery_Percent_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Battery_Percent.Checked;
            comboBox_Battery_Percent_Image.Enabled = b;
            numericUpDown_Battery_Percent_X.Enabled = b;
            numericUpDown_Battery_Percent_Y.Enabled = b;

            label300.Enabled = b;
            label301.Enabled = b;
            label302.Enabled = b;
            label303.Enabled = b;
        }

        private void checkBox_Battery_Img_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Battery_Img.Checked;
            numericUpDown_Battery_Img_Count.Enabled = b;
            comboBox_Battery_Img_Image.Enabled = b;
            numericUpDown_Battery_Img_X.Enabled = b;
            numericUpDown_Battery_Img_Y.Enabled = b;

            label194.Enabled = b;
            label195.Enabled = b;
            label196.Enabled = b;
            label197.Enabled = b;
            label198.Enabled = b;
        }

        private void checkBox_Battery_Scale_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Battery_Scale.Checked;
            numericUpDown_Battery_Scale_Center_X.Enabled = b;
            numericUpDown_Battery_Scale_Center_Y.Enabled = b;
            numericUpDown_Battery_Scale_Radius_X.Enabled = b;
            checkBox_Battery_Scale_Image.Enabled = b;
            comboBox_Battery_Scale_Image.Enabled = b;
            numericUpDown_Battery_Scale_ImageX.Enabled = b;
            numericUpDown_Battery_Scale_ImageY.Enabled = b;

            numericUpDown_Battery_Scale_Width.Enabled = b;
            comboBox_Battery_Scale_Color.Enabled = b;
            numericUpDown_Battery_Scale_EndAngle.Enabled = b;
            numericUpDown_Battery_Scale_StartAngle.Enabled = b;
            comboBox_Battery_Flatness.Enabled = b;

            label199.Enabled = b;
            label200.Enabled = b;
            label201.Enabled = b;
            label202.Enabled = b;
            label203.Enabled = b;
            label204.Enabled = b;
            label205.Enabled = b;
            label206.Enabled = b;
            label347.Enabled = b;
            label428.Enabled = b;
            label429.Enabled = b;

            if (b)
            {
                if (checkBox_Battery_Scale_Image.Checked)
                {
                    comboBox_Battery_Scale_Image.Enabled = true;
                    numericUpDown_Battery_Scale_ImageX.Enabled = true;
                    numericUpDown_Battery_Scale_ImageY.Enabled = true;
                    label428.Enabled = true;
                    label429.Enabled = true;
                    comboBox_Battery_Scale_Color.Enabled = false;
                    numericUpDown_Battery_Scale_Center_X.Enabled = false;
                    numericUpDown_Battery_Scale_Center_Y.Enabled = false;
                    label201.Enabled = false;
                    label205.Enabled = false;
                    label206.Enabled = false;
                }
                else
                {
                    comboBox_Battery_Scale_Image.Enabled = false;
                    numericUpDown_Battery_Scale_ImageX.Enabled = false;
                    numericUpDown_Battery_Scale_ImageY.Enabled = false;
                    label428.Enabled = false;
                    label429.Enabled = false;
                    comboBox_ActivityCaloriesScale_Color.Enabled = true;
                    numericUpDown_Battery_Scale_Center_X.Enabled = true;
                    numericUpDown_Battery_Scale_Center_Y.Enabled = true;
                    label115.Enabled = true;
                    label205.Enabled = true;
                    label206.Enabled = true;
                }
            }
        }

        private void checkBox_Battery_Scale_Image_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Battery_Scale_Image.Checked)
            {
                comboBox_Battery_Scale_Image.Enabled = true;
                numericUpDown_Battery_Scale_ImageX.Enabled = true;
                numericUpDown_Battery_Scale_ImageY.Enabled = true;
                label428.Enabled = true;
                label429.Enabled = true;
                comboBox_Battery_Scale_Color.Enabled = false;
                numericUpDown_Battery_Scale_Center_X.Enabled = false;
                numericUpDown_Battery_Scale_Center_Y.Enabled = false;
                label201.Enabled = false;
                label205.Enabled = false;
                label206.Enabled = false;
            }
            else
            {
                comboBox_Battery_Scale_Image.Enabled = false;
                numericUpDown_Battery_Scale_ImageX.Enabled = false;
                numericUpDown_Battery_Scale_ImageY.Enabled = false;
                label428.Enabled = false;
                label429.Enabled = false;
                comboBox_Battery_Scale_Color.Enabled = true;
                numericUpDown_Battery_Scale_Center_X.Enabled = true;
                numericUpDown_Battery_Scale_Center_Y.Enabled = true;
                label201.Enabled = true;
                label205.Enabled = true;
                label206.Enabled = true;
            }
        }

        private void checkBox_Battery_ClockHand_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Battery_ClockHand.Checked;
            numericUpDown_Battery_ClockHand_X.Enabled = b;
            numericUpDown_Battery_ClockHand_Y.Enabled = b;
            numericUpDown_Battery_ClockHand_Offset_X.Enabled = b;
            numericUpDown_Battery_ClockHand_Offset_Y.Enabled = b;
            comboBox_Battery_ClockHand_Image.Enabled = b;
            numericUpDown_Battery_ClockHand_StartAngle.Enabled = b;
            numericUpDown_Battery_ClockHand_EndAngle.Enabled = b;
            label368.Enabled = b;
            label369.Enabled = b;
            label370.Enabled = b;
            label371.Enabled = b;
            label372.Enabled = b;
            label373.Enabled = b;
            label374.Enabled = b;
        }

        private void checkBox_Battery_IconSet_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Battery_IconSet.Checked;
            comboBox_Battery_IconSet_Image.Enabled = b;
            dataGridView_Battery_IconSet.Enabled = b;
            label405.Enabled = b;
        }

        private void checkBox_AnalogClock_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_AnalogClock.Checked;
            //groupBox_AnalogClock_Hour.Enabled = b;
            //groupBox_AnalogClock_Min.Enabled = b;
            //groupBox_AnalogClock_Sec.Enabled = b;
            tabControl_AnalogClock.Enabled = b;
        }

        private void checkBox_HourCenterImage_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_HourCenterImage.Checked;
            numericUpDown_HourCenterImage_X.Enabled = b;
            numericUpDown_HourCenterImage_Y.Enabled = b;
            comboBox_HourCenterImage_Image.Enabled = b;

            label310.Enabled = b;
            label311.Enabled = b;
            label312.Enabled = b;
        }

        private void checkBox_MinCenterImage_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_MinCenterImage.Checked;
            numericUpDown_MinCenterImage_X.Enabled = b;
            numericUpDown_MinCenterImage_Y.Enabled = b;
            comboBox_MinCenterImage_Image.Enabled = b;

            label307.Enabled = b;
            label308.Enabled = b;
            label309.Enabled = b;
        }

        private void checkBox_SecCenterImage_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_SecCenterImage.Checked;
            numericUpDown_SecCenterImage_X.Enabled = b;
            numericUpDown_SecCenterImage_Y.Enabled = b;
            comboBox_SecCenterImage_Image.Enabled = b;

            label304.Enabled = b;
            label305.Enabled = b;
            label306.Enabled = b;
        }

        private void checkBox_AnalogClock_Sec_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_AnalogClock_Sec.Checked;
            comboBox_AnalogClock_Sec_Image.Enabled = b;
            numericUpDown_AnalogClock_Sec_X.Enabled = b;
            numericUpDown_AnalogClock_Sec_Y.Enabled = b;
            numericUpDown_AnalogClock_Sec_Offset_X.Enabled = b;
            numericUpDown_AnalogClock_Sec_Offset_Y.Enabled = b;

            label210.Enabled = b;
            label211.Enabled = b;
            label212.Enabled = b;
            label353.Enabled = b;
            label354.Enabled = b;
        }

        private void checkBox_AnalogClock_Min_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_AnalogClock_Min.Checked;
            comboBox_AnalogClock_Min_Image.Enabled = b;
            numericUpDown_AnalogClock_Min_X.Enabled = b;
            numericUpDown_AnalogClock_Min_Y.Enabled = b;
            numericUpDown_AnalogClock_Min_Offset_X.Enabled = b;
            numericUpDown_AnalogClock_Min_Offset_Y.Enabled = b;

            label207.Enabled = b;
            label208.Enabled = b;
            label209.Enabled = b;
            label351.Enabled = b;
            label352.Enabled = b;
        }

        private void checkBox_AnalogClock_Hour_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_AnalogClock_Hour.Checked;
            comboBox_AnalogClock_Hour_Image.Enabled = b;
            numericUpDown_AnalogClock_Hour_X.Enabled = b;
            numericUpDown_AnalogClock_Hour_Y.Enabled = b;
            numericUpDown_AnalogClock_Hour_Offset_X.Enabled = b;
            numericUpDown_AnalogClock_Hour_Offset_Y.Enabled = b;

            label215.Enabled = b;
            label216.Enabled = b;
            label217.Enabled = b;
            label349.Enabled = b;
            label350.Enabled = b;
        }

        private void checkBox_Weather_Text_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Weather_Text.Checked;
            numericUpDown_Weather_Text_StartCorner_X.Enabled = b;
            numericUpDown_Weather_Text_StartCorner_Y.Enabled = b;
            numericUpDown_Weather_Text_EndCorner_X.Enabled = b;
            numericUpDown_Weather_Text_EndCorner_Y.Enabled = b;
            //comboBox_Weather_Text_MinusImage.Enabled = b;
            //comboBox_Weather_Text_DegImage.Enabled = b;
            //comboBox_Weather_Text_NDImage.Enabled = b;
            comboBox_Weather_Text_Image.Enabled = b;
            comboBox_Weather_Text_Alignment.Enabled = b;
            numericUpDown_Weather_Text_Spacing.Enabled = b;
            numericUpDown_Weather_Text_Count.Enabled = b;

            label270.Enabled = b;
            label271.Enabled = b;
            label272.Enabled = b;
            label273.Enabled = b;
            label274.Enabled = b;
            label275.Enabled = b;
            label276.Enabled = b;
            label277.Enabled = b;
            label278.Enabled = b;
            label279.Enabled = b;
            //label284.Enabled = b;
            //label285.Enabled = b;
            //label286.Enabled = b;

            if ((checkBox_Weather_Text.Checked) || (checkBox_Weather_Day.Checked) || (checkBox_Weather_Night.Checked))
            {
                groupBox_Symbols.Enabled = true;
            }
            else
            {
                groupBox_Symbols.Enabled = false;
            }
        }

        private void checkBox_Weather_Icon_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Weather_Icon.Checked;
            numericUpDown_Weather_Icon_X.Enabled = b;
            numericUpDown_Weather_Icon_Y.Enabled = b;
            numericUpDown_Weather_Icon_Count.Enabled = b;
            comboBox_Weather_Icon_NDImage.Enabled = b;
            comboBox_Weather_Icon_Image.Enabled = b;

            label280.Enabled = b;
            label281.Enabled = b;
            label282.Enabled = b;
            label283.Enabled = b;
            label287.Enabled = b;
        }

        private void checkBox_Weather_Day_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Weather_Day.Checked;
            numericUpDown_Weather_Day_StartCorner_X.Enabled = b;
            numericUpDown_Weather_Day_StartCorner_Y.Enabled = b;
            numericUpDown_Weather_Day_EndCorner_X.Enabled = b;
            numericUpDown_Weather_Day_EndCorner_Y.Enabled = b;
            //comboBox_Weather_Text_MinusImage.Enabled = b;
            //comboBox_Weather_Text_DegImage.Enabled = b;
            //comboBox_Weather_Text_NDImage.Enabled = b;
            comboBox_Weather_Day_Image.Enabled = b;
            comboBox_Weather_Day_Alignment.Enabled = b;
            numericUpDown_Weather_Day_Spacing.Enabled = b;
            numericUpDown_Weather_Day_Count.Enabled = b;

            label289.Enabled = b;
            label290.Enabled = b;
            label291.Enabled = b;
            label292.Enabled = b;
            label293.Enabled = b;
            label294.Enabled = b;
            label295.Enabled = b;
            label296.Enabled = b;
            label297.Enabled = b;
            label298.Enabled = b;

            checkBox_Weather_Night.Checked = b;

            if ((checkBox_Weather_Text.Checked) || (checkBox_Weather_Day.Checked) || (checkBox_Weather_Night.Checked))
            {
                groupBox_Symbols.Enabled = true;
            }
            else
            {
                groupBox_Symbols.Enabled = false;
            }
        }

        private void checkBox_Weather_Night_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Weather_Night.Checked;
            numericUpDown_Weather_Night_StartCorner_X.Enabled = b;
            numericUpDown_Weather_Night_StartCorner_Y.Enabled = b;
            numericUpDown_Weather_Night_EndCorner_X.Enabled = b;
            numericUpDown_Weather_Night_EndCorner_Y.Enabled = b;
            //comboBox_Weather_Text_MinusImage.Enabled = b;
            //comboBox_Weather_Text_DegImage.Enabled = b;
            //comboBox_Weather_Text_NDImage.Enabled = b;
            comboBox_Weather_Night_Image.Enabled = b;
            comboBox_Weather_Night_Alignment.Enabled = b;
            numericUpDown_Weather_Night_Spacing.Enabled = b;
            numericUpDown_Weather_Night_Count.Enabled = b;

            label313.Enabled = b;
            label314.Enabled = b;
            label315.Enabled = b;
            label316.Enabled = b;
            label317.Enabled = b;
            label318.Enabled = b;
            label319.Enabled = b;
            label320.Enabled = b;
            label321.Enabled = b;
            label322.Enabled = b;

            checkBox_Weather_Day.Checked = b;

            if ((checkBox_Weather_Text.Checked) || (checkBox_Weather_Day.Checked) || (checkBox_Weather_Night.Checked))
            {
                groupBox_Symbols.Enabled = true;
            }
            else
            {
                groupBox_Symbols.Enabled = false;
            }
        }

        private void checkBox_Shortcuts_CheckedChanged(object sender, EventArgs e)
        {
            tabControl_Shortcuts.Enabled = checkBox_Shortcuts.Checked;
        }

        private void checkBox_Shortcuts_Steps_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Shortcuts_Steps.Checked;
            numericUpDown_Shortcuts_Steps_X.Enabled = b;
            numericUpDown_Shortcuts_Steps_Y.Enabled = b;
            numericUpDown_Shortcuts_Steps_Width.Enabled = b;
            numericUpDown_Shortcuts_Steps_Height.Enabled = b;

            label435.Enabled = b;
            label438.Enabled = b;
            label439.Enabled = b;
            label440.Enabled = b;
            label442.Enabled = b;
            label443.Enabled = b;

        }

        private void checkBox_Shortcuts_Puls_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Shortcuts_Puls.Checked;
            numericUpDown_Shortcuts_Puls_X.Enabled = b;
            numericUpDown_Shortcuts_Puls_Y.Enabled = b;
            numericUpDown_Shortcuts_Puls_Width.Enabled = b;
            numericUpDown_Shortcuts_Puls_Height.Enabled = b;

            label434.Enabled = b;
            label436.Enabled = b;
            label437.Enabled = b;
            label441.Enabled = b;
            label444.Enabled = b;
            label445.Enabled = b;
        }

        private void checkBox_Shortcuts_Weather_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Shortcuts_Weather.Checked;
            numericUpDown_Shortcuts_Weather_X.Enabled = b;
            numericUpDown_Shortcuts_Weather_Y.Enabled = b;
            numericUpDown_Shortcuts_Weather_Width.Enabled = b;
            numericUpDown_Shortcuts_Weather_Height.Enabled = b;

            label446.Enabled = b;
            label447.Enabled = b;
            label448.Enabled = b;
            label449.Enabled = b;
            label450.Enabled = b;
            label451.Enabled = b;
        }

        private void checkBox_Shortcuts_Energy_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Shortcuts_Energy.Checked;
            numericUpDown_Shortcuts_Energy_X.Enabled = b;
            numericUpDown_Shortcuts_Energy_Y.Enabled = b;
            numericUpDown_Shortcuts_Energy_Width.Enabled = b;
            numericUpDown_Shortcuts_Energy_Height.Enabled = b;

            label452.Enabled = b;
            label453.Enabled = b;
            label454.Enabled = b;
            label455.Enabled = b;
            label456.Enabled = b;
            label457.Enabled = b;
        }
        #endregion

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)(sender);
            int i = 0;
            if(Int32.TryParse(comboBox.Text, out i))
            {
                try
                {
                    using (FileStream stream = new FileStream(ListImagesFullName[comboBox.SelectedIndex], FileMode.Open, FileAccess.Read))
                    {
                        pictureBox1.Image = Image.FromStream(stream);
                        timer1.Enabled = true;
                    }
                }
                catch { }
            }
            //pictureBox1.Image = null;
            JSON_write();
            PreviewImage();
        }

        private void checkBox_Delimiter_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Delimiter.Checked)
            {
                numericUpDown_Delimiter_X.Enabled = true;
                numericUpDown_Delimiter_Y.Enabled = true;
                comboBox_Delimiter_Image.Enabled = true;

                label37.Enabled = true;
                label39.Enabled = true;
                label38.Enabled = true;
            }
            else
            {
                numericUpDown_Delimiter_X.Enabled = false;
                numericUpDown_Delimiter_Y.Enabled = false;
                comboBox_Delimiter_Image.Enabled = false;

                label37.Enabled = false;
                label39.Enabled = false;
                label38.Enabled = false;
            }
        }

        private void checkBox_Click(object sender, EventArgs e)
        {
            JSON_write();
            PreviewImage();
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            JSON_write();
            PreviewImage();
        }
        

#region сворачиваем и разварачиваем панели с предустановками
        private void button_Set1_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 125;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
            SetPreferences1();
            PreviewImage();
        }

        private void button_Set2_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 125;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
            SetPreferences2();
            PreviewImage();
        }
        private void button_Set3_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 125;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
            SetPreferences3();
            PreviewImage();
        }
        private void button_Set4_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 125;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
            SetPreferences4();
            PreviewImage();
        }
        private void button_Set5_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 125;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
            SetPreferences5();
            PreviewImage();
        }
        private void button_Set6_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 125;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
            SetPreferences6();
            PreviewImage();
        }
        private void button_Set7_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 125;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
            SetPreferences7();
            PreviewImage();
        }
        private void button_Set8_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 125;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
            SetPreferences8();
            PreviewImage();
        }
        private void button_Set9_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 125;
            panel_Set10.Height = 1;
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
            SetPreferences9();
            PreviewImage();
        }
        private void button_Set10_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 125;
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
            SetPreferences10();
            PreviewImage();
        }
        private void button_Set11_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            panel_Set11.Height = 125;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
            SetPreferences11();
            PreviewImage();
        }
        private void button_Set12_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            panel_Set11.Height = 1;
            panel_Set12.Height = 125;
            panel_Set13.Height = 1;
            SetPreferences12();
            PreviewImage();
        }
        private void button_Set13_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 125;
            SetPreferences13();
            PreviewImage();
        }

        private void button_SetWeather_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 60;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
        }
#endregion

#region поменялись предустановки
        private void dateTimePicker_Time_Set1_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences1();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set2_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences2();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set3_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences3();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set4_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences4();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set5_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences5();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set6_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences6();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set7_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences7();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set8_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences8();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set9_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences9();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set10_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences10();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set11_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences11();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set12_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences12();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set13_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences13();
            PreviewImage();
        }
        //////////////////////////////
        private void numericUpDown_Battery_Set1_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences1();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set2_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences2();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set3_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences3();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set4_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences4();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set5_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences5();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set6_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences6();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set7_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences7();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set8_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences8();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set9_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences9();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set10_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences10();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set11_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences11();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set12_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences12();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set13_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences13();
            PreviewImage();
        }
        //////////////////////////////
        private void check_BoxBluetooth_Set1_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences1();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set2_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences2();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set3_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences3();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set4_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences4();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set5_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences5();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set6_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences6();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set7_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences7();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set8_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences8();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set9_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences9();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set10_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences10();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set11_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences11();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set12_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences12();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set13_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences13();
            PreviewImage();
        }
#endregion

        // переключаем цвет фона в таблице с картинками
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                dataGridView_ImagesList.DefaultCellStyle.BackColor = Color.Black;
                dataGridView_ImagesList.DefaultCellStyle.ForeColor = Color.White;
            }
            else
            {
                dataGridView_ImagesList.DefaultCellStyle.BackColor = Color.White;
                dataGridView_ImagesList.DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void comboBox_StepsProgress_Color_Click(object sender, EventArgs e)
        {
            if (colorDialog_StepsProgress.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            comboBox_StepsProgress_Color.BackColor = colorDialog_StepsProgress.Color; PreviewImage();
            JSON_write();
            PreviewImage();
        }
        
        private void numericUpDown_StepsProgress_Radius_X_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_StepsProgress_Radius_Y.Value = numericUpDown_StepsProgress_Radius_X.Value;
            PreviewImage();
        }
        
        private void comboBox_Battery_Scale_Color_Click(object sender, EventArgs e)
        {
            if (colorDialog_Battery.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            comboBox_Battery_Scale_Color.BackColor = colorDialog_Battery.Color;
            JSON_write();
            PreviewImage();
        }

        private void comboBox_ActivityPulsScale_Color_Click(object sender, EventArgs e)
        {
            if (colorDialog_Pulse.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            comboBox_ActivityPulsScale_Color.BackColor = colorDialog_Pulse.Color;
            JSON_write();
            PreviewImage();
        }

        private void comboBox_ActivityCaloriesScale_Color_Click(object sender, EventArgs e)
        {
            if (colorDialog_Calories.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            comboBox_ActivityCaloriesScale_Color.BackColor = colorDialog_Calories.Color;
            JSON_write();
            PreviewImage();
        }


        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            if ((formPreview == null) || (!formPreview.Visible))
            {
                formPreview = new Form_Preview();
                formPreview.Show(this);
                //formPreview.Show();

                switch (Program_Settings.Scale)
                {
                    case 0.5f:
                        formPreview.radioButton_small.Checked = true;
                        break;
                    case 1.5f:
                        formPreview.radioButton_large.Checked = true;
                        break;
                    case 2.0f:
                        formPreview.radioButton_xlarge.Checked = true;
                        break;
                    case 2.5f:
                        formPreview.radioButton_xxlarge.Checked = true;
                        break;
                    default:
                        formPreview.radioButton_normal.Checked = true;
                        break;

                }

                formPreview.panel_Preview.Resize += (object senderResize, EventArgs eResize) =>
                {
                    Form_Preview.Model_Wath.model_gtr47 = radioButton_47.Checked;
                    Form_Preview.Model_Wath.model_gtr42 = radioButton_42.Checked;
                    Form_Preview.Model_Wath.model_gts = radioButton_gts.Checked;
                    Form_Preview.Model_Wath.model_TRex = radioButton_TRex.Checked;
                    Graphics gPanelPreviewResize = formPreview.panel_Preview.CreateGraphics();
                    gPanelPreviewResize.Clear(panel_Preview.BackColor);
                    formPreview.radioButton_CheckedChanged(sender, e);
                    float scalePreviewResize = 1.0f;
                    if (formPreview.radioButton_small.Checked) scalePreviewResize = 0.5f;
                    if (formPreview.radioButton_large.Checked) scalePreviewResize = 1.5f;
                    if (formPreview.radioButton_xlarge.Checked) scalePreviewResize = 2.0f;
                    if (formPreview.radioButton_xxlarge.Checked) scalePreviewResize = 2.5f;

                    Program_Settings.Scale = scalePreviewResize;
                    string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
                    {
                        //DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);

                    PreviewToBitmap(gPanelPreviewResize, scalePreviewResize, checkBox_crop.Checked,
                        checkBox_WebW.Checked, checkBox_WebB.Checked, checkBox_border.Checked, 
                        checkBox_Show_Shortcuts.Checked, checkBox_Shortcuts_Area.Checked, checkBox_Shortcuts_Border.Checked);
                    gPanelPreviewResize.Dispose();
                };

                formPreview.panel_Preview.Paint += (object senderPaint, PaintEventArgs ePaint) =>
                {
                    //Form_Preview.Model_GTR47.model_gtr47 = radioButton_47.Checked;
                    //Graphics gPanelPreviewPaint = formPreview.panel_Preview.CreateGraphics();
                    //gPanelPreviewPaint.Clear(panel_Preview.BackColor);
                    //formPreview.radioButton_CheckedChanged(sender, e);
                    //float scalePreviewPaint = 1.0f;
                    //if (formPreview.radioButton_small.Checked) scalePreviewPaint = 0.5f;
                    //if (formPreview.radioButton_large.Checked) scalePreviewPaint = 1.5f;
                    //if (formPreview.radioButton_xlarge.Checked) scalePreviewPaint = 2.0f;
                    //if (formPreview.radioButton_xxlarge.Checked) scalePreviewPaint = 2.5f;
                    //PreviewToBitmap(gPanelPreviewPaint, scalePreviewPaint, radioButton_47.Checked, checkBox_WebW.Checked, checkBox_WebB.Checked);
                    //gPanelPreviewPaint.Dispose();
                    timer2.Enabled = false;
                    timer2.Enabled = true;
                };

                formPreview.FormClosing += (object senderClosing, FormClosingEventArgs eClosing) =>
                {
                    button_PreviewBig.Enabled = true;
                };
            }

            Form_Preview.Model_Wath.model_gtr47 = radioButton_47.Checked;
            Form_Preview.Model_Wath.model_gtr42 = radioButton_42.Checked;
            Form_Preview.Model_Wath.model_gts = radioButton_gts.Checked;
            Form_Preview.Model_Wath.model_TRex = radioButton_TRex.Checked;
            Graphics gPanel = formPreview.panel_Preview.CreateGraphics();
            gPanel.Clear(panel_Preview.BackColor);
            //Pen pen = new Pen(Color.Blue, 1);
            //Random rnd = new Random();
            //gPanel.DrawLine(pen, new Point(0, 0), new Point(rnd.Next(0, 450), rnd.Next(0, 450)));
            //Form_Preview.Model_GTR47.model_gtr47 = radioButton_47.Checked;
            formPreview.radioButton_CheckedChanged(sender, e);
            float scale = 1.0f;
            if (formPreview.radioButton_small.Checked) scale = 0.5f;
            if (formPreview.radioButton_large.Checked) scale = 1.5f;
            if (formPreview.radioButton_xlarge.Checked) scale = 2.0f;
            if (formPreview.radioButton_xxlarge.Checked) scale = 2.5f;

            PreviewToBitmap(gPanel, scale, checkBox_crop.Checked, checkBox_WebW.Checked, checkBox_WebB.Checked, 
                checkBox_border.Checked, checkBox_Show_Shortcuts.Checked, checkBox_Shortcuts_Area.Checked, 
                checkBox_Shortcuts_Border.Checked);
            gPanel.Dispose();

            button_PreviewBig.Enabled = false;
        }

        // считываем параметры из JsonPreview
        private void button_JsonPreview_Read_Click(object sender, EventArgs e)
        {
            //string subPath = Application.StartupPath + @"\Watch_face\";
            //if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = FullFileDir;
            openFileDialog.Filter = "Json files (*.json) | *.json";
            openFileDialog.FileName = "PreviewStates.json";
            //openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            ////openFileDialog1.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_PreviewStates;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullfilename = openFileDialog.FileName;
                JsonPreview_Read(fullfilename);
            }
        }

        // считываем параметры из JsonPreview
        private void JsonPreview_Read (string fullfilename)
        {
            string text = File.ReadAllText(fullfilename);

            PreviewView = false;
            PREWIEV_STATES_Json ps = new PREWIEV_STATES_Json();
            try
            {
                var objson = JsonConvert.DeserializeObject<object[]>(text);

                int count = objson.Count();

                string JSON_Text = JsonConvert.SerializeObject(objson, Formatting.Indented, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                //richTextBox_JSON.Text = JSON_Text;

                if (count == 0) return;
                if (count > 13) count = 13;
                for (int i = 0; i < count; i++)
                {
                    ps = JsonConvert.DeserializeObject<PREWIEV_STATES_Json>(objson[i].ToString(), new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    int year = ps.Time.Year;
                    int month = ps.Time.Month;
                    int day = ps.Time.Day;
                    int hour = ps.Time.Hour;
                    int min = ps.Time.Minute;
                    int sec = ps.Time.Second;
                    int battery = ps.BatteryLevel;
                    int calories = ps.Calories;
                    int pulse = ps.Pulse;
                    int distance = ps.Distance;
                    int steps = ps.Steps;
                    int goal = ps.Goal;
                    bool bluetooth = ps.Bluetooth;
                    bool alarm = ps.Alarm;
                    bool unlocked = ps.Unlocked;
                    bool dnd = ps.DoNotDisturb;
                    switch (i)
                    {
                        case 0:
                            dateTimePicker_Date_Set1.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set1.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set1.Value = battery;
                            numericUpDown_Calories_Set1.Value = calories;
                            numericUpDown_Pulse_Set1.Value = pulse;
                            numericUpDown_Distance_Set1.Value = distance;
                            numericUpDown_Steps_Set1.Value = steps;
                            numericUpDown_Goal_Set1.Value = goal;
                            check_BoxBluetooth_Set1.Checked = bluetooth;
                            checkBox_Alarm_Set1.Checked = alarm;
                            checkBox_Lock_Set1.Checked = unlocked;
                            checkBox_DoNotDisturb_Set1.Checked = dnd;
                            button_Set1.PerformClick();
                            break;
                        case 1:
                            dateTimePicker_Date_Set2.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set2.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set2.Value = battery;
                            numericUpDown_Calories_Set2.Value = calories;
                            numericUpDown_Pulse_Set2.Value = pulse;
                            numericUpDown_Distance_Set2.Value = distance;
                            numericUpDown_Steps_Set2.Value = steps;
                            numericUpDown_Goal_Set2.Value = goal;
                            check_BoxBluetooth_Set2.Checked = bluetooth;
                            checkBox_Alarm_Set2.Checked = alarm;
                            checkBox_Lock_Set2.Checked = unlocked;
                            checkBox_DoNotDisturb_Set2.Checked = dnd;
                            button_Set2.PerformClick();
                            break;
                        case 2:
                            dateTimePicker_Date_Set3.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set3.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set3.Value = battery;
                            numericUpDown_Calories_Set3.Value = calories;
                            numericUpDown_Pulse_Set3.Value = pulse;
                            numericUpDown_Distance_Set3.Value = distance;
                            numericUpDown_Steps_Set3.Value = steps;
                            numericUpDown_Goal_Set3.Value = goal;
                            check_BoxBluetooth_Set3.Checked = bluetooth;
                            checkBox_Alarm_Set3.Checked = alarm;
                            checkBox_Lock_Set3.Checked = unlocked;
                            checkBox_DoNotDisturb_Set3.Checked = dnd;
                            button_Set3.PerformClick();
                            break;
                        case 3:
                            dateTimePicker_Date_Set4.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set4.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set4.Value = battery;
                            numericUpDown_Calories_Set4.Value = calories;
                            numericUpDown_Pulse_Set4.Value = pulse;
                            numericUpDown_Distance_Set4.Value = distance;
                            numericUpDown_Steps_Set4.Value = steps;
                            numericUpDown_Goal_Set4.Value = goal;
                            check_BoxBluetooth_Set4.Checked = bluetooth;
                            checkBox_Alarm_Set4.Checked = alarm;
                            checkBox_Lock_Set4.Checked = unlocked;
                            checkBox_DoNotDisturb_Set4.Checked = dnd;
                            button_Set4.PerformClick();
                            break;
                        case 4:
                            dateTimePicker_Date_Set5.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set5.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set5.Value = battery;
                            numericUpDown_Calories_Set5.Value = calories;
                            numericUpDown_Pulse_Set5.Value = pulse;
                            numericUpDown_Distance_Set5.Value = distance;
                            numericUpDown_Steps_Set5.Value = steps;
                            numericUpDown_Goal_Set5.Value = goal;
                            check_BoxBluetooth_Set5.Checked = bluetooth;
                            checkBox_Alarm_Set5.Checked = alarm;
                            checkBox_Lock_Set5.Checked = unlocked;
                            checkBox_DoNotDisturb_Set5.Checked = dnd;
                            button_Set5.PerformClick();
                            break;
                        case 5:
                            dateTimePicker_Date_Set6.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set6.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set6.Value = battery;
                            numericUpDown_Calories_Set6.Value = calories;
                            numericUpDown_Pulse_Set6.Value = pulse;
                            numericUpDown_Distance_Set6.Value = distance;
                            numericUpDown_Steps_Set6.Value = steps;
                            numericUpDown_Goal_Set6.Value = goal;
                            check_BoxBluetooth_Set6.Checked = bluetooth;
                            checkBox_Alarm_Set6.Checked = alarm;
                            checkBox_Lock_Set6.Checked = unlocked;
                            checkBox_DoNotDisturb_Set6.Checked = dnd;
                            button_Set6.PerformClick();
                            break;
                        case 6:
                            dateTimePicker_Date_Set7.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set7.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set7.Value = battery;
                            numericUpDown_Calories_Set7.Value = calories;
                            numericUpDown_Pulse_Set7.Value = pulse;
                            numericUpDown_Distance_Set7.Value = distance;
                            numericUpDown_Steps_Set7.Value = steps;
                            numericUpDown_Goal_Set7.Value = goal;
                            check_BoxBluetooth_Set7.Checked = bluetooth;
                            checkBox_Alarm_Set7.Checked = alarm;
                            checkBox_Lock_Set7.Checked = unlocked;
                            checkBox_DoNotDisturb_Set7.Checked = dnd;
                            button_Set7.PerformClick();
                            break;
                        case 7:
                            dateTimePicker_Date_Set8.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set8.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set8.Value = battery;
                            numericUpDown_Calories_Set8.Value = calories;
                            numericUpDown_Pulse_Set8.Value = pulse;
                            numericUpDown_Distance_Set8.Value = distance;
                            numericUpDown_Steps_Set8.Value = steps;
                            numericUpDown_Goal_Set8.Value = goal;
                            check_BoxBluetooth_Set8.Checked = bluetooth;
                            checkBox_Alarm_Set8.Checked = alarm;
                            checkBox_Lock_Set8.Checked = unlocked;
                            checkBox_DoNotDisturb_Set8.Checked = dnd;
                            button_Set8.PerformClick();
                            break;
                        case 8:
                            dateTimePicker_Date_Set9.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set9.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set9.Value = battery;
                            numericUpDown_Calories_Set9.Value = calories;
                            numericUpDown_Pulse_Set9.Value = pulse;
                            numericUpDown_Distance_Set9.Value = distance;
                            numericUpDown_Steps_Set9.Value = steps;
                            numericUpDown_Goal_Set9.Value = goal;
                            check_BoxBluetooth_Set9.Checked = bluetooth;
                            checkBox_Alarm_Set9.Checked = alarm;
                            checkBox_Lock_Set9.Checked = unlocked;
                            checkBox_DoNotDisturb_Set9.Checked = dnd;
                            button_Set9.PerformClick();
                            break;
                        case 9:
                            dateTimePicker_Date_Set10.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set10.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set10.Value = battery;
                            numericUpDown_Calories_Set10.Value = calories;
                            numericUpDown_Pulse_Set10.Value = pulse;
                            numericUpDown_Distance_Set10.Value = distance;
                            numericUpDown_Steps_Set10.Value = steps;
                            numericUpDown_Goal_Set10.Value = goal;
                            check_BoxBluetooth_Set10.Checked = bluetooth;
                            checkBox_Alarm_Set10.Checked = alarm;
                            checkBox_Lock_Set10.Checked = unlocked;
                            checkBox_DoNotDisturb_Set10.Checked = dnd;
                            button_Set10.PerformClick();
                            break;
                        case 10:
                            dateTimePicker_Date_Set11.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set11.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set11.Value = battery;
                            numericUpDown_Calories_Set11.Value = calories;
                            numericUpDown_Pulse_Set11.Value = pulse;
                            numericUpDown_Distance_Set11.Value = distance;
                            numericUpDown_Steps_Set11.Value = steps;
                            numericUpDown_Goal_Set11.Value = goal;
                            check_BoxBluetooth_Set11.Checked = bluetooth;
                            checkBox_Alarm_Set11.Checked = alarm;
                            checkBox_Lock_Set11.Checked = unlocked;
                            checkBox_DoNotDisturb_Set11.Checked = dnd;
                            button_Set11.PerformClick();
                            break;
                        case 11:
                            dateTimePicker_Date_Set12.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set12.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set12.Value = battery;
                            numericUpDown_Calories_Set12.Value = calories;
                            numericUpDown_Pulse_Set12.Value = pulse;
                            numericUpDown_Distance_Set12.Value = distance;
                            numericUpDown_Steps_Set12.Value = steps;
                            numericUpDown_Goal_Set12.Value = goal;
                            check_BoxBluetooth_Set12.Checked = bluetooth;
                            checkBox_Alarm_Set12.Checked = alarm;
                            checkBox_Lock_Set12.Checked = unlocked;
                            checkBox_DoNotDisturb_Set12.Checked = dnd;
                            button_Set12.PerformClick();
                            break;
                        case 12:
                            dateTimePicker_Date_Set13.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set13.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set13.Value = battery;
                            numericUpDown_Calories_Set13.Value = calories;
                            numericUpDown_Pulse_Set13.Value = pulse;
                            numericUpDown_Distance_Set13.Value = distance;
                            numericUpDown_Steps_Set13.Value = steps;
                            numericUpDown_Goal_Set13.Value = goal;
                            check_BoxBluetooth_Set13.Checked = bluetooth;
                            checkBox_Alarm_Set13.Checked = alarm;
                            checkBox_Lock_Set13.Checked = unlocked;
                            checkBox_DoNotDisturb_Set13.Checked = dnd;
                            button_Set13.PerformClick();
                            break;
                    }
                }

            }
            catch (Exception)
            {
                MessageBox.Show(Properties.FormStrings.Message_JsonReadError_Text, Properties.FormStrings.Message_Error_Caption, 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //richTextBox_JSON.Text = JsonConvert.SerializeObject(objson, Formatting.Indented, new JsonSerializerSettings
            //{
            //    //DefaultValueHandling = DefaultValueHandling.Ignore,
            //    NullValueHandling = NullValueHandling.Ignore
            //});
            //PreviewView = false;
            PreviewView = true;
            PreviewImage();
        }

        // записываем параметры в JsonPreview
        private void button_JsonPreview_Write_Click(object sender, EventArgs e)
        {
            
            object[] objson = new object[] { };
            int count = 0;
            for (int i = 0; i < 13; i++)
            {
                PREWIEV_STATES_Json ps = new PREWIEV_STATES_Json();
                ps.Time = new TimePreview();
                switch (i)
                {
                    case 0:
                        ps.Time.Year = dateTimePicker_Date_Set1.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set1.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set1.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set1.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set1.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set1.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set1.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set1.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set1.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set1.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set1.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set1.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set1.Checked;
                        ps.Alarm = checkBox_Alarm_Set1.Checked;
                        ps.Unlocked = checkBox_Lock_Set1.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set1.Checked;

                        if (numericUpDown_Calories_Set1.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1); 
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 1:
                        ps.Time.Year = dateTimePicker_Date_Set2.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set2.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set2.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set2.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set2.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set2.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set2.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set2.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set2.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set2.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set2.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set2.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set2.Checked;
                        ps.Alarm = checkBox_Alarm_Set2.Checked;
                        ps.Unlocked = checkBox_Lock_Set2.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set2.Checked;

                        if (numericUpDown_Calories_Set2.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 2:
                        ps.Time.Year = dateTimePicker_Date_Set3.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set3.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set3.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set3.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set3.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set3.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set3.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set3.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set3.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set3.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set3.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set3.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set3.Checked;
                        ps.Alarm = checkBox_Alarm_Set3.Checked;
                        ps.Unlocked = checkBox_Lock_Set3.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set3.Checked;

                        if (numericUpDown_Calories_Set3.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 3:
                        ps.Time.Year = dateTimePicker_Date_Set4.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set4.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set4.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set4.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set4.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set4.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set4.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set4.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set4.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set4.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set4.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set4.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set4.Checked;
                        ps.Alarm = checkBox_Alarm_Set4.Checked;
                        ps.Unlocked = checkBox_Lock_Set4.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set4.Checked;

                        if (numericUpDown_Calories_Set4.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 4:
                        ps.Time.Year = dateTimePicker_Date_Set5.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set5.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set5.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set5.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set5.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set5.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set5.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set5.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set5.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set5.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set5.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set5.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set5.Checked;
                        ps.Alarm = checkBox_Alarm_Set5.Checked;
                        ps.Unlocked = checkBox_Lock_Set5.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set5.Checked;

                        if (numericUpDown_Calories_Set5.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 5:
                        ps.Time.Year = dateTimePicker_Date_Set6.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set6.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set6.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set6.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set6.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set6.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set6.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set6.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set6.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set6.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set6.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set6.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set6.Checked;
                        ps.Alarm = checkBox_Alarm_Set6.Checked;
                        ps.Unlocked = checkBox_Lock_Set6.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set6.Checked;

                        if (numericUpDown_Calories_Set6.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 6:
                        ps.Time.Year = dateTimePicker_Date_Set7.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set7.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set7.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set7.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set7.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set7.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set7.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set7.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set7.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set7.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set7.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set7.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set7.Checked;
                        ps.Alarm = checkBox_Alarm_Set7.Checked;
                        ps.Unlocked = checkBox_Lock_Set7.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set7.Checked;

                        if (numericUpDown_Calories_Set7.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 7:
                        ps.Time.Year = dateTimePicker_Date_Set8.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set8.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set8.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set8.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set8.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set8.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set8.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set8.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set8.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set8.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set8.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set8.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set8.Checked;
                        ps.Alarm = checkBox_Alarm_Set8.Checked;
                        ps.Unlocked = checkBox_Lock_Set8.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set8.Checked;

                        if (numericUpDown_Calories_Set8.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 8:
                        ps.Time.Year = dateTimePicker_Date_Set9.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set9.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set9.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set9.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set9.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set9.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set9.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set9.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set9.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set9.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set9.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set9.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set9.Checked;
                        ps.Alarm = checkBox_Alarm_Set9.Checked;
                        ps.Unlocked = checkBox_Lock_Set9.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set9.Checked;

                        if (numericUpDown_Calories_Set9.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 9:
                        ps.Time.Year = dateTimePicker_Date_Set10.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set10.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set10.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set10.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set10.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set10.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set10.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set10.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set10.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set10.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set10.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set10.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set10.Checked;
                        ps.Alarm = checkBox_Alarm_Set10.Checked;
                        ps.Unlocked = checkBox_Lock_Set10.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set10.Checked;

                        if (numericUpDown_Calories_Set10.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 10:
                        ps.Time.Year = dateTimePicker_Date_Set11.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set11.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set11.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set11.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set11.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set11.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set11.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set11.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set11.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set11.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set11.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set11.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set11.Checked;
                        ps.Alarm = checkBox_Alarm_Set10.Checked;
                        ps.Unlocked = checkBox_Lock_Set11.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set11.Checked;

                        if (numericUpDown_Calories_Set11.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 11:
                        ps.Time.Year = dateTimePicker_Date_Set12.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set12.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set12.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set12.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set12.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set12.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set12.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set12.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set12.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set12.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set12.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set12.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set12.Checked;
                        ps.Alarm = checkBox_Alarm_Set12.Checked;
                        ps.Unlocked = checkBox_Lock_Set12.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set12.Checked;

                        if (numericUpDown_Calories_Set12.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 12:
                        ps.Time.Year = dateTimePicker_Date_Set13.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set13.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set13.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set13.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set13.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set13.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set13.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set13.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set13.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set13.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set13.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set13.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set13.Checked;
                        ps.Alarm = checkBox_Alarm_Set13.Checked;
                        ps.Unlocked = checkBox_Lock_Set13.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set13.Checked;

                        if (numericUpDown_Calories_Set13.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                }
            }

            string string_json_temp = JsonConvert.SerializeObject(objson, Formatting.None, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            var objsontemp = JsonConvert.DeserializeObject<object[]>(string_json_temp);

            string formatted = JsonConvert.SerializeObject(objsontemp, Formatting.Indented);
            richTextBox_JSON.Text = formatted;


            if (formatted.Length < 10)
            {
                MessageBox.Show(Properties.FormStrings.Message_SaveOnly1234_Text);
                return;
            }
            //text = text.Replace(@"\", "");
            //text = text.Replace("\"{", "{");
            //text = text.Replace("}\"", "}");
            //text = text.Replace(",", ",\r\n");
            //text = text.Replace(":", ": ");
            //text = text.Replace(": {", ": {\r\n");
            //string formatted = JsonConvert.SerializeObject(text, Formatting.Indented);

            


            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //openFileDialog.InitialDirectory = subPath;
            saveFileDialog.Filter = "Json files (*.json) | *.json";
            saveFileDialog.FileName = "PreviewStates.json";
            //openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            ////openFileDialog1.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = Properties.FormStrings.Dialog_Title_PreviewStates;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullfilename = saveFileDialog.FileName;
                richTextBox_JSON.Text = formatted;
                File.WriteAllText(fullfilename, formatted, Encoding.UTF8);
            }
        }

        // случайные значения ностроек
        private void button_JsonPreview_Random_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            DateTime now = DateTime.Now;
            Random rnd = new Random();
            for (int i = 0; i < 13; i++)
            {
                int year = now.Year;
                int month = rnd.Next(0, 12)+1;
                int day = rnd.Next(0, 28)+1;
                int hour = rnd.Next(0, 24);
                int min = rnd.Next(0, 60);
                int sec = rnd.Next(0, 60);
                int battery = rnd.Next(0, 101);
                int calories = rnd.Next(0, 2500);
                int pulse = rnd.Next(45, 150);
                int distance = rnd.Next(0, 15000);
                int steps = rnd.Next(0, 15000);
                int goal = rnd.Next(0, 15000);
                bool bluetooth = rnd.Next(2) == 0 ? false : true;
                bool alarm = rnd.Next(2) == 0 ? false : true;
                bool unlocked = rnd.Next(2) == 0 ? false : true;
                bool dnd = rnd.Next(2) == 0 ? false : true;
                switch (i)
                {
                    case 0:
                        dateTimePicker_Date_Set1.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set1.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set1.Value = battery;
                        numericUpDown_Calories_Set1.Value = calories;
                        numericUpDown_Pulse_Set1.Value = pulse;
                        numericUpDown_Distance_Set1.Value = distance;
                        numericUpDown_Steps_Set1.Value = steps;
                        numericUpDown_Goal_Set1.Value = goal;
                        check_BoxBluetooth_Set1.Checked = bluetooth;
                        checkBox_Alarm_Set1.Checked = alarm;
                        checkBox_Lock_Set1.Checked = unlocked;
                        checkBox_DoNotDisturb_Set1.Checked = dnd;
                        //button_Set1.PerformClick();
                        break;
                    case 1:
                        dateTimePicker_Date_Set2.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set2.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set2.Value = battery;
                        numericUpDown_Calories_Set2.Value = calories;
                        numericUpDown_Pulse_Set2.Value = pulse;
                        numericUpDown_Distance_Set2.Value = distance;
                        numericUpDown_Steps_Set2.Value = steps;
                        numericUpDown_Goal_Set2.Value = goal;
                        check_BoxBluetooth_Set2.Checked = bluetooth;
                        checkBox_Alarm_Set2.Checked = alarm;
                        checkBox_Lock_Set2.Checked = unlocked;
                        checkBox_DoNotDisturb_Set2.Checked = dnd;
                        //button_Set2.PerformClick();
                        break;
                    case 2:
                        dateTimePicker_Date_Set3.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set3.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set3.Value = battery;
                        numericUpDown_Calories_Set3.Value = calories;
                        numericUpDown_Pulse_Set3.Value = pulse;
                        numericUpDown_Distance_Set3.Value = distance;
                        numericUpDown_Steps_Set3.Value = steps;
                        numericUpDown_Goal_Set3.Value = goal;
                        check_BoxBluetooth_Set3.Checked = bluetooth;
                        checkBox_Alarm_Set3.Checked = alarm;
                        checkBox_Lock_Set3.Checked = unlocked;
                        checkBox_DoNotDisturb_Set3.Checked = dnd;
                        //button_Set3.PerformClick();
                        break;
                    case 3:
                        dateTimePicker_Date_Set4.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set4.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set4.Value = battery;
                        numericUpDown_Calories_Set4.Value = calories;
                        numericUpDown_Pulse_Set4.Value = pulse;
                        numericUpDown_Distance_Set4.Value = distance;
                        numericUpDown_Steps_Set4.Value = steps;
                        numericUpDown_Goal_Set4.Value = goal;
                        check_BoxBluetooth_Set4.Checked = bluetooth;
                        checkBox_Alarm_Set4.Checked = alarm;
                        checkBox_Lock_Set4.Checked = unlocked;
                        checkBox_DoNotDisturb_Set4.Checked = dnd;
                        //button_Set4.PerformClick();
                        break;
                    case 4:
                        dateTimePicker_Date_Set5.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set5.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set5.Value = battery;
                        numericUpDown_Calories_Set5.Value = calories;
                        numericUpDown_Pulse_Set5.Value = pulse;
                        numericUpDown_Distance_Set5.Value = distance;
                        numericUpDown_Steps_Set5.Value = steps;
                        numericUpDown_Goal_Set5.Value = goal;
                        check_BoxBluetooth_Set5.Checked = bluetooth;
                        checkBox_Alarm_Set5.Checked = alarm;
                        checkBox_Lock_Set5.Checked = unlocked;
                        checkBox_DoNotDisturb_Set5.Checked = dnd;
                        //button_Set5.PerformClick();
                        break;
                    case 5:
                        dateTimePicker_Date_Set6.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set6.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set6.Value = battery;
                        numericUpDown_Calories_Set6.Value = calories;
                        numericUpDown_Pulse_Set6.Value = pulse;
                        numericUpDown_Distance_Set6.Value = distance;
                        numericUpDown_Steps_Set6.Value = steps;
                        numericUpDown_Goal_Set6.Value = goal;
                        check_BoxBluetooth_Set6.Checked = bluetooth;
                        checkBox_Alarm_Set6.Checked = alarm;
                        checkBox_Lock_Set6.Checked = unlocked;
                        checkBox_DoNotDisturb_Set6.Checked = dnd;
                        //button_Set6.PerformClick();
                        break;
                    case 6:
                        dateTimePicker_Date_Set7.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set7.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set7.Value = battery;
                        numericUpDown_Calories_Set7.Value = calories;
                        numericUpDown_Pulse_Set7.Value = pulse;
                        numericUpDown_Distance_Set7.Value = distance;
                        numericUpDown_Steps_Set7.Value = steps;
                        numericUpDown_Goal_Set7.Value = goal;
                        check_BoxBluetooth_Set7.Checked = bluetooth;
                        checkBox_Alarm_Set7.Checked = alarm;
                        checkBox_Lock_Set7.Checked = unlocked;
                        checkBox_DoNotDisturb_Set7.Checked = dnd;
                        //button_Set7.PerformClick();
                        break;
                    case 7:
                        dateTimePicker_Date_Set8.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set8.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set8.Value = battery;
                        numericUpDown_Calories_Set8.Value = calories;
                        numericUpDown_Pulse_Set8.Value = pulse;
                        numericUpDown_Distance_Set8.Value = distance;
                        numericUpDown_Steps_Set8.Value = steps;
                        numericUpDown_Goal_Set8.Value = goal;
                        check_BoxBluetooth_Set8.Checked = bluetooth;
                        checkBox_Alarm_Set8.Checked = alarm;
                        checkBox_Lock_Set8.Checked = unlocked;
                        checkBox_DoNotDisturb_Set8.Checked = dnd;
                        //button_Set8.PerformClick();
                        break;
                    case 8:
                        dateTimePicker_Date_Set9.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set9.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set9.Value = battery;
                        numericUpDown_Calories_Set9.Value = calories;
                        numericUpDown_Pulse_Set9.Value = pulse;
                        numericUpDown_Distance_Set9.Value = distance;
                        numericUpDown_Steps_Set9.Value = steps;
                        numericUpDown_Goal_Set9.Value = goal;
                        check_BoxBluetooth_Set9.Checked = bluetooth;
                        checkBox_Alarm_Set9.Checked = alarm;
                        checkBox_Lock_Set9.Checked = unlocked;
                        checkBox_DoNotDisturb_Set9.Checked = dnd;
                        //button_Set9.PerformClick();
                        break;
                    case 9:
                        dateTimePicker_Date_Set10.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set10.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set10.Value = battery;
                        numericUpDown_Calories_Set10.Value = calories;
                        numericUpDown_Pulse_Set10.Value = pulse;
                        numericUpDown_Distance_Set10.Value = distance;
                        numericUpDown_Steps_Set10.Value = steps;
                        numericUpDown_Goal_Set10.Value = goal;
                        check_BoxBluetooth_Set10.Checked = bluetooth;
                        checkBox_Alarm_Set10.Checked = alarm;
                        checkBox_Lock_Set10.Checked = unlocked;
                        checkBox_DoNotDisturb_Set10.Checked = dnd;
                        //button_Set10.PerformClick();
                        break;
                    case 10:
                        dateTimePicker_Date_Set11.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set11.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set11.Value = battery;
                        numericUpDown_Calories_Set11.Value = calories;
                        numericUpDown_Pulse_Set11.Value = pulse;
                        numericUpDown_Distance_Set11.Value = distance;
                        numericUpDown_Steps_Set11.Value = steps;
                        numericUpDown_Goal_Set11.Value = goal;
                        check_BoxBluetooth_Set11.Checked = bluetooth;
                        checkBox_Alarm_Set11.Checked = alarm;
                        checkBox_Lock_Set11.Checked = unlocked;
                        checkBox_DoNotDisturb_Set11.Checked = dnd;
                        //button_Set11.PerformClick();
                        break;
                    case 11:
                        dateTimePicker_Date_Set12.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set12.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set12.Value = battery;
                        numericUpDown_Calories_Set12.Value = calories;
                        numericUpDown_Pulse_Set12.Value = pulse;
                        numericUpDown_Distance_Set12.Value = distance;
                        numericUpDown_Steps_Set12.Value = steps;
                        numericUpDown_Goal_Set12.Value = goal;
                        check_BoxBluetooth_Set12.Checked = bluetooth;
                        checkBox_Alarm_Set12.Checked = alarm;
                        checkBox_Lock_Set12.Checked = unlocked;
                        checkBox_DoNotDisturb_Set12.Checked = dnd;
                        //button_Set12.PerformClick();
                        break;
                    case 12:
                        dateTimePicker_Date_Set13.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set13.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set13.Value = battery;
                        numericUpDown_Calories_Set13.Value = calories;
                        numericUpDown_Pulse_Set13.Value = pulse;
                        numericUpDown_Distance_Set13.Value = distance;
                        numericUpDown_Steps_Set13.Value = steps;
                        numericUpDown_Goal_Set13.Value = goal;
                        check_BoxBluetooth_Set13.Checked = bluetooth;
                        checkBox_Alarm_Set13.Checked = alarm;
                        checkBox_Lock_Set13.Checked = unlocked;
                        checkBox_DoNotDisturb_Set13.Checked = dnd;
                        //button_Set13.PerformClick();
                        break;
                }
            }

            numericUpDown_WeatherSet_Temp.Value = rnd.Next(-25, 35) + 1;
            numericUpDown_WeatherSet_DayTemp.Value = numericUpDown_WeatherSet_Temp.Value;
            numericUpDown_WeatherSet_NightTemp.Value = numericUpDown_WeatherSet_Temp.Value - rnd.Next(3, 10);
            comboBox_WeatherSet_Icon.SelectedIndex = rnd.Next(0, 25);

            PreviewView = true;
            //PreviewImage();
            button_Set13.PerformClick();
        }

        private void checkBox_WebW_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_WebW.Checked) checkBox_WebB.Checked = false;
            PreviewImage();
        }

        private void checkBox_WebB_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_WebB.Checked) checkBox_WebW.Checked = false;
            PreviewImage();
        }

        private void numericUpDown_Battery_Scale_Radius_X_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_Battery_Scale_Radius_Y.Value = numericUpDown_Battery_Scale_Radius_X.Value;
            PreviewImage();
        }

        private void button_TextToJson_Click(object sender, EventArgs e)
        {
            string text = richTextBox_JSON.Text;
            //richTextBox_JSON.Text = text;


            try
            {
                Watch_Face = JsonConvert.DeserializeObject<WATCH_FACE_JSON>(text, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            catch (Exception)
            {

                MessageBox.Show(Properties.FormStrings.Message_JsonError_Text, Properties.FormStrings.Message_Error_Caption, 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            richTextBox_JSON.Text = JsonConvert.SerializeObject(Watch_Face, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            PreviewView = false;
            JSON_read();
            PreviewView = true;
            PreviewImage();
        }

        private void button_SaveJson_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = FullFileDir;
            saveFileDialog.FileName = FileName;
            saveFileDialog.Filter = "Json files (*.json) | *.json";

            //openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            ////openFileDialog1.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = Properties.FormStrings.Dialog_Title_Pack;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullfilename = saveFileDialog.FileName;
                File.WriteAllText(fullfilename, richTextBox_JSON.Text, Encoding.UTF8);

                FileName = Path.GetFileName(fullfilename);
                FullFileDir = Path.GetDirectoryName(fullfilename);
                if (checkBox_JsonWarnings.Checked) jsonWarnings();
            }
        }

        private void jsonWarnings()
        {
            if (Watch_Face.AnalogDialFace != null)
            {
                int i = 0;
                if ((Watch_Face.AnalogDialFace.Hours != null) && (Watch_Face.AnalogDialFace.Hours.Image != null)) i++;
                if ((Watch_Face.AnalogDialFace.Minutes != null) && (Watch_Face.AnalogDialFace.Minutes.Image != null)) i++;
                if ((Watch_Face.AnalogDialFace.Seconds != null) && (Watch_Face.AnalogDialFace.Seconds.Image != null)) i++;
                if (i < 3) MessageBox.Show(Properties.FormStrings.Message_Warning3clockHand_Text, 
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (Watch_Face.Time != null)
            {
                bool err = false;
                if ((Watch_Face.Time.Hours != null) && (Watch_Face.Time.Hours.Tens != null) && (Watch_Face.Time.Hours.Ones != null))
                {
                    if (Watch_Face.Time.Hours.Tens.ImageIndex != Watch_Face.Time.Hours.Ones.ImageIndex) err = true;
                }
                if ((Watch_Face.Time.Minutes != null) && (Watch_Face.Time.Minutes.Tens != null) && (Watch_Face.Time.Minutes.Ones != null))
                {
                    if (Watch_Face.Time.Minutes.Tens.ImageIndex != Watch_Face.Time.Minutes.Ones.ImageIndex) err = true;
                }
                if ((Watch_Face.Time.Seconds != null) && (Watch_Face.Time.Seconds.Tens != null) && (Watch_Face.Time.Seconds.Ones != null))
                {
                    if (Watch_Face.Time.Seconds.Tens.ImageIndex != Watch_Face.Time.Seconds.Ones.ImageIndex) err = true;
                }
                if (err) MessageBox.Show(Properties.FormStrings.Message_WarningTensOnes_Text,
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (Watch_Face.Time != null)
            {
                if ((Watch_Face.Time.Minutes != null) && (Watch_Face.Time.Hours == null))
                {
                    MessageBox.Show(Properties.FormStrings.Message_WarningOnlyMin_Text,
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if ((Watch_Face.Time.Seconds != null) && ((Watch_Face.Time.Minutes == null) || (Watch_Face.Time.Hours == null)))
                {
                    MessageBox.Show(Properties.FormStrings.Message_WarningOnlySec_Text,
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if ((Watch_Face.Activity != null) && (Watch_Face.Activity.Distance != null))
            {
                if(Watch_Face.Activity.Distance.SuffixImageIndex==null)
                    MessageBox.Show(Properties.FormStrings.Message_WarningDistanceSuffix,
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void checkBox_Weather_CheckedChanged(object sender, EventArgs e)
        {
            tabControl_Weather.Enabled = checkBox_Weather.Checked;
            if ((checkBox_Weather_Text.Checked) || (checkBox_Weather_Day.Checked) || (checkBox_Weather_Night.Checked))
            {
                groupBox_Symbols.Enabled = true;
            }
            else
            {
                groupBox_Symbols.Enabled = false;
            }
        }

        private void comboBox_WeatherSet_Icon_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))
            {
                comboBox_WeatherSet_Icon.Text = "";
                comboBox_WeatherSet_Icon.SelectedIndex = -1;
                JSON_write();
                PreviewImage();
            }
        }

        private void checkBox_WeatherSet_Temp_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown_WeatherSet_Temp.Enabled = checkBox_WeatherSet_Temp.Checked;
        }

        private void checkBox_WeatherSet_DayTemp_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown_WeatherSet_NightTemp.Enabled = checkBox_WeatherSet_DayTemp.Checked;
            numericUpDown_WeatherSet_DayTemp.Enabled = checkBox_WeatherSet_DayTemp.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            //pictureBox1.Image.Save(@"C:\test.png");
            pictureBox1.Image = null;
        }

        private void panel_Preview_DoubleClick(object sender, EventArgs e)
        {
            if (panel_Preview.Height < 300) button_PreviewBig.PerformClick();
            else
            {
                //if (radioButton_47.Checked) button_PreviewSmall.PerformClick();
                //else button_PreviewSmall_42.PerformClick();
            }
        }

        private void button_SavePNG_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //openFileDialog.InitialDirectory = subPath;
            saveFileDialog.Filter = "PNG Files: (*.png)|*.png";
            saveFileDialog.FileName = "Preview.png";
            //openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            ////openFileDialog1.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = Properties.FormStrings.Dialog_Title_SavePNG;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bitmap = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
                Bitmap mask = new Bitmap(@"Mask\mask_gtr47.png");
                if (radioButton_42.Checked)
                {
                    bitmap = new Bitmap(Convert.ToInt32(390), Convert.ToInt32(390), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(@"Mask\mask_gtr42.png");
                }
                if (radioButton_gts.Checked)
                {
                    bitmap = new Bitmap(Convert.ToInt32(348), Convert.ToInt32(442), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(@"Mask\mask_gts.png");
                }
                Graphics gPanel = Graphics.FromImage(bitmap);
                PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false);
                if(checkBox_crop.Checked) bitmap = ApplyMask(bitmap, mask);
                bitmap.Save(saveFileDialog.FileName, ImageFormat.Png);
            }
        }

        private void button_SaveGIF_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //openFileDialog.InitialDirectory = subPath;
            saveFileDialog.Filter = "GIF Files: (*.gif)|*.gif";
            saveFileDialog.FileName = "Preview.gif";
            //openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            ////openFileDialog1.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = Properties.FormStrings.Dialog_Title_SaveGIF;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bitmap = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
                Bitmap mask = new Bitmap(@"Mask\mask_gtr47.png");
                if (radioButton_42.Checked)
                {
                    bitmap = new Bitmap(Convert.ToInt32(390), Convert.ToInt32(390), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(@"Mask\mask_gtr42.png");
                }
                if (radioButton_gts.Checked)
                {
                    bitmap = new Bitmap(Convert.ToInt32(348), Convert.ToInt32(442), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(@"Mask\mask_gts.png");
                }
                Graphics gPanel = Graphics.FromImage(bitmap);
                bool save = false;
                Random rnd = new Random();
                PreviewView = false;

                using (MagickImageCollection collection = new MagickImageCollection())
                {
                    for (int i = 0; i < 13; i++)
                    {
                        save = false;
                        switch (i)
                        {
                            case 0:
                                //button_Set1.PerformClick();
                                SetPreferences1();
                                save = true;
                                break;
                            case 1:
                                if (numericUpDown_Calories_Set2.Value != 1234)
                                {
                                    //button_Set2.PerformClick();
                                    SetPreferences2();
                                    save = true;
                                }
                                break;
                            case 2:
                                if (numericUpDown_Calories_Set3.Value != 1234)
                                {
                                    //button_Set3.PerformClick();
                                    SetPreferences3();
                                    save = true;
                                }
                                break;
                            case 3:
                                if (numericUpDown_Calories_Set4.Value != 1234)
                                {
                                    //button_Set4.PerformClick();
                                    SetPreferences4();
                                    save = true;
                                }
                                break;
                            case 4:
                                if (numericUpDown_Calories_Set5.Value != 1234)
                                {
                                    //button_Set5.PerformClick();
                                    SetPreferences5();
                                    save = true;
                                }
                                break;
                            case 5:
                                if (numericUpDown_Calories_Set6.Value != 1234)
                                {
                                    //button_Set6.PerformClick();
                                    SetPreferences6();
                                    save = true;
                                }
                                break;
                            case 6:
                                if (numericUpDown_Calories_Set7.Value != 1234)
                                {
                                    //button_Set7.PerformClick();
                                    SetPreferences7();
                                    save = true;
                                }
                                break;
                            case 7:
                                if (numericUpDown_Calories_Set8.Value != 1234)
                                {
                                    //button_Set8.PerformClick();
                                    SetPreferences8();
                                    save = true;
                                }
                                break;
                            case 8:
                                if (numericUpDown_Calories_Set9.Value != 1234)
                                {
                                    //button_Set9.PerformClick();
                                    SetPreferences9();
                                    save = true;
                                }
                                break;
                            case 9:
                                if (numericUpDown_Calories_Set10.Value != 1234)
                                {
                                    //button_Set10.PerformClick();
                                    SetPreferences10();
                                    save = true;
                                }
                                break;
                            case 10:
                                if (numericUpDown_Calories_Set11.Value != 1234)
                                {
                                    //button_Set11.PerformClick();
                                    SetPreferences11();
                                    save = true;
                                }
                                break;
                            case 11:
                                if (numericUpDown_Calories_Set12.Value != 1234)
                                {
                                    //button_Set12.PerformClick();
                                    SetPreferences12();
                                    save = true;
                                }
                                break;
                            case 12:
                                if (numericUpDown_Calories_Set13.Value != 1234)
                                {
                                    //button_Set13.PerformClick();
                                    SetPreferences13();
                                    save = true;
                                }
                                break;
                        }

                        if (save)
                        {
                            int WeatherSet_Temp = (int)numericUpDown_WeatherSet_Temp.Value;
                            int WeatherSet_DayTemp = (int)numericUpDown_WeatherSet_DayTemp.Value;
                            int WeatherSet_NightTemp = (int)numericUpDown_WeatherSet_NightTemp.Value;
                            int WeatherSet_Icon = comboBox_WeatherSet_Icon.SelectedIndex;

                            numericUpDown_WeatherSet_Temp.Value = rnd.Next(-25, 35) + 1;
                            numericUpDown_WeatherSet_DayTemp.Value = numericUpDown_WeatherSet_Temp.Value;
                            numericUpDown_WeatherSet_NightTemp.Value = numericUpDown_WeatherSet_Temp.Value - rnd.Next(3, 10);
                            comboBox_WeatherSet_Icon.SelectedIndex = rnd.Next(0, 25);

                            PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false);
                            if (checkBox_crop.Checked) {
                                bitmap = ApplyMask(bitmap, mask);
                                gPanel = Graphics.FromImage(bitmap);
                            }
                            // Add first image and set the animation delay to 100ms
                            MagickImage item = new MagickImage(bitmap);
                            //ExifProfile profile = item.GetExifProfile();
                            collection.Add(item);
                            //collection[collection.Count - 1].AnimationDelay = 100;
                            collection[collection.Count - 1].AnimationDelay = (int)(100 * numericUpDown_Gif_Speed.Value);

                            numericUpDown_WeatherSet_Temp.Value = WeatherSet_Temp;
                            numericUpDown_WeatherSet_DayTemp.Value = WeatherSet_DayTemp;
                            numericUpDown_WeatherSet_NightTemp.Value = WeatherSet_NightTemp;
                            comboBox_WeatherSet_Icon.SelectedIndex = WeatherSet_Icon;
                        }
                    }
                    
                    // Optionally reduce colors
                    QuantizeSettings settings = new QuantizeSettings();
                    //settings.Colors = 256;
                    //collection.Quantize(settings);

                    // Optionally optimize the images (images should have the same size).
                    collection.OptimizeTransparency();
                    //collection.Optimize();

                    // Save gif
                    collection.Write(saveFileDialog.FileName);
                }
                PreviewView = true;
                mask.Dispose();
            }
        }

        public Bitmap ApplyMask(Bitmap inputImage, Bitmap mask)
        {
            //ushort[] bgColors = { 203, 255, 240 };
            ImageMagick.MagickImage image = new ImageMagick.MagickImage(inputImage);
            //ImageMagick.MagickImage image = new ImageMagick.MagickImage("0.png");
            ImageMagick.MagickImage combineMask = new ImageMagick.MagickImage(mask);

            image.Composite(combineMask, ImageMagick.CompositeOperator.CopyAlpha, Channels.Alpha);
            //image.Settings.BackgroundColor = new ImageMagick.MagickColor(bgColors[0], bgColors[1], bgColors[2]);
            //image.Alpha(ImageMagick.AlphaOption.Remove);
            //image.Transparent(ImageMagick.MagickColor.FromRgba(0, 0, 0, 0));

            return image.ToBitmap();
        }

        private void radioButton_47_CheckedChanged(object sender, EventArgs e)
        {
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
            else if (radioButton_gts.Checked)
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
            else if (radioButton_TRex.Checked)
            {
                this.Text = "T-Rex watch face editor";
                panel_Preview.Height = 183;
                panel_Preview.Width = 183;
                offSet_X = 180;
                offSet_Y = 180;

                textBox_unpack_command.Text = Program_Settings.unpack_command_TRex;
                textBox_pack_command.Text = Program_Settings.pack_command_TRex;

                button_unpack.Enabled = true;
                button_pack.Enabled = true;
                button_zip.Enabled = true;
            }

            if ((formPreview != null) && (formPreview.Visible))
            {
                Form_Preview.Model_Wath.model_gtr47 = radioButton_47.Checked;
                Form_Preview.Model_Wath.model_gtr42 = radioButton_42.Checked;
                Form_Preview.Model_Wath.model_gts = radioButton_gts.Checked;
                Form_Preview.Model_Wath.model_TRex = radioButton_TRex.Checked;
                formPreview.radioButton_CheckedChanged(sender, e);
            }

            Program_Settings.Model_GTR47 = radioButton_47.Checked;
            Program_Settings.Model_GTR42 = radioButton_42.Checked;
            Program_Settings.Model_GTS = radioButton_gts.Checked;
            Program_Settings.Model_TRex = radioButton_TRex.Checked;
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);

        PreviewImage();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            if ((formPreview != null) && (formPreview.Visible))
            {
                Form_Preview.Model_Wath.model_gtr47 = radioButton_47.Checked;
                Form_Preview.Model_Wath.model_gtr42 = radioButton_42.Checked;
                Form_Preview.Model_Wath.model_gts = radioButton_gts.Checked;
                Graphics gPanelPreviewPaint = formPreview.panel_Preview.CreateGraphics();
                gPanelPreviewPaint.Clear(panel_Preview.BackColor);
                formPreview.radioButton_CheckedChanged(sender, e);
                float scalePreviewPaint = 1.0f;
                if (formPreview.radioButton_small.Checked) scalePreviewPaint = 0.5f;
                if (formPreview.radioButton_large.Checked) scalePreviewPaint = 1.5f;
                if (formPreview.radioButton_xlarge.Checked) scalePreviewPaint = 2.0f;
                if (formPreview.radioButton_xxlarge.Checked) scalePreviewPaint = 2.5f;
                PreviewToBitmap(gPanelPreviewPaint, scalePreviewPaint, checkBox_crop.Checked,
                    checkBox_WebW.Checked, checkBox_WebB.Checked, checkBox_border.Checked, 
                    checkBox_Show_Shortcuts.Checked, checkBox_Shortcuts_Area.Checked, checkBox_Shortcuts_Border.Checked);
                gPanelPreviewPaint.Dispose();
            }
        }

        private void panel_Preview_MouseMove(object sender, MouseEventArgs e)
        {
            int CursorX = e.X;
            int CursorY = e.Y;

            label_preview_X.Text = "X=" + (CursorX * 2).ToString();
            label_preview_Y.Text = "Y=" + (CursorY * 2).ToString();

            label_preview_X.Visible = true;
            label_preview_Y.Visible = true;
        }

        private void panel_Preview_MouseLeave(object sender, EventArgs e)
        {
            label_preview_X.Visible = false;
            label_preview_Y.Visible = false;

        }

        private void comboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))
            {
                ComboBox comboBox = sender as ComboBox;
                comboBox.Text = "";
                comboBox.SelectedIndex = -1;
                JSON_write();
                PreviewImage();
            }
        }

        private void comboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        //private void numericUpDown_X_DoubleClick(object sender, EventArgs e)
        //{
        //    NumericUpDown numericUpDown = sender as NumericUpDown;
        //    numericUpDown.Value = MouseСoordinates.X;
        //}

        //private void numericUpDown_Y_DoubleClick(object sender, EventArgs e)
        //{
        //    NumericUpDown numericUpDown = sender as NumericUpDown;
        //    numericUpDown.Value = MouseСoordinates.Y;
        //}

        //private void contextMenuStrip_X_Click(object sender, EventArgs e)
        //{
        //    ContextMenuStrip menu = sender as ContextMenuStrip;
        //    Control sourceControl = menu.SourceControl;
        //    NumericUpDown numericUpDown = sourceControl as NumericUpDown;
        //    numericUpDown.Value = MouseСoordinates.X;
        //}

        //private void contextMenuStrip_Y_Click(object sender, EventArgs e)
        //{
        //    ContextMenuStrip menu = sender as ContextMenuStrip;
        //    Control sourceControl = menu.SourceControl;
        //    NumericUpDown numericUpDown = sourceControl as NumericUpDown;
        //    numericUpDown.Value = MouseСoordinates.Y;
        //}

        private void contextMenuStrip_X_Opening(object sender, CancelEventArgs e)
        {
            if ((MouseСoordinates.X < 0) || (MouseСoordinates.Y < 0))
            {
                contextMenuStrip_X.Items[0].Enabled = false;
            }
            else
            {
                contextMenuStrip_X.Items[0].Enabled = true;
            }
            decimal i = 0;
            if ((Clipboard.ContainsText() == true) && (decimal.TryParse(Clipboard.GetText(), out i)))
            { 
                contextMenuStrip_X.Items[2].Enabled = true;
            }
            else
            {
                contextMenuStrip_X.Items[2].Enabled = false;
            }
        }

        private void contextMenuStrip_Y_Opening(object sender, CancelEventArgs e)
        {
            if ((MouseСoordinates.X < 0) || (MouseСoordinates.Y < 0))
            {
                contextMenuStrip_Y.Items[0].Enabled = false;
            }
            else
            {
                contextMenuStrip_Y.Items[0].Enabled = true;
            }
            decimal i = 0;
            if ((Clipboard.ContainsText() == true) && (decimal.TryParse(Clipboard.GetText(), out i)))
            {
                contextMenuStrip_Y.Items[2].Enabled = true;
            }
            else
            {
                contextMenuStrip_Y.Items[2].Enabled = false;
            }
        }

        private void вставитьКоординатуХToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    NumericUpDown numericUpDown = sourceControl as NumericUpDown;
                    numericUpDown.Value = MouseСoordinates.X;
                }
            }
        }

        private void вставитьКоординатуYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    NumericUpDown numericUpDown = sourceControl as NumericUpDown;
                    numericUpDown.Value = MouseСoordinates.Y;
                }
            }
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    NumericUpDown numericUpDown = sourceControl as NumericUpDown;
                    Clipboard.SetText(numericUpDown.Value.ToString());
                }
            }
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    NumericUpDown numericUpDown = sourceControl as NumericUpDown;
                    //Если в буфере обмен содержится текст
                    if (Clipboard.ContainsText() == true)
                    {
                        //Извлекаем (точнее копируем) его и сохраняем в переменную
                        decimal i = 0;
                        if (decimal.TryParse(Clipboard.GetText(), out i))
                        {
                            if (i > numericUpDown.Maximum) i = numericUpDown.Maximum;
                            if (i < numericUpDown.Minimum) i = numericUpDown.Minimum;
                            numericUpDown.Value = i;
                        } 
                    }

                }
            }
        }
        
        private void numericUpDown_X_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.X < 0) return;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                numericUpDown.Value = MouseСoordinates.X;
            }
            else
            {
                if (e.Y <= numericUpDown.Controls[1].Height / 2)
                {
                    // Click is on Up arrow
                }
                else
                {
                    // Click is on Down arrow
                }
            }
        }

        private void numericUpDown_Y_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.Y < 0) return;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                numericUpDown.Value = MouseСoordinates.Y;
            }
            else
            {
                if (e.Y <= numericUpDown.Controls[1].Height / 2)
                {
                    // Click is on Up arrow
                }
                else
                {
                    // Click is on Down arrow
                }
            }
        }

        private void numericUpDown_OffSetX_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.X < 0) return;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                numericUpDown.Value = MouseСoordinates.X - offSet_X;
            }
            else
            {
                if (e.Y <= numericUpDown.Controls[1].Height / 2)
                {
                    // Click is on Up arrow
                }
                else
                {
                    // Click is on Down arrow
                }
            }
        }

        private void numericUpDown_OffSetY_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.Y < 0) return;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                numericUpDown.Value = MouseСoordinates.Y - offSet_Y;
            }
            else
            {
                if (e.Y <= numericUpDown.Controls[1].Height / 2)
                {
                    // Click is on Up arrow
                }
                else
                {
                    // Click is on Down arrow
                }
            }
        }

        // сохраняем настройки
        private void radioButton_Settings_Unpack_Dialog_CheckedChanged(object sender, EventArgs e)
        {
            if (Settings_Load) return;
            Program_Settings.Settings_AfterUnpack_Dialog = radioButton_Settings_AfterUnpack_Dialog.Checked;
            Program_Settings.Settings_AfterUnpack_DoNothing = radioButton_Settings_AfterUnpack_DoNothing.Checked;
            Program_Settings.Settings_AfterUnpack_Download = radioButton_Settings_AfterUnpack_Download.Checked;

            Program_Settings.Settings_Open_Dialog = radioButton_Settings_Open_Dialog.Checked;
            Program_Settings.Settings_Open_DoNotning = radioButton_Settings_Open_DoNotning.Checked;
            Program_Settings.Settings_Open_Download = radioButton_Settings_Open_Download.Checked;

            Program_Settings.Settings_Pack_Copy = radioButton_Settings_Pack_Copy.Checked;
            Program_Settings.Settings_Pack_Dialog = radioButton_Settings_Pack_Dialog.Checked;
            Program_Settings.Settings_Pack_DoNotning = radioButton_Settings_Pack_DoNotning.Checked;
            Program_Settings.Settings_Pack_GoToFile = radioButton_Settings_Pack_GoToFile.Checked;

            Program_Settings.Settings_Unpack_Dialog = radioButton_Settings_Unpack_Dialog.Checked;
            Program_Settings.Settings_Unpack_Replace = radioButton_Settings_Unpack_Replace.Checked;
            Program_Settings.Settings_Unpack_Save = radioButton_Settings_Unpack_Save.Checked;

            //string JSON_String = JObject.FromObject(Program_Settings).ToString();
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
            //File.WriteAllText(fullfilename, richTextBox_JSON.Text, Encoding.UTF8);
        }
        private void numericUpDown_Gif_Speed_ValueChanged(object sender, EventArgs e)
        {
            Program_Settings.Gif_Speed = (float)numericUpDown_Gif_Speed.Value;
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }
        private void checkBox_border_CheckedChanged(object sender, EventArgs e)
        {
            Program_Settings.ShowBorder = checkBox_border.Checked;
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }
        private void checkBox_crop_CheckedChanged(object sender, EventArgs e)
        {
            Program_Settings.Crop = checkBox_crop.Checked;
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }
        private void checkBox_Show_Shortcuts_CheckedChanged(object sender, EventArgs e)
        {
            Program_Settings.Show_Shortcuts = checkBox_Show_Shortcuts.Checked;
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }
        private void textBox_unpack_command_Leave(object sender, EventArgs e)
        {
            if (radioButton_47.Checked)
            {
                Program_Settings.unpack_command_GTR47 = textBox_unpack_command.Text;
                Program_Settings.pack_command_GTR47 = textBox_pack_command.Text;
            }
            else if(radioButton_42.Checked)
            {
                Program_Settings.unpack_command_GTR42 = textBox_unpack_command.Text;
                Program_Settings.pack_command_GTR42 = textBox_pack_command.Text;
            }
            else if (radioButton_gts.Checked)
            {
                Program_Settings.unpack_command_GTS = textBox_unpack_command.Text;
                Program_Settings.pack_command_GTS = textBox_pack_command.Text;
            }
            else if (radioButton_TRex.Checked)
            {
                Program_Settings.unpack_command_TRex = textBox_unpack_command.Text;
                Program_Settings.pack_command_TRex = textBox_pack_command.Text;
            }

            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText("Settings.json", JSON_String, Encoding.UTF8);
        }
        private void comboBox_Language_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Settings.language = comboBox_Language.Text;
            SetLanguage();
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
            if (!Settings_Load)
            {
                if (MessageBox.Show(Properties.FormStrings.Message_Restart_Text1 + Environment.NewLine +
                                Properties.FormStrings.Message_Restart_Text2, Properties.FormStrings.Message_Restart_Caption, 
                                MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Application.Restart();
                }
            }
        }

        private void button_Reset_Click(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + @"\Settings.json"))
            {
                File.Delete(Application.StartupPath + @"\Settings.json");
                if (MessageBox.Show(Properties.FormStrings.Message_Restart_Text1 + Environment.NewLine +
                                Properties.FormStrings.Message_Restart_Text2, Properties.FormStrings.Message_Restart_Caption,
                                MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Application.Restart();
                }
            }
        }

        // картинки в выпадающем списке
        private void comboBox_Image_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            //if (comboBox.Items.Count < 10) comboBox.DropDownHeight = comboBox.Items.Count * 35;
            //else comboBox.DropDownHeight = 106;
            float size = comboBox.Font.Size;
            Font myFont;
            FontFamily family = comboBox.Font.FontFamily;
            e.DrawBackground();
            int itemWidth = e.Bounds.Height;
            int itemHeight = e.Bounds.Height - 4;

            //SolidBrush solidBrush = new SolidBrush(Color.Black);
            //Rectangle rectangleFill = new Rectangle(2, e.Bounds.Top + 2,
            //        e.Bounds.Width, e.Bounds.Height - 4);
            //e.Graphics.FillRectangle(solidBrush, rectangleFill);
            //var src = new Bitmap(ListImagesFullName[image_index]);
            if (e.Index >= 0)
            {
                try
                {
                    using (FileStream stream = new FileStream(ListImagesFullName[e.Index], FileMode.Open, FileAccess.Read))
                    {
                        Image image = Image.FromStream(stream);
                        float scale = (float)itemWidth / image.Width;
                        if ((float)itemHeight / image.Height < scale) scale = (float)itemHeight / image.Height;
                        float itemWidthRec = image.Width * scale;
                        float itemHeightRec = image.Height * scale;
                        Rectangle rectangle = new Rectangle((int)(itemWidth- itemWidthRec)/2+2,
                            e.Bounds.Top + (int)(itemHeight - itemHeightRec) / 2 + 2, (int)itemWidthRec, (int)itemHeightRec);
                        e.Graphics.DrawImage(image, rectangle);
                    }
                }
                catch { }
            }
            //e.Graphics.DrawImage(imageList1.Images[e.Index], rectangle);
            myFont = new Font(family, size);
            StringFormat lineAlignment = new StringFormat();
            //lineAlignment.Alignment = StringAlignment.Center;
            lineAlignment.LineAlignment = StringAlignment.Center;
            if (e.Index >= 0)
                e.Graphics.DrawString(comboBox.Items[e.Index].ToString(), myFont, System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X + itemWidth, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height), lineAlignment);
            e.DrawFocusRectangle();

        }
        private void comboBox_Image_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 35;
        }

        // проверка разрядности системы
        public static bool Is64Bit()
        {
            if (Environment.Is64BitOperatingSystem) return true;
            else return false;
        }

        public string GetFileSize(FileInfo file)
        {
            try
            {
                double sizeinbytes = file.Length;
                double sizeinkbytes = Math.Round((sizeinbytes / 1024), 2);
                double sizeinmbytes = Math.Round((sizeinkbytes / 1024), 2);
                double sizeingbytes = Math.Round((sizeinmbytes / 1024), 2);
                if (sizeingbytes > 1)
                    return string.Format("{0} GB", sizeingbytes); //размер в гигабайтах
                else if (sizeinmbytes > 1)
                    return string.Format("{0} MB", sizeinmbytes); //возвращает размер в мегабайтах, если размер файла менее одного гигабайта
                else if (sizeinkbytes > 1)
                    return string.Format("{0} KB", sizeinkbytes); //возвращает размер в килобайтах, если размер файла менее одного мегабайта
                else
                    return string.Format("{0} B", sizeinbytes); //возвращает размер в байтах, если размер файла менее одного килобайта
            }
            catch { return "Ошибка получения размера файла"; } //перехват ошибок и возврат сообщения об ошибке
        }

        public double GetFileSizeMB(FileInfo file)
        {
            try
            {
                double sizeinbytes = file.Length;
                double sizeinkbytes = Math.Round((sizeinbytes / 1024), 2);
                double sizeinmbytes = Math.Round((sizeinkbytes / 1024), 2);
                double sizeingbytes = Math.Round((sizeinmbytes / 1024), 2);
                return sizeinmbytes;
            }
            catch { return 0; } //перехват ошибок и возврат сообщения об ошибке
        }

        private void dataGridView_IconSet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                Regex my_reg = new Regex(@"[^-\d]");
                string oldValue = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                string newValue = my_reg.Replace(oldValue, "");
                dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = newValue;
                if (newValue.Length == 0) dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
            }

            try
            {
                if ((dataGridView.Rows[e.RowIndex].Cells[0].Value == null) &&
                        (dataGridView.Rows[e.RowIndex].Cells[1].Value == null) && (e.RowIndex < dataGridView.Rows.Count - 1))
                    dataGridView.Rows.RemoveAt(e.RowIndex);
            }
            catch (Exception )
            {
            }
            JSON_write();
            PreviewImage();
        }

        private void dataGridView_IconSet_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            JSON_write();
            PreviewImage();
        }
        
        private void dataGridView_IconSet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (e.ColumnIndex == -1)
            {
                dataGridView.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
                dataGridView.EndEdit();
            }
            else if (dataGridView.EditMode != DataGridViewEditMode.EditOnEnter)
            {
                dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
                dataGridView.BeginEdit(false);
            }

            try
            {
                for (int i = dataGridView.Rows.Count - 1; i > -1; i--)
                {
                    DataGridViewRow row = dataGridView.Rows[i];
                    if (!row.IsNewRow && row.Cells[0].Value == null && row.Cells[1].Value == null)
                    {
                        dataGridView.Rows.Remove(row);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView_IconSet_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            object head = dataGridView.Rows[e.RowIndex].HeaderCell.Value;
            if (head == null || !head.Equals((e.RowIndex + 1).ToString()))
                dataGridView.Rows[e.RowIndex].HeaderCell.Value = (e.RowIndex + 1).ToString();
        }

        private void dataGridView_IconSet_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (e.ColumnIndex == 0 && MouseСoordinates.X >= 0)
            {
                dataGridView.Rows[e.RowIndex].Cells[0].Value = MouseСoordinates.X;
            }
            if (e.ColumnIndex == 1 && MouseСoordinates.Y >= 0)
            {
                dataGridView.Rows[e.RowIndex].Cells[1].Value = MouseСoordinates.Y;
            }
        }

        private void вставитьКоординатыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    DataGridView dataGridView = sourceControl as DataGridView;
                    DataGridViewRow row = dataGridView.CurrentRow;
                    row.Cells[0].Value = MouseСoordinates.X;
                    row.Cells[1].Value = MouseСoordinates.Y;
                    JSON_write();
                    PreviewImage();
                }
            }
        }

        private void копироватьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    DataGridView dataGridView = sourceControl as DataGridView;
                    DataGridViewCell cell = dataGridView.CurrentCell;
                    Clipboard.SetText(cell.Value.ToString());
                }
            }
        }

        private void вставитьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    DataGridView dataGridView = sourceControl as DataGridView;
                    DataGridViewCell cell = dataGridView.CurrentCell;
                    //Если в буфере обмен содержится текст
                    if (Clipboard.ContainsText() == true)
                    {
                        //Извлекаем (точнее копируем) его и сохраняем в переменную
                        decimal i = 0;
                        if (decimal.TryParse(Clipboard.GetText(), out i))
                        {
                            cell.Value = i;
                            int x = dataGridView.CurrentCellAddress.X;
                            int y = dataGridView.CurrentCellAddress.Y;
                            dataGridView.CurrentCell = dataGridView.Rows[0].Cells[0];
                            dataGridView.CurrentCell = dataGridView.Rows[y].Cells[x];
                            dataGridView.BeginEdit(false);
                            JSON_write();
                            PreviewImage();
                        }
                    }

                }
            }
        }

        private void удалитьСтрокуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    DataGridView dataGridView = sourceControl as DataGridView;
                    try
                    {
                        int rowIndex = dataGridView.CurrentCellAddress.Y;
                        dataGridView.Rows.RemoveAt(rowIndex);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void dataGridView_IconSet_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView dataGridView = sender as DataGridView;
                dataGridView.CurrentCell = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

        private void contextMenuStrip_XY_InTable_Opening(object sender, CancelEventArgs e)
        {
            if ((MouseСoordinates.X < 0) || (MouseСoordinates.Y < 0))
            {
                contextMenuStrip_XY_InTable.Items[0].Enabled = false;
            }
            else
            {
                contextMenuStrip_XY_InTable.Items[0].Enabled = true;
            }
            decimal i = 0;
            if ((Clipboard.ContainsText() == true) && (decimal.TryParse(Clipboard.GetText(), out i)))
            {
                contextMenuStrip_XY_InTable.Items[2].Enabled = true;
            }
            else
            {
                contextMenuStrip_XY_InTable.Items[2].Enabled = false;
            }
        }

        private void linkLabel_py_amazfit_tools_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/amazfitbip/py_amazfit_tools/releases/tag/v0.2-beta");
        }

        private void linkLabel_help_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //helpProvider1.SetHelpNavigator(this, HelpNavigator.Topic);
            //helpProvider1.SetHelpKeyword(this, "kratkaya_instruktsiya.htm");
            //SendKeys.Send("{F1}");
            //Help.ShowHelp(this, Application.StartupPath + Properties.FormStrings.File_ReadMy);
            string helpfile = Application.StartupPath + Properties.FormStrings.File_ReadMy;
            HelpNavigator navigator = HelpNavigator.Topic;
            Help.ShowHelp(this, helpfile, navigator, "kratkaya_instruktsiya.htm");
        }

        private void checkBox_Shortcuts_Area_CheckedChanged(object sender, EventArgs e)
        {
            Program_Settings.Shortcuts_Area = checkBox_Shortcuts_Area.Checked;
            Program_Settings.Shortcuts_Border = checkBox_Shortcuts_Border.Checked;

            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }

        private void numericUpDown_ActivityPulsScale_Radius_X_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_ActivityPulsScale_Radius_Y.Value = numericUpDown_ActivityPulsScale_Radius_X.Value;
            PreviewImage();
        }

        private void numericUpDown_ActivityCaloriesScale_Radius_X_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_ActivityCaloriesScale_Radius_Y.Value = numericUpDown_ActivityCaloriesScale_Radius_X.Value;
            PreviewImage();
        }
    }
}





public static class MouseСoordinates
{
    //public static int X { get; set; }
    //public static int Y { get; set; }
    public static int X = -1;
    public static int Y = -1;
}


/*static class Logger
{
    //----------------------------------------------------------
    // Статический метод записи строки в файл лога без переноса
    //----------------------------------------------------------
    public static void Write(string text)
    {
        using (StreamWriter sw = new StreamWriter(Application.StartupPath + "\\log.txt", true))
        {
            sw.Write(text);
        }
    }

    //---------------------------------------------------------
    // Статический метод записи строки в файл лога с переносом
    //---------------------------------------------------------
    public static void WriteLine(string message)
    {
        using (StreamWriter sw = new StreamWriter(Application.StartupPath + "\\log.txt", true))
        {
            sw.WriteLine(String.Format("{0,-23} {1}", DateTime.Now.ToString() + ":", message));
        }
    }
}*/