using AppTaskManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaskManager.Controllers
{
    public class MockTaskController : ITaskController
    {
        private List<TaskModel> _tasks = new List<TaskModel>();

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

        public void AddTask(TaskModel newTask)
        {
            if (_tasks == null)
            {
                _tasks = GetAllTasks().ToList();
            }
            _tasks.Add(newTask);
        }

        public IEnumerable<TaskModel> GetAllTasks()
        {
            _tasks.Sort((x, y) => x.CompareTo(y));
            return _tasks;
        }

        public void SaveTasks(IEnumerable<TaskModel> tasks)
        {
            _tasks.Clear();
            foreach (TaskModel task in tasks)
            {
                _tasks.Add(task);
            }
        }

        public void UpdateTask(TaskModel updatedTask)
        {
            if (updatedTask == null)
            {
                return;
            }
            TaskModel task = _tasks.FirstOrDefault(t => t.Id == updatedTask.Id);
            if (task == null)
            {
                _tasks.Add(updatedTask);
            }
            else
            {
                {
                    task = updatedTask;
                }
            }
        }

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
        /// Generate new task id: This method generate new id for new task
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
