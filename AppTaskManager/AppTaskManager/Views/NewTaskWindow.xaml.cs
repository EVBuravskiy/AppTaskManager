using AppTaskManager.Controllers;
using AppTaskManager.Models;
using AppTaskManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppTaskManager.Views
{
    /// <summary>
    /// Логика взаимодействия для NewTaskWindow.xaml
    /// </summary>
    public partial class NewTaskWindow : Window
    {
        private TaskViewModel _TaskViewModel;

        public NewTaskWindow(MainWindowViewModel mainViewModel)
        {
            InitializeComponent();
            _TaskViewModel = new TaskViewModel(mainViewModel);
            DataContext = _TaskViewModel;
        }

        private void TaskList_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (TaskList.SelectedItem is TaskCheck selectedCheck)
            {
                _TaskViewModel.SelectedCheck = selectedCheck;
            }
        }

        private void Importance_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxImportance.SelectedItem is TaskImportance importance)
            {
                _TaskViewModel.TaskModel.TaskImportance = importance;
            }
        }

        private void Category_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCategory.SelectedItem is TaskCategory category)
            {
                _TaskViewModel.TaskModel.TaskCategory = category;
            }
        }
    }
}
