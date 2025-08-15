using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace AppTaskManager.Utilities
{
    /// <summary>
    /// Converter for the enum description 
    /// </summary>

    [ValueConversion(typeof(Enum), typeof(string))]
    public class EnumDescriptionConverter : IValueConverter
    {
        /// <summary>
        /// Convert
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                var field = enumValue.GetType().GetField(enumValue.ToString());
                if (field != null)
                {
                    var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    return attribute == null ? enumValue.ToString() : attribute.Description;
                }
            }
            return value?.ToString();
        }

        /// <summary>
        /// Convert back
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
