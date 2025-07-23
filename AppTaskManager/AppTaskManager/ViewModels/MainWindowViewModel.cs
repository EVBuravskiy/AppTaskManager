using AppTaskManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppTaskManager.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ICommand IOpenNewWindow => new RelayCommand(OpenNewWindow);
        private void OpenNewWindow()
        {
            NewTaskWindow newTaskWindow = new NewTaskWindow();
            newTaskWindow.Show();
        }
    }
}