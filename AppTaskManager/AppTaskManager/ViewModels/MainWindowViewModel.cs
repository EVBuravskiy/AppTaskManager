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
using System.Windows.Media.Animation;
using static AppTaskManager.Views.TasksViewMainWindow;

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
                if (value == null)
                {
                    CreateDefaultSelectTask();
                }
                TaskImportance = _selectedTask.TaskImportance;
                foreach (var check in _selectedTask.TaskChecks)
                {
                    if (check.IsComplete == false)
                    {
                        IsCompleted = false;
                        break;
                    }
                    IsCompleted = true;
                }
                CheckCount = _selectedTask.TaskChecks.Count;
                CheckChecked = 0;
                foreach (var check in _selectedTask.TaskChecks)
                {
                    if (check.IsComplete == true)
                    {
                        CheckChecked++;
                    }
                }
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

        public ObservableCollection<TaskModel> EndTimeTaskModels {  get; private set; }

        private DateTime _selectedDate;
        public DateTime SelectedDate 
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                FindTasksByDate();
            }
        }

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

        public bool checkBoxEnabled { get; set; }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
            }
        }

        private double _checkCount;
        public double CheckCount {
            get => _checkCount;
            set
            {
                _checkCount = value;
                OnPropertyChanged(nameof(CheckCount));
            }
        }

        private double _checkChecked;
        public double CheckChecked 
        {
            get => _checkChecked;
            set
            {
                _checkChecked = value;
                OnPropertyChanged(nameof(CheckChecked));
            }
        }

        public ICodeBehind MainWindowCodeBehind { get; set; }

        public List<DateTime> TasksDates { get; set; }

        private List<DateTime> _selectedDates;
        public List<DateTime> SelectedDates 
        { 
            get => _selectedDates;
            set
            {
                _selectedDates = value;
                OnPropertyChanged(nameof(SelectedDates));
            }
        }

        public MainWindowViewModel(TasksViewMainWindow mainWindow)
        {
            TaskController = new MockTaskController();
            LoadUncompletedTasks();
            TaskImportance = SelectedTask.TaskImportance;
            InitializeEndTimeTask();
            InitializeDates();
            SelectedDates = new List<DateTime>();

            MainWindowCodeBehind = mainWindow;
            MainWindowCodeBehind.SelectManyDates(TasksDates);
        }

        private void CreateDefaultSelectTask()
        {
            _selectedTask = new TaskModel()
            {
                Id = 0,
                Title = "Наименование задачи",
                Description = "Описание задачи",
                CreationTime = DateTime.Today,
                StartTime = DateTime.Today,
                EndTime = DateTime.Today,
                IsCompleted = false,
                TaskState = TaskState.Create,
                TaskCategory = TaskCategory.Work,
                TaskImportance = TaskImportance.Low,
                TaskChecks = new List<TaskCheck>()
            };
        }

        private void InitializeEndTimeTask()
        {
            var currentTime = DateTime.Today;
            if (EndTimeTaskModels == null)
            {
                EndTimeTaskModels = new ObservableCollection<TaskModel>();
            }
            EndTimeTaskModels.Clear();
            foreach(var task in TaskModels)
            {
                var checkTime = task.EndTime.AddDays(-3);
                int result1 = DateTime.Compare(currentTime, checkTime);
                if (result1 >= 0 && (task.TaskState != TaskState.Completed || task.TaskState != TaskState.Deleted))
                {
                    EndTimeTaskModels.Add(task);
                    task.TaskImportance = TaskImportance.High;
                }
                int result2 = DateTime.Compare(currentTime, task.EndTime);
                if (result2 > 0 && (task.TaskState != TaskState.Completed || task.TaskState != TaskState.Deleted))
                {
                    task.TaskState = TaskState.Late;
                    task.TaskImportance = TaskImportance.High;
                }
            }
        }

        private void InitializeDates()
        {
            TasksDates = new List<DateTime>();
            foreach (var task in TaskModels)
            {
                TasksDates.Add(task.EndTime);
            }
        }

        public void LoadTasksSelectedDates(List<DateTime> selectedDates)
        {
            if (TaskModels == null)
            {
                TaskModels = new ObservableCollection<TaskModel>();
            }
            TaskModels.Clear();
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                foreach (var date in selectedDates)
                {
                    if (task.EndTime == date) TaskModels.Add(task);
                }
            }
            if(TaskModels.Count == 0)
            {
                MessageBox.Show("В заданном диапазоне задачи не найдены");
                LoadUncompletedTasks();
            }
        }

        public ICommand ILoadUncompletedTasks => new RelayCommand(all => LoadUncompletedTasks());
        private void LoadUncompletedTasks()
        {
            if (TaskModels == null)
            {
                TaskModels = new ObservableCollection<TaskModel>();
            }
            TaskModels.Clear();
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                if (task.TaskState != TaskState.Completed && task.TaskState != TaskState.Deleted)
                {
                    TaskModels.Add(task);
                }
            }
            if (TaskModels.Count > 0)
            {
                if (SelectedTask == null || SelectedTask.Id == 0)
                {
                    SelectedTask = TaskModels.First();
                }
            }
            else if(SelectedTask.Id != 0) CreateDefaultSelectTask();
        }

        public ICommand ILoadAllTasks => new RelayCommand(all => LoadAllTasks());
        private void LoadAllTasks()
        {
            if (TaskModels == null)
            {
                TaskModels = new ObservableCollection<TaskModel>();
            }
            TaskModels.Clear();
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                TaskModels.Add(task);
            }
            if (TaskModels.Count > 0)
            {
                if (SelectedTask == null || SelectedTask.Id == 0)
                {
                    SelectedTask = TaskModels.First();
                }
            }
            else if (SelectedTask.Id != 0) CreateDefaultSelectTask();
        }

        public ICommand ILoadCriticalTasks => new RelayCommand(critical => LoadCriticalTasks());
        private void LoadCriticalTasks()
        {
            if (TaskModels == null)
            {
                TaskModels = new ObservableCollection<TaskModel>();
            }
            TaskModels.Clear();
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                if (task.TaskImportance == TaskImportance.Critical)
                {
                    TaskModels.Add(task);
                }
            }
            if (TaskModels.Count > 0)
            {
                if (SelectedTask == null || SelectedTask.Id == 0)
                {
                    SelectedTask = TaskModels.First();
                }
            }
            else if (SelectedTask.Id != 0) CreateDefaultSelectTask();
        }

        public ICommand ILoadHighTasks => new RelayCommand(high => LoadHighTasks());
        private void LoadHighTasks()
        {
            if (TaskModels == null)
            {
                TaskModels = new ObservableCollection<TaskModel>();
            }
            TaskModels.Clear();
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                if (task.TaskImportance == TaskImportance.High)
                {
                    TaskModels.Add(task);
                }
            }
            if (TaskModels.Count > 0)
            {
                if (SelectedTask == null || SelectedTask.Id == 0)
                {
                    SelectedTask = TaskModels.First();
                }
            }
            else if (SelectedTask.Id != 0) CreateDefaultSelectTask();
        }

        public ICommand ILoadMediumTasks => new RelayCommand(medium => LoadMediumTasks());
        private void LoadMediumTasks()
        {
            if (TaskModels == null)
            {
                TaskModels = new ObservableCollection<TaskModel>();
            }
            TaskModels.Clear();
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                if (task.TaskImportance == TaskImportance.Medium)
                {
                    TaskModels.Add(task);
                }
            }
            if (TaskModels.Count > 0)
            {
                if (SelectedTask == null || SelectedTask.Id == 0)
                {
                    SelectedTask = TaskModels.First();
                }
            }
            else if (SelectedTask.Id != 0) CreateDefaultSelectTask();
        }

        public ICommand ILoadLowTasks => new RelayCommand(low => LoadLowTasks());
        private void LoadLowTasks()
        {
            if (TaskModels == null)
            {
                TaskModels = new ObservableCollection<TaskModel>();
            }
            TaskModels.Clear();
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                if (task.TaskImportance == TaskImportance.Low)
                {
                    TaskModels.Add(task);
                }
            }
            if (TaskModels.Count > 0)
            {
                if (SelectedTask == null || SelectedTask.Id == 0)
                {
                    SelectedTask = TaskModels.First();
                }
            }
            else if (SelectedTask.Id != 0) CreateDefaultSelectTask();
        }

        public ICommand ILoadCompletedTasks => new RelayCommand(completed => LoadCompletedTasks());

        private void LoadCompletedTasks()
        {
            if (TaskModels == null)
            {
                TaskModels = new ObservableCollection<TaskModel>();
            }
            TaskModels.Clear();
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                if (task.IsCompleted)
                {
                    TaskModels.Add(task);
                }
            }
            if (TaskModels.Count > 0)
            {
                if (SelectedTask == null || SelectedTask.Id == 0)
                {
                    SelectedTask = TaskModels.First();
                }
            }
            else if (SelectedTask.Id != 0) CreateDefaultSelectTask();
        }

        public ICommand ILoadDeletedTasks => new RelayCommand(deleted => LoadDeletedTasks());

        private void LoadDeletedTasks()
        {
            if (TaskModels == null)
            {
                TaskModels = new ObservableCollection<TaskModel>();
            }
            TaskModels.Clear();
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                if (task.TaskState == TaskState.Deleted)
                {
                    TaskModels.Add(task);
                }
            }
            if (TaskModels.Count > 0)
            {
                if (SelectedTask == null || SelectedTask.Id == 0)
                {
                    SelectedTask = TaskModels.First();
                }
            }
            else if (SelectedTask.Id != 0) CreateDefaultSelectTask();
        }

        public ICommand ILoadHomeTasks => new RelayCommand(deleted => LoadHomeTasks());

        private void LoadHomeTasks()
        {
            if (TaskModels == null)
            {
                TaskModels = new ObservableCollection<TaskModel>();
            }
            TaskModels.Clear();
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                if (task.TaskCategory == TaskCategory.Home)
                {
                    TaskModels.Add(task);
                }
            }
            if (TaskModels.Count > 0)
            {
                if (SelectedTask == null || SelectedTask.Id == 0)
                {
                    SelectedTask = TaskModels.First();
                }
            }
            else if (SelectedTask.Id != 0) CreateDefaultSelectTask();
        }

        public ICommand ILoadWorkTasks => new RelayCommand(deleted => LoadWorkTasks());

        private void LoadWorkTasks()
        {
            if (TaskModels == null)
            {
                TaskModels = new ObservableCollection<TaskModel>();
            }
            TaskModels.Clear();
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                if (task.TaskCategory == TaskCategory.Work)
                {
                    TaskModels.Add(task);
                }
            }
            if (TaskModels.Count > 0)
            {
                if (SelectedTask == null || SelectedTask.Id == 0)
                {
                    SelectedTask = TaskModels.First();
                }
            }
            else if (SelectedTask.Id != 0) CreateDefaultSelectTask();
        }

        public ICommand ILoadEducationTasks => new RelayCommand(deleted => LoadEducationTasks());

        private void LoadEducationTasks()
        {
            if (TaskModels == null)
            {
                TaskModels = new ObservableCollection<TaskModel>();
            }
            TaskModels.Clear();
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                if (task.TaskCategory == TaskCategory.Education)
                {
                    TaskModels.Add(task);
                }
            }
            if (TaskModels.Count > 0)
            {
                if (SelectedTask == null || SelectedTask.Id == 0)
                {
                    SelectedTask = TaskModels.First();
                }
            }
            else if (SelectedTask.Id != 0) CreateDefaultSelectTask();
        }

        public void AddTaskToTaskModels(TaskModel newTask)
        {
            TaskController.AddTask(newTask);
            LoadUncompletedTasks();
            InitializeEndTimeTask();
            if (SelectedTask == null)
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
                LoadUncompletedTasks();
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

        private void FindTasksByDate()
        {
            if (TaskModels == null)
            {
                TaskModels = new ObservableCollection<TaskModel>();
            }
            TaskModels.Clear();
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                if (task.EndTime == SelectedDate)
                {
                    TaskModels.Add(task);
                }
            }
        }

        public ICommand IBeginTask => new RelayCommand(begin => BeginTask());
        private void BeginTask()
        {
            if (SelectedTask.Id == 0)
            {
                return;
            }
            SelectedTask.StartTime = DateTime.Now;
            SelectedTask.TaskState = TaskState.InProgress;
            UpdateTask();
            GetCurrentTaskById(SelectedTask.Id);
        }


        public ICommand ISaveChanges => new RelayCommand(update => UpdateTask());

        public void UpdateTask()
        {
            if (SelectedTask.Id == 0)
            {
                return;
            }
            if (SelectedTask.StartTime == null)
            {
                SelectedTask.StartTime = DateTime.Today;
            }
            if (SelectedTask.TaskState == TaskState.Create)
            {
                SelectedTask.TaskState = TaskState.InProgress;
            }
            TaskController.UpdateTask(SelectedTask);
            GetCurrentTaskById(SelectedTask.Id);
        }

        private void GetCurrentTaskById(int currentTaskId)
        {
            if (TaskModels.Count == 0)
            {
                LoadAllTasks();
            }
            var currentTask = TaskModels.FirstOrDefault(t => t.Id == currentTaskId);
            if (currentTask != null)
            {
                SelectedTask = currentTask;
            }
            else
            {
                CreateDefaultSelectTask();
            }
        }

        public ICommand ICheckChecksComplete => new RelayCommand(checks => CheckChecksComplete());
        public void CheckChecksComplete()
        {
            if (SelectedTask.TaskState != TaskState.InProgress)
            {
                SelectedTask.TaskState = TaskState.InProgress;
            }
            if (SelectedTask.TaskChecks.Count > 0)
            {
                foreach (TaskCheck check in SelectedTask.TaskChecks)
                {
                    if (check.IsComplete != true)
                    {
                        IsCompleted = false;
                        SelectedTask.TaskState = TaskState.InProgress;
                        SelectedTask.IsCompleted = false;
                        break;
                    }
                    SelectedTask.TaskState = TaskState.Completed;
                    SelectedTask.IsCompleted = true;
                    IsCompleted = true;
                }
            }
            UpdateTask();
            CheckChecked = 0;
            foreach (var check in _selectedTask.TaskChecks)
            {
                if (check.IsComplete == true)
                {
                    CheckChecked++;
                }
            }
            if (!IsCompleted)
            {
                LoadUncompletedTasks();
                InitializeEndTimeTask();
            }
        }

        public ICommand IOpenEditTask => new RelayCommand(open => OpenNewWindow(false));

        public ICommand IOpenNewWindow => new RelayCommand(open => OpenNewWindow());
        private void OpenNewWindow(bool isNew = true)
        {
            if (SelectedTask.Id == 0)
            {
                isNew = true;
            }
            NewTaskWindow newTaskWindow = new NewTaskWindow(this, isNew);
            newTaskWindow.Show();
        }

        public ICommand ICompletedCommand => new RelayCommand(complete => CompleteTask());

        private void CompleteTask()
        {
            if (SelectedTask.Id == 0)
            {
                return;
            }
            foreach (TaskCheck check in SelectedTask.TaskChecks)
            {
                if (check.IsComplete != true)
                {
                    MessageBox.Show("Невозможно завершить выполнение задачи. Не весь контроль выполнен");
                    return;
                }
            }
            SelectedTask.IsCompleted = true;
            SelectedTask.TaskState = TaskState.Completed;
            UpdateTask();
            LoadUncompletedTasks();
            InitializeEndTimeTask();
            IsCompleted = false;
            if (TaskModels.Count > 0)
            {
                SelectedTask = TaskModels[0];
            }
        }

        public ICommand DeleteTaskCommand => new RelayCommand(delete => DeleteTask());

        /// <summary>
        /// Delete task: This method remove task from the list of tasks from the TaskDataService and updates the Tasks collection.
        /// </summary>
        private void DeleteTask()
        {
            //TaskController.DeleteTask(SelectedTask);
            //MessageBox.Show("Удаление задачи успешно завершено");
            //CreateDefaultSelectTask();
            //TaskImportance = SelectedTask.TaskImportance;
            //LoadUncompletedTasks();
            SelectedTask.TaskState = TaskState.Deleted;
            UpdateTask();
            LoadUncompletedTasks();
            InitializeEndTimeTask();
            if (TaskModels.Count > 0)
            {
                SelectedTask = TaskModels[0];
            }
            else
            {
                CreateDefaultSelectTask();
            }
        }
    }
}

