using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.System;
using Windows.UI.Notifications;

namespace FaultTreeEditor.Services
{
    public static class NotificationService
    {
        public static void FileSavedNotification(FileInfo fileInfo)
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = $"{fileInfo.Name} was successfully saved."
                            },
                            new AdaptiveText()
                            {
                                Text = $"{fileInfo.DirectoryName}"
                            }
                        },
                        /*AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = "https://unsplash.it/64?image=1005",
                            HintCrop = ToastGenericAppLogoCrop.Circle
                        }*/
                    }
                },

                /*Actions = new ToastActionsCustom()
                {
                    Buttons =
                    {
                        new ToastButton("Open folder", $"action=openFolder;filePath={fileInfo.FullName}")
                    }
                },*/
                //Launch = $"action=openFolder;filePath={fileInfo.FullName}"
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // Send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }

        public static void FileNotSavedNotification(FileInfo fileInfo)
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = $"{fileInfo.Name} couldn't be saved."
                            },
                        },
                    }
                },
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // Send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }

        public static async Task HandleNotificationActivationAsync(IActivatedEventArgs eargs)
        {
            if (eargs.Kind == ActivationKind.ToastNotification)
            {
                var toastArgs = eargs as ToastNotificationActivatedEventArgs;

                if (String.IsNullOrWhiteSpace(toastArgs.Argument))
                    return;

                // Parse the query string (using QueryString.NET)
                QueryString args = QueryString.Parse(toastArgs.Argument);

                switch (args["action"])
                {
                    case "openFolder":
                        string filePath = args["filePath"];
                        //await Launcher.LaunchFolderAsync(await StorageFolder.GetFolderFromPathAsync(filePath));
                        //await Launcher.LaunchUriAsync(new Uri(filePath));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
