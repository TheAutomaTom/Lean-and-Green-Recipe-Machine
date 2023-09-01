using System;
using Xamarin.Forms;

namespace LGRM.XamF.Converters
{
    public class ConvertStringExists2Visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {

                if (string.IsNullOrEmpty((string)value))
                    return false;
                else
                    return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
