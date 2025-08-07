using AppTaskManager.Models;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AppTaskManager.Utilities
{
    [ValueConversion(typeof(TaskImportance), typeof(Color))]
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TaskImportance importance)
            {
                return importance switch
                {
                    TaskImportance.Low => Brushes.Green,
                    TaskImportance.Medium => Brushes.Blue,
                    TaskImportance.High => Brushes.Orange,
                    TaskImportance.Critical => Brushes.Red,
                    _ => Brushes.WhiteSmoke,
                };
            }
            if (value is TaskCategory category)
            {
                return category switch
                {
                    TaskCategory.Work => Brushes.BlueViolet,
                    TaskCategory.Home => Brushes.CadetBlue,
                    TaskCategory.Education => Brushes.CornflowerBlue,
                };
            }
            if (value is TaskState state)
            {
                return state switch
                {
                    TaskState.Create => Brushes.LightGreen,
                    TaskState.InProgress => Brushes.LightSteelBlue,
                    TaskState.Completed => Brushes.Green,
                    TaskState.Late => Brushes.Red,
                    TaskState.Deleted => Brushes.Gray,
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
