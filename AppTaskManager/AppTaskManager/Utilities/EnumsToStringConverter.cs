using AppTaskManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaskManager.Utilities
{
    public class EnumsToStringConverter
    {
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
                    TaskCategory.Education => "Учеба",
                };
            }
            return "";
        }

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
