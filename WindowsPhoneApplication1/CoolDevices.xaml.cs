using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;
using System.Device.Location;


namespace WindowsPhoneApplication1
{
    public partial class CoolDevices : PhoneApplicationPage
    {
        public CoolDevices()
        {
            InitializeComponent();

            // add each of the devices to the listbox
            AddNewDevice(new AccelerometerDevice());
            AddNewDevice(new LocationDevice());
        }

        public void RecordReading(int deviceIndex, string readingValue)
        {
            MyListBox.Dispatcher.BeginInvoke(() => { TextBox displayTextBox = (MyListBox.Items[deviceIndex] as TextBox); displayTextBox.Text = (displayTextBox.Tag as PhoneDevice).Name + ":" + readingValue; });
        }

        private void AddNewDevice(PhoneDevice newDevice)
        {
            Debug.Assert(null != newDevice);
            TextBox tBlock;
            int deviceIndex = MyListBox.Items.Count;

            tBlock = new TextBox();
            tBlock.Name = newDevice.Name + "TextBox";
            tBlock.Tag = newDevice;
            tBlock.IsReadOnly = true;
            tBlock.TextWrapping = System.Windows.TextWrapping.Wrap;
            tBlock.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            tBlock.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            MyListBox.Items.Add(tBlock);

            newDevice.Initialize(deviceIndex, this);
            newDevice.Start();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public abstract class PhoneDevice
    {
        // keep track of positions in the listbox --- replace soon with a name-based lookup
        public int deviceListIndex = 0;
        public CoolDevices devicesPage;
        public void Initialize(int index, CoolDevices devPage)
        {
            deviceListIndex = index;
            devicesPage = devPage;
        }
        public void RecordReading(string readingValue)
        {
            devicesPage.RecordReading(deviceListIndex, readingValue);
        }

        public abstract void Start(); 
        public abstract string Name { get;}
    }

    /// <summary>
    /// 
    /// </summary>
    public class AccelerometerDevice : PhoneDevice
    {
        public AccelerometerDevice() 
        {
            accelerometer = new Accelerometer();
        }

        public override void Start()
        {
            accelerometer.ReadingChanged += OnAccelerometerReadingChanged;
            try
            {
                accelerometer.Start();
                RecordReading(ReadingValue(null));
            }
            catch (Exception ex)
            {
                RecordReading("Failed to start" + ex.ToString());
            }
        }

        public override string Name { get { return "Accelerometer";} }

        private Accelerometer accelerometer;

        private string ReadingValue(AccelerometerReadingEventArgs args)
        {
            string AccelerationVector = "<null>";
            if (null != args)
            {
                AccelerationVector = String.Format("X = {0}; Y = {1}; Z = {2}; TS = {3}", args.X, args.Y, args.Z, args.Timestamp);
            }
            return AccelerationVector;
        }

        private void OnAccelerometerReadingChanged(object sender, AccelerometerReadingEventArgs args)
        {
            RecordReading(ReadingValue(args));
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class LocationDevice : PhoneDevice
    {
        public LocationDevice() 
        {
            geoWatcher = new GeoCoordinateWatcher();
        }

        public override void Start()
        {
            geoWatcher.PositionChanged += OnGeoCoordinatesChanged;

            try
            {
                geoWatcher.Start();
                RecordReading(ReadingValue(null));
            }
            catch (Exception ex)
            {
                RecordReading("Failed to start" + ex.ToString());
            }
        }

        public override string Name { get { return "Location";} }

        private GeoCoordinateWatcher geoWatcher;
 
        private string ReadingValue(GeoPositionChangedEventArgs<GeoCoordinate> args)
        {
            GeoCoordinate Location;
            if (null != args)
            {
                Location = args.Position.Location;
            }
            else if ((null != geoWatcher) && (geoWatcher.Status == GeoPositionStatus.Ready))
            {
                Location = geoWatcher.Position.Location;
            }
            else
            {
                Location = new GeoCoordinate(35.4, 70.2, 0);
            }

            // TODO: modify the map control based on the location change. Nasty hack way to do it for now --- need cleaner way to pass the location
            devicesPage.BingMapControl.SetView(Location, devicesPage.BingMapControl.ZoomLevel);

            string GeoVector = String.Format("Lat = {0}; Long = {1}; Alt = {2}",
                    Location.Latitude,
                    Location.Longitude,
                    Location.Altitude);
            return GeoVector;
        }

        private void OnGeoCoordinatesChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> args)
        {
            RecordReading(ReadingValue(args));
        }
 
    }


}