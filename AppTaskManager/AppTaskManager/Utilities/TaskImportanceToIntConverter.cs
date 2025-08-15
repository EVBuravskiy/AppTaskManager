using AppTaskManager.Models;
using System.Globalization;
using System.Windows.Data;

namespace AppTaskManager.Utilities
{
    /// <summary>
    /// Converter enum importance to int
    /// </summary>
    [ValueConversion(typeof(TaskImportance), typeof(int))]
    class TaskImportanceToIntConverter: IValueConverter
    {

        /// <summary>
        /// Convert enum importance to int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TaskImportance importance)
            {
                return importance switch
                {
                    TaskImportance.Low => 0,
                    TaskImportance.Medium => 1,
                    TaskImportance.High => 2,
                    TaskImportance.Critical => 3,
                    _ => 0,
                };
            }
            return 0;
        }

        /// <summary>
        /// Convert int to enum importance
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int inputvalue = (int)value;
            if (inputvalue == -1) return TaskImportance.Low;
            return value switch
            {
                0 => TaskImportance.Low,
                1 => TaskImportance.Medium,
                2 => TaskImportance.High,
                3 => TaskImportance.Critical,
                _ => TaskImportance.Low,
            };
        }
    }
}
