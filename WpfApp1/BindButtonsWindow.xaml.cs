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
    public class ActionInfo
    {
        public string ActionName { get; set; }
        public string InputBinding { get; set; }
        public string NewInputBinding { get; set; } 
    }

}