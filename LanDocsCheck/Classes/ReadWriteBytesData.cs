using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LanDOXer
{
    public class ReadWriteBytesData
    {
        private static FileInfo TargetFile => new FileInfo(Path.Combine(Environment.CurrentDirectory, "data.dat"));

        public void CreateFileIfNotExists()
        {
            if (TargetFile.Exists)
            {
                if (TargetFile.IsReadOnly)
                {
                    TargetFile.IsReadOnly = false;
                    if (MessageBox.Show("Файл data заблокирован от редактирования. \n Перезапустите приложение вручную.", "LanDOXer") == DialogResult.OK)
                    {
                        Application.Exit();
                        throw new FileLoadException("Файл data заблокирован от редактирования.");
                    }
                }
            }
            else
            { File.WriteAllText(TargetFile.ToString(), "0"); }
        }

        public void WriteDataToFile(long dateTime)
        {
            File.WriteAllText(TargetFile.ToString(), dateTime.ToString());
        }

        public long ReadDataFromFile()
        {
            var readString = File.ReadAllText(TargetFile.ToString());
            long result;
            try
            { result = Convert.ToInt64(readString); }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //По-злодейски поступаем с теми, кто редачит файлик :о
                File.Delete(TargetFile.ToString());
                if (MessageBox.Show("Файл data имел неккоректные данные и был удален. Перезапустите приложение.") ==
                    DialogResult.OK)
                {
                    Application.Exit();
                    Environment.Exit(0);
                }
                throw;
            }
            return result;
        }

        public bool FolderHasChangedByTime(DateTime dateTime)
        {
            var timeInFile = ReadDataFromFile();
            return dateTime.ToBinary() != timeInFile;
        }

        public bool FolderHasChangedByTimeInFiles(DirectoryInfo directory)
        {
            var timeInFile = ReadDataFromFile();
            var changed = false;
            foreach (var file in directory.GetFiles(".", SearchOption.AllDirectories))
            {
                var time = file.LastWriteTime.ToBinary();
                if (timeInFile < time)
                {
                    WriteDataToFile(time);
                    changed = true;
                }
            }
            return changed;
        }

        public bool FolderHasChangedByBytesInFiles(DirectoryInfo directory)
        {
            var bytesInFile = ReadDataFromFile();
            long bytesInDirectory = 0;
            try
            {
                foreach (var file in directory.GetFiles(".", SearchOption.AllDirectories))
                {
                    if (file.Name.Contains("~$")) continue;
                    if (file.DirectoryName != null &&
                        (file.DirectoryName.Contains("524") || file.DirectoryName.Contains("Контрагент"))) continue;
                    bytesInDirectory += file.Length;
                }
            }
            catch
            {
                MessageBox.Show("Потеряно соединение с сетью во время работы приложения. Перезапустите приложение",
                    "LanDOXer");
                Application.Exit();
                Environment.Exit(30);
            }

            if (bytesInFile == bytesInDirectory) return false;
            WriteDataToFile(bytesInDirectory);
            return true;
        }
    }
}
