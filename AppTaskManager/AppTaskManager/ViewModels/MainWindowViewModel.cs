using AppTaskManager.Controllers;
using AppTaskManager.Models;
using AppTaskManager.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppTaskManager.ViewModels
{
    public class MainWindowViewModel : BaseObservableClass
    {
       
        public ITaskController TaskController;

        public ObservableCollection<TaskModel> TaskModels { get; private set; }
        public MainWindowViewModel()
        {
            TaskController = new MockTaskController();
            TaskModels = new ObservableCollection<TaskModel>();
            LoadTasksFromController();
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
