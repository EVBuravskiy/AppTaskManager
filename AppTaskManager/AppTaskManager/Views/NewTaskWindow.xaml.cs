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
    /// A class associated with a view that contains the logic of the work.
    /// </summary>
    public partial class NewTaskWindow : Window
    {
        public NewTaskWindow()
        {
            InitializeComponent();

            DataContext = new TaskViewModel();
        }

        private void TaskList_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (TaskList.SelectedItem is TaskCheck selectedCheck)
            {
                TaskViewModel viewModel = (TaskViewModel)DataContext;
                viewModel.CheckedTask = selectedCheck;
            }
        }

        private void Importance_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxImportance.SelectedItem is TaskImportance importance)
            {
                TaskViewModel viewModel = (TaskViewModel)DataContext;
                viewModel.TaskModel.TaskImportance = importance;
            }
        }

        private void Category_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCategory.SelectedItem is TaskCategory category)
            {
                TaskViewModel viewModel = (TaskViewModel)DataContext;
                viewModel.TaskModel.TaskCategory = category;
            }
        }
    }
}
