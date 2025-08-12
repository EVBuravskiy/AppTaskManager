using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;

namespace AppTaskManager.Models
{
    public class TaskModel : IComparable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool IsCompleted { get; set; }

        public TaskState TaskState { get; set; }

        public TaskCategory TaskCategory { get; set; }

        public TaskImportance TaskImportance { get; set; }

        public List<TaskCheck> TaskChecks { get; set; }

        public int CompareTo(object? obj)
        {
            if (obj is TaskModel other) return EndTime.CompareTo(other.EndTime);
            return 0;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is TaskModel other) return EndTime == other.EndTime;
            return false;
        }
        public override int GetHashCode()
        {
            return EndTime.GetHashCode();
        }
    }

    public enum TaskState
    {
        [Description("Задача создана")]
        Create,
        [Description("Задача выполняется")]
        InProgress,
        [Description("Задача выполнена")]
        Completed,
        [Description("Задача не выполнена")]
        Late,
        [Description("Задача удалена")]
        Deleted
    }

    public enum TaskCategory
    {
        [Description("Работа")]
        Work,
        [Description("Дом")]
        Home,
        [Description("Обучение")]
        Education
    }

    public enum TaskImportance
    {
        [Description("Важность низкая")]
        Low,
        [Description("Важность средняя")]
        Medium,
        [Description("Важность высокая")]
        High,
        [Description("Важность критическая")]
        Critical,
    }
}
