using AppTaskManager.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace AppTaskManager.Utilities
{
    [ValueConversion(typeof(TaskState), typeof(int))]
    class TaskStateToIntConverter: IValueConverter
    {
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
