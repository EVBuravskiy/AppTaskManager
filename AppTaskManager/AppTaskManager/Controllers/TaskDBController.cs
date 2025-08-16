using AppTaskManager.Models;

namespace AppTaskManager.Controllers
{
    /// <summary>
    /// Task DB controller
    /// </summary>
    public class TaskDBController : ITaskController
    {
        /// <summary>
        /// Add Task: This method adds a new task to the tasks and saves it to the DB.
        /// </summary>
        /// <param name="newTask"></param>
        public void AddTask(TaskModel newTask)
        {
            using (AppContext context = new AppContext())
            {
                context.Add(newTask);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete Task: This method removes the task from the task list and saves it to the DB.
        /// </summary>
        /// <param name="task"></param>
        public void DeleteTask(TaskModel task)
        {
            using (AppContext context = new AppContext())
            {
                TaskModel deletedTask = context.TaskModels.FirstOrDefault(task => task.Id == task.Id);
                if (deletedTask != null)
                {
                    context.Remove(deletedTask);
                }
                IEnumerable<TaskCheck> taskChecks = context.TaskChecks.Where(check => check.TaskModel.Id == task.Id);
                if(taskChecks != null)
                {
                    context.RemoveRange(taskChecks);
                }
                context.SaveChanges();
            }
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
        /// Get all tasks: This method allows you to get and sort all tasks from DB.
        /// </summary>
        /// <returns>Collection of tasks</returns>
        public IEnumerable<TaskModel> GetAllTasks()
        {
            using (AppContext context = new AppContext())
            {
                List<TaskModel> taskModels = context.TaskModels.ToList();
                List<TaskCheck> taskChecks = context.TaskChecks.ToList();
                foreach (var task in taskModels)
                {
                    task.TaskChecks = new List<TaskCheck>();
                    foreach(var check in taskChecks)
                    {
                        if(check.TaskModel.Id == task.Id)
                        {
                            task.TaskChecks.Add(check);
                        }
                    }
                }
                taskModels.Sort((x, y) => x.CompareTo(y));
                return taskModels;
            }
        }

        /// <summary>
        /// Update Task: This method adds tasks in the collection and saves it to the DB.
        /// </summary>
        /// <param name="tasks"></param>
        public void SaveTasks(IEnumerable<TaskModel> tasks)
        {
            using (AppContext context = new AppContext())
            {
                context.TaskModels.AddRange(tasks);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update Task: This method updates the task in the collection and saves it to the DB.
        /// </summary>
        /// <param name="task"></param>
        public void UpdateTask(TaskModel inputTask)
        {
            if (inputTask.Id == 0) return;
            using (AppContext context = new AppContext())
            {
                TaskModel DBTask = context.TaskModels.FirstOrDefault(t => t.Id == inputTask.Id);
                if (DBTask != null)
                {
                    DBTask.Title = inputTask.Title;
                    DBTask.Description = inputTask.Description;
                    DBTask.CreationTime = inputTask.CreationTime;
                    DBTask.StartTime = inputTask.StartTime;
                    DBTask.EndTime = inputTask.EndTime;
                    DBTask.IsCompleted = inputTask.IsCompleted;
                    DBTask.TaskState = inputTask.TaskState;
                    DBTask.TaskCategory = inputTask.TaskCategory;
                    DBTask.TaskImportance = inputTask.TaskImportance;
                    IEnumerable<TaskCheck>? checks = context.TaskChecks.Where(check => check.TaskModel.Id == inputTask.Id);
                    if (checks != null)
                    {
                        context.TaskChecks.RemoveRange(checks);
                    }
                    DBTask.TaskChecks = inputTask.TaskChecks;
                    context.SaveChanges();
                }
            }
        }
    }
}
