using AppTaskManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static AppTaskManager.Views.TasksViewMainWindow;

namespace AppTaskManager.Views
{
    /// <summary>
    /// Логика взаимодействия для TasksViewMainWindow.xaml
    /// </summary>
    public partial class TasksViewMainWindow : Window, ICodeBehind
    {
        public interface ICodeBehind
        {
            void SelectManyDates(List<DateTime> manyDates);
        }

        private MainWindowViewModel mainModel;

        public TasksViewMainWindow()
        {
            InitializeComponent();
            mainModel = new MainWindowViewModel(this);
            DataContext = mainModel;
            ((MainWindowViewModel)DataContext).MainWindowCodeBehind = this;
        }
        public void SelectManyDates(List<DateTime> manyDates)
        {
            SelectedDatesCollection Dates = TaskCalendar.SelectedDates;
            foreach (DateTime date in manyDates)
            {
                Dates.Add(date);
            }
        }

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
