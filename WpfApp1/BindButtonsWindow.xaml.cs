using System.Windows;
using Newtonsoft.Json;
namespace WpfApp1
{
    public partial class BindButtonsWindow : Window
    {
        public BindButtonsWindow(string selectedFilePath)
        {
            InitializeComponent();
            DataContext = new BindButtonsWindowViewModel(selectedFilePath);
        }
    }    public class ActionInfo
         {
             [JsonProperty("ActionName")]
             public string ActionName { get; set; }
     
             [JsonProperty("InputBinding")]
             public string InputBinding { get; set; }
     
             [JsonProperty("NewInputBinding")]
             public string NewInputBinding { get; set; } 
         }
         
     
     }
