using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace AppTaskManager.Models
{
    /// <summary>
    /// Main class - task 
    /// </summary>
    public class TaskModel
    {
        /// <summary>
        /// Task ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Task Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Description of the task
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Date and time of task creation
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Task start date and time
        /// </summary>
        public DateTime? StartDateTime { get; set; }


        /// <summary>
        /// Task end date and time
        /// </summary>
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Indicator whether the task has been completed
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Execution Timer
        /// </summary>
        public TimeSpan Timer { get; set; }

        /// <summary>
        /// Task Processing
        /// </summary>
        public TaskState TaskState { get; set; }

        /// <summary>
        /// Task category
        /// </summary>
        public TaskCategory TaskCategory { get; set; }

        /// <summary>
        /// Task importance
        /// </summary>
        public TaskImportance TaskImportance { get; set; }

        /// <summary>
        /// Collection of TaskCheckList
        /// </summary>
        public List<TaskCheck> TaskChecklist {get; set;} = new List<TaskCheck>();
    }

    /// <summary>
    /// Task Processing
    /// </summary>
    public enum TaskState
    {
        /// <summary>
        /// The task hasn't been started yet
        /// </summary>
        NotStarted,
        /// <summary>
        /// The task is still in progress
        /// </summary>
        InProgress,
        /// <summary>
        /// The task has been marked as completed
        /// </summary>
        Complete,
        /// <summary>
        /// The task is late
        /// </summary>
        Late,
        /// <summary>
        /// The task has been archived
        /// </summary>        
        Archived,
        /// <summary>
        /// The task has been marked as deleted
        /// </summary>
        Deleted,
    }

    /// <summary>
    /// Task categories
    /// </summary>
    public enum TaskCategory
    {
        /// <summary>
        /// Task related to your job, like meetings, project deadlines, or professional development activities.
        /// </summary>
        Work,
        /// <summary>
        /// Daily routines, hobbies, or personal goals.
        /// </summary>
        Home,
        /// <summary>
        /// Exercive routines, meal planning, doctor's appointments, or meditation.
        /// </summary>
        Education,
        /// <summary>
        /// Planning for trips, packing lists, or travel itineraries.
        /// </summary>
        Projects
    }

    /// <summary>
    /// Task importance
    /// </summary>
    public enum TaskImportance
    {
        /// <summary>
        /// Low importance. Suitable for tasks that are not urgent and can be deferred
        /// </summary>
        Low,
        /// <summary>
        /// Medium importance. For tasks that are of regular priority
        /// </summary>
        Medium,
        /// <summary>
        /// High importance. Tasks that need to be completed soon, but are not critical
        /// </summary>
        High,
        /// <summary>
        /// Critical importance. These tasks have the highest priority and often have tight deadlines
        /// </summary>
        Critical,
    }
}
