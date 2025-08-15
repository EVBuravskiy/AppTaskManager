using AppTaskManager.Models;
using System.Globalization;
using System.Windows.Data;

namespace AppTaskManager.Utilities
{
    /// <summary>
    /// Converter enum to int
    /// </summary>
    [ValueConversion(typeof(TaskCategory), typeof(int))]
    class TaskCategoryToIntConverter: IValueConverter
    {
        /// <summary>
        /// Convert enum to int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TaskCategory category)
            {
                return category switch
                {
                    TaskCategory.Work => 0,
                    TaskCategory.Home => 1,
                    TaskCategory.Education => 2,
                };
            }
            return 0;
        }

        /// <summary>
        /// Convert int to enum
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int inputvalue = (int)value;
            if (inputvalue == -1) inputvalue = 0;
            return inputvalue switch
            {
                0 => TaskCategory.Work,
                1 => TaskCategory.Home,
                2 => TaskCategory.Education,
                _ => TaskCategory.Work,
            };
        }
    }
}
