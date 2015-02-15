using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using OpenTK.Graphics;

namespace CorpusFrisky.VisualSynth.Converters
{
    class WindowsToOpenGlColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class OpenGlToWindowsColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Color4 openGlColor4 = (Color4)value;
                var colorInt = openGlColor4.ToArgb();
                byte[] colorBytes = BitConverter.GetBytes(colorInt);

                return Color.FromArgb(colorBytes[0], colorBytes[1], colorBytes[2], colorBytes[3]);
            }
            catch (Exception)
            {
                // TODO:
                return new Color();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Color windowsColor = (Color) value;
                return new Color4(windowsColor.R, windowsColor.G, windowsColor.B, windowsColor.A);
            }
            catch (Exception)
            {
                // TODO:
                return new Color4();
            }
        }
    }
}
