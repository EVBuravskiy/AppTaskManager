using AppTaskManager.Models;

namespace AppTaskManager.Controllers
{
    /// <summary>
    /// Controller interface for working with tasks
    /// </summary>
    public interface ITaskController
    {
        /// <summary>
        /// Get all task: Method for get all tasks
        /// </summary>
        /// <returns>Collection of tasks</returns>
        IEnumerable<TaskModel> GetAllTasks();

        /// <summary>
        /// Save tasks: Method for saving tasks
        /// </summary>
        /// <param name="tasks"></param>
        void SaveTasks(IEnumerable<TaskModel> tasks);

        /// <summary>
        /// Add task: Method for add new task
        /// </summary>
        /// <param name="newTask"></param>
        void AddTask(TaskModel newTask);

        /// <summary>
        /// Update task: Method for update task
        /// </summary>
        /// <param name="task"></param>
        void UpdateTask(TaskModel task);

        /// <summary>
        /// Delete task: Method for delete task
        /// </summary>
        /// <param name="task"></param>
        void DeleteTask(TaskModel task);

        /// <summary>
        /// Generate Id for task: Method generate new id for task
        /// </summary>
        /// <returns></returns>
        public int GenerateNewTaskId();
    }
}
