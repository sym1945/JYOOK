using System;
using System.Globalization;

namespace JYOOK
{
    public class NumToRateConverter : ValueConverterBase<NumToRateConverter>
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
            return value.ToString().Replace("%", "");
        }
    }
}