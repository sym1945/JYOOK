using System;
using System.Globalization;

namespace JYOOK
{
    public class PositiveNumToBooleanConverter : ValueConverterBase<PositiveNumToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double numValue)
                return numValue > 0;

            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}