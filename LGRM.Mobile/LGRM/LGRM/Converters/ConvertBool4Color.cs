using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace LGRM.XamF.Converters
{
    public class ConvertBool4Color : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var x = (bool)value;
            Color color;

            if (x == true)
            {
                color = Color.LightSkyBlue;
            }
            else
            {
                color = Color.WhiteSmoke;
            }

            return color;

        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }


    }
}
