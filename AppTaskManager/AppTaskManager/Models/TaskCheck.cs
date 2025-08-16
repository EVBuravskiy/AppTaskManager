namespace AppTaskManager.Models
{
    /// <summary>
    /// Check for task
    /// </summary>
    public class TaskCheck
    {
        /// <summary>
        /// Property storing check ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Property storing task model
        /// </summary>
        public TaskModel TaskModel { get; set; }

        /// <summary>
        /// Property storing check description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Property storing a pointer to the execution state of check
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Override equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is TaskCheck other) return Description == other.Description;
            return false;
        }

        /// <summary>
        /// Override get gash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Description.GetHashCode();
        }
    }
}
