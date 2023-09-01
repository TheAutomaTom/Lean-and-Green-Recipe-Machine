using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace LGRM.XamF.Converters
{
    public class ConvertFloat2Color : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var x = (float)value;
            Color color;
                        
            if (x > 0)
            {
                color = Color.LightGoldenrodYellow;
            }
            else
            {
                color = Color.White;
            }

            return color;
            
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }


    }
}
