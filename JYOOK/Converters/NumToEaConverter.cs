using System;
using System.Globalization;

namespace JYOOK
{
    public class NumToEaConverter : ValueConverterBase<NumToEaConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().Replace("개", "");
        }
    }
}