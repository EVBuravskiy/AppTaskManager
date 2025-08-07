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
    [ValueConversion(typeof(TaskImportance), typeof(string))]
    class TaskEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TaskImportance importance)
            {
                return importance switch
                {
                    TaskImportance.Low => "Низкая важность",
                    TaskImportance.Medium => "Средняя важность",
                    TaskImportance.High => "Высокая важность",
                    TaskImportance.Critical => "Критическая важность",
                    _ => Brushes.WhiteSmoke,
                };
            }
            if (value is TaskCategory category)
            {
                return category switch
                {
                    TaskCategory.Work => "Работа",
                    TaskCategory.Home => "Дом",
                    TaskCategory.Education => "Обучение",
                };
            }
            if (value is TaskState state)
            {
                return state switch
                {
                    TaskState.Create => "Задача создана",
                    TaskState.InProgress => "Задача в процессе выполнения",
                    TaskState.Completed => "Задача завершена",
                    TaskState.Late => "Задача не выполнена",
                    TaskState.Deleted => "Задача удалена",
                };
            }
            return Brushes.WhiteSmoke;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
