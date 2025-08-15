using AppTaskManager.Models;

namespace AppTaskManager.Controllers
{
    /// <summary>
    /// Mock task controller
    /// </summary>
    public class MockTaskController : ITaskController
    {
        /// <summary>
        /// Collection of tasks
        /// </summary>
        private List<TaskModel> _tasks = new List<TaskModel>();

        /// <summary>
        /// Mock controller constructor with initialize collection of tasks
        /// </summary>
        public MockTaskController() 
        {
            TaskModel firstTaskModel = new TaskModel()
            {
                Id = 1,
                Title = "First task",
                Description = "Description of first task",
                CreationTime = DateTime.Today,
                StartTime = DateTime.Today,
                EndTime = DateTime.Today.AddDays(1),
                IsCompleted = false,
                TaskState = TaskState.Create,
                TaskCategory = TaskCategory.Work,
                TaskImportance = TaskImportance.High,
                TaskChecks = new List<TaskCheck>(),
            };
            TaskCheck firstTaskCheckForFirstTask = new TaskCheck()
            {
                Id = 1,
                TaskModel = firstTaskModel,
                Description = "1. Check for first task",
                IsComplete = false,
            };
            TaskCheck secondTaskCheckForFirstTask = new TaskCheck()
            {
                Id = 2,
                TaskModel = firstTaskModel,
                Description = "2. Check for first task",
                IsComplete = false,
            };
            firstTaskModel.TaskChecks.Add(firstTaskCheckForFirstTask);
            firstTaskModel.TaskChecks.Add(secondTaskCheckForFirstTask);

            TaskModel secondTaskModel = new TaskModel()
            {
                Id = 2,
                Title = "Second task",
                Description = "Description of second task",
                CreationTime = DateTime.Today,
                StartTime = DateTime.Today,
                EndTime = DateTime.Today.AddDays(2),
                IsCompleted = false,
                TaskState = TaskState.Create,
                TaskCategory = TaskCategory.Education,
                TaskImportance = TaskImportance.Low,
                TaskChecks = new List<TaskCheck>(),
            };
            TaskCheck firstTaskCheckForSecondTask = new TaskCheck()
            {
                Id = 1,
                TaskModel = firstTaskModel,
                Description = "1. Check for second task",
                IsComplete = false,
            };
            TaskCheck secondTaskCheckForSecondTask = new TaskCheck()
            {
                Id = 2,
                TaskModel = firstTaskModel,
                Description = "2. Check for second task",
                IsComplete = false,
            };
            secondTaskModel.TaskChecks.Add(firstTaskCheckForSecondTask);
            secondTaskModel.TaskChecks.Add(secondTaskCheckForSecondTask);

            TaskModel thirdTaskModel = new TaskModel()
            {
                Id = 3,
                Title = "Third task",
                Description = "Description of third task",
                CreationTime = DateTime.Today,
                StartTime = DateTime.Today,
                EndTime = DateTime.Today.AddDays(3),
                IsCompleted = false,
                TaskState = TaskState.Create,
                TaskCategory = TaskCategory.Home,
                TaskImportance = TaskImportance.Critical,
                TaskChecks = new List<TaskCheck>(),
            };
            TaskCheck firstTaskCheckForThirdTask = new TaskCheck()
            {
                Id = 1,
                TaskModel = firstTaskModel,
                Description = "1. Check for third task",
                IsComplete = false,
            };
            TaskCheck secondTaskCheckForThirdTask = new TaskCheck()
            {
                Id = 2,
                TaskModel = firstTaskModel,
                Description = "2. Check for third task",
                IsComplete = false,
            };
            thirdTaskModel.TaskChecks.Add(firstTaskCheckForThirdTask);
            thirdTaskModel.TaskChecks.Add(secondTaskCheckForThirdTask);

            TaskModel fourthTaskModel = new TaskModel()
            {
                Id = 4,
                Title = "Fourth task",
                Description = "Description of fourth task",
                CreationTime = DateTime.Today,
                StartTime = DateTime.Today,
                EndTime = DateTime.Today.AddDays(4),
                IsCompleted = false,
                TaskState = TaskState.Create,
                TaskCategory = TaskCategory.Work,
                TaskImportance = TaskImportance.Medium,
                TaskChecks = new List<TaskCheck>(),
            };
            TaskCheck firstTaskCheckForFourthTask = new TaskCheck()
            {
                Id = 1,
                TaskModel = firstTaskModel,
                Description = "1. Check for fourth task",
                IsComplete = false,
            };
            TaskCheck secondTaskCheckForFourthTask = new TaskCheck()
            {
                Id = 2,
                TaskModel = firstTaskModel,
                Description = "2. Check for fourth task",
                IsComplete = false,
            };
            fourthTaskModel.TaskChecks.Add(firstTaskCheckForFourthTask);
            fourthTaskModel.TaskChecks.Add(secondTaskCheckForFourthTask);

            TaskModel fifthTaskModel = new TaskModel()
            {
                Id = 5,
                Title = "Fifth task",
                Description = "Description of fifth task",
                CreationTime = DateTime.Today,
                StartTime = DateTime.Today,
                EndTime = DateTime.Today.AddDays(-1),
                IsCompleted = false,
                TaskState = TaskState.Create,
                TaskCategory = TaskCategory.Work,
                TaskImportance = TaskImportance.Medium,
                TaskChecks = new List<TaskCheck>(),
            };
            TaskCheck firstTaskCheckForFifthTask = new TaskCheck()
            {
                Id = 1,
                TaskModel = firstTaskModel,
                Description = "1. Check for fifth task",
                IsComplete = false,
            };
            TaskCheck secondTaskCheckForFifthTask = new TaskCheck()
            {
                Id = 2,
                TaskModel = firstTaskModel,
                Description = "2. Check for fifth task",
                IsComplete = false,
            };
            fifthTaskModel.TaskChecks.Add(firstTaskCheckForFifthTask);
            fifthTaskModel.TaskChecks.Add(secondTaskCheckForFifthTask);

            _tasks.Add(firstTaskModel);
            _tasks.Add(secondTaskModel);
            _tasks.Add(thirdTaskModel);
            _tasks.Add(fourthTaskModel);
            _tasks.Add(fifthTaskModel);
        }

        /// <summary>
        /// Add task: this method for add new task to collection
        /// </summary>
        /// <param name="newTask"></param>
        public void AddTask(TaskModel newTask)
        {
            if (_tasks == null)
            {
                _tasks = GetAllTasks().ToList();
            }
            _tasks.Add(newTask);
        }

        /// <summary>
        /// Get all task: this method for get and sort all tasks from collection
        /// </summary>
        /// <returns>Collection of tasks</returns>
        public IEnumerable<TaskModel> GetAllTasks()
        {
            _tasks.Sort((x, y) => x.CompareTo(y));
            return _tasks;
        }

        /// <summary>
        /// Save tasks: this method for saving tasks into collection
        /// </summary>
        /// <param name="tasks"></param>
        public void SaveTasks(IEnumerable<TaskModel> tasks)
        {
            _tasks.Clear();
            foreach (TaskModel task in tasks)
            {
                _tasks.Add(task);
            }
        }

        /// <summary>
        /// Update task: this method update task in collection
        /// </summary>
        /// <param name="task"></param>
        public void UpdateTask(TaskModel updatedTask)
        {
            if (updatedTask == null)
            {
                return;
            }
            TaskModel taskModel = _tasks.FirstOrDefault(t => t.Id == updatedTask.Id);
            if (taskModel == null)
            {
                _tasks.Add(updatedTask);
            }
            else
            {
                {
                    taskModel.Id = updatedTask.Id;
                    taskModel.Title = updatedTask.Title;
                    taskModel.Description = updatedTask.Description;
                    taskModel.CreationTime = updatedTask.CreationTime;
                    taskModel.StartTime = updatedTask.StartTime;
                    taskModel.EndTime = updatedTask.EndTime;
                    taskModel.IsCompleted = updatedTask.IsCompleted;
                    taskModel.TaskState = updatedTask.TaskState;
                    taskModel.TaskCategory = updatedTask.TaskCategory;
                    taskModel.TaskImportance = updatedTask.TaskImportance;
                    taskModel.TaskChecks.Clear();
                    foreach(TaskCheck check in updatedTask.TaskChecks)
                    {
                        taskModel.TaskChecks.Add(check);
                    }
                }
            }
        }

        /// <summary>
        /// Delete task: this method delete task from collection
        /// </summary>
        /// <param name="task"></param>
        public void DeleteTask(TaskModel deletedTask)
        {
            if(deletedTask == null)
            {
                return;
            }
            TaskModel task = _tasks.FirstOrDefault(t => t.Id == deletedTask.Id);
            if (task == null)
            {
                return;
            }
            else
            {
                _tasks.Remove(deletedTask);
            }
        }

        /// <summary>
        /// Generate new task id: this method generate new id for new task
        /// </summary>
        /// <returns>new id</returns>
        public int GenerateNewTaskId()
        {
            var tasks = _tasks;
            if (!tasks.Any())
            {
                return 1;
            }
            int maxId = tasks.Max(t => t.Id);
            return maxId + 1;
        }
    }
}
