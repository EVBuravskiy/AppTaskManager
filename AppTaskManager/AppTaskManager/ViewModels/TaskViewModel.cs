using AppTaskManager.Controllers;
using AppTaskManager.Models;
using AppTaskManager.Utilities;
using AppTaskManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace AppTaskManager.ViewModels
{
    public class TaskViewModel : BaseObservableClass
    {
        private MainWindowViewModel _MainWindowViewModel;

        /// <summary>
        /// Private field holding the instance of the TaskModel class
        /// </summary>
        private TaskModel _taskModel;

        /// <summary>
        /// Public property for the instance of the TaskModel class
        /// </summary>
        public TaskModel TaskModel
        {
            get { return _taskModel; }
            set 
            { 
                _taskModel = value; 
                OnPropertyChanged(nameof(TaskModel));
            }
        }

        public List<string> TaskCategories { get; set; }
        public string InputCategory { get; set; }

        public List<string> TaskImportancies { get; set; }
        public string InputImportance { get; set; }

        /// <summary>
        /// Public property holding selected TaskCheck
        /// </summary>
        public TaskCheck SelectedCheck { get; set; }

        public string TaskChecksMessage { get; set; } = "Введите название контроля проверки выполнения задачи...";

        public SolidColorBrush BackgroundCheckDescription { get; set; } = new SolidColorBrush(Colors.WhiteSmoke);

        private int checkId => TaskCheckList.Count + 1;

        private string _checkDescription;
        /// <summary>
        /// Public property for control check description
        /// </summary>
        public string CheckDescription 
        {
            get => _checkDescription;
            set
            {
                _checkDescription = Validate.TrimInputString(value);
                if (!Validate.ValidateString(value, 3))
                {
                    BackgroundCheckDescription = Brushes.LightCoral;
                    OnPropertyChanged(nameof(BackgroundCheckDescription));
                    TaskChecksMessage = "Проверьте введенное название контроля";
                    OnPropertyChanged(nameof(TaskChecksMessage));
                }
                else
                {
                    _checkDescription = $"{checkId}. {_checkDescription}";
                    IsEnabledAddControlCheck = true;
                    BackgroundCheckDescription = Brushes.WhiteSmoke;
                    OnPropertyChanged(nameof(BackgroundCheckDescription));
                    TaskTitleMessage = "Контроль введен";
                    OnPropertyChanged(nameof(TaskChecksMessage));
                }
                OnPropertyChanged(nameof(CheckDescription));
            }
        }

        private bool _isEnabledAddControlCheck = false;

        public bool IsEnabledAddControlCheck 
        {
            get => _isEnabledAddControlCheck;
            set
            {
                _isEnabledAddControlCheck = value;
                OnPropertyChanged(nameof(IsEnabledAddControlCheck));
            }
        }

        /// <summary>
        /// Public property holding the observable collection of check list
        /// </summary>
        public ObservableCollection<TaskCheck> TaskCheckList {  get; set; }

        private bool IsNew { get; set; }

        public bool StartTask { get; set; }

        public SolidColorBrush BackgroundTitle { get; set; } = new SolidColorBrush(Colors.WhiteSmoke);

        public string TaskTitleMessage { get; set; } = "Введите наименование задачи...";

        private bool TaskHaveTitle = false;

        private string _taskTitle;
        public string TaskTitle 
        {
            get => _taskTitle;
            set
            {
                _taskTitle = Validate.TrimInputString(value);
                if (!Validate.ValidateString(value, 3))
                {
                    BackgroundTitle = Brushes.LightCoral;
                    OnPropertyChanged(nameof(BackgroundTitle));
                    TaskTitleMessage = "Проверьте наименование задачи";
                    OnPropertyChanged(nameof(TaskTitleMessage));
                }
                else
                {
                    BackgroundTitle = Brushes.WhiteSmoke;
                    OnPropertyChanged(nameof(BackgroundTitle));
                    TaskTitleMessage = "Наименование задачи введено";
                    OnPropertyChanged(nameof(TaskTitleMessage));
                    TaskHaveTitle = true;
                }
                OnPropertyChanged(nameof(TaskTitle));
            }
        }

        public SolidColorBrush BackgroundDescription { get; set; } = new SolidColorBrush(Colors.WhiteSmoke);

        public string TaskDescriptionMessage { get; set; } = "Введите описание задачи...";

        private bool TaskHaveDescription = false;

        private string _taskDescription;

        public string TaskDescription 
        {
            get => _taskDescription;
            set
            {
                _taskDescription = Validate.TrimInputString(value);
                if (!Validate.ValidateString(value, 10))
                {
                    BackgroundDescription = Brushes.LightCoral;
                    OnPropertyChanged(nameof(BackgroundDescription));
                    TaskDescriptionMessage = "Проверьте корректность описания задачи";
                    OnPropertyChanged(nameof(TaskDescriptionMessage));
                }
                else
                {
                    BackgroundDescription = Brushes.WhiteSmoke;
                    OnPropertyChanged(nameof(BackgroundDescription));
                    TaskDescriptionMessage = "Описание задачи введено";
                    OnPropertyChanged(nameof(TaskDescriptionMessage));
                    TaskHaveDescription = true;
                }
                OnPropertyChanged(nameof(TaskDescription));
            }
        }

        public SolidColorBrush BorderDatePicker { get; set; } = new SolidColorBrush(Colors.WhiteSmoke);

        /// <summary>
        /// Constructor for the TaskViewModel. 
        /// It get TaskController, load all tasks, initialize instance of the TaskModel class, and clear TaskCheckList
        /// </summary>
        /// <param name="taskController"></param>
        public TaskViewModel(MainWindowViewModel mainViewModel, bool isNew = true)
        {
            IsNew = isNew;
            _MainWindowViewModel = mainViewModel;
            //Initialize instance of the TaskModel class
            InitializeTaskModel();
            InitializeListCategories();
        }

        private void InitializeListCategories()
        {
            TaskImportancies = new List<string>();
            var importancies = Enum.GetValues(typeof(TaskImportance));
            foreach (var important in importancies)
            {
                TaskImportancies.Add(EnumsToStringConverter.EnumToString(important));
            };
            TaskCategories = new List<string>();
            var categories = Enum.GetValues(typeof(TaskCategory));
            foreach (var category in categories)
            {
                TaskCategories.Add(EnumsToStringConverter.EnumToString(category));
            }
        }


        private void InitializeTaskModel()
        {
            //var indexCategory = Enum.GetValue(typeof(TaskCategory));
            //Initialize observable collection of TaskCheckList
            if (TaskCheckList != null)
            {
                TaskCheckList.Clear();
            }
            else
            {
                TaskCheckList = new ObservableCollection<TaskCheck>();
            }

            //Initialize instance of the TaskModel class
            if (IsNew)
            {
                _taskModel = new TaskModel();
                _taskModel.TaskCategory = TaskCategory.Work;
                _taskModel.TaskImportance = TaskImportance.Low;
                _taskModel.CreationTime = DateTime.Today;
                _taskModel.EndTime = DateTime.Today;
            }
            else
            {
                _taskModel = _MainWindowViewModel.SelectedTask;
                TaskTitle = _taskModel.Title;
                TaskDescription = _taskModel.Description;
                foreach (TaskCheck check in _MainWindowViewModel.SelectedTask.TaskChecks)
                {
                    TaskCheckList.Add(check);
                }
            }
        }

        /// <summary>
        /// Add control check command
        /// </summary>
        public ICommand IAddControlCheck => new RelayCommand(addcheck => AddControlCheck());

        /// <summary>
        /// Add control check
        /// </summary>
        public void AddControlCheck()
        {
            if (IsEnabledAddControlCheck)
            {
                TaskCheck taskCheck = new TaskCheck();
                taskCheck.Description = CheckDescription;
                taskCheck.IsComplete = false;
                TaskCheckList.Add(taskCheck);
                TaskModel.TaskChecks = TaskCheckList.ToList();
                CheckDescription = "";
                OnPropertyChanged(CheckDescription);
                IsEnabledAddControlCheck = false;
            }
            else
            {
                BackgroundCheckDescription = Brushes.LightCoral;
                OnPropertyChanged(nameof(BackgroundCheckDescription));
                MessageBox.Show("Проверьте корректность введения контроля проверки выполнения задачи");
            }
        }

        /// <summary>
        /// Remove task check command
        /// </summary>
        public ICommand IRemoveTaskCheck => new RelayCommand(removetaskcheck => RemoveTaskCheck());

        /// <summary>
        /// Remove task check: This method remove selected task check from collection of task check
        /// </summary>
        public void RemoveTaskCheck()
        {
            TaskCheckList.Remove(SelectedCheck);
            TaskModel.TaskChecks = TaskCheckList.ToList();
        }

        /// <summary>
        /// Add new task command
        /// </summary>
        public ICommand IAddNewTask => new RelayCommand(addnewtask => AddNewTask());

        /// <summary>
        /// Add new task: This method add new task to the list of tasks from the TaskDataService and updates the Tasks collection.
        /// </summary>
        public void AddNewTask()
        {
            if(TaskModel.EndTime <= DateTime.Now)
            {
                BorderDatePicker = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Дата окончания задачи не ранее текущей даты или текущей датой");
                OnPropertyChanged(nameof(BorderDatePicker));
                return;
            }

            if (!TaskHaveTitle)
            {
                MessageBox.Show("Отсутствует наименование задачи");
                return;
            }
            else
            {
                TaskModel.Title = TaskTitle;
            }

            if (!TaskHaveDescription)
            {
                MessageBox.Show("Отсутствует описание задачи");
                return;
            }
            else
            {
                TaskModel.Description = TaskDescription;
            }

            if (TaskCheckList.Count == 0)
            {
                MessageBox.Show("Не введен контроль выполнения задачи");
                return;
            }

            if (StartTask)
            {
                TaskModel.StartTime = DateTime.Today;
                TaskModel.TaskState = TaskState.InProgress;
            }
            if (IsNew)
            {
                TaskModel.Id = _MainWindowViewModel.TaskController.GenerateNewTaskId();
                TaskModel newTask = new TaskModel()
                {
                    Id = TaskModel.Id,
                    Title = TaskModel.Title,
                    Description = TaskModel.Description,
                    CreationTime = TaskModel.CreationTime.Date,
                    StartTime = TaskModel.StartTime,
                    EndTime = TaskModel.EndTime.Date,
                    IsCompleted = TaskModel.IsCompleted,
                    TaskState = TaskModel.TaskState,
                    TaskCategory = TaskModel.TaskCategory,
                    TaskImportance = TaskModel.TaskImportance,
                    TaskChecks = new List<TaskCheck>()
                };
                foreach (TaskCheck check in TaskCheckList)
                {
                    newTask.TaskChecks.Add(check);
                }
                _MainWindowViewModel.AddTaskToTaskModels(newTask);
                ClearFields();
            }
            else
            {
                _MainWindowViewModel.UpdateTask();
                IsNew = true;
                ClearFields();
            }
        }

        /// <summary>
        /// Clear fields command
        /// </summary>
        public ICommand IClearTask => new RelayCommand(cleartask => ClearTask());

        /// <summary>
        /// Clear task: This method clear entered data from fields
        /// </summary>
        public void ClearTask()
        {
            ClearFields();
        }

        /// <summary>
        /// Clear fields: This method remove all data from fields
        /// </summary>
        private void ClearFields()
        {
            InitializeTaskModel();
            TaskTitle = "";
            OnPropertyChanged(nameof(TaskTitle));
            TaskModel.Title = TaskTitle;
            TaskDescription = "";
            OnPropertyChanged(nameof(TaskDescription));
            TaskModel.Description = TaskDescription;
            TaskModel.CreationTime= DateTime.Now;
            TaskModel.TaskCategory = TaskCategory.Work;
            TaskModel.TaskImportance = TaskImportance.Low;
            TaskCheckList.Clear();
            TaskModel.TaskChecks = TaskCheckList.ToList();
            BorderDatePicker = new SolidColorBrush(Colors.WhiteSmoke);
            BackgroundTitle = Brushes.WhiteSmoke;
            OnPropertyChanged(nameof(BackgroundTitle));
            TaskTitleMessage = "Введите наименование задачи...";
            BackgroundDescription = Brushes.WhiteSmoke;
            OnPropertyChanged(nameof(BackgroundDescription));
            TaskDescriptionMessage = "Введите описание задачи...";
            BorderDatePicker = new SolidColorBrush(Colors.WhiteSmoke);
            OnPropertyChanged(nameof(BorderDatePicker));
            OnPropertyChanged(nameof(TaskModel));
            OnPropertyChanged(nameof(TaskCheckList));
        }
    }
}
