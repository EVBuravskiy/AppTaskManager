using AppTaskManager.Models;
using AppTaskManager.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace AppTaskManager.Views
{
    /// <summary>
    /// Логика взаимодействия для NewTaskWindow.xaml
    /// </summary>
    public partial class NewTaskWindow : Window
    {
        private TaskViewModel _TaskViewModel;

        public NewTaskWindow(MainWindowViewModel mainViewModel, bool isNew = true)
        {
            InitializeComponent();
            _TaskViewModel = new TaskViewModel(mainViewModel, isNew);
            DataContext = _TaskViewModel;
        }

        /// <summary>
        /// Get selected check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskList_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (TaskList.SelectedItem is TaskCheck selectedCheck)
            {
                _TaskViewModel.SelectedCheck = selectedCheck;
            }
        }

        /// <summary>
        /// Get selected importance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Importance_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxImportance.SelectedItem is TaskImportance importance)
            {
                _TaskViewModel.TaskModel.TaskImportance = importance;
            }
        }

        /// <summary>
        /// Get selected category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Category_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCategory.SelectedItem is TaskCategory category)
            {
                _TaskViewModel.TaskModel.TaskCategory = category;
            }
        }
    }
}
