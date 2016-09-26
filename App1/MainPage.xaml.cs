using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Enumeration;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.System.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using App1;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page
    {
        Windows.Storage.ApplicationDataContainer localSettings = null;
        Windows.Storage.StorageFolder localFolder = null;
        public  MainPage()
        {

            this.InitializeComponent();
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            DataContext = this;

        }

        private async void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            var cameraSettinsgs = new CameraSettingsDialog();
            await cameraSettinsgs.ShowAsync();
        }

        private void recordVideoButton_Click(object sender, RoutedEventArgs e)
        {
            if(mainFrame.IsEnabled == true)
            {
                RecordVideoButton.Visibility = Visibility.Collapsed;
                mainFrame.Navigate(typeof(Capture));
            }
        }




        /*private async void WriteSetting_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values["cmbValue"] = SettingsCamera.SelectedIndex;
            StorageFile sampleFile = await localFolder.CreateFileAsync("proporties1.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, localSettings.Values["cmbValue"].ToString());
          
        }
        void DisplayOutput()
        {
            var value = localSettings.Values["cmbValue"];
            SettingsCamera.SelectedIndex = int.Parse(value.ToString());
        }

        private async void ReadSetting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFile sampleFile = await localFolder.GetFileAsync("proporties.txt");
                String timestamp = await FileIO.ReadTextAsync(sampleFile);
                textbox.Text = timestamp;
                //DisplayOutput();
            }
            catch (Exception)
            {
                // Timestamp not found
            }

        }

       
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }
       

        private void GoToCamera_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CameraPage));

        }

        /*private async void login_Click_1(object sender, RoutedEventArgs e)
        {
             private const string XMLFILENAME = "data.xml";

            Camera camera = new Camera();
            camera.Email= IdTextBox.Text;
            camera.Name = NameTextBox.Text;
            await SaveAsync();
            this.Frame.Navigate(typeof(MainPage));
            < Button x: Name = "save" Content = "Login" Click = "login_Click_1" FontSize = "25"  Canvas.Top = "225" Canvas.Left = "265" Height = "45" Width = "85" RenderTransformOrigin = "0.294,0.444" ></ Button >
            < Button x: Name = "retrive" Content = "Retrive" Click = "retrive_Click_1" FontSize = "25"  Canvas.Top = "225" Canvas.Left = "175" Height = "45" Width = "85" RenderTransformOrigin = "0.294,0.444" ></ Button >

        }

        public async Task SaveAsync()
        {
            StorageFile userdetailsfile = await ApplicationData.Current.LocalFolder.CreateFileAsync("Camera",
            CreationCollisionOption.ReplaceExisting);
            IRandomAccessStream raStream = await userdetailsfile.OpenAsync(FileAccessMode.ReadWrite);
            using (IOutputStream outStream = raStream.GetOutputStreamAt(0))
            {
                // Serialize the Session State.
                DataContractSerializer serializer = new DataContractSerializer(typeof(Camera));
                serializer.WriteObject(outStream.AsStreamForWrite(), camera);
                await outStream.FlushAsync();
            }

        }
        public async Task ReterieveAsync()
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("Camera");
            if (file == null) return;
            IRandomAccessStream inStream = await file.OpenReadAsync();
            // Deserialize the Session State.
            DataContractSerializer serializer = new DataContractSerializer(typeof(Camera));
            var StatsDetails = (Camera)serializer.ReadObject(inStream.AsStreamForRead());
            inStream.Dispose();
            NameTextBox.Text = "Welcome " + StatsDetails.Name;
            IdTextBox.Text= StatsDetails.Email;

        }
        protected  override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void retrive_Click_1(object sender, RoutedEventArgs e)
        {
            await ReterieveAsync();

        }*/
    }
}