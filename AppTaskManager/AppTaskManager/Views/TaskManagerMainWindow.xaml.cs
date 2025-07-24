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
    public partial class TaskManagerMainWindow : Window
    {
        public TaskManagerMainWindow()
        {
            InitializeComponent();

            //Adding TaskViewModel to the data context
            this.DataContext = new TaskViewModel();
        }
    }
}
