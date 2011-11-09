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
using Microsoft.Phone.Tasks;
using System.Windows.Threading;
using Microsoft.Devices.Radio;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;

namespace MyUtilities
{
    public class TextBlockUtilities
    {
        // if you want to rotate any text block .... 
        public void Rotate(TextBlock textBlock, int rotationAngleDegrees)
        {
            RotateTransform rTransform = new RotateTransform();
            rTransform.Angle = rotationAngleDegrees;
            textBlock.RenderTransformOrigin = new Point(0.5, 0.5); 
            textBlock.RenderTransform = rTransform;
        }
    }
}
