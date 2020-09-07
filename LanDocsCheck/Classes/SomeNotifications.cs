using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime;
using IPersistFile = System.Runtime.InteropServices.ComTypes.IPersistFile;

namespace LanDOXer.Classes
{
    // TODO : Toast Notifications in future
    public class SomeNotifications
    {
        private const string APP_ID = "LanDOXer";

        public void LanDoxMessageBoxNotification(string message, string title, string lanDoxPath,
            ReadWriteBytesData fileData, DirectoryInfo lanDoxDirectory)
        {
            var result = MessageBox.Show(message,
                title,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button2);
            var reminder = "Напоминаю!! \n" + message;

            if (result == DialogResult.Yes)
                Process.Start("explorer.exe", lanDoxPath);
            if (result == DialogResult.No)
            {
                Thread.Sleep(Program.TimeToSleep);
                fileData.FolderHasChangedByBytesInFiles(lanDoxDirectory);
                LanDoxMessageBoxNotification("Напоминаю!!\n" + message, title, lanDoxPath, fileData, lanDoxDirectory);
            }
        }
    }
}

/*
public bool TryCreateShortcut(string appName)
{
    String shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "\\Microsoft\\Windows\\Start Menu\\Programs\\" , APP_ID + ".lnk");
    if (!File.Exists(shortcutPath))
    {
        InstallShortcut(shortcutPath);
        return true;
    }
    return false;
}

private void InstallShortcut(String shortcutPath)
{
    // Find the path to the current executable
    String exePath = Process.GetCurrentProcess().MainModule.FileName;
    IShellLinkW newShortcut = (IShellLinkW)new CShellLink();

    // Create a shortcut to the exe
    ErrorHelper.VerifySucceeded(newShortcut.SetPath(exePath));
    ErrorHelper.VerifySucceeded(newShortcut.SetArguments(""));

    // Open the shortcut property store, set the AppUserModelId property
    IPropertyStore newShortcutProperties = (IPropertyStore)newShortcut;

    using (PropVariant appId = new PropVariant(APP_ID))
    {
        var setValue = newShortcutProperties.SetValue(SystemProperties.System.AppUserModel.ID, appId);
        ErrorHelper.VerifySucceeded(setValue);
        var commit = newShortcutProperties.Commit();
        ErrorHelper.VerifySucceeded(commit);
    }

    // Commit the shortcut to disk
    IPersistFile newShortcutSave = (IPersistFile)newShortcut;
    newShortcutSave.Save(shortcutPath, true);
}



/*
public void SendSomeToast()
{
    {
        // In a real app, these would be initialized with actual data
        string title = "Andrew sent you a picture";
        string content = "Check this out, Happy Canyon in Utah!";
        string image = "D:\\Времяночка\\1.png";
        string logo = "D:\\Времяночка\\1.png"; ;
        int conversationId = 384928;

        // Construct the visuals of the toast
        ToastVisual visual = new ToastVisual()
        {
            BindingGeneric = new ToastBindingGeneric()
            {
                Children =
                {
                    new AdaptiveText()
                    {
                        Text = title
                    },

                    new AdaptiveText()
                    {
                        Text = content
                    },

                    new AdaptiveImage()
                    {
                        Source = image
                    }
                },

                AppLogoOverride = new ToastGenericAppLogo()
                {
                    Source = logo,
                    HintCrop = ToastGenericAppLogoCrop.Circle
                }
            }
        };

        // Construct the actions for the toast (inputs and buttons)
        ToastActionsCustom actions = new ToastActionsCustom()
        {
            Inputs =
            {
                new ToastTextBox("tbReply")
                {
                    PlaceholderContent = "Type a response"
                }
            },

            Buttons =
            {
                new ToastButton("Reply", new QueryString()
                {
                    {"action", "reply"},
                    {"conversationId", conversationId.ToString()}

                }.ToString())
                {
                    ActivationType = ToastActivationType.Background,
                    ImageUri = "Assets/Reply.png",

                    // Reference the text box's ID in order to
                    // place this button next to the text box
                    TextBoxId = "tbReply"
                },

                new ToastButton("Like", new QueryString()
                {
                    {"action", "like"},
                    {"conversationId", conversationId.ToString()}

                }.ToString())
                {
                    ActivationType = ToastActivationType.Background
                },

                new ToastButton("View", new QueryString()
                {
                    {"action", "viewImage"},
                    {"imageUrl", image}

                }.ToString())
            }
        };


        // Now we can construct the final toast content
        ToastContent toastContent = new ToastContent()
        {
            Visual = visual,
            Actions = actions,

            // Arguments when the user taps body of toast
            Launch = new QueryString()
            {
                {"action", "viewConversation"},
                {"conversationId", conversationId.ToString()}

            }.ToString()
        };


        // And create the toast notification
        ToastNotification notification = new ToastNotification(toastContent.GetXml());


        // And then send the toast
        ToastNotificationManager.CreateToastNotifier(APP_ID).Show(notification);
    }
} 
   */