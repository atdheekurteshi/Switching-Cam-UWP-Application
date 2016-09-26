using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Enumeration;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Media;
using Windows.Media.Capture;
using Windows.System.Display;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Background;
using App1;
using Microsoft.VisualBasic.CompilerServices;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Capture : Page
    {
        private enum StEnum { NEXT, PLAY, EXIT };
        private Statechart<StEnum> state;
        private State<StEnum> RecordingState;
        private LowLagMediaRecording llmr;

        Windows.Storage.ApplicationDataContainer localSettings = null;
        Windows.Storage.StorageFolder localFolder = null;
        // Receive notifications about rotation of the device and UI and apply any necessary rotation to the preview stream and UI controls       
        private readonly DisplayInformation _displayInformation = DisplayInformation.GetForCurrentView();
        private readonly SimpleOrientationSensor _orientationSensor = SimpleOrientationSensor.GetDefault();
        private SimpleOrientation _deviceOrientation = SimpleOrientation.NotRotated;
        private DisplayOrientations _displayOrientation = DisplayOrientations.Portrait;

        // Rotation metadata to apply to the preview stream and recorded videos (MF_MT_VIDEO_ROTATION)
        // Reference: http://msdn.microsoft.com/en-us/library/windows/apps/xaml/hh868174.aspx
        private static readonly Guid RotationKey = new Guid("C380465D-2271-428C-9B83-ECEA3B4A85C1");

        // Prevent the screen from sleeping while the camera is running
        private readonly DisplayRequest _displayRequest = new DisplayRequest();

        // For listening to media property changes
        private readonly SystemMediaTransportControls _systemMediaControls = SystemMediaTransportControls.GetForCurrentView();

        // MediaCapture and its state variables
        private MediaCapture _mediaCapture;
        private bool _isInitialized;
        private bool _isPreviewing;
        private bool _isRecording;

        // Information about the camera device
        private bool _mirroringPreview;
        private bool _externalCamera;

        public Capture()
        {
            this.InitializeComponent();
            DataContext = this;
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
           
            InitializeMediaCaptureAsync();

        }
        private async void InitStatechart() {

            RecordingState = new State<StEnum>();
            RecordingState.exit = async () =>
            {
                Utils.ReleaseDisplay();
                //recTimer.Stop();
                await llmr.StopAsync();

                //App.State.Recording = false;
            };

        }
        private async Task InitializeMediaCaptureAsync()
        {

            Debug.WriteLine("InitializeCameraAsync");

            if (_mediaCapture == null)
            {
                //string cameraDevice;
                //Get available devices for capturing pictures
                var allVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
                //StorageFile sampleFile = await localFolder.GetFileAsync("proporties.txt");
                //String timestamp = await FileIO.ReadTextAsync(sampleFile);
                var cameraDevice = localSettings.Values["camValue"].ToString();
                // Get the desired camera by panel
                //var cameraDevice = allVideoDevices.FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == (Windows.Devices.Enumeration.Panel.Back));

                // If there is no device mounted on the desired panel, return the first device found
                //cameraDevice= cameraDevice ?? allVideoDevices.FirstOrDefault();

                if (allVideoDevices == null)
                {
                    Debug.WriteLine("No camera device found!");

                }
                // Create MediaCapture and its settings
                _mediaCapture = new MediaCapture();
                var settings = new MediaCaptureInitializationSettings { VideoDeviceId = allVideoDevices[int.Parse(cameraDevice)].Id };

                await _mediaCapture.InitializeAsync(settings);
                //_isInitialized = true;
                // Prevent the device from sleeping while the preview is running
                //_displayRequest.RequestActive();

                // Set the preview source in the UI and mirror it if necessary
                CaptureElement.Source = _mediaCapture;
                CaptureElement.FlowDirection = _mirroringPreview ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

                // Start the preview
                await _mediaCapture.StartPreviewAsync();


            }
        }
        private async void Application_Suspending(object sender, SuspendingEventArgs e)
        {
            // Handle global application events only if this page is active
            if (Frame.CurrentSourcePageType == typeof(MainPage))
            {
                var deferral = e.SuspendingOperation.GetDeferral();

                //await CleanupCameraAsync();

                //await CleanupUiAsync();

                deferral.Complete();
            }
        }

        private async void Application_Resuming(object sender, object o)
        {
            // Handle global application events only if this page is active
            if (Frame.CurrentSourcePageType == typeof(MainPage))
            {
                //await SetupUiAsync();

                //await InitializeCameraAsync();
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //localSettings.Values["cmbValue"].ToString();
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            // Handling of this event is included for completenes, as it will only fire when navigating between pages and this sample only includes one page

            //await CleanupCameraAsync();

            //await CleanupUiAsync();

        }



        private async void cancelVideoButton_Click(object sender, RoutedEventArgs e)
        {
            //var leave = true;

            //if (!state.IsInState(RecordingState))
                //leave = await ShowLeaveViewAsync();
            //if (leave)
                //Frame.Navigate(typeof(MainPage));
           

        }
        public static async Task<bool> ShowLeaveViewAsync()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            var dialog = new MessageDialog(loader.GetString("MessageLeaveRecording"));
            dialog.Title = loader.GetString("MessageLeaveRecordingTitle");
            dialog.Commands.Add(new UICommand(loader.GetString("Yes"), null, 0));
            dialog.Commands.Add(new UICommand(loader.GetString("No"), null, 1));

            dialog.CancelCommandIndex = 1;
            dialog.DefaultCommandIndex = 0;

            var result = await dialog.ShowAsync();

            return Convert.ToInt16(result.Id) != 1;
        }
    }
}
