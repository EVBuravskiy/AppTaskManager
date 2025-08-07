using System;
using System.Collections.Generic;
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
        Create,
        InProgress,
        Completed,
        Late,
        Deleted
    }

    public enum TaskCategory
    {
        Work,
        Home,
        Education
    }

    public enum TaskImportance
    {
        Low,
        Medium,
        High,
        Critical,
    }
}
