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
    [ValueConversion(typeof(TaskCategory), typeof(int))]
    class TaskCategoryToIntConverter: IValueConverter
    {
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
