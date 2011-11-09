using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WindowsPhoneApplication1
{
    public partial class CameraPage : PhoneApplicationPage
    {
        public CameraPage()
        {
            InitializeComponent();
            // PageTwoTextBox.Text = (Application.Current as App).AppState.StateString;
            // PageTwoTextBox.Text = PhoneApplicationService.Current.State["PageTwo"].ToString();
            takePhotoTask = new CameraCaptureTask();
            choosePhotoTask = new PhotoChooserTask();

            takePhotoTask.Completed += OnCameraCaptureCompleted;
            choosePhotoTask.Completed += OnCameraCaptureCompleted; 

        }

        private void SaveState()
        {
            // dummy use of application state
            PhoneApplicationService.Current.State[this.Name] = "dummy";
        }

        private void RestoreState()
        {
            // TODO
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnCameraCaptureCompleted(object sender, PhotoResult args)
        {
            if (args.TaskResult == TaskResult.OK)
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(args.ChosenPhoto);
                BackdropImage.Source = image;
                CameraFailedTextBlock.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                CameraFailedTextBlock.Visibility = System.Windows.Visibility.Visible;
                CameraFailedTextBlock.Text += ":" + args.TaskResult.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TakePhotoButton_Click(object sender, EventArgs e)
        {
            takePhotoTask.Show();
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChoosePhotoButton_Click(object sender, EventArgs e)
        {
            choosePhotoTask.Show();
        }

        private CameraCaptureTask takePhotoTask;
        private PhotoChooserTask choosePhotoTask;


    }
}