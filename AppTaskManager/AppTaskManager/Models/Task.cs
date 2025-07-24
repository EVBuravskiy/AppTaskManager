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
    public class Task
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
        /// Date and time of task create
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Date and time of task begining
        /// </summary>
        public DateTime? StartDateTime { get; set; }

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
        public List<TaskChecklist> TaskChecklist {get; set;} = new List<TaskChecklist>();
    }

    /// <summary>
    /// Task Processing
    /// </summary>
    public enum TaskState
    {
        /// <summary>
        /// The task is still in progress
        /// </summary>
        InProgress,
        /// <summary>
        /// The task has been marked as completed
        /// </summary>
        Complete,
        /// <summary>
        /// The task hasn't been started yet
        /// </summary>
        NotStarted,
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
        Personal,
        /// <summary>
        /// Household chores, repairs, or home improvement projects.
        /// </summary>
        Home,
        /// <summary>
        /// Exercive routines, meal planning, doctor's appointments, or meditation.
        /// </summary>
        HealthWelbeing,
        /// <summary>
        /// Budgeting, bill payments, and financial planning.
        /// </summary>
        Finance,
        /// <summary>
        /// Grocery lists, clothing, or other shopping needs.
        /// </summary>
        Shopping,
        /// <summary>
        /// Family gatherings, social events, or activities with friends
        /// </summary>
        SocialFamily,
        /// <summary>
        /// Study session, assignments, or education goals.
        /// </summary>
        Education,
        /// <summary>
        /// Planning for trips, packing lists, or travel itineraries.
        /// </summary>
        Travel,
        /// <summary>
        /// Tasks like going to the post office, bank, or dry cleaners.
        /// </summary>
        Errands,
        /// <summary>
        /// Time set aside for hobbies or leisure activities
        /// </summary>
        HobbiesLeisure,
        /// <summary>
        /// Community service or volunteering activities.
        /// </summary>
        VolunteeringCommunity,
        /// <summary>
        /// Spectial dates and colobrations.
        /// </summary>
        BurthdaysAnniversaries,
        /// <summary>
        /// Larger tasks that wight span over multiple days or weeks.
        /// </summary>
        Projects,
        /// <summary>
        /// Objectives or goals that you're working towards over a longer period.
        /// </summary>
        LongTermGoals,
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
