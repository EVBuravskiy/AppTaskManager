using AppTaskManager.Controllers;
using AppTaskManager.Models;
using AppTaskManager.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AppTaskManager.ViewModels
{
    public class MainWindowViewModel : BaseObservableClass
    {

        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get { return _selectedTask; }
            set 
            { 
                _selectedTask = value;
                TaskImportance = _selectedTask.TaskImportance;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        private TaskImportance _taskImportance;
        public TaskImportance TaskImportance
        {
            get { return _taskImportance; }
            set
            {
                _taskImportance = value;
                OnPropertyChanged(nameof(TaskImportance));
            }
        }

        public ITaskController TaskController;

        public ObservableCollection<TaskModel> TaskModels { get; private set; }

        private string _searchTitle;
        public string SearchTitle
        {
            get { return _searchTitle; }
            set 
            { 
                _searchTitle = value; 
                OnPropertyChanged(nameof(SearchTitle));
            }
        }

        public MainWindowViewModel()
        {
            TaskController = new MockTaskController();
            TaskModels = new ObservableCollection<TaskModel>();
            LoadTasksFromController();
            SelectedTask = new TaskModel()
            {
                Id = 0,
                Title = "Наименование задачи",
                Description = "Описание задачи",
                CreationTime = DateTime.Now,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                IsCompleted = false,
                TaskState = TaskState.Create,
                TaskCategory = TaskCategory.Work,
                TaskImportance = TaskImportance.Low,
                TaskChecks = new List<TaskCheck>()
            };
            TaskImportance = SelectedTask.TaskImportance;
        }

        private void LoadTasksFromController()
        {
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                TaskModels.Add(task);
            }
        }

        public void AddTaskToTaskModels(TaskModel newTask)
        {
            TaskModels.Add(newTask);
            OnPropertyChanged(nameof(TaskModel));
        }

        public ICommand IFindTasks => new RelayCommand(find => FindTitleInTasks());
        public void FindTitleInTasks()
        {
            ObservableCollection<TaskModel> searchTask = new ObservableCollection<TaskModel>();
            if (SearchTitle == String.Empty)
            {
                LoadTasksFromController();
                return;
            }
            foreach (TaskModel task in TaskModels)
            {
                var title = task.Title.ToLower();
                if (title.Contains(_searchTitle.ToLower()))
                {
                    searchTask.Add(task);
                }
            }
            if (searchTask.Count != 0)
            {
                TaskModels.Clear();
                foreach (TaskModel task in searchTask)
                {
                    TaskModels.Add(task);
                }
            }
            else
            {
                MessageBox.Show("Задача с таким названием отсутствует");
            }
        }

        public ICommand IOpenNewWindow => new RelayCommand(open => OpenNewWindow());
        private void OpenNewWindow()
        {
            NewTaskWindow newTaskWindow = new NewTaskWindow(this);
            newTaskWindow.Show();
        }

        /// <summary>
        /// Update task: This method update task into the list of tasks from the TaskDataService and updates the Tasks collection.
        /// </summary>
        public void UpdateTask(TaskModel updateTask)
        {
            //_taskController.UpdateTask(updateTask);
            //LoadTasks();
        }

        /// <summary>
        /// Delete task: This method remove task from the list of tasks from the TaskDataService and updates the Tasks collection.
        /// </summary>
        public void DeleteTask(TaskModel taskModel)
        {
            //_taskController.DeleteTask(taskModel);
            //LoadTasks();
        }
    }
}
