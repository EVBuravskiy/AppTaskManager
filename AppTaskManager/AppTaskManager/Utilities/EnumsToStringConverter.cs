using AppTaskManager.Models;

namespace AppTaskManager.Utilities
{
    /// <summary>
    /// Converter enum to string
    /// </summary>
    public class EnumsToStringConverter
    {
        /// <summary>
        /// Convert enum to string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EnumToString(object input)
        {
            if (input is TaskImportance importance)
            {
                return importance switch
                { 
                    TaskImportance.Low => "Важность низкая",
                    TaskImportance.Medium => "Важность средняя",
                    TaskImportance.High => "Важность высокая",
                    TaskImportance.Critical => "Важность критическая"
                };
            }
            if(input is TaskCategory category)
            {
                return category switch
                {
                    TaskCategory.Home => "Дом",
                    TaskCategory.Work => "Работа",
                    TaskCategory.Education => "Обучение",
                };
            }
            return "";
        }

        /// <summary>
        /// Convert string to enum
        /// </summary>
        /// <param name="inputstring"></param>
        /// <returns></returns>
        public static Enum StringToEnum(string inputstring)
        {
            return inputstring switch
            {
                "Важность низкая" => TaskImportance.Low,
                "Важность средняя" => TaskImportance.Medium,
                "Важность высокая" => TaskImportance.High,
                "Важность критическая" => TaskImportance.Critical,
                "Дом" => TaskCategory.Home,
                "Работа" => TaskCategory.Work,
                "Учеба" => TaskCategory.Education,
            };
        }
    }
}
