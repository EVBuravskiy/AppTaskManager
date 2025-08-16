using AppTaskManager.Controllers;
using AppTaskManager.Models;
using AppTaskManager.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using static AppTaskManager.Views.TasksViewMainWindow;

namespace AppTaskManager.ViewModels
{
    /// <summary>
    /// Main Window View Model
    /// </summary>
    public class MainWindowViewModel : BaseObservableClass
    {
        /// <summary>
        /// Field storing an instance of the Task View Main Window
        /// </summary>

        private TasksViewMainWindow TasksViewMainWindow;

        /// <summary>
        /// Field storing selected task
        /// </summary>
        private TaskModel _selectedTask;

        /// <summary>
        /// Property for field of selected task
        /// </summary>
        public TaskModel SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                if (value != null)
                {
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
                }
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        /// <summary>
        /// Field storing task importance
        /// </summary>
        private TaskImportance _taskImportance;

        /// <summary>
        /// Property for field of task importance
        /// </summary>
        public TaskImportance TaskImportance
        {
            get { return _taskImportance; }
            set
            {
                _taskImportance = value;
                OnPropertyChanged(nameof(TaskImportance));
            }
        }

        /// <summary>
        /// Field storing task controller
        /// </summary>
        public ITaskController TaskController;

        /// <summary>
        /// Collection of tasks
        /// </summary>
        public ObservableCollection<TaskModel> TaskModels { get; private set; }

        /// <summary>
        /// Collection of tasks with upcoming due dates
        /// </summary>
        public ObservableCollection<TaskModel> EndTimeTaskModels {  get; private set; }

        /// <summary>
        /// Field storing the selected task execution date
        /// </summary>
        private DateTime _selectedDate;

        /// <summary>
        /// Property for field storing the selected task execution date
        /// </summary>
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

        /// <summary>
        /// Field storing the task title for the search
        /// </summary>
        private string _searchTitle;

        /// <summary>
        /// Property for field storing the task title for the search
        /// </summary>
        public string SearchTitle
        {
            get { return _searchTitle; }
            set
            {
                _searchTitle = value;
                OnPropertyChanged(nameof(SearchTitle));
            }
        }

        /// <summary>
        /// Property that stores the state of the enabled checkbox
        /// </summary>
        public bool checkBoxEnabled { get; set; }

        /// <summary>
        /// Field that stores the state of the task state
        /// </summary>
        private bool _isCompleted;

        /// <summary>
        /// Property for field that stores the state of the task state
        /// </summary>
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
            }
        }

        /// <summary>
        /// Field storing the number of task checks 
        /// </summary>
        private double _checkCount;

        /// <summary>
        /// Property for the field storing the number of task checks
        /// </summary>
        public double CheckCount {
            get => _checkCount;
            set
            {
                _checkCount = value;
                OnPropertyChanged(nameof(CheckCount));
            }
        }

        /// <summary>
        /// Field storing the check state
        /// </summary>
        private double _checkChecked;

        /// <summary>
        /// Property for the field storing the check state
        /// </summary>
        public double CheckChecked 
        {
            get => _checkChecked;
            set
            {
                _checkChecked = value;
                OnPropertyChanged(nameof(CheckChecked));
            }
        }

        /// <summary>
        /// Property code behind for calendar
        /// </summary>
        public ICodeBehind MainWindowCodeBehind { get; set; }

        /// <summary>
        /// Property for collection of task dates
        /// </summary>
        public List<DateTime> TasksDates { get; set; }

        /// <summary>
        /// Field storing collection of selected dates from tasks
        /// </summary>
        private List<DateTime> _selectedDates;

        /// <summary>
        /// Property for the field storing collection of selected dates from tasks
        /// </summary>
        public List<DateTime> SelectedDates 
        { 
            get => _selectedDates;
            set
            {
                _selectedDates = value;
                OnPropertyChanged(nameof(SelectedDates));
            }
        }

        /// <summary>
        /// Main Window View Model Constructor
        /// </summary>
        /// <param name="mainWindow"></param>
        public MainWindowViewModel(TasksViewMainWindow mainWindow)
        {
            //Initialize Task Controller
            TaskController = new TaskJsonController();

            //Load uncompleted tasks into tasks collection
            LoadUncompletedTasks();

            //Initialize task importance
            TaskImportance = SelectedTask.TaskImportance;

            //Initialize collection of tasks with upcoming due dates
            InitializeEndTimeTask();

            //Initialize collection of selected dates
            SelectedDates = new List<DateTime>();

            //Initialize dates for calendar
            InitializeDates();

            //Initialize code behind
            MainWindowCodeBehind = mainWindow;

            //Initialize code behind with collection of task dates
            MainWindowCodeBehind.SelectManyDates(TasksDates);
        }

        /// <summary>
        /// Create default select task
        /// </summary>
        private void CreateDefaultSelectTask()
        {
            SelectedTask = new TaskModel()
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

        /// <summary>
        /// Get collection of tasks with upcoming due dates
        /// </summary>
        private void InitializeEndTimeTask()
        {
            var currentTime = DateTime.Today;
            if (EndTimeTaskModels == null)
            {
                EndTimeTaskModels = new ObservableCollection<TaskModel>();
            }
            int currentTaskId = 0;
            if (SelectedTask != null)
            {
                currentTaskId = SelectedTask.Id;
            }
            EndTimeTaskModels.Clear();
            bool fl = false;
            foreach(var task in TaskModels)
            {
                var checkTime = task.EndTime.AddDays(-3);
                int result1 = DateTime.Compare(currentTime, checkTime);
                if (result1 >= 0 && task.TaskState != TaskState.Completed && task.TaskState != TaskState.Deleted)
                {
                    if (task.TaskImportance != TaskImportance.Critical)
                    {
                        task.TaskImportance = TaskImportance.High;
                        fl = true;
                    }
                    EndTimeTaskModels.Add(task);
                }
                int result2 = DateTime.Compare(currentTime, task.EndTime);
                if (result2 > 0 && task.TaskState != TaskState.Completed && task.TaskState != TaskState.Deleted)
                {
                    task.TaskState = TaskState.Late;
                    task.TaskImportance = TaskImportance.Critical;
                    fl = true;
                }
                if (fl)
                {
                    TaskController.UpdateTask(task);
                }
                fl = false;
            }
            if (currentTaskId != 0)
            {
                GetCurrentTaskById(currentTaskId);
            }
            else if (TaskModels.Count > 0)
            {
                SelectedTask = TaskModels.First();
            }
            else
            {
                CreateDefaultSelectTask();
            }
        }

        /// <summary>
        /// Get collection of task dates
        /// </summary>
        private void InitializeDates()
        {
            TasksDates = new List<DateTime>();
            foreach (var task in TaskModels)
            {
                TasksDates.Add(task.EndTime);
            }
        }

        /// <summary>
        /// Load task with selected dates
        /// </summary>
        /// <param name="selectedDates"></param>
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
            else
            {
                SelectedTask = TaskModels.First();
            }
        }

        /// <summary>
        /// Command load uncompleted tasks
        /// </summary>
        public ICommand ILoadUncompletedTasks => new RelayCommand(all => LoadUncompletedTasks());

        /// <summary>
        /// Load uncompleted tasks
        /// </summary>
        private void LoadUncompletedTasks()
        {
            if (TaskModels == null)
            {
                TaskModels = new ObservableCollection<TaskModel>();
            }
            int currentTaskId = 0;
            if(SelectedTask != null)
            {
                currentTaskId = SelectedTask.Id;
            }
            TaskModels.Clear();
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                if (task.TaskState != TaskState.Deleted && task.TaskState != TaskState.Completed)
                {
                    TaskModel newTask = new TaskModel()
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        CreationTime = task.CreationTime.Date,
                        StartTime = task.StartTime,
                        EndTime = task.EndTime.Date,
                        IsCompleted = task.IsCompleted,
                        TaskState = task.TaskState,
                        TaskCategory = task.TaskCategory,
                        TaskImportance = task.TaskImportance,
                        TaskChecks = new List<TaskCheck>()
                    };
                    foreach (TaskCheck check in task.TaskChecks)
                    {
                        newTask.TaskChecks.Add(check);
                    }
                    TaskModels.Add(newTask);
                }
            }
            
            if (currentTaskId != 0)
            {
                GetCurrentTaskById(currentTaskId);
            }
            else if(TaskModels.Count > 0) 
            {
                SelectedTask = TaskModels.First();
            }
            else
            {
                CreateDefaultSelectTask();
            }
        }

        /// <summary>
        /// Command load all tasks
        /// </summary>
        public ICommand ILoadAllTasks => new RelayCommand(all => LoadAllTasks());

        /// <summary>
        /// Load all tasks
        /// </summary>
        private void LoadAllTasks()
        {
            if (TaskModels == null)
            {
                TaskModels = new ObservableCollection<TaskModel>();
            }
            int currentTaskId = 0;
            if (SelectedTask != null)
            {
                currentTaskId = SelectedTask.Id;
            }
            TaskModels.Clear();
            var tasks = TaskController.GetAllTasks();
            foreach (var task in tasks)
            {
                TaskModel newTask = new TaskModel()
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    CreationTime = task.CreationTime.Date,
                    StartTime = task.StartTime,
                    EndTime = task.EndTime.Date,
                    IsCompleted = task.IsCompleted,
                    TaskState = task.TaskState,
                    TaskCategory = task.TaskCategory,
                    TaskImportance = task.TaskImportance,
                    TaskChecks = new List<TaskCheck>()
                };
                foreach (TaskCheck check in task.TaskChecks)
                {
                    newTask.TaskChecks.Add(check);
                }
                TaskModels.Add(newTask);
            }
            if (currentTaskId != 0)
            {
                GetCurrentTaskById(currentTaskId);
            }
            else if (TaskModels.Count > 0)
            {
                SelectedTask = TaskModels.First();
            }
            else
            {
                CreateDefaultSelectTask();
            }
        }

        /// <summary>
        /// Command load critical importance tasks
        /// </summary>
        public ICommand ILoadCriticalTasks => new RelayCommand(critical => LoadCriticalTasks());

        /// <summary>
        /// Load critical importance tasks
        /// </summary>
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
                    TaskModel newTask = new TaskModel()
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        CreationTime = task.CreationTime.Date,
                        StartTime = task.StartTime,
                        EndTime = task.EndTime.Date,
                        IsCompleted = task.IsCompleted,
                        TaskState = task.TaskState,
                        TaskCategory = task.TaskCategory,
                        TaskImportance = task.TaskImportance,
                        TaskChecks = new List<TaskCheck>()
                    };
                    foreach (TaskCheck check in task.TaskChecks)
                    {
                        newTask.TaskChecks.Add(check);
                    }
                    TaskModels.Add(newTask);
                }
            }
            if (TaskModels.Count > 0)
            {
                SelectedTask = TaskModels.First();
            }
            else
            {
                CreateDefaultSelectTask();
            }
        }

        /// <summary>
        /// Command load high importance tasks 
        /// </summary>
        public ICommand ILoadHighTasks => new RelayCommand(high => LoadHighTasks());

        /// <summary>
        /// Load high importance tasks 
        /// </summary>
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
                    TaskModel newTask = new TaskModel()
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        CreationTime = task.CreationTime.Date,
                        StartTime = task.StartTime,
                        EndTime = task.EndTime.Date,
                        IsCompleted = task.IsCompleted,
                        TaskState = task.TaskState,
                        TaskCategory = task.TaskCategory,
                        TaskImportance = task.TaskImportance,
                        TaskChecks = new List<TaskCheck>()
                    };
                    foreach (TaskCheck check in task.TaskChecks)
                    {
                        newTask.TaskChecks.Add(check);
                    }
                    TaskModels.Add(newTask);
                }
            }
            if (TaskModels.Count > 0)
            {
                SelectedTask = TaskModels.First();
            }
            else
            {
                CreateDefaultSelectTask();
            }
        }

        /// <summary>
        /// Command load medium importance tasks
        /// </summary>
        public ICommand ILoadMediumTasks => new RelayCommand(medium => LoadMediumTasks());

        /// <summary>
        /// Load medium importance tasks
        /// </summary>
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
                    TaskModel newTask = new TaskModel()
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        CreationTime = task.CreationTime.Date,
                        StartTime = task.StartTime,
                        EndTime = task.EndTime.Date,
                        IsCompleted = task.IsCompleted,
                        TaskState = task.TaskState,
                        TaskCategory = task.TaskCategory,
                        TaskImportance = task.TaskImportance,
                        TaskChecks = new List<TaskCheck>()
                    };
                    foreach (TaskCheck check in task.TaskChecks)
                    {
                        newTask.TaskChecks.Add(check);
                    }
                    TaskModels.Add(newTask);
                }
            }
            if (TaskModels.Count > 0)
            {
                SelectedTask = TaskModels.First();
            }
            else
            {
                CreateDefaultSelectTask();
            }
        }

        /// <summary>
        /// Command load low importance tasks
        /// </summary>
        public ICommand ILoadLowTasks => new RelayCommand(low => LoadLowTasks());

        /// <summary>
        /// Load low importance tasks
        /// </summary>
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
                    TaskModel newTask = new TaskModel()
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        CreationTime = task.CreationTime.Date,
                        StartTime = task.StartTime,
                        EndTime = task.EndTime.Date,
                        IsCompleted = task.IsCompleted,
                        TaskState = task.TaskState,
                        TaskCategory = task.TaskCategory,
                        TaskImportance = task.TaskImportance,
                        TaskChecks = new List<TaskCheck>()
                    };
                    foreach (TaskCheck check in task.TaskChecks)
                    {
                        newTask.TaskChecks.Add(check);
                    }
                    TaskModels.Add(newTask);
                }
            }
            if (TaskModels.Count > 0)
            {
                SelectedTask = TaskModels.First();
            }
            else
            {
                CreateDefaultSelectTask();
            }
        }

        /// <summary>
        /// Command load completed tasks
        /// </summary>
        public ICommand ILoadCompletedTasks => new RelayCommand(completed => LoadCompletedTasks());

        /// <summary>
        /// Load completed tasks
        /// </summary>
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
                if (task.IsCompleted && task.TaskState == TaskState.Completed)
                {
                    TaskModel newTask = new TaskModel()
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        CreationTime = task.CreationTime.Date,
                        StartTime = task.StartTime,
                        EndTime = task.EndTime.Date,
                        IsCompleted = task.IsCompleted,
                        TaskState = task.TaskState,
                        TaskCategory = task.TaskCategory,
                        TaskImportance = task.TaskImportance,
                        TaskChecks = new List<TaskCheck>()
                    };
                    foreach (TaskCheck check in task.TaskChecks)
                    {
                        newTask.TaskChecks.Add(check);
                    }
                    TaskModels.Add(newTask);
                }
            }
            if (TaskModels.Count > 0)
            {
                SelectedTask = TaskModels.First();
            }
            else
            {
                CreateDefaultSelectTask();
            }
        }

        /// <summary>
        /// Command load deleted tasks
        /// </summary>
        public ICommand ILoadDeletedTasks => new RelayCommand(deleted => LoadDeletedTasks());

        /// <summary>
        /// Load deleted tasks
        /// </summary>
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
                    TaskModel newTask = new TaskModel()
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        CreationTime = task.CreationTime.Date,
                        StartTime = task.StartTime,
                        EndTime = task.EndTime.Date,
                        IsCompleted = task.IsCompleted,
                        TaskState = task.TaskState,
                        TaskCategory = task.TaskCategory,
                        TaskImportance = task.TaskImportance,
                        TaskChecks = new List<TaskCheck>()
                    };
                    foreach (TaskCheck check in task.TaskChecks)
                    {
                        newTask.TaskChecks.Add(check);
                    }
                    TaskModels.Add(newTask);
                }
            }
            if (TaskModels.Count > 0)
            {
                SelectedTask = TaskModels.First();
            }
            else
            {
                CreateDefaultSelectTask();
            }
        }

        /// <summary>
        /// Command load home tasks
        /// </summary>
        public ICommand ILoadHomeTasks => new RelayCommand(deleted => LoadHomeTasks());

        /// <summary>
        /// Load home tasks
        /// </summary>
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
                    TaskModel newTask = new TaskModel()
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        CreationTime = task.CreationTime.Date,
                        StartTime = task.StartTime,
                        EndTime = task.EndTime.Date,
                        IsCompleted = task.IsCompleted,
                        TaskState = task.TaskState,
                        TaskCategory = task.TaskCategory,
                        TaskImportance = task.TaskImportance,
                        TaskChecks = new List<TaskCheck>()
                    };
                    foreach (TaskCheck check in task.TaskChecks)
                    {
                        newTask.TaskChecks.Add(check);
                    }
                    TaskModels.Add(newTask);
                }
            }
            if (TaskModels.Count > 0)
            {
                SelectedTask = TaskModels.First();
            }
            else
            {
                CreateDefaultSelectTask();
            }
        }

        /// <summary>
        /// Command load work tasks
        /// </summary>
        public ICommand ILoadWorkTasks => new RelayCommand(deleted => LoadWorkTasks());

        /// <summary>
        /// Load work tasks
        /// </summary>
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
                    TaskModel newTask = new TaskModel()
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        CreationTime = task.CreationTime.Date,
                        StartTime = task.StartTime,
                        EndTime = task.EndTime.Date,
                        IsCompleted = task.IsCompleted,
                        TaskState = task.TaskState,
                        TaskCategory = task.TaskCategory,
                        TaskImportance = task.TaskImportance,
                        TaskChecks = new List<TaskCheck>()
                    };
                    foreach (TaskCheck check in task.TaskChecks)
                    {
                        newTask.TaskChecks.Add(check);
                    }
                    TaskModels.Add(newTask);
                }
            }
            if (TaskModels.Count > 0)
            {
                SelectedTask = TaskModels.First();
            }
            else
            {
                CreateDefaultSelectTask();
            }
        }

        /// <summary>
        /// Command load education tasks
        /// </summary>
        public ICommand ILoadEducationTasks => new RelayCommand(deleted => LoadEducationTasks());

        /// <summary>
        /// Load education tasks
        /// </summary>
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
                    TaskModel newTask = new TaskModel()
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        CreationTime = task.CreationTime.Date,
                        StartTime = task.StartTime,
                        EndTime = task.EndTime.Date,
                        IsCompleted = task.IsCompleted,
                        TaskState = task.TaskState,
                        TaskCategory = task.TaskCategory,
                        TaskImportance = task.TaskImportance,
                        TaskChecks = new List<TaskCheck>()
                    };
                    foreach (TaskCheck check in task.TaskChecks)
                    {
                        newTask.TaskChecks.Add(check);
                    }
                    TaskModels.Add(newTask);
                }
            }
            if (TaskModels.Count > 0)
            {
                SelectedTask = TaskModels.First();
            }
            else
            {
                CreateDefaultSelectTask();
            }
        }

        /// <summary>
        /// Add task to task collection
        /// </summary>
        public void AddTaskToTaskModels(TaskModel newTask)
        {
            TaskController.AddTask(newTask);
            CreateDefaultSelectTask();
            LoadUncompletedTasks();
            InitializeEndTimeTask();
        }

        /// <summary>
        /// Command find task by title
        /// </summary>
        public ICommand IFindTasks => new RelayCommand(find => FindTitleInTasks());

        /// <summary>
        /// Find task by title
        /// </summary>
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

        /// <summary>
        /// Find task by date
        /// </summary>
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
                    TaskModel newTask = new TaskModel()
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        CreationTime = task.CreationTime.Date,
                        StartTime = task.StartTime,
                        EndTime = task.EndTime.Date,
                        IsCompleted = task.IsCompleted,
                        TaskState = task.TaskState,
                        TaskCategory = task.TaskCategory,
                        TaskImportance = task.TaskImportance,
                        TaskChecks = new List<TaskCheck>()
                    };
                    foreach (TaskCheck check in task.TaskChecks)
                    {
                        newTask.TaskChecks.Add(check);
                    }
                    TaskModels.Add(newTask);
                }
            }
            if (TaskModels.Count > 0)
            {
                SelectedTask = TaskModels.First();
            }
            else
            {
                CreateDefaultSelectTask();
            }
        }

        /// <summary>
        /// Command begin task
        /// </summary>
        public ICommand IBeginTask => new RelayCommand(begin => BeginTask());

        /// <summary>
        /// Begin task
        /// </summary>
        private void BeginTask()
        {
            if (SelectedTask.Id == 0)
            {
                return;
            }
            SelectedTask.StartTime = DateTime.Now;
            SelectedTask.TaskState = TaskState.InProgress;
            TaskController.UpdateTask(SelectedTask);
            LoadUncompletedTasks();
            InitializeEndTimeTask();
        }


        /// <summary>
        /// Command to save the modified task
        /// </summary>
        public ICommand ISaveChanges => new RelayCommand(update => UpdateTask());

        /// <summary>
        /// Save the modified current task
        /// </summary>
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
            LoadUncompletedTasks();
            InitializeEndTimeTask();
        }

        /// <summary>
        /// Save the modified new or change task
        /// </summary>
        public void UpdateTask(TaskModel inputTask)
        {
            if (inputTask.Id == 0)
            {
                return;
            }
            TaskController.UpdateTask(inputTask);
            CreateDefaultSelectTask();
            LoadUncompletedTasks();
            InitializeEndTimeTask();
        }

        /// <summary>
        /// Get task by id
        /// </summary>
        /// <param name="currentTaskId"></param>
        private void GetCurrentTaskById(int currentTaskId)
        {
            var currentTask = TaskModels.FirstOrDefault(t => t.Id == currentTaskId);
            if (currentTask == null)
            {
                CreateDefaultSelectTask();
                return;
            }
            else
            {
                SelectedTask = currentTask;
            }
            //OnPropertyChanged(nameof(SelectedTask));
            CheckChecked = 0;
            foreach (var check in _selectedTask.TaskChecks)
            {
                if (check.IsComplete == true)
                {
                    CheckChecked++;
                }
            }
        }

        /// <summary>
        /// Command check that all controls are completed
        /// </summary>
        public ICommand ICheckChecksComplete => new RelayCommand(checks => CheckChecksComplete());

        /// <summary>
        /// Check that all controls are completed
        /// </summary>
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
                        SelectedTask.IsCompleted = false;
                        SelectedTask.TaskState = TaskState.InProgress;
                        break;
                    }
                    SelectedTask.TaskState = TaskState.Completed;
                    SelectedTask.IsCompleted = true;
                    IsCompleted = true;
                }
            }
            TaskController.UpdateTask(SelectedTask);
            CheckChecked = 0;
            if (_selectedTask != null)
            {
                foreach (var check in _selectedTask.TaskChecks)
                {
                    if (check.IsComplete == true)
                    {
                        CheckChecked++;
                    }
                }
            }
            if (SelectedTask.IsCompleted)
            {
                CreateDefaultSelectTask();
            }
            LoadUncompletedTasks();
            InitializeEndTimeTask();
        }

        /// <summary>
        /// Command open edit task window
        /// </summary>
        public ICommand IOpenEditTask => new RelayCommand(open => OpenNewWindow(false));

        /// <summary>
        /// Open edit task window
        /// </summary>
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

        /// <summary>
        /// Command complete task
        /// </summary>
        public ICommand ICompletedCommand => new RelayCommand(complete => CompleteTask());

        /// <summary>
        /// Complete task
        /// </summary>
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
            TaskController.UpdateTask(SelectedTask);
            LoadUncompletedTasks();
            InitializeEndTimeTask();
            IsCompleted = false;
            if (TaskModels.Count > 0)
            {
                SelectedTask = TaskModels[0];
            }
        }

        /// <summary>
        /// Command delete task
        /// </summary>
        public ICommand DeleteTaskCommand => new RelayCommand(delete => DeleteTask());

        /// <summary>
        /// Delete task: This method specifies the deleted property for the task being deleted
        /// </summary>
        private void DeleteTask()
        {
            SelectedTask.TaskState = TaskState.Deleted;
            TaskController.UpdateTask(SelectedTask);
            CreateDefaultSelectTask();
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
            MessageBox.Show("Удаление задачи успешно завершено");
        }
    }
}

