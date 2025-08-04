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
        private TaskViewModel _inputTaskViewModel;

        public NewTaskWindow(TaskViewModel inputTaskViewModel)
        {
            InitializeComponent();
            _inputTaskViewModel = inputTaskViewModel;
            DataContext = _inputTaskViewModel;
        }

        private void TaskList_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (TaskList.SelectedItem is TaskCheck selectedCheck)
            {
                _inputTaskViewModel.SelectedCheck = selectedCheck;
            }
        }

        private void Importance_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxImportance.SelectedItem is TaskImportance importance)
            {
                _inputTaskViewModel.TaskModel.TaskImportance = importance;
            }
        }

        private void Category_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCategory.SelectedItem is TaskCategory category)
            {
                _inputTaskViewModel.TaskModel.TaskCategory = category;
            }
        }
    }
}
