using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;


namespace LanDOXer.Classes
{
    public class DirectoryCheck
    {
        const string StartPath = @"\\SERVERRAID\z\LanDocs Читать всем !!!!!\";

        public DirectoryInfo LanDoxDirectory => new DirectoryInfo(FullPath());

        public string FullPath()
        {
            var year = 2019; //default
            CheckDirectoryExistence(StartPath);
            foreach (var file in Directory.GetDirectories(StartPath))
            {
                for (var i = 2019; i < 2030; i++)
                {
                    if (file.Contains(i.ToString()) && i > year)
                        year = i;
                }
            }
            var resultPath = Path.Combine(StartPath, year.ToString());
            return resultPath;
        }

        public void CheckDirectoryExistence(string path)
        { 
            var directory = new DirectoryInfo(path);
            if (!directory.Exists)
            {
                var connectError = MessageBox.Show("Нет доступа к папке LanDocs.\n\n" + 
                                                   "Кнопка Пропустить отложит процесс проверки доступа на несколько минут",
                                            "LanDOXer", MessageBoxButtons.AbortRetryIgnore);
                switch (connectError)
                {
                    case DialogResult.Ignore:
                        Thread.Sleep(Program.TimeToSleep);
                        CheckDirectoryExistence(path);
                        break;
                    case DialogResult.Retry:
                        CheckDirectoryExistence(path);
                        break;
                    case DialogResult.Abort:
                        Environment.Exit(0);
                        break;
                }
            }
        }

    }
    
}
