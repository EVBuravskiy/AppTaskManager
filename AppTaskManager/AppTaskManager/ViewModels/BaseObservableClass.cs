using System.ComponentModel;


namespace AppTaskManager.ViewModels
{
    /// <summary>
    /// Base observable class for view models
    /// </summary>
    public class BaseObservableClass : INotifyPropertyChanged
    {
        /// <summary>
        /// Property change event
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Generate change event if property was change
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
