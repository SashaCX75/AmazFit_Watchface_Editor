using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WatchFace_PackUnpack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button_unpack_Click(object sender, EventArgs e)
        {
            // если копируем в папку
            if(checkBox_Watchface_Path.Checked)
            {
                string subPath = Application.StartupPath + @"\Watch_face\";
                if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);
                string _pack_unpack_dir = Application.StartupPath + @"\main\main.exe";
                if (radioButton_loly.Checked)
                    _pack_unpack_dir = Application.StartupPath + @"\py_amazfit_tools-dev_gtr\main.py";

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

                    Process _process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = _pack_unpack_dir;
                    string _pack_unpack_command = "--gtr 47 --file";
                    if(radioButton_42.Checked) _pack_unpack_command = "--gtr 42 --file";
                    startInfo.Arguments = _pack_unpack_command + " " + fullPath;
                    _process.StartInfo = startInfo;
                    _process.Start();
                }
            }
            else
            {
                string _pack_unpack_dir = Application.StartupPath + @"\main\main.exe";
                if (radioButton_loly.Checked)
                    _pack_unpack_dir = Application.StartupPath + @"\py_amazfit_tools-dev_gtr\main.py";

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
                    
                    string _pack_unpack_command = "--gtr 47 --file";
                    if (radioButton_42.Checked) _pack_unpack_command = "--gtr 42 --file";
                    Process _process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = _pack_unpack_dir;
                    startInfo.Arguments = _pack_unpack_command + " " + fullfilename;
                    _process.StartInfo = startInfo;
                    _process.Start();
                }
            }

        }

        private void button_pack_Click(object sender, EventArgs e)
        {
            string _pack_unpack_dir = Application.StartupPath + @"\main\main.exe";
            if (radioButton_loly.Checked)
                _pack_unpack_dir = Application.StartupPath + @"\py_amazfit_tools-dev_gtr\main.py";

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

                string _pack_unpack_command = "--gtr 47 --file";
                if (radioButton_42.Checked) _pack_unpack_command = "--gtr 42 --file";
                Process _process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = _pack_unpack_dir;
                startInfo.Arguments = _pack_unpack_command + " " + fullfilename;
                _process.StartInfo = startInfo;
                _process.Start();
            }
        }
    }
}
