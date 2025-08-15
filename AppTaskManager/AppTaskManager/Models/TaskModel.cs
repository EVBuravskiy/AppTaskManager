using System.ComponentModel;

namespace AppTaskManager.Models
{
    /// <summary>
    /// Task
    /// </summary>
    public class TaskModel : IComparable
    {
        /// <summary>
        /// Property storing task id 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Property storing task title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Property storing task description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Property storing task creation time
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Property storing task start time
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Property storing task end time
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Property storing a pointer to the execution state of task
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Property storing the enum of task state
        /// </summary>
        public TaskState TaskState { get; set; }

        /// <summary>
        /// Property storing the enum of task category
        /// </summary>
        public TaskCategory TaskCategory { get; set; }

        /// <summary>
        /// Property storing the enum of task importance
        /// </summary>
        public TaskImportance TaskImportance { get; set; }

        /// <summary>
        /// Property storing collection of the task checks
        /// </summary>
        public List<TaskCheck> TaskChecks { get; set; }

        /// <summary>
        /// Override comparator comparison
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>int</returns>
        public int CompareTo(object? obj)
        {
            if (obj is TaskModel other) return EndTime.CompareTo(other.EndTime);
            return 0;
        }

        /// <summary>
        /// Override equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is TaskModel other) return EndTime == other.EndTime;
            return false;
        }

        /// <summary>
        /// Override get gash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return EndTime.GetHashCode();
        }
    }

    /// <summary>
    /// Enum task states
    /// </summary>
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

    /// <summary>
    /// Enum task categories
    /// </summary>
    public enum TaskCategory
    {
        [Description("Работа")]
        Work,
        [Description("Дом")]
        Home,
        [Description("Обучение")]
        Education
    }

    /// <summary>
    /// Enum task importance
    /// </summary>
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
