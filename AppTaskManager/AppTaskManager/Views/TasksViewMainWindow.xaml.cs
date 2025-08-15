using AppTaskManager.ViewModels;
using System.Windows;
using System.Windows.Controls;
using static AppTaskManager.Views.TasksViewMainWindow;

namespace AppTaskManager.Views
{
    /// <summary>
    /// Логика взаимодействия для TasksViewMainWindow.xaml
    /// </summary>
    public partial class TasksViewMainWindow : Window, ICodeBehind
    {
        /// <summary>
        /// Interface code behind for selected dates in calendar
        /// </summary>
        public interface ICodeBehind
        {
            void SelectManyDates(List<DateTime> manyDates);
        }

        /// <summary>
        /// Field holding Main Window View Model
        /// </summary>
        private MainWindowViewModel mainModel;

        /// <summary>
        /// Task View Main Window constructor
        /// </summary>
        public TasksViewMainWindow()
        {
            InitializeComponent();
            mainModel = new MainWindowViewModel(this);
            DataContext = mainModel;
            ((MainWindowViewModel)DataContext).MainWindowCodeBehind = this;
        }

        /// <summary>
        /// Send a collection of dates to a calendar
        /// </summary>
        /// <param name="manyDates"></param>
        public void SelectManyDates(List<DateTime> manyDates)
        {
            SelectedDatesCollection Dates = TaskCalendar.SelectedDates;
            foreach (DateTime date in manyDates)
            {
                Dates.Add(date);
            }
        }

        /// <summary>
        /// Get selected dates from calendar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                return;
            }
            List<DateTime> manyDates = new List<DateTime>();
            foreach (DateTime date in e.AddedItems)
            {
                manyDates.Add(date);
            }
            mainModel.LoadTasksSelectedDates(manyDates);
        }
    }
}
