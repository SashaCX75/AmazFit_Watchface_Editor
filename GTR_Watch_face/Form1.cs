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
using System.Text;
using System.Windows.Forms;
using LineCap = System.Drawing.Drawing2D.LineCap;

namespace GTR_Watch_face
{
    public partial class Form1 : Form
    {
        WATCH_FACE_JSON Watch_Face;
        WATCH_FACE_PREWIEV Watch_Face_Preview;
        WATCH_FACE_PREWIEV_SET Watch_Face_Preview_Set;
        List<string> ListImages = new List<string>();
        List<string> ListImagesFullName = new List<string>();
        bool PreviewView;
        bool Settings_Load;
        string FileName;
        string FullFileDir;
        string StartFileName;
        Form_Preview formPreview;
        PROGRAM_SETTINGS Program_Settings;


        public Form1(string[] args)
        {
            InitializeComponent();

            Watch_Face_Preview_Set = new WATCH_FACE_PREWIEV_SET();
            Watch_Face_Preview_Set.Activity = new ActivityS();
            Watch_Face_Preview_Set.Date = new DateS();
            Watch_Face_Preview_Set.Status = new StatusS();
            Watch_Face_Preview_Set.Time = new TimeS();

            Watch_Face_Preview = new WATCH_FACE_PREWIEV();
            Watch_Face_Preview.Date = new DateP();
            Watch_Face_Preview.Date.Day = new TwoDigitsP();
            Watch_Face_Preview.Date.Month = new TwoDigitsP();

            Watch_Face_Preview.Time = new TimeP();
            Watch_Face_Preview.Time.Hours = new TwoDigitsP();
            Watch_Face_Preview.Time.Minutes = new TwoDigitsP();
            Watch_Face_Preview.Time.Seconds = new TwoDigitsP();

            Watch_Face_Preview.TimePm = new TimePmP();
            Watch_Face_Preview.TimePm.Hours = new TwoDigitsP();
            Watch_Face_Preview.TimePm.Minutes = new TwoDigitsP();
            Watch_Face_Preview.TimePm.Seconds = new TwoDigitsP();

            Program_Settings = new PROGRAM_SETTINGS();

            PreviewView = true;
            Settings_Load = false;


            if (args.Length == 1)
            {
                string fileName = args[0].ToString();
                if ((File.Exists(fileName)) && (Path.GetExtension(fileName)==".json"))
                {
                    //LoadJsonAndImage(fileName);
                    StartFileName = fileName;
                }
            }

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
            openFileDialog.Title = "Путь к файлу упаковщика";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //string filename = openFileDialog1.FileName;
                //string text = File.ReadAllText(filename);
                //richTextBox1.Text = text;
                textBox_pack_unpack_dir.Text = openFileDialog.FileName;

                Program_Settings.pack_unpack_dir = openFileDialog.FileName;
                string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                File.WriteAllText("Settings.json", JSON_String, Encoding.UTF8);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.pack_unpack_dir = textBox_pack_unpack_dir.Text;
            if (radioButton_47.Checked)
            {
                Properties.Settings.Default.unpack_command = textBox_unpack_command.Text;
                Properties.Settings.Default.pack_command = textBox_pack_command.Text;
            }
            else
            {
                Properties.Settings.Default.unpack_command_42 = textBox_unpack_command.Text;
                Properties.Settings.Default.pack_command_42 = textBox_pack_command.Text;
            }
            Properties.Settings.Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            helpProvider1.HelpNamespace = Application.StartupPath + @"\readme.chm";
#if Puthon
            string subPath = Application.StartupPath + @"\py_amazfit_tools-dev_gtr\main.py";
#else
            string subPath = Application.StartupPath + @"\main\main.exe";
#endif
            //textBox_pack_unpack_dir.Text = subPath;
            //if (Properties.Settings.Default.pack_unpack_dir.Length > 1)
            //    textBox_pack_unpack_dir.Text = Properties.Settings.Default.pack_unpack_dir;
            //if (Properties.Settings.Default.unpack_command.Length > 1)
            //    textBox_unpack_command.Text = Properties.Settings.Default.unpack_command;
            //if (Properties.Settings.Default.pack_command.Length > 1)
            //    textBox_pack_command.Text = Properties.Settings.Default.pack_command;

            try
            {
                Program_Settings = JsonConvert.DeserializeObject<PROGRAM_SETTINGS>
                    (File.ReadAllText("Settings.json"), new JsonSerializerSettings
                    {
                        //DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
            }
            catch (Exception)
            {

            }

            if (Program_Settings.pack_unpack_dir == null)
            {
                Program_Settings.pack_unpack_dir = subPath;
                string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                File.WriteAllText("Settings.json", JSON_String, Encoding.UTF8);
            }
            textBox_pack_unpack_dir.Text = Program_Settings.pack_unpack_dir;
            if (Program_Settings.Model_47)
            {
                radioButton_47.Checked = Program_Settings.Model_47;
                textBox_unpack_command.Text = Program_Settings.unpack_command_47;
                textBox_pack_command.Text = Program_Settings.pack_command_47;
            }
            else
            {
                radioButton_42.Checked = Program_Settings.Model_42;
                textBox_unpack_command.Text = Program_Settings.unpack_command_42;
                textBox_pack_command.Text = Program_Settings.pack_command_42;
            }

            PreviewView = false;
            checkBox_border.Checked = Program_Settings.ShowBorder;
            comboBox_MonthAndDayD_Alignment.Text = "Вверх влево";
            comboBox_MonthAndDayM_Alignment.Text = "Вверх влево";
            comboBox_OneLine_Alignment.Text = "Вверх влево";

            comboBox_ActivityGoal_Alignment.Text = "Вверх влево";
            comboBox_ActivitySteps_Alignment.Text = "Вверх влево";
            comboBox_ActivityDistance_Alignment.Text = "Вверх влево";
            comboBox_ActivityPuls_Alignment.Text = "Вверх влево";
            comboBox_ActivityCalories_Alignment.Text = "Вверх влево";
            comboBox_Battery_Text_Alignment.Text = "Вверх влево";

            comboBox_Weather_Text_Alignment.Text = "Вверх влево";
            comboBox_Weather_Day_Alignment.Text = "Вверх влево";
            comboBox_Weather_Night_Alignment.Text = "Вверх влево";

            comboBox_Battery_Flatness.Text = "Круглое";
            comboBox_StepsProgress_Flatness.Text = "Круглое";

            Version www = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            label_version.Text = "v " +
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
            Settings_Load = false;

            SetPreferences1();
            PreviewView = true;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if ((StartFileName != null) && (StartFileName.Length > 0)) LoadJsonAndImage(StartFileName);
        }

        private void Form1_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            //FormFileExists f = new FormFileExists();
            //f.ShowDialog();
            SendKeys.Send("{F1}");
            e.Cancel = true;
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
            openFileDialog.Title = "Путь к файлу циферблата";

#if !DEBUG
            if (!File.Exists(textBox_pack_unpack_dir.Text))
            {
                MessageBox.Show("Путь [" + textBox_pack_unpack_dir.Text +
                    "] к утилите распаковки/запаковки указан неверно.\r\n\r\n" +
                    "Укажите верный путь к утилите распаковки/запаковки.",
                    "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
#endif

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullfilename = openFileDialog.FileName;
                string filename = Path.GetFileName(fullfilename);
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
                            if (MessageBox.Show("Открыть распакованный проект?", "Открытие проекта",
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
            openFileDialog.Title = "Путь к файлу настроек циферблата";

#if !DEBUG
            if (!File.Exists(textBox_pack_unpack_dir.Text))
            {
                MessageBox.Show("Путь [" + textBox_pack_unpack_dir.Text +
                    "] к утилите распаковки/запаковки указан неверно.\r\n\r\n" +
                    "Укажите верный путь к утилите распаковки/запаковки.",
                    "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                        //if (radioButton_Settings_Pack_Dialog.Checked)
                        if (Program_Settings.Settings_Pack_Dialog)
                        {
                            if (MessageBox.Show("Перейти к файлу?", "Перейти к файлу",
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
            openFileDialog.Title = "Выбор файлов изображений";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.Rows.Clear();
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
                            dataGridView1.Rows.Add(RowNew);
                            ListImages.Add(i.ToString());
                            ListImagesFullName.Add(file);
                        }

                    }
                    catch
                    {
                        // Could not load the image - probably related to Windows file system permissions.
                        MessageBox.Show("Невозможно открыть изображение: " + file.Substring(file.LastIndexOf('\\'))
                            + ". У Вас нет прав на чтение файла, или изображение повреждено.");
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
            openFileDialog.Title = "Выбор файла настроек циферблата";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //string fullfilename = openFileDialog.FileName;
                LoadJsonAndImage(openFileDialog.FileName);
            }
        }

        private void LoadJsonAndImage(string fullfilename)
        {
            FileName = Path.GetFileName(fullfilename);
            FullFileDir = Path.GetDirectoryName(fullfilename);
            string text = File.ReadAllText(fullfilename);
            //richTextBox_JSON.Text = text;
            ListImages.Clear();
            ListImagesFullName.Clear();
            dataGridView1.Rows.Clear();

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
                        dataGridView1.Rows.Add(RowNew);
                        count++;
                        ListImages.Add(i.ToString());
                        ListImagesFullName.Add(file.FullName);
                    }
                }
                catch
                {
                    // Could not load the image - probably related to Windows file system permissions.
                    MessageBox.Show("Невозможно открыть изображение: " + file.FullName.Substring(file.FullName.LastIndexOf('\\'))
                        + ". У Вас нет прав на чтение файла, или изображение повреждено.");
                }
            }

            //loadedImage.Dispose();
            int LastImage = Int32.Parse(ListImages.Last()) + 1;
#if !DEBUG
            if (count != LastImage) MessageBox.Show("PNG файлы идут не по порядку или часть файлов отсутствует.\r\n" +
                 "Присвойте имена PNG файлам в порядке возрастания.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif

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

                MessageBox.Show("Неверный JSON файл.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            richTextBox_JSON.Text = JsonConvert.SerializeObject(Watch_Face, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });

            

            PreviewView = false;
            JSON_read();

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
                    if (MessageBox.Show("Загрузить имеющийся файл настроек предпросмотра PreviewStates.json?", 
                        "Загрузка настроек предпросмотра", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        JsonPreview_Read(newFullName);
                    }
                }
            }
            PreviewView = true;
            PreviewImage();
        }

        // заполняем поля с настройками из JSON файла
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
                        checkBoxSetText(comboBox_OneLine_Delimiter, Watch_Face.Date.MonthAndDay.OneLine.DelimiterImageIndex);
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
            }
            else
            {
                checkBox_Date.Checked = false;
                checkBox_WeekDay.Checked = false;
                checkBox_OneLine.Checked = false;
                checkBox_MonthAndDayD.Checked = false;
                checkBox_MonthAndDayM.Checked = false;
                checkBox_MonthName.Checked = false;
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

        // формируем изображение для предпросмотра
        private void PreviewImage()
        {
            if (!PreviewView) return;
            Graphics gPanel = panel_Preview.CreateGraphics();
            gPanel.Clear(panel_Preview.BackColor);
            float scale = 1.0f;
            if (panel_Preview.Height < 300) scale = 0.5f;
            PreviewToBitmap(gPanel, scale, radioButton_47.Checked, checkBox_WebW.Checked, checkBox_WebB.Checked, checkBox_border.Checked);
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
                PreviewToBitmap(gPanelPreview, scalePreview, radioButton_47.Checked, checkBox_WebW.Checked, 
                    checkBox_WebB.Checked, checkBox_border.Checked);
                gPanelPreview.Dispose();
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
        /// <param name="model_47">Модель 47мм</param>
        public void DrawAnalogClock(Graphics graphics, int x1, int y1, int offsetX, int offsetY, int image_index, float angle, bool model_47)
        {
            //graphics.RotateTransform(angle);
            var src = new Bitmap(ListImagesFullName[image_index]);
            //graphics.DrawImage(src, new Rectangle(227 - x1, 227 - y1, src.Width, src.Height));
            int offSet = 227;
            if (!model_47) offSet = 195;
            graphics.TranslateTransform(offSet + offsetX, offSet + offsetY);
            graphics.RotateTransform(angle);
            graphics.DrawImage(src, new Rectangle(-x1, -y1, src.Width, src.Height));
            graphics.RotateTransform(-angle);
            graphics.TranslateTransform(-offSet - offsetX, -offSet - offsetY);
            src.Dispose();
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
        /// <param name="BBorder">Рисовать рамку по координатам, вокруг элементов с выравниванием</param>
        public void DrawNumber(Graphics graphics, int x1, int y1, int x2, int y2, int image_index, int spacing,
            int alignment, double data_number, int suffix, int dec, bool BBorder)
        {
            data_number = Math.Round(data_number, 2);
            var Dagit = new Bitmap(ListImagesFullName[image_index]);
            //var Delimit = new Bitmap(1, 1);
            //if (dec >= 0) Delimit = new Bitmap(ListImagesFullName[dec]);
            string data_numberS = data_number.ToString();
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


            comboBox_ActivityGoal_Image.Text = "";
            comboBox_ActivityGoal_Image.Items.Clear();
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
            comboBox_ActivityCalories_Image.Text = "";
            comboBox_ActivityCalories_Image.Items.Clear();
            comboBox_ActivityStar_Image.Text = "";
            comboBox_ActivityStar_Image.Items.Clear();

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

#region выбираем данные для предпросмотра
        private void SetPreferences1()
        {
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
            Watch_Face_Preview.Date.Month.Tens = (int)Watch_Face_Preview_Set.Date.Month / 10;
            Watch_Face_Preview.Date.Month.Ones = Watch_Face_Preview_Set.Date.Month -
                Watch_Face_Preview.Date.Month.Tens * 10;
            Watch_Face_Preview.Date.Day.Tens = (int)Watch_Face_Preview_Set.Date.Day / 10;
            Watch_Face_Preview.Date.Day.Ones = Watch_Face_Preview_Set.Date.Day -
                Watch_Face_Preview.Date.Day.Tens * 10;

            Watch_Face_Preview.Time.Hours.Tens = (int)Watch_Face_Preview_Set.Time.Hours / 10;
            Watch_Face_Preview.Time.Hours.Ones = Watch_Face_Preview_Set.Time.Hours -
                Watch_Face_Preview.Time.Hours.Tens * 10;
            Watch_Face_Preview.Time.Minutes.Tens = (int)Watch_Face_Preview_Set.Time.Minutes / 10;
            Watch_Face_Preview.Time.Minutes.Ones = Watch_Face_Preview_Set.Time.Minutes -
                Watch_Face_Preview.Time.Minutes.Tens * 10;
            Watch_Face_Preview.Time.Seconds.Tens = (int)Watch_Face_Preview_Set.Time.Seconds / 10;
            Watch_Face_Preview.Time.Seconds.Ones = Watch_Face_Preview_Set.Time.Seconds -
                Watch_Face_Preview.Time.Seconds.Tens * 10;

            int hour = Watch_Face_Preview_Set.Time.Hours;
            if (Watch_Face_Preview_Set.Time.Hours > 12)
            {
                hour = hour - 12;
                Watch_Face_Preview.TimePm.Pm = true;
            }
            else
            {
                Watch_Face_Preview.TimePm.Pm = false;
            }
            Watch_Face_Preview.TimePm.Hours.Tens = hour / 10;
            Watch_Face_Preview.TimePm.Hours.Ones = hour - (int)Watch_Face_Preview.TimePm.Hours.Tens * 10;
            Watch_Face_Preview.TimePm.Minutes.Tens = (int)Watch_Face_Preview_Set.Time.Minutes / 10;
            Watch_Face_Preview.TimePm.Minutes.Ones = (int)Watch_Face_Preview_Set.Time.Minutes -
                (int)Watch_Face_Preview.TimePm.Minutes.Tens * 10;
            Watch_Face_Preview.TimePm.Seconds.Tens = (int)Watch_Face_Preview_Set.Time.Seconds / 10;
            Watch_Face_Preview.TimePm.Seconds.Ones = (int)Watch_Face_Preview_Set.Time.Seconds -
                Watch_Face_Preview.TimePm.Seconds.Tens * 10;
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
                panel_Background.Height = 30;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1; 
            }
            else panel_Background.Height = 1;
        }

        private void button_Time_Click(object sender, EventArgs e)
        {
            if (panel_Time.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 330;
                panel_Date.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
            }
            else panel_Time.Height = 1;
        }

        private void button_Date_Click(object sender, EventArgs e)
        {
            if (panel_Date.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 350;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
            }
            else panel_Date.Height = 1;
        }

        private void button_StepsProgress_Click(object sender, EventArgs e)
        {
            if (panel_StepsProgress.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_StepsProgress.Height = 105;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
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
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 190;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
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
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 82;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
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
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 155;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
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
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 193;
                panel_Weather.Height = 1;
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
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 230;
            }
            else panel_Weather.Height = 1;
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

        private void checkBox_Activity_CheckedChanged(object sender, EventArgs e)
        {
            tabControl_Activity.Enabled = checkBox_Activity.Checked;
        }

        private void checkBox_CircleScale_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_StepsProgress.Checked;
            numericUpDown_StepsProgress_Center_X.Enabled = b;
            numericUpDown_StepsProgress_Center_Y.Enabled = b;
            numericUpDown_StepsProgress_Radius_X.Enabled = b;
            //numericUpDown_StepsProgress_Radius_Y.Enabled = b;
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

        private void checkBox_ActivityGoal_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivityGoal.Checked;
            numericUpDown_ActivityGoal_StartCorner_X.Enabled = b;
            numericUpDown_ActivityGoal_StartCorner_Y.Enabled = b;
            numericUpDown_ActivityGoal_EndCorner_X.Enabled = b;
            numericUpDown_ActivityGoal_EndCorner_Y.Enabled = b;

            comboBox_ActivityGoal_Image.Enabled = b;
            numericUpDown_ActivityGoal_Count.Enabled = b;
            numericUpDown_ActivityGoal_Spacing.Enabled = b;
            comboBox_ActivityGoal_Alignment.Enabled = b;

            label112.Enabled = b;
            label113.Enabled = b;
            label114.Enabled = b;
            label115.Enabled = b;
            label116.Enabled = b;
            label117.Enabled = b;
            label118.Enabled = b;
            label119.Enabled = b;
            label120.Enabled = b;
            label121.Enabled = b;
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
            //numericUpDown_Battery_Scale_Radius_Y.Enabled = b;

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
#endregion

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)(sender);
            //if (comboBox.Name != "comboBox_WeatherSet_Icon")
            //{
            //    try
            //    {
            //        using (FileStream stream = new FileStream(ListImagesFullName[comboBox.SelectedIndex], FileMode.Open, FileAccess.Read))
            //        {
            //            pictureBox1.Image = Image.FromStream(stream);
            //            timer1.Enabled = true;
            //        }
            //    }
            //    catch { }
            //}
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

        // формируем JSON файл из настроек
        private void JSON_write()
        {
            if (!PreviewView) return;
            Watch_Face = new WATCH_FACE_JSON();
            if ((comboBox_Background.SelectedIndex>=0)||(comboBox_Preview.SelectedIndex >= 0))
            {
                Watch_Face.Background = new Background();
                if(comboBox_Background.SelectedIndex >= 0)
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
                    if (Watch_Face.Date.MonthAndDay == null) Watch_Face.Date.MonthAndDay = new Monthandday();
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
                    if (Watch_Face.Date.MonthAndDay == null) Watch_Face.Date.MonthAndDay = new Monthandday();
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
                    if (Watch_Face.Date.MonthAndDay == null) Watch_Face.Date.MonthAndDay = new Monthandday();
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
                    if (Watch_Face.Date.MonthAndDay == null) Watch_Face.Date.MonthAndDay = new Monthandday();
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
                if (comboBox_Bluetooth_Off.SelectedIndex>=0)
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
                dataGridView1.DefaultCellStyle.BackColor = Color.Black;
                dataGridView1.DefaultCellStyle.ForeColor = Color.White;
            }
            else
            {
                dataGridView1.DefaultCellStyle.BackColor = Color.White;
                dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void comboBox_StepsProgress_Color_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            comboBox_StepsProgress_Color.BackColor = colorDialog1.Color; PreviewImage();
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
            if (colorDialog2.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            comboBox_Battery_Scale_Color.BackColor = colorDialog2.Color;
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
                    Form_Preview.Model_47.model_47 = radioButton_47.Checked;
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
                    File.WriteAllText("Settings.json", JSON_String, Encoding.UTF8);

                    PreviewToBitmap(gPanelPreviewResize, scalePreviewResize, radioButton_47.Checked, 
                        checkBox_WebW.Checked, checkBox_WebB.Checked, checkBox_border.Checked);
                    gPanelPreviewResize.Dispose();
                };

                formPreview.panel_Preview.Paint += (object senderPaint, PaintEventArgs ePaint) =>
                {
                    //Form_Preview.Model_47.model_47 = radioButton_47.Checked;
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

            Form_Preview.Model_47.model_47 = radioButton_47.Checked;
            Graphics gPanel = formPreview.panel_Preview.CreateGraphics();
            gPanel.Clear(panel_Preview.BackColor);
            //Pen pen = new Pen(Color.Blue, 1);
            //Random rnd = new Random();
            //gPanel.DrawLine(pen, new Point(0, 0), new Point(rnd.Next(0, 450), rnd.Next(0, 450)));
            //Form_Preview.Model_47.model_47 = radioButton_47.Checked;
            formPreview.radioButton_CheckedChanged(sender, e);
            float scale = 1.0f;
            if (formPreview.radioButton_small.Checked) scale = 0.5f;
            if (formPreview.radioButton_large.Checked) scale = 1.5f;
            if (formPreview.radioButton_xlarge.Checked) scale = 2.0f;
            if (formPreview.radioButton_xxlarge.Checked) scale = 2.5f;

            PreviewToBitmap(gPanel, scale, radioButton_47.Checked, checkBox_WebW.Checked, checkBox_WebB.Checked, checkBox_border.Checked);
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
            openFileDialog.Title = "Выбор файла настроек циферблата";
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
            ClassPreview ps = new ClassPreview();
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
                    ps = JsonConvert.DeserializeObject<ClassPreview>(objson[i].ToString(), new JsonSerializerSettings
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

                MessageBox.Show("Ошибка чтения JSON файла.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                ClassPreview ps = new ClassPreview();
                ps.Time = new TimePreview();
                switch (i)
                {
                    case 0:
                        ps.Time.Year = dateTimePicker_Date_Set1.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set1.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set1.Value.Day;
                        ps.Time.Hour = dateTimePicker_Date_Set1.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Date_Set1.Value.Minute;
                        ps.Time.Second = dateTimePicker_Date_Set1.Value.Second;
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
                        ps.Time.Hour = dateTimePicker_Date_Set2.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Date_Set2.Value.Minute;
                        ps.Time.Second = dateTimePicker_Date_Set2.Value.Second;
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
                        ps.Time.Hour = dateTimePicker_Date_Set3.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Date_Set3.Value.Minute;
                        ps.Time.Second = dateTimePicker_Date_Set3.Value.Second;
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
                        ps.Time.Hour = dateTimePicker_Date_Set4.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Date_Set4.Value.Minute;
                        ps.Time.Second = dateTimePicker_Date_Set4.Value.Second;
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
                        ps.Time.Hour = dateTimePicker_Date_Set5.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Date_Set5.Value.Minute;
                        ps.Time.Second = dateTimePicker_Date_Set5.Value.Second;
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
                        ps.Time.Hour = dateTimePicker_Date_Set6.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Date_Set6.Value.Minute;
                        ps.Time.Second = dateTimePicker_Date_Set6.Value.Second;
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
                        ps.Time.Hour = dateTimePicker_Date_Set7.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Date_Set7.Value.Minute;
                        ps.Time.Second = dateTimePicker_Date_Set7.Value.Second;
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
                        ps.Time.Hour = dateTimePicker_Date_Set8.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Date_Set8.Value.Minute;
                        ps.Time.Second = dateTimePicker_Date_Set8.Value.Second;
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
                        ps.Time.Hour = dateTimePicker_Date_Set9.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Date_Set9.Value.Minute;
                        ps.Time.Second = dateTimePicker_Date_Set9.Value.Second;
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
                        ps.Time.Hour = dateTimePicker_Date_Set10.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Date_Set10.Value.Minute;
                        ps.Time.Second = dateTimePicker_Date_Set10.Value.Second;
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
                        ps.Time.Hour = dateTimePicker_Date_Set11.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Date_Set11.Value.Minute;
                        ps.Time.Second = dateTimePicker_Date_Set11.Value.Second;
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
                        ps.Time.Hour = dateTimePicker_Date_Set12.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Date_Set12.Value.Minute;
                        ps.Time.Second = dateTimePicker_Date_Set12.Value.Second;
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
                        ps.Time.Hour = dateTimePicker_Date_Set13.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Date_Set13.Value.Minute;
                        ps.Time.Second = dateTimePicker_Date_Set13.Value.Second;
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
                MessageBox.Show("Для сохранения предустановленных параметром установите хотябы в одном поле" +
                    " 'Калории' значение отличное от '1234'.");
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
            saveFileDialog.Title = "Выбор файла настроек циферблата";
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

                MessageBox.Show("Неверный JSON файл.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            saveFileDialog.Title = "Выбор файла настроек циферблата";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullfilename = saveFileDialog.FileName;
                File.WriteAllText(fullfilename, richTextBox_JSON.Text, Encoding.UTF8);

                FileName = Path.GetFileName(fullfilename);
                FullFileDir = Path.GetDirectoryName(fullfilename);
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
                if (radioButton_47.Checked) button_PreviewSmall.PerformClick();
                else button_PreviewSmall_42.PerformClick();
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
            saveFileDialog.Title = "Сохранить предпросмотр";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bitmap = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
                if(radioButton_42.Checked) bitmap = new Bitmap(Convert.ToInt32(390), Convert.ToInt32(390), PixelFormat.Format32bppArgb);
                Graphics gPanel = Graphics.FromImage(bitmap);
                //float scale = 1.0f;
                //if (formPreview.radioButton_small.Checked) scale = 0.5f;
                //if (formPreview.radioButton_large.Checked) scale = 1.5f;
                //if (formPreview.radioButton_xlarge.Checked) scale = 2.0f;
                //if (formPreview.radioButton_xxlarge.Checked) scale = 2.5f;
                PreviewToBitmap(gPanel, 1.0f, radioButton_47.Checked, false, false, false);
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
            saveFileDialog.Title = "Сохранить анимированный предпросмотр";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bitmap = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
                if (radioButton_42.Checked) bitmap = new Bitmap(Convert.ToInt32(390), Convert.ToInt32(390), PixelFormat.Format32bppArgb);
                Graphics gPanel = Graphics.FromImage(bitmap);
                bool save = false;
                Random rnd = new Random();
                PreviewView = false;

                using (MagickImageCollection collection = new MagickImageCollection())
                {
                    for (int i = 0; i < 13; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                button_Set1.PerformClick();
                                save = true;
                                break;
                            case 1:
                                if (numericUpDown_Calories_Set2.Value != 1234)
                                {
                                    button_Set2.PerformClick();
                                    save = true;
                                }
                                break;
                            case 2:
                                if (numericUpDown_Calories_Set3.Value != 1234)
                                {
                                    button_Set3.PerformClick();
                                    save = true;
                                }
                                break;
                            case 3:
                                if (numericUpDown_Calories_Set4.Value != 1234)
                                {
                                    button_Set4.PerformClick();
                                    save = true;
                                }
                                break;
                            case 4:
                                if (numericUpDown_Calories_Set5.Value != 1234)
                                {
                                    button_Set5.PerformClick();
                                    save = true;
                                }
                                break;
                            case 5:
                                if (numericUpDown_Calories_Set6.Value != 1234)
                                {
                                    button_Set6.PerformClick();
                                    save = true;
                                }
                                break;
                            case 6:
                                if (numericUpDown_Calories_Set7.Value != 1234)
                                {
                                    button_Set7.PerformClick();
                                    save = true;
                                }
                                break;
                            case 7:
                                if (numericUpDown_Calories_Set8.Value != 1234)
                                {
                                    button_Set8.PerformClick();
                                    save = true;
                                }
                                break;
                            case 8:
                                if (numericUpDown_Calories_Set9.Value != 1234)
                                {
                                    button_Set9.PerformClick();
                                    save = true;
                                }
                                break;
                            case 9:
                                if (numericUpDown_Calories_Set10.Value != 1234)
                                {
                                    button_Set10.PerformClick();
                                    save = true;
                                }
                                break;
                            case 10:
                                if (numericUpDown_Calories_Set11.Value != 1234)
                                {
                                    button_Set11.PerformClick();
                                    save = true;
                                }
                                break;
                            case 11:
                                if (numericUpDown_Calories_Set10.Value != 1234)
                                {
                                    button_Set12.PerformClick();
                                    save = true;
                                }
                                break;
                            case 12:
                                if (numericUpDown_Calories_Set10.Value != 1234)
                                {
                                    button_Set12.PerformClick();
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

                            PreviewToBitmap(gPanel, 1.0f, radioButton_47.Checked, false, false, false);
                            // Add first image and set the animation delay to 100ms
                            MagickImage item = new MagickImage(bitmap);
                            collection.Add(item);
                            collection[collection.Count - 1].AnimationDelay = 100;

                            numericUpDown_WeatherSet_Temp.Value = WeatherSet_Temp;
                            numericUpDown_WeatherSet_DayTemp.Value = WeatherSet_DayTemp;
                            numericUpDown_WeatherSet_NightTemp.Value = WeatherSet_NightTemp;
                            comboBox_WeatherSet_Icon.SelectedIndex = WeatherSet_Icon;
                        }
                    }



                    // Optionally reduce colors
                    //QuantizeSettings settings = new QuantizeSettings();
                    //settings.Colors = 256;
                    //collection.Quantize(settings);

                    // Optionally optimize the images (images should have the same size).
                    collection.OptimizeTransparency();
                    //collection.Optimize();

                    // Save gif
                    collection.Write(saveFileDialog.FileName);
                }
                PreviewView = true;
            }
        }


        /// <summary>формируем изображение на панедли Graphics</summary>
        /// <param name="gPanel">Поверхность для рисования</param>
        /// <param name="scale">Масштаб прорисовки</param>
        /// <param name="model_47">Модель 47мм</param>
        /// <param name="WMesh">Рисовать белую сетку</param>
        /// <param name="BMesh">Рисовать черную сетку</param>
        /// <param name="BBorder">Рисовать рамку по координатам, вокруг элементов с выравниванием</param>
        private void PreviewToBitmap(Graphics gPanel, float scale, bool model_47, bool WMesh, bool BMesh, bool BBorder)
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
                            i = comboBox_Hours_Tens_Image.SelectedIndex + Watch_Face_Preview.TimePm.Hours.Tens;
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
                            i = comboBox_Hours_Ones_Image.SelectedIndex + Watch_Face_Preview.TimePm.Hours.Ones;
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
                            i = comboBox_Min_Tens_Image.SelectedIndex + Watch_Face_Preview.TimePm.Minutes.Tens;
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
                            i = comboBox_Min_Ones_Image.SelectedIndex + Watch_Face_Preview.TimePm.Minutes.Ones;
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
                            i = comboBox_Sec_Tens_Image.SelectedIndex + Watch_Face_Preview.TimePm.Seconds.Tens;
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
                            i = comboBox_Sec_Ones_Image.SelectedIndex + Watch_Face_Preview.TimePm.Seconds.Ones;
                            if (i < ListImagesFullName.Count)
                            {
                                src = new Bitmap(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Sec_Ones_X.Value,
                                    (int)numericUpDown_Sec_Ones_Y.Value, src.Width, src.Height));
                                src.Dispose(); 
                            }
                        }
                    }

                    if (Watch_Face_Preview.TimePm.Pm)
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
                            i = comboBox_Hours_Tens_Image.SelectedIndex + Watch_Face_Preview.Time.Hours.Tens;
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
                            i = comboBox_Hours_Ones_Image.SelectedIndex + Watch_Face_Preview.Time.Hours.Ones;
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
                            i = comboBox_Min_Tens_Image.SelectedIndex + Watch_Face_Preview.Time.Minutes.Tens;
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
                            i = comboBox_Min_Ones_Image.SelectedIndex + Watch_Face_Preview.Time.Minutes.Ones;
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
                            i = comboBox_Sec_Tens_Image.SelectedIndex + Watch_Face_Preview.Time.Seconds.Tens;
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
                            i = comboBox_Sec_Ones_Image.SelectedIndex + Watch_Face_Preview.Time.Seconds.Ones;
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
                    if ((checkBox_TwoDigitsMonth.Checked) || (Watch_Face_Preview.Date.Month.Tens > 0))
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

                    if ((checkBox_TwoDigitsMonth.Checked) || (Watch_Face_Preview.Date.Month.Tens > 0))
                    {
                        i = comboBox_MonthAndDayM_Image.SelectedIndex + Watch_Face_Preview.Date.Month.Tens;
                        src = new Bitmap(ListImagesFullName[i]);
                        gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + Dagit.Width + (int)numericUpDown_MonthAndDayM_Spacing.Value;
                        src.Dispose();
                    }
                    i = comboBox_MonthAndDayM_Image.SelectedIndex + Watch_Face_Preview.Date.Month.Ones;
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
                    if ((checkBox_TwoDigitsDay.Checked) || (Watch_Face_Preview.Date.Day.Tens > 0))
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

                    if ((checkBox_TwoDigitsDay.Checked) || (Watch_Face_Preview.Date.Day.Tens > 0))
                    {
                        i = comboBox_MonthAndDayD_Image.SelectedIndex + Watch_Face_Preview.Date.Day.Tens;
                        src = new Bitmap(ListImagesFullName[i]);
                        gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + Dagit.Width + (int)numericUpDown_MonthAndDayD_Spacing.Value;
                        src.Dispose();
                    }
                    i = comboBox_MonthAndDayD_Image.SelectedIndex + Watch_Face_Preview.Date.Day.Ones;
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
                    if ((!checkBox_TwoDigitsMonth.Checked) && (Watch_Face_Preview.Date.Month.Tens == 0))
                        DateLenght = DateLenght - Dagit.Width - (int)numericUpDown_OneLine_Spacing.Value;
                    if ((!checkBox_TwoDigitsDay.Checked) && (Watch_Face_Preview.Date.Day.Tens == 0))
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

                    if ((checkBox_TwoDigitsMonth.Checked) || (Watch_Face_Preview.Date.Month.Tens > 0))
                    {
                        i = comboBox_OneLine_Image.SelectedIndex + Watch_Face_Preview.Date.Month.Tens;
                        src = new Bitmap(ListImagesFullName[i]);
                        gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + Dagit.Width + (int)numericUpDown_OneLine_Spacing.Value;
                        src.Dispose();
                    }
                    i = comboBox_OneLine_Image.SelectedIndex + Watch_Face_Preview.Date.Month.Ones;
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

                    if ((checkBox_TwoDigitsDay.Checked) || (Watch_Face_Preview.Date.Day.Tens > 0))
                    {
                        i = comboBox_OneLine_Image.SelectedIndex + Watch_Face_Preview.Date.Day.Tens;
                        src = new Bitmap(ListImagesFullName[i]);
                        gPanel.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + Dagit.Width + (int)numericUpDown_OneLine_Spacing.Value;
                    }
                    i = comboBox_OneLine_Image.SelectedIndex + Watch_Face_Preview.Date.Day.Ones;
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
            }
#endregion

#region Weather
            if (checkBox_Weather.Checked)
            {
                if ((checkBox_Weather_Icon.Checked) && (comboBox_Weather_Icon_Image.SelectedIndex >= 0))
                {
                    //int x1 = (int)numericUpDown_ActivityDistance_StartCorner_X.Value;
                    //int y1 = (int)numericUpDown_ActivityDistance_StartCorner_Y.Value;
                    //int x2 = (int)numericUpDown_ActivityDistance_EndCorner_X.Value;
                    //int y2 = (int)numericUpDown_ActivityDistance_EndCorner_Y.Value;
                    //int image_index = comboBox_ActivityDistance_Image.SelectedIndex;
                    //int spacing = (int)numericUpDown_ActivityDistance_Spacing.Value;
                    //int alignment = comboBox_ActivityDistance_Alignment.SelectedIndex;
                    //double data_number = Watch_Face_Preview_Set.Activity.Distance / 1000.0;
                    //int suffix = comboBox_ActivityDistance_Suffix.SelectedIndex;
                    //int dec = comboBox_ActivityDistance_Decimal.SelectedIndex;
                    //DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, suffix, dec);

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
                    int x2 = (int)numericUpDown_Weather_Text_EndCorner_X.Value+1;
                    int y2 = (int)numericUpDown_Weather_Text_EndCorner_Y.Value+1;
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
                    int x2 = (int)numericUpDown_Weather_Day_EndCorner_X.Value+1;
                    int y2 = (int)numericUpDown_Weather_Day_EndCorner_Y.Value+1;
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
                    int x2 = (int)numericUpDown_Weather_Night_EndCorner_X.Value+1;
                    int y2 = (int)numericUpDown_Weather_Night_EndCorner_Y.Value+1;
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
                switch (comboBox_StepsProgress_Flatness.Text)
                {
                    case "Треугольное":
                        pen.EndCap = LineCap.Triangle;
                        pen.StartCap = LineCap.Triangle;
                        break;
                    case "Плоское":
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
#endregion

            if (scale != 0.5) gPanel.SmoothingMode = SmoothingMode.Default;

#region Activity
            if (checkBox_Activity.Checked)
            {
                if ((checkBox_ActivityGoal.Checked) && (comboBox_ActivityGoal_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_ActivityGoal_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_ActivityGoal_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_ActivityGoal_EndCorner_X.Value;
                    int y2 = (int)numericUpDown_ActivityGoal_EndCorner_Y.Value;
                    x2++;
                    y2++;
                    int image_index = comboBox_ActivityGoal_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_ActivityGoal_Spacing.Value;
                    int alignment = comboBox_ActivityGoal_Alignment.SelectedIndex;
                    int data_number = Watch_Face_Preview_Set.Activity.StepsGoal;
                    DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, BBorder);
                }

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
                    DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, BBorder);
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
                    double data_number = Watch_Face_Preview_Set.Activity.Distance / 1000.0;
                    int suffix = comboBox_ActivityDistance_Suffix.SelectedIndex;
                    int dec = comboBox_ActivityDistance_Decimal.SelectedIndex;
                    DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, suffix, dec, BBorder);
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
                    DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, BBorder);
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
                    DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, BBorder);
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
                        DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, BBorder);
                    }

                    if ((checkBox_Battery_Percent.Checked) && (comboBox_Battery_Percent_Image.SelectedIndex >= 0))
                    {
                        src = new Bitmap(ListImagesFullName[comboBox_Battery_Percent_Image.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Battery_Percent_X.Value,
                            (int)numericUpDown_Battery_Percent_Y.Value, src.Width, src.Height));
                        src.Dispose();
                    }

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

                    if (checkBox_Battery_Scale.Checked)
                    {
                        gPanel.SmoothingMode = SmoothingMode.AntiAlias;
                        Pen pen = new Pen(comboBox_Battery_Scale_Color.BackColor,
                            (float)numericUpDown_Battery_Scale_Width.Value);


                        //pen.EndCap = LineCap.Flat;
                        //pen.StartCap = LineCap.Triangle;
                        switch (comboBox_Battery_Flatness.Text)
                        {
                            case "Треугольное":
                                pen.EndCap = LineCap.Triangle;
                                pen.StartCap = LineCap.Triangle;
                                break;
                            case "Плоское":
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
                        DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle, model_47);
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
                    DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle, model_47);
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
                    DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle, model_47);
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
                        DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle, model_47);
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
            int center = 227;
            if (!model_47) center = 195;

            if (WMesh)
            {
                Pen pen = new Pen(Color.White, 1);
                int LineDistance = 30;
                if (panel_Preview.Height > 300) LineDistance = 15;
                for (i = 0; i < 30; i++)
                {
                    gPanel.DrawLine(pen, new Point(center + i * LineDistance, 0), new Point(center + i * LineDistance, 454));
                    gPanel.DrawLine(pen, new Point(center - i * LineDistance, 0), new Point(center - i * LineDistance, 454));

                    gPanel.DrawLine(pen, new Point(0, center + i * LineDistance), new Point(454, center + i * LineDistance));
                    gPanel.DrawLine(pen, new Point(0, center - i * LineDistance), new Point(454, center - i * LineDistance));
                }
            }

            if (BMesh)
            {
                Pen pen = new Pen(Color.Black, 1);
                int LineDistance = 30;
                if (panel_Preview.Height > 300) LineDistance = 15;
                for (i = 0; i < 30; i++)
                {
                    gPanel.DrawLine(pen, new Point(center + i * LineDistance, 0), new Point(center + i * LineDistance, 454));
                    gPanel.DrawLine(pen, new Point(center - i * LineDistance, 0), new Point(center - i * LineDistance, 454));

                    gPanel.DrawLine(pen, new Point(0, center + i * LineDistance), new Point(454, center + i * LineDistance));
                    gPanel.DrawLine(pen, new Point(0, center - i * LineDistance), new Point(454, center - i * LineDistance));
                }
            }
#endregion

            src.Dispose();
        }

        private void radioButton_47_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_47.Checked)
            {
                panel_Preview.Height = 230;
                panel_Preview.Width = 230;

                //Properties.Settings.Default.unpack_command_42 = textBox_unpack_command.Text;
                //Properties.Settings.Default.pack_command_42 = textBox_pack_command.Text;
                //Properties.Settings.Default.Save();
                Program_Settings.unpack_command_42 = textBox_unpack_command.Text;
                Program_Settings.pack_command_42 = textBox_pack_command.Text;

                //textBox_unpack_command.Text = "--gtr 47 --file";
                //textBox_pack_command.Text = "--gtr 47 --file";
                //if (Properties.Settings.Default.unpack_command.Length > 1)
                //    textBox_unpack_command.Text = Properties.Settings.Default.unpack_command;
                //if (Properties.Settings.Default.pack_command.Length > 1)
                //    textBox_pack_command.Text = Properties.Settings.Default.pack_command;
                textBox_unpack_command.Text = Program_Settings.unpack_command_47;
                textBox_pack_command.Text = Program_Settings.pack_command_47;
            }
            else
            {
                panel_Preview.Height = 198;
                panel_Preview.Width = 198;

                //Properties.Settings.Default.unpack_command = textBox_unpack_command.Text;
                //Properties.Settings.Default.pack_command = textBox_pack_command.Text;
                //Properties.Settings.Default.Save();
                Program_Settings.unpack_command_47 = textBox_unpack_command.Text;
                Program_Settings.pack_command_47 = textBox_pack_command.Text;

                //textBox_unpack_command.Text = "--gtr 42 --file";
                //textBox_pack_command.Text = "--gtr 42 --file";
                //if (Properties.Settings.Default.unpack_command_42.Length > 1)
                //    textBox_unpack_command.Text = Properties.Settings.Default.unpack_command_42;
                //if (Properties.Settings.Default.pack_command_42.Length > 1)
                //    textBox_pack_command.Text = Properties.Settings.Default.pack_command_42;
                textBox_unpack_command.Text = Program_Settings.unpack_command_42;
                textBox_pack_command.Text = Program_Settings.pack_command_42;
            }

            if ((formPreview != null) && (formPreview.Visible))
            {
                Form_Preview.Model_47.model_47 = radioButton_47.Checked;
                formPreview.radioButton_CheckedChanged(sender, e);
            }

            Program_Settings.Model_47 = radioButton_47.Checked;
            Program_Settings.Model_42 = radioButton_42.Checked;
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText("Settings.json", JSON_String, Encoding.UTF8);

        PreviewImage();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            if ((formPreview != null) && (formPreview.Visible))
            {
                Form_Preview.Model_47.model_47 = radioButton_47.Checked;
                Graphics gPanelPreviewPaint = formPreview.panel_Preview.CreateGraphics();
                gPanelPreviewPaint.Clear(panel_Preview.BackColor);
                formPreview.radioButton_CheckedChanged(sender, e);
                float scalePreviewPaint = 1.0f;
                if (formPreview.radioButton_small.Checked) scalePreviewPaint = 0.5f;
                if (formPreview.radioButton_large.Checked) scalePreviewPaint = 1.5f;
                if (formPreview.radioButton_xlarge.Checked) scalePreviewPaint = 2.0f;
                if (formPreview.radioButton_xxlarge.Checked) scalePreviewPaint = 2.5f;
                PreviewToBitmap(gPanelPreviewPaint, scalePreviewPaint, radioButton_47.Checked, 
                    checkBox_WebW.Checked, checkBox_WebB.Checked, checkBox_border.Checked);
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
            File.WriteAllText("Settings.json", JSON_String, Encoding.UTF8);
            //File.WriteAllText(fullfilename, richTextBox_JSON.Text, Encoding.UTF8);
        }

        private void checkBox_border_CheckedChanged(object sender, EventArgs e)
        {
            Program_Settings.ShowBorder = checkBox_border.Checked;
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText("Settings.json", JSON_String, Encoding.UTF8);
        }

        private void textBox_unpack_command_Leave(object sender, EventArgs e)
        {
            if (radioButton_47.Checked)
            {
                Program_Settings.unpack_command_47 = textBox_unpack_command.Text;
                Program_Settings.pack_command_47 = textBox_pack_command.Text;
            }
            else
            {
                Program_Settings.unpack_command_42 = textBox_unpack_command.Text;
                Program_Settings.pack_command_42 = textBox_pack_command.Text;
            }

            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText("Settings.json", JSON_String, Encoding.UTF8);
        }

        private void comboBox_Image_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int www = comboBox.SelectedIndex;
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
            //e.ItemWidth = 560;
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

public class PROGRAM_SETTINGS
{
    public bool Settings_Unpack_Dialog = true;
    public bool Settings_Unpack_Save = false;
    public bool Settings_Unpack_Replace = false;
    
    public bool Settings_Pack_Dialog = false;
    public bool Settings_Pack_GoToFile = true;
    public bool Settings_Pack_Copy = false;
    public bool Settings_Pack_DoNotning = false;

    public bool Settings_AfterUnpack_Dialog = false;
    public bool Settings_AfterUnpack_Download = true;
    public bool Settings_AfterUnpack_DoNothing = false;

    public bool Settings_Open_Dialog = false;
    public bool Settings_Open_Download = true;
    public bool Settings_Open_DoNotning = false;

    public bool Model_47 = true;
    public bool Model_42 = false;

    public bool ShowBorder = false;
    public float Scale = 1f;

    public string pack_unpack_dir { get; set; }
    public string unpack_command_47 = "--gtr 47 --file";
    public string pack_command_47 = "--gtr 47 --file";
    public string unpack_command_42 = "--gtr 42 --file";
    public string pack_command_42 = "--gtr 42 --file";
}




#region WATCH_FACE_PREWIEV
/// <summary>отдельные цифры для даты и времени</summary>
public class WATCH_FACE_PREWIEV
    {
        public DateP Date { get; set; }
        public TimeP Time { get; set; }
        public TimePmP TimePm { get; set; }
    }

    public class DateP
    {
        public TwoDigitsP Month { get; set; }
        public TwoDigitsP Day { get; set; }
    }

    public class TimeP
    {
        public TwoDigitsP Hours { get; set; }
        public TwoDigitsP Minutes { get; set; }
        public TwoDigitsP Seconds { get; set; }
    }

    public class TimePmP
    {
        public TwoDigitsP Hours { get; set; }
        public TwoDigitsP Minutes { get; set; }
        public TwoDigitsP Seconds { get; set; }
        public bool Pm { get; set; }
    }

    public class TwoDigitsP
    {
        public int Tens { get; set; }
        public int Ones { get; set; }
    }
#endregion

#region WATCH_FACE_PREWIEV_SET
    /// <summary>набор настроек для предпросмотра</summary>
    public class WATCH_FACE_PREWIEV_SET
    {
        public DateS Date { get; set; }
        public TimeS Time { get; set; }
        public ActivityS Activity { get; set; }
        public StatusS Status { get; set; }
        public int Battery { get; set; }
    }

    public class DateS
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int WeekDay { get; set; }
    }

    public class TimeS
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }

    public class ActivityS
    {
        public int Calories { get; set; }
        public int Pulse { get; set; }
        public int Distance { get; set; }
        public int Steps { get; set; }
        public int StepsGoal { get; set; }
    }

    public class StatusS
    {
        public bool Bluetooth { get; set; }
        public bool Alarm { get; set; }
        public bool Lock { get; set; }
        public bool DoNotDisturb { get; set; }
    }
#endregion

#region PreviewStates
    public class PREWIEV_STATES
    {
        public ClassPreview[] Property1 { get; set; }
    }

    public class ClassPreview
    {
        public TimePreview Time { get; set; }
        public int Steps { get; set; }
        public int Goal { get; set; }
        public int Pulse { get; set; }
        public int BatteryLevel { get; set; }
        public int Distance { get; set; }
        public int Calories { get; set; }
        public bool Bluetooth { get; set; }
        public bool Unlocked { get; set; }
        public bool Alarm { get; set; }
        public bool DoNotDisturb { get; set; }
    }

    public class TimePreview
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
    }
#endregion

