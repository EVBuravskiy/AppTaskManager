using AppTaskManager.Controllers;
using AppTaskManager.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace AppTaskManager.ViewModels
{
    public class TaskViewModel : BasePropertyChanged
    {
        /// <summary>
        /// TaskDataService Declaration. It used to interact with the underlying data storage
        /// </summary>
        private readonly TaskController _taskController;

        /// <summary>
        /// Private field holding the instance of the TaskModel class
        /// </summary>
        private Models.TaskModel _taskModel;

        /// <summary>
        /// Public property for the instance of the TaskModel class
        /// </summary>
        public Models.TaskModel TaskModel
        {
            get { return _taskModel; }
            set { OnPropertyChanged(nameof(TaskModel)); }
        }

        /// <summary>
        /// Private field holding the selected instance of the TaskModel class
        /// </summary>
        private Models.TaskModel _selectedTask;
        
        /// <summary>
        /// Pulbic property for the selected instance of the TaskModel class
        /// </summary>
        public Models.TaskModel SelectedTask
        {
            get { return _selectedTask; }
            set { OnPropertyChanged(nameof(SelectedTask)); }
        }

        /// <summary>
        /// Array of task categories for combobox
        /// </summary>
        public Array TaskCategories => Enum.GetValues(typeof(TaskCategory));
        
        /// <summary>
        /// Array of task categories for combobox
        /// </summary>
        public Array TaskImportancies => Enum.GetValues(typeof(TaskImportance));

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
                OnPropertyChanged(nameof(TaskCheckList));
            }
        }

        /// <summary>
        /// Public property for control check description
        /// </summary>
        public string ControlCheckDescription{ get; set; }

        /// <summary>
        /// Public property holding selected TaskCheck
        /// </summary>
        public TaskCheck CheckedTask { get; set; }

        /// <summary>
        /// Private field holding the collection of task models
        /// </summary>
        private ObservableCollection<Models.TaskModel> _TaskModels;

        /// <summary>
        /// Public property for tasks
        /// </summary>
        public ObservableCollection<Models.TaskModel> TaskModels
        {
            get { return _TaskModels; }
            set
            {
                _TaskModels = value;
                OnPropertyChanged(nameof(TaskModels));
            }
        }

        /// <summary>
        /// Constructor for the TaskViewModel class. It initializes the the TaskDataService and loads tasks
        /// </summary>
        public TaskViewModel()
        {
            _taskModel = new Models.TaskModel();
            _TaskModels = new ObservableCollection<Models.TaskModel>();
            _taskModel.CreateDateTime = DateTime.Now;
            _taskModel.EndDateTime = DateTime.Now;
            _taskController = new TaskController();
            if (TaskCheckList != null)
            {
                TaskCheckList.Clear();
            }
            else
            {
                TaskCheckList = new ObservableCollection<TaskCheck>();
            }
            LoadTasks();
        }
        
        /// <summary>
        /// Load Tasks: This method loads the list of tasks from the TaskDataService and updates the Tasks collection.
        /// The Tasks collection is bound to the view, so updating this collection will update the UI accordingly.
        /// </summary>
        private void LoadTasks()
        {
            var TaskList = _taskController.LoadTasks();
            TaskModels = new ObservableCollection<Models.TaskModel>(TaskList);
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
            taskCheck.Description = this.ControlCheckDescription;
            taskCheck.IsComplete = false;
            TaskCheckList.Add(taskCheck);
            TaskModel.TaskChecklist = TaskCheckList.ToList();
            ControlCheckDescription = "";
            OnPropertyChanged(ControlCheckDescription);
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
            TaskCheckList.Remove(CheckedTask);
            TaskModel.TaskChecklist = TaskCheckList.ToList();
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
            Models.TaskModel newTask = new Models.TaskModel
            {
                Id = _taskController.GenerateNewTaskId(),
                Title = TaskModel.Title,
                Description = TaskModel.Description,
                CreateDateTime = DateTime.Now,
                StartDateTime = null,
                EndDateTime = TaskModel.EndDateTime,
                IsComplete = false,
                Timer = new TimeSpan(0),
                TaskState = TaskState.NotStarted,
                TaskCategory = TaskModel.TaskCategory,
                TaskImportance = TaskModel.TaskImportance,
                TaskChecklist = TaskModel.TaskChecklist,
            };
            _taskController.AddTask(newTask);
            LoadTasks();
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
            TaskModel.CreateDateTime = DateTime.Now;
            TaskModel.TaskCategory = TaskCategory.Work;
            TaskModel.TaskImportance = TaskImportance.Low;
            TaskCheckList.Clear();
            TaskModel.TaskChecklist = TaskCheckList.ToList();
            OnPropertyChanged(nameof(TaskModel));
            OnPropertyChanged(nameof(TaskCheckList));
        }

        /// <summary>
        /// Update task: This method update task into the list of tasks from the TaskDataService and updates the Tasks collection.
        /// </summary>
        public void UpdateTask(Models.TaskModel updateTask)
        {
            _taskController.UpdateTask(updateTask);
            LoadTasks();
        }

        /// <summary>
        /// Delete task: This method remove task from the list of tasks from the TaskDataService and updates the Tasks collection.
        /// </summary>
        public void DeleteTask(Models.TaskModel task)
        {
            _taskController.DeleteTask(task);
            LoadTasks();
        }
    }
}
