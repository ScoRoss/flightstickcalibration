using System.Windows;

namespace WpfApp1
{
    public partial class BindButtonsWindow : Window
    {
        public BindButtonsWindow(string selectedFilePath)
        {
            InitializeComponent();
            DataContext = new BindButtonsWindowViewModel(selectedFilePath);
        }
    }
}