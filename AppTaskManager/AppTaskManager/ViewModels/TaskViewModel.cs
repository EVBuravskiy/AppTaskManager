using AppTaskManager.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Constructor for the TaskViewModel class. It initializes the the TaskDataService and loads tasks
        /// </summary>
        public TaskViewModel()
        {
            _taskController = new TaskController();
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
        /// Add new task: This method add new task to the list of tasks from the TaskDataService and updates the Tasks collection.
        /// </summary>
        public void AddNewTask(Models.Task newTask)
        {
            _taskController.AddTask(newTask);
            LoadTasks();
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
