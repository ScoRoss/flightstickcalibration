using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;

namespace WpfApp1
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private XmlFileHandler _xmlFileManager;
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
            DataContext = this;

            JoystickManager.Initialize();
            _xmlFileManager = new XmlFileHandler(null, null); // Initialize XmlFileManager instance
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                // Generate a new file name for the copied file, if needed
                string destinationFilePath = Path.Combine(Path.GetDirectoryName(selectedFilePath), "copied_" + Path.GetFileName(selectedFilePath));

                try
                {
                    // Copy the XML file to the destination path
                    File.Copy(selectedFilePath, destinationFilePath, true);

                    // Set the copied file path to the SelectedFilePath property
                    SelectedFilePath = destinationFilePath;

                    // Initialize the XmlFileHandler with the copied file path
                    _xmlFileManager = new XmlFileHandler(SelectedFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error copying XML file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void BindButtonsButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> joystickNames = JoystickManager.GetJoystickNames();
            MessageBox.Show($"Connected Joysticks:\n{string.Join("\n", joystickNames)}", "Joystick Detection");

            if (joystickNames.Count > 0)
            {
                string selectedJoystickName = joystickNames[0];

                var bindButtonsWindow = new BindButtonsWindow(_selectedFilePath, selectedJoystickName);
                bindButtonsWindow.ShowDialog();

                JoystickDevice selectedJoystick = JoystickManager.GetJoystickByName(selectedJoystickName);

                if (selectedJoystick != null)
                {

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
