using System;
using System.Globalization;

namespace JYOOK
{
    public class NumToKilogramConverter : ValueConverterBase<NumToKilogramConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return value;
            else
                return $"{value:f1}";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().Replace("kg", "");
        }
    }
}