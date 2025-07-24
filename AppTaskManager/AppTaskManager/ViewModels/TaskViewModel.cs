using AppTaskManager.Controllers;
using AppTaskManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppTaskManager.ViewModels
{
    /// <summary>
    /// Model view for Task
    /// </summary>
    public class TaskViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// TaskController Declaration. It used to interact with the underlying data storage
        /// </summary>
        private readonly TaskController _taskController;

        /// <summary>
        /// Task ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Task Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of the task
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Date and time of task create
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Date and time of task begining
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// Indicator whether the task has been completed
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Execution Timer
        /// </summary>
        public TimeSpan Timer { get; set; }

        /// <summary>
        /// Task Processing
        /// </summary>
        public TaskState TaskState { get; set; }

        /// <summary>
        /// Task category
        /// </summary>
        public TaskCategory TaskCategory { get; set; }

        /// <summary>
        /// Task importance
        /// </summary>
        public TaskImportance TaskImportance { get; set; }

        /// <summary>
        /// ObservableCollection for check list
        /// </summary>
        public ObservableCollection<TaskChecklist> TaskChecklist { get; set; }

        /// <summary>
        /// Constructor for the TaskViewModel class. It initializes the the TaskDataService and loads tasks
        /// </summary>
        public TaskViewModel()
        {
            _taskController = new TaskController();
            TaskChecklist = new ObservableCollection<TaskChecklist>();
            DateTime = DateTime.Now;
        }

        /// <summary>
        /// Event declaration for PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Invoke event PropertyChanged
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Private field holding the collection of task models
        /// </summary>
        private ObservableCollection<Models.Task> _tasks;

        /// <summary>
        /// Public property for tasks
        /// </summary>
        public ObservableCollection<Models.Task> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }

        /// <summary>
        /// Load Tasks: This method loads the list of tasks from the TaskDataService and updates the Tasks collection.
        /// The Tasks collection is bound to the view, so updating this collection will update the UI accordingly.
        /// </summary>
        private void LoadTasks()
        {
            var TaskList = _taskController.LoadTasks();
            Tasks = new ObservableCollection<Models.Task>(TaskList);
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
            Models.Task newTask = new Models.Task
            {
                Id = _taskController.GenerateNewTaskId(),
                Title = this.Title,
                Description = this.Description,
                DateTime = this.DateTime,
                IsComplete = false,
                StartDateTime = DateTime.Now,
                TaskCategory = TaskCategory.Education,
                TaskChecklist = this.TaskChecklist.ToList(),
                TaskImportance = TaskImportance.Critical,
                TaskState = TaskState.Late,
                Timer = new TimeSpan(0),
            };
            _taskController.AddTask(newTask);
            //Reload tasks to reflect the new addition
            LoadTasks();
            //Remove all data from fields
            ClearFields();
        }

        /// <summary>
        /// Clear task command
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
            Title = "";
            Description = "";
            DateTime = DateTime.Now;
            TaskChecklist.Clear();
            OnPropertyChanged(Title);
            OnPropertyChanged(Description);
            OnPropertyChanged(nameof(DateTime));
            OnPropertyChanged(nameof(TaskChecklist));
        }

        /// <summary>
        /// Update task: This method update task into the list of tasks from the TaskDataService and updates the Tasks collection.
        /// </summary>
        public void UpdateTask(Models.Task updateTask)
        {
            _taskController.UpdateTask(updateTask);
            LoadTasks();
        }

        /// <summary>
        /// Delete task: This method remove task from the list of tasks from the TaskDataService and updates the Tasks collection.
        /// </summary>
        public void DeleteTask(Models.Task task)
        {
            _taskController.DeleteTask(task);
            LoadTasks();
        }
    }
}
