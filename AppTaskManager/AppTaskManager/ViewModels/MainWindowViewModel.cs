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
        private TasksViewMainWindow TasksViewMainWindow;

        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get { return _selectedTask; }
            set 
            { 
                _selectedTask = value;
                if(value == null)
                {
                    CreateDefaultSelectTask();
                }
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

        public bool checkBoxEnabled {  get; set; }

        public MainWindowViewModel()
        {
            TaskController = new MockTaskController();
            LoadTasksFromController();
            CreateDefaultSelectTask();
            TaskImportance = SelectedTask.TaskImportance;
        }

        private void CreateDefaultSelectTask()
        {
            _selectedTask = new TaskModel()
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
        }

        private void LoadTasksFromController()
        {
            if (TaskModels == null)
            {
                TaskModels = new ObservableCollection<TaskModel>();
            }
            TaskModels.Clear();
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                if (!task.IsCompleted)
                {
                    TaskModels.Add(task);
                }
            }
        }

        public void AddTaskToTaskModels(TaskModel newTask)
        {
            TaskController.AddTask(newTask);
            LoadTasksFromController();
            if(SelectedTask == null)
            {
                CreateDefaultSelectTask();
            }
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

        public ICommand ISaveChanges => new RelayCommand(update => UpdateTask());

        public void UpdateTask()
        {
            TaskController.UpdateTask(SelectedTask);
            LoadTasksFromController();
        }

        public ICommand IOpenEditTask => new RelayCommand(open => OpenNewWindow(false));

        public ICommand IOpenNewWindow => new RelayCommand(open => OpenNewWindow());
        private void OpenNewWindow(bool isNew = true)
        {
            if(SelectedTask.Id == 0)
            {
                isNew = true;
            }
            NewTaskWindow newTaskWindow = new NewTaskWindow(this, isNew);
            newTaskWindow.Show();
        }

        public ICommand IBeginTask => new RelayCommand(begin => BeginTask());
        private void BeginTask()
        {
            if (SelectedTask.Id == 0)
            {
                return;
            }
            SelectedTask.StartTime = DateTime.Now;
            UpdateTask();
        }

        public ICommand ICompletedCommand => new RelayCommand(complete => CompleteTask());

        private void CompleteTask()
        {
            foreach(TaskCheck check in SelectedTask.TaskChecks)
            {
                if(check.IsComplete != true)
                {
                    MessageBox.Show("Невозможно завершить выполнение задачи. Не весь контроль выполнен");
                    return;
                }
            }
            SelectedTask.IsCompleted = true;
            UpdateTask();
        }

        public ICommand DeleteTaskCommand => new RelayCommand(delete => DeleteTask());

        /// <summary>
        /// Delete task: This method remove task from the list of tasks from the TaskDataService and updates the Tasks collection.
        /// </summary>
        private void DeleteTask()
        {
            TaskController.DeleteTask(SelectedTask);
            MessageBox.Show("Удаление задачи успешно завершено");
            CreateDefaultSelectTask();
            TaskImportance = SelectedTask.TaskImportance;
            LoadTasksFromController();
        }
    }
}
