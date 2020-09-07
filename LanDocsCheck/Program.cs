using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Threading;
using System.Windows.Forms;
using LanDOXer.Classes;
using Microsoft.Win32;

namespace LanDOXer
{
    class Program
    {
        public const int TimeToSleep = 5 * 1000 * 60; // нормальное время - 5 минут
        //public const int TimeToSleep = 5 * 1000; // debug

        public static void MakeAutoLaunch()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", RegistryKeyPermissionCheck.ReadWriteSubTree);
            key.SetValue("LanDOXer", System.Reflection.Assembly.GetEntryAssembly().Location);
        }

        static void Main()
        {
            string thisProcessName = Process.GetCurrentProcess().ProcessName;
            if (Process.GetProcesses().Count(p => p.ProcessName == thisProcessName) > 1)
            {
                MessageBox.Show("LanDOXer уже запущен", "LanDOXer");
                return;
            }

            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            MakeAutoLaunch();
            var lanDox = new DirectoryCheck();
            var fileWithByteData = new ReadWriteBytesData();
            var notifications = new SomeNotifications();

            while (true)
            {
                lanDox.CheckDirectoryExistence(lanDox.FullPath());
                fileWithByteData.CreateFileIfNotExists();
                if (fileWithByteData.FolderHasChangedByBytesInFiles(lanDox.LanDoxDirectory))
                {
                    const string boxTitle = "LanDOXer";
                    const string boxText =
                        "В Ландоксе изменения! Взгляните! \n\"Да\" - открыть папку LanDocs \n \"Нет\" - напомнить посмотреть позже \"";
                    notifications.LanDoxMessageBoxNotification(boxText, boxTitle, lanDox.FullPath(), fileWithByteData,
                        lanDox.LanDoxDirectory);
                }

                Thread.Sleep(TimeToSleep);
            }
        }
        // TODO: Впоследствии отключить, чтобы не отлавливать любые ошибки
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString(), "LanDOXer");
            // Environment.Exit(1);
        }
    }
}
