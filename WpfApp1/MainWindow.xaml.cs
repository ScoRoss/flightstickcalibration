using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _selectedFilePath;
        private string _copiedFilePath;

        public string CopiedFilePath
        {
            get { return _copiedFilePath; }
            set { _copiedFilePath = value; }
        }

        public string SelectedFilePath
        {
            get { return _selectedFilePath; }
            set
            {
                _selectedFilePath = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            JoystickManager.Initialize();
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedFilePath = openFileDialog.FileName;
                CopiedFilePath = SelectedFilePath; // Set CopiedFilePath when file is selected
                MessageBox.Show($"Selected File: {SelectedFilePath}");
            }
        }

        private void BindButtonsButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> joystickNames = JoystickManager.GetJoystickNames();
            MessageBox.Show($"Connected Joysticks:\n{string.Join("\n", joystickNames)}", "Joystick Detection");

            if (joystickNames.Count > 0)
            {
                string selectedJoystickName = joystickNames[0];

                // Create an instance of JsonFileManager using the constructor that accepts CopiedFilePath
                JsonFileManager jsonFileManager = new JsonFileManager(null, null)
                {
                    CopiedFilePath = CopiedFilePath
                };

                BindButtonsWindow bindButtonsWindow = new BindButtonsWindow(SelectedFilePath, selectedJoystickName);
                bindButtonsWindow.Show();

                JoystickDevice selectedJoystick = JoystickManager.GetJoystickByName(selectedJoystickName);

                if (selectedJoystick != null)
                {
                    selectedJoystick.DebugMonitorButtonPresses();
                }
                else
                {
                    MessageBox.Show("Selected joystick not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No joysticks detected.", "Joystick Detection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
