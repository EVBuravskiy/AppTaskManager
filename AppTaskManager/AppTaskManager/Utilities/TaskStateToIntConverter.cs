using AppTaskManager.Models;
using System.Globalization;
using System.Windows.Data;

namespace AppTaskManager.Utilities
{
    /// <summary>
    /// Converter enum state to int
    /// </summary>
    [ValueConversion(typeof(TaskState), typeof(int))]
    class TaskStateToIntConverter: IValueConverter
    {
        /// <summary>
        /// Convert enum state to int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TaskState state)
            {
                return state switch
                {
                    TaskState.Create => 0,
                    TaskState.InProgress => 1,
                    TaskState.Completed => 2,
                    TaskState.Late => 3,
                    TaskState.Deleted => 4,
                };
            }
            return 0;
        }

        /// <summary>
        /// Convert int to enum state
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
                return value switch
            {
                0 => TaskState.Create,
                1 => TaskState.InProgress,
                2 => TaskState.Completed,
                3 => TaskState.Late,
                4 => TaskState.Deleted,
                _ => TaskState.Create,
            };
        }
    }
}
