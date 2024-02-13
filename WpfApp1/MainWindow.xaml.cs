using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window, INotifyPropertyChanged
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

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this; // Set DataContext to the current instance of MainWindow

            // Initialize the JoystickManager
            JoystickManager.Initialize();
        }


        private void DetectJoysticksButton_Click(object sender, RoutedEventArgs e)
        {
            // Detect and display connected joysticks
            List<string> joystickNames = JoystickManager.GetJoystickNames();

            // You can display the joystick names in a MessageBox or any other UI element
            MessageBox.Show($"Connected Joysticks:\n{string.Join("\n", joystickNames)}", "Joystick Detection");
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Update the SelectedFilePath property
                SelectedFilePath = openFileDialog.FileName;
                MessageBox.Show($"Selected File: {SelectedFilePath}");
            }
        }

        private void BindButtonsButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the BindButtonsWindow and pass the selected file path
            BindButtonsWindow bindButtonsWindow = new BindButtonsWindow(SelectedFilePath);
            bindButtonsWindow.Show();
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
