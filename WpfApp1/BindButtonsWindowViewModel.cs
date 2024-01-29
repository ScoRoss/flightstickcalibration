using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1
{
    public class BindButtonsWindowViewModel : INotifyPropertyChanged
    {
        
        private string _selectedFilePath;

        public string SelectedFilePath
        {
            get { return _selectedFilePath; }
            set
            {
                _selectedFilePath = value;
                OnPropertyChanged();
            }
        }

        public BindButtonsWindowViewModel(string selectedFilePath)
        {
            SelectedFilePath = selectedFilePath;
        }
        

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}