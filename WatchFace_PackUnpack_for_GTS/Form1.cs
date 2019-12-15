using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WatchFace_PackUnpack_for_GTS
{
    public partial class Form1 : Form
    {
        //static bool Is64Bit
        //{

        //    get { return Marshal.SizeOf(typeof(IntPtr)) == 8; }
        //}

        public Form1()
        {
            InitializeComponent();
        }

        private void button_unpack_Click(object sender, EventArgs e)
        {
            // если копируем в папку
            if (checkBox_Watchface_Path.Checked)
            {
                string subPath = Application.StartupPath + @"\Watch_face\";
                if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);
                string _pack_unpack_dir = Application.StartupPath + @"\Res_Packer_Unpacker\x86_64\resunpacker.exe";
                if (radioButton_32.Checked)
                    _pack_unpack_dir = Application.StartupPath + @"\Res_Packer_Unpacker\x86\resunpacker.exe";

                OpenFileDialog openFileDialog = new OpenFileDialog();
                //openFileDialog.Filter = "Json files (*.json) | *.json";
                openFileDialog.Filter = "Binary File (*.bin)|*.bin";
                ////openFileDialog1.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = false;
                openFileDialog.Title = "Путь к файлу циферблата";

                if (!File.Exists(_pack_unpack_dir))
                {
                    MessageBox.Show("Путь [" + _pack_unpack_dir +
                        "] к утилите распаковки/запаковки указан неверно.\r\n\r\n" +
                        "Отсутствуют необходимые компоненты программы.",
                        "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fullfilename = openFileDialog.FileName;
                    string filename = Path.GetFileName(fullfilename);
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

                    //Process _process = new Process();
                    //ProcessStartInfo startInfo = new ProcessStartInfo();
                    //startInfo.FileName = _pack_unpack_dir;
                    //string _pack_unpack_command = "--gtr 47 --file";
                    ////if (radioButton_42.Checked) _pack_unpack_command = "--gtr 42 --file";
                    //startInfo.Arguments = _pack_unpack_command + " " + fullPath;
                    //_process.StartInfo = startInfo;
                    //_process.Start();

                    try
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = _pack_unpack_dir;
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
                        string newFullName_bin = Path.Combine(path, fileNameOnly + ".unp.bin");

                        //MessageBox.Show(newFullName);
                        if (File.Exists(newFullName_unp))
                        {
                            File.Copy(newFullName_unp, newFullName_bin, true);
                            //this.BringToFront();
                            //Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName_bin));
                            if (File.Exists(newFullName_bin))
                            {
                                //_pack_unpack_dir = Application.StartupPath + @"\Tool-Wf-GTS\main.py";
                                _pack_unpack_dir = Application.StartupPath + @"\main\main.exe";
                                startInfo.FileName = _pack_unpack_dir;
                                startInfo.Arguments = "--gts --file " + newFullName_bin;
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

                                if (File.Exists(newFullName))
                                {
                                    Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName));
                                }
                            }
                        }
                    }
                    catch
                    {
                        // сюда писать команды при ошибке вызова 
                    }


                }
            }
            else
            {
                string _pack_unpack_dir = Application.StartupPath + @"\Res_Packer_Unpacker\x86_64\resunpacker.exe";
                if (radioButton_32.Checked)
                    _pack_unpack_dir = Application.StartupPath + @"\Res_Packer_Unpacker\x86\resunpacker.exe";

                OpenFileDialog openFileDialog = new OpenFileDialog();
                //openFileDialog.Filter = "Json files (*.json) | *.json";
                openFileDialog.Filter = "Binary File (*.bin)|*.bin";
                ////openFileDialog1.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = false;
                openFileDialog.Title = "Путь к файлу циферблата";

                if (!File.Exists(_pack_unpack_dir))
                {
                    MessageBox.Show("Путь [" + _pack_unpack_dir +
                        "] к утилите распаковки/запаковки указан неверно.\r\n\r\n" +
                        "Отсутствуют необходимые компоненты программы.",
                        "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fullPath = openFileDialog.FileName;

                    try
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = _pack_unpack_dir;
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
                        string newFullName_bin = Path.Combine(path, fileNameOnly + ".unp.bin");

                        //MessageBox.Show(newFullName);
                        if (File.Exists(newFullName_unp))
                        {
                            File.Copy(newFullName_unp, newFullName_bin, true);
                            //this.BringToFront();
                            //Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName_bin));
                            if (File.Exists(newFullName_bin))
                            {
                                //_pack_unpack_dir = Application.StartupPath + @"\Tool-Wf-GTS\main.py";
                                _pack_unpack_dir = Application.StartupPath + @"\main\main.exe";
                                startInfo.FileName = _pack_unpack_dir;
                                startInfo.Arguments = "--gts --file " + newFullName_bin;
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

                                if (File.Exists(newFullName))
                                {
                                    Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName));
                                }
                            }
                        }
                    }
                    catch
                    {
                        // сюда писать команды при ошибке вызова 
                    }


                    //string _pack_unpack_command = "--gtr 47 --file";
                    ////if (radioButton_42.Checked) _pack_unpack_command = "--gtr 42 --file";
                    //Process _process = new Process();
                    //ProcessStartInfo startInfo = new ProcessStartInfo();
                    //startInfo.FileName = _pack_unpack_dir;
                    //startInfo.Arguments = _pack_unpack_command + " " + fullfilename;
                    //_process.StartInfo = startInfo;
                    //_process.Start();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Is64Bit()) radioButton_64.Checked=true; else radioButton_32.Checked=true;
#if DEBUG
            MessageBox.Show(GetOSBit());
#endif
        }

        private void button_pack_Click(object sender, EventArgs e)
        {
            string _pack_unpack_dir = Application.StartupPath + @"\main\main.exe";
            //if (radioButton_loly.Checked)
            //    _pack_unpack_dir = Application.StartupPath + @"\py_amazfit_tools-dev_gtr\main.py";
            string _pack_unpack_command = "--gts --file";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = subPath;
            openFileDialog.Filter = "Json files (*.json) | *.json";
            //openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            ////openFileDialog1.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Путь к файлу настроек циферблата";

            if (!File.Exists(_pack_unpack_dir))
            {
                MessageBox.Show("Путь [" + _pack_unpack_dir +
                    "] к утилите распаковки/запаковки указан неверно.\r\n\r\n" +
                    "Отсутствуют необходимые компоненты программы.",
                    "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullfilename = openFileDialog.FileName;
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = _pack_unpack_dir;
                    startInfo.Arguments = _pack_unpack_command + "   " + fullfilename;
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
                        if (fileSize > 1.95) MessageBox.Show("Размер несжатого файла превышает 1,95МБ.\r\n\r\n" + "Циферблат может не работать.",
                            "Большой размер файла", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        _pack_unpack_dir = Application.StartupPath + @"\Res_Packer_Unpacker\x86_64\respacker.exe";
                        if (radioButton_32.Checked)
                            _pack_unpack_dir = Application.StartupPath + @"\Res_Packer_Unpacker\x86\respacker.exe";

                        startInfo.FileName = _pack_unpack_dir;
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
                        string newFullName_bin = Path.Combine(path, fileNameOnly + ".cmp.bin");
                        if (File.Exists(newFullName_cmp))
                        {
                            File.Copy(newFullName_cmp, newFullName_bin);
                            Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName_bin));
                        }
                            
                    }
                }
                catch
                {
                    // сюда писать команды при ошибке вызова 
                }
            }
        }

        public static string GetOSBit()
        {
            bool is64bit = Is64Bit();
            if (is64bit)
                return "x64";
            else
                return "x32";
        }
        

        public static bool Is64Bit()
        {
            if (Environment.Is64BitOperatingSystem) return true;
            else return false;
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

    }
}
