using AppTaskManager.Controllers;
using AppTaskManager.Models;
using AppTaskManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppTaskManager.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
       
        private TaskViewModel _taskViewModel;
        public TaskViewModel TaskViewModel
        {
            get { return _taskViewModel; }
            set { OnPropertyChanged(ref  _taskViewModel, value); }
        }

        private ITaskController _taskController;

        private ObservableCollection<TaskModel> _tasks;
        public ObservableCollection<TaskModel> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged(ref _tasks, value);
            }
        }

        public MainWindowViewModel()
        {
            _taskController = new MockTaskController();
            TaskViewModel = new TaskViewModel(_taskController);
            Tasks = TaskViewModel.TasksModels;
        }

        public ICommand IOpenNewWindow => new RelayCommand(OpenNewWindow);
        private void OpenNewWindow()
        {
            NewTaskWindow newTaskWindow = new NewTaskWindow(TaskViewModel);
            newTaskWindow.Show();
        }
    }
}
