using AppTaskManager.Controllers;
using AppTaskManager.Models;
using AppTaskManager.Utilities;
using AppTaskManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
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

        /// <summary>
        /// Array of task categories for combobox
        /// </summary>
        //public Array TaskCategories => Enum.GetValues(typeof(TaskCategory));
        public List<string> TaskCategories { get; set; }
        public string InputCategory { get; set; }

        /// <summary>
        /// Array of task categories for combobox
        /// </summary>
        //public Array TaskImportancies => Enum.GetValues(typeof(TaskImportance));

        public List<string> TaskImportancies { get; set; }
        public string InputImportance { get; set; }

        /// <summary>
        /// Public property holding selected TaskCheck
        /// </summary>
        public TaskCheck SelectedCheck { get; set; }

        /// <summary>
        /// Public property for control check description
        /// </summary>
        public string CheckDescription { get; set; }

        /// <summary>
        /// Public property holding the observable collection of check list
        /// </summary>
        public ObservableCollection<TaskCheck> TaskCheckList {  get; set; }

        private bool IsNew { get; set; }

        /// <summary>
        /// Constructor for the TaskViewModel. 
        /// It get TaskController, load all tasks, initialize instance of the TaskModel class, and clear TaskCheckList
        /// </summary>
        /// <param name="taskController"></param>
        public TaskViewModel(MainWindowViewModel mainViewModel, bool isNew = true)
        {
            IsNew = isNew;
            _MainWindowViewModel = mainViewModel;
            //Intialize instance of the TaskModel class
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
                _taskModel.CreationTime = DateTime.Now;
                _taskModel.EndTime = DateTime.Now;
            }
            else
            {
                _taskModel = _MainWindowViewModel.SelectedTask;
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
            TaskCheck taskCheck = new TaskCheck();
            taskCheck.Description = CheckDescription;
            taskCheck.IsComplete = false;
            TaskCheckList.Add(taskCheck);
            TaskModel.TaskChecks = TaskCheckList.ToList();
            CheckDescription = "";
            OnPropertyChanged(CheckDescription);
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
            if (IsNew)
            {
                TaskModel.Id = _MainWindowViewModel.TaskController.GenerateNewTaskId();
                TaskModel newTask = new TaskModel()
                {
                    Id = TaskModel.Id,
                    Title = TaskModel.Title,
                    Description = TaskModel.Description,
                    CreationTime = TaskModel.CreationTime,
                    StartTime = TaskModel.StartTime,
                    EndTime = TaskModel.EndTime,
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
            TaskModel.Title = "";
            TaskModel.Description = "";
            TaskModel.CreationTime= DateTime.Now;
            TaskModel.TaskCategory = TaskCategory.Work;
            TaskModel.TaskImportance = TaskImportance.Low;
            TaskCheckList.Clear();
            TaskModel.TaskChecks = TaskCheckList.ToList();
            OnPropertyChanged(nameof(TaskModel));
            OnPropertyChanged(nameof(TaskCheckList));
        }
    }
}
