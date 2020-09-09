using JYOOK.Application;
using System;
using System.Globalization;
using System.Windows;

namespace JYOOK
{
    public class EnumToDescriptionConverter : ValueConverterBase<EnumToDescriptionConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum enumValue)) 
                return DependencyProperty.UnsetValue;

            return enumValue.GetDescription();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}