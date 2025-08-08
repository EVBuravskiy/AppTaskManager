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
    [ValueConversion(typeof(TaskImportance), typeof(int))]
    class TaskImportanceToIntConverter: IValueConverter
    {
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
