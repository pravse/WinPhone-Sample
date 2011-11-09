using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Threading;
using Microsoft.Devices.Radio;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;

namespace WindowsPhoneApplication1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            TimerTextBlock.Text = DateTime.Now.ToString();

            dispatcher = new ScheduledDispatcher(OnTimerTick, TimeSpan.FromSeconds(5));
            coolDevices = new CoolDevices();
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            // this causes the default handler to be invoked
            base.OnOrientationChanged(e);

            // any additional handling should occur here ...
           
            switch (e.Orientation)
            {
                case PageOrientation.Landscape:
                case PageOrientation.LandscapeLeft:
                case PageOrientation.LandscapeRight:
                    /**
                    OrientationTextBlock.Text = "Landscape";
                     * **/
                    break;
                default:
                    /**
                    OrientationTextBlock.Text = "Portrait";
                     * **/
                    break;
            }
        }


        void OnTimerTick(object sender, EventArgs args)
        {
            TimerTextBlock.Text = DateTime.Now.ToString();

            // any additional handling should happen here
        }


        private ScheduledDispatcher dispatcher = null;
        private CoolDevices coolDevices;



        private void SaveState()
        {
            // dummy use of application state
            /***
            (Application.Current as App).AppState.StateString = OrientationTextBlock.Text + AccelerometerTextBlock.Text;
             * ***/
            PhoneApplicationService.Current.State[this.Name] = "foo";
        }

        private void RestoreState()
        {
            // TODO
        }

        private void NextPageButton_Click(object sender, EventArgs e)
        {
            SaveState();
            this.NavigationService.Navigate(new Uri("/CameraPage.xaml", UriKind.Relative));
        }

        private void DevicesButton_Click(object sender, RoutedEventArgs e)
        {
            SaveState();
            this.NavigationService.Navigate(new Uri("/CoolDevices.xaml", UriKind.Relative));
        }

        private void CameraButton_Click(object sender, RoutedEventArgs e)
        {
            SaveState();
            this.NavigationService.Navigate(new Uri("/CameraPage.xaml", UriKind.Relative));
        }
    }

    public class ScheduledDispatcher
    {
        private DispatcherTimer timer;

        public ScheduledDispatcher(EventHandler TimerHandler, TimeSpan TimerInterval)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimerInterval;
            timer.Tick += TimerHandler;
            timer.Start();
        }

    }

}