using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;

namespace AppTaskManager.Models
{
    public class TaskModel
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
        [Description("Низкая важность")]
        Low,
        [Description("Средняя важность")]
        Medium,
        [Description("Высокая важность")]
        High,
        [Description("Незамедлительно")]
        Critical,
    }
}
