using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.System.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    public sealed partial class CameraSettingsDialog : ContentDialog
    {
        Windows.Storage.ApplicationDataContainer localSettings = null;
        Windows.Storage.StorageFolder localFolder = null;
       

        public CameraSettingsDialog()
        {
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            this.InitializeComponent();
            InitializeCameraAsync();
            DataContext = this;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
        private async void InitializeCameraAsync()
        {
            DeviceInformationCollection videoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            if (videoDevices.Count == 0)
            {
                //throw new NullReferenceException();
                //SettingsDialogRestartText.Text = "Somthing went wrong please initilaize your Video Device USB";

                Debug.WriteLine("Somthing went wrong please initilaize your Video Device USB");

            }
            else {

                for (int i = 0; i < videoDevices.Count; i++)
                {
                    SettingsCamera.Items.Add(videoDevices[i].Name);
                }
                if (SettingsCamera.SelectedIndex == -1 ||SettingsCamera.SelectedIndex == 0 || SettingsCamera.SelectedIndex == 1)
                {
                    if (localSettings.Values.ContainsKey("camValue"))
                    {
                        SettingsCamera.SelectedIndex = int.Parse(localSettings.Values["camValue"].ToString());
                        StorageFile sampleFile = await localFolder.CreateFileAsync("camproporties.txt", CreationCollisionOption.ReplaceExisting);
                        await FileIO.WriteTextAsync(sampleFile, localSettings.Values["camValue"].ToString());
                    }
                    else if (!localSettings.Values.ContainsKey("camValue"))
                    {
                        SettingsCamera.SelectedIndex = 0;
                        localSettings.Values["camValue"] = SettingsCamera.SelectedIndex;
                        StorageFile sampleFile = await localFolder.CreateFileAsync("micproporties.txt", CreationCollisionOption.ReplaceExisting);
                        await FileIO.WriteTextAsync(sampleFile, localSettings.Values["camValue"].ToString());

                    }
                }
            }

        }

      
        private async void SettingsCamera_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SettingsCamera.SelectedIndex == 0 )
            {
                localSettings.Values["camValue"] = SettingsCamera.SelectedIndex;
                StorageFile sampleFile = await localFolder.CreateFileAsync("camproporties.txt", CreationCollisionOption.ReplaceExisting);
                //await FileIO.WriteTextAsync(sampleFile, localSettings.Values["camValue"].ToString());
            }

        }
    }
}
