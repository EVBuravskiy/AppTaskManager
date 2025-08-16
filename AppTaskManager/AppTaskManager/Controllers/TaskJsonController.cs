using AppTaskManager.Models;
using Newtonsoft.Json;
using System.IO;


namespace AppTaskManager.Controllers
{
    /// <summary>
    /// Task json controller
    /// </summary>
    public class TaskJsonController : ITaskController
    {
        /// <summary>
        /// Field containing the path to the file
        /// </summary>
        private readonly string _dataPath = "taskdata.json";

        /// <summary>
        /// Add Task: This method adds a new task to the tasks and saves the JSON.
        /// </summary>
        /// <param name="newTask"></param>
        public void AddTask(TaskModel newTask)
        {
            List<TaskModel> _tasks = GetAllTasks().ToList();
            _tasks.Add(newTask);
            SaveTasks(_tasks);
        }

        /// <summary>
        /// Delete Task: This method removes the task from the task list and saves the JSON.
        /// </summary>
        /// <param name="task"></param>
        public void DeleteTask(TaskModel task)
        {
            List<TaskModel> _tasks = GetAllTasks().ToList();
            _tasks.Remove(task);
            SaveTasks(_tasks);
        }

        /// <summary>
        /// Generate New Task ID: This method generates a new ID for a new task.
        /// </summary>
        /// <returns>new id</returns>
        public int GenerateNewTaskId()
        {
            List<TaskModel> _tasks = GetAllTasks().ToList();
            if (!_tasks.Any())
            {
                return 1;
            }
            int maxId = _tasks.Max(t => t.Id);
            return maxId + 1;
        }

        /// <summary>
        /// Get all tasks: This method allows you to get and sort all tasks from JSON.
        /// </summary>
        /// <returns>Collection of tasks</returns>
        public IEnumerable<TaskModel> GetAllTasks()
        {
            if (!File.Exists(_dataPath))
            {
                File.Create(_dataPath).Close();
            }
            string serializedTasks = File.ReadAllText(_dataPath);
            List<TaskModel> _tasks = JsonConvert.DeserializeObject<IEnumerable<TaskModel>>(serializedTasks).ToList();
            if (_tasks == null)
            {
                return new List<TaskModel>();
            }
            _tasks.Sort((x, y) => x.CompareTo(y));
            return _tasks;
        }

        /// <summary>
        /// Update Task: This method adds tasks in the collection and saves it in JSON format.
        /// </summary>
        /// <param name="tasks"></param>
        public void SaveTasks(IEnumerable<TaskModel> tasks)
        {
            var serializedTasks = JsonConvert.SerializeObject(tasks);
            if (!File.Exists(_dataPath))
            {
                File.Create(_dataPath).Close();
            }
            File.WriteAllText(_dataPath, serializedTasks);
        }

        /// <summary>
        /// Update Task: This method updates the task in the collection and saves it in JSON format.
        /// </summary>
        /// <param name="task"></param>
        public void UpdateTask(TaskModel inputTask)
        {
            List<TaskModel> _tasks = GetAllTasks().ToList();
            int index = _tasks.IndexOf(inputTask);
            if (index == -1)
            {
                return;
            }
            TaskModel currentTask = _tasks[index];
            if (currentTask == null)
            {
                return;
            }
            else
            {
                currentTask.Id = inputTask.Id;
                currentTask.Title = inputTask.Title;
                currentTask.Description = inputTask.Description;
                currentTask.CreationTime = inputTask.CreationTime;
                currentTask.StartTime = inputTask.StartTime;
                currentTask.EndTime = inputTask.EndTime;
                currentTask.IsCompleted = inputTask.IsCompleted;
                currentTask.TaskState = inputTask.TaskState;
                currentTask.TaskCategory = inputTask.TaskCategory;
                currentTask.TaskImportance = inputTask.TaskImportance;
                currentTask.TaskChecks.Clear();
                foreach (TaskCheck check in inputTask.TaskChecks)
                {
                    currentTask.TaskChecks.Add(check);
                }
            }
            SaveTasks(_tasks);
        }
    }
}
