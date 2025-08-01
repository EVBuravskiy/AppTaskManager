using AppTaskManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AppTaskManager.ViewModels
{
    public class MainWindowViewModel : BasePropertyChanged
    {
        private TaskViewModel _taskViewModel;
        public TaskViewModel TaskViewModel {  
            get { return _taskViewModel; } 
            set { 
                _taskViewModel = value; 
                OnPropertyChanged(nameof(TaskViewModel));
            }
        }

        public MainWindowViewModel()
        {
            TaskViewModel = new TaskViewModel();
        }

        public ICommand IOpenNewWindow => new RelayCommand(OpenNewWindow);
        private void OpenNewWindow()
        {
            NewTaskWindow newTaskWindow = new NewTaskWindow();
            newTaskWindow.Show();
        }
    }
}
