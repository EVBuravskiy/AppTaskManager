using AppTaskManager.Controllers;
using AppTaskManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppTaskManager.ViewModels
{
    public class TaskViewModel : ObservableObject
    {
        /// <summary>
        /// TaskController Declaration. It used to interact with the underlying data storage
        /// </summary>
        private readonly ITaskController _taskController;

        /// <summary>
        /// Private field holding the instance of the TaskModel class
        /// </summary>
        private TaskModel _taskModel;

        /// <summary>
        /// Public property for the instance of the TaskModel class
        /// </summary>
        public TaskModel TaskModel { 
            get { return _taskModel; } 
            set { OnPropertyChanged(ref  _taskModel, value); }
        }

        /// <summary>
        /// Private field holding the selected instance of the TaskModel class
        /// </summary>
        private TaskModel _selectedTaskModel;

        /// <summary>
        /// Public property for the selected instance of the TaskModel class
        /// </summary>
        public TaskModel SelectedTaskModel
        {
            get { return _selectedTaskModel; }
            set { OnPropertyChanged(ref _selectedTaskModel, value); }
        }

        /// <summary>
        /// Public property for the observable collection of TaskModels
        /// </summary>
        public ObservableCollection<TaskModel> TasksModels { get; set; }

        /// <summary>
        /// Array of task categories for combobox
        /// </summary>
        public Array TaskCategories => Enum.GetValues(typeof(TaskCategory));

        /// <summary>
        /// Array of task categories for combobox
        /// </summary>
        public Array TaskImportancies => Enum.GetValues(typeof(TaskImportance));

        /// <summary>
        /// Public property holding selected TaskCheck
        /// </summary>
        public TaskCheck SelectedCheck { get; set; }

        /// <summary>
        /// Public property for control check description
        /// </summary>
        public string CheckDescription { get; set; }

        /// <summary>
        /// Private field holding the observable collection of check list
        /// </summary>
        private ObservableCollection<Models.TaskCheck> _taskChecklist;

        /// <summary>
        /// Public property for observable collection of check list
        /// </summary>
        public ObservableCollection<Models.TaskCheck> TaskCheckList
        {
            get { return _taskChecklist; }
            set
            {
                _taskChecklist = value;
                OnPropertyChanged(ref _taskChecklist, value);
            }
        }

        /// <summary>
        /// Constructor for the TaskViewModel. 
        /// It get TaskController, load all tasks, initialize instance of the TaskModel class, and clear TaskCheckList
        /// </summary>
        /// <param name="taskController"></param>
        public TaskViewModel(ITaskController taskController)
        {
            //Intialize instance of the TaskModel class
            _taskModel = new TaskModel();
            _taskModel.CreationTime = DateTime.Now;
            _taskModel.EndTime = DateTime.Now;
            _taskController = taskController;
            //Initialize observable collection of TaskModels
            LoadTasks();

            //Initialize observable collection of TaskCheckList
            if (TaskCheckList != null)
            {
                TaskCheckList.Clear();
            }
            else
            {
                TaskCheckList = new ObservableCollection<TaskCheck>();
            }
        }

        /// <summary>
        /// Load all tasks
        /// </summary>
        private void LoadTasks()
        {
            var tasks = _taskController.GetAllTasks();
            if (TasksModels != null)
            {
                TasksModels.Clear();
            }
            else
            {
                TasksModels = new ObservableCollection<TaskModel>();
            }
            foreach (var task in tasks)
            {
                TasksModels.Add(task);
            }
        }

        /// <summary>
        /// Add control check command
        /// </summary>
        public ICommand IAddControlCheck => new RelayCommand(AddControlCheck);

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
        public ICommand IRemoveTaskCheck => new RelayCommand(RemoveTaskCheck);

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
        public ICommand IAddNewTask => new RelayCommand(AddNewTask);

        /// <summary>
        /// Add new task: This method add new task to the list of tasks from the TaskDataService and updates the Tasks collection.
        /// </summary>
        public void AddNewTask()
        {
            TaskModel newTask = new TaskModel
            {
                Id = _taskController.GenerateNewTaskId(),
                Title = TaskModel.Title,
                Description = TaskModel.Description,
                CreationTime = DateTime.Now,
                StartTime = null,
                EndTime = TaskModel.EndTime,
                IsCompleted = false,
                TaskState = TaskState.Create,
                TaskCategory = TaskModel.TaskCategory,
                TaskImportance = TaskModel.TaskImportance,
                TaskChecks = TaskModel.TaskChecks,
            };
            TasksModels.Add(newTask);
            _taskController.SaveTasks(TasksModels);
            ClearFields();
        }

        /// <summary>
        /// Clear fields command
        /// </summary>
        public ICommand IClearTask => new RelayCommand(ClearTask);

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
