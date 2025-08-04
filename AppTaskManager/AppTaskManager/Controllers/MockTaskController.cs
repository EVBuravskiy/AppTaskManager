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
                CreationTime = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(10),
                IsCompleted = false,
                TaskState = TaskState.Create,
                TaskCategory = TaskCategory.Education,
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
                CreationTime = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(15),
                IsCompleted = false,
                TaskState = TaskState.Create,
                TaskCategory = TaskCategory.Education,
                TaskImportance = TaskImportance.High,
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

            _tasks.Add(firstTaskModel);
            _tasks.Add(secondTaskModel);
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
