using AppTaskManager.Models;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;


namespace AppTaskManager.Controllers
{
    public class TaskJsonController : ITaskController
    {

        private readonly string _dataPath = "taskdata.json";

        public void AddTask(TaskModel newTask)
        {
            List<TaskModel> _tasks = GetAllTasks().ToList();
            _tasks.Add(newTask);
            SaveTasks(_tasks);
        }

        public void DeleteTask(TaskModel task)
        {
            List<TaskModel> _tasks = GetAllTasks().ToList();
            _tasks.Remove(task);
            SaveTasks(_tasks);
        }

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

        public void SaveTasks(IEnumerable<TaskModel> tasks)
        {
            var serializedTasks = JsonConvert.SerializeObject(tasks);
            if (!File.Exists(_dataPath))
            {
                File.Create(_dataPath).Close();
            }
            File.WriteAllText(_dataPath, serializedTasks);
        }

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
