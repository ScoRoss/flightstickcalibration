using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;
using SharpDX.DirectInput;

namespace WpfApp1
{
    public class BindButtonsWindowViewModel : INotifyPropertyChanged
    {
        private string _selectedFilePath;
        private string _jsonContent; // Store JSON content as a variable
        private string _loadedFilePath; // Added LoadedFilePath property
        private string _currentButtonPressed; // New property for currently pressed button
        private JoystickDevice _selectedJoystick; // Add this field

        public string SelectedFilePath
        {
            get { return _selectedFilePath; }
            set
            {
                _selectedFilePath = value;
                LoadJsonContent(); // Load JSON content when the file path changes
                OnPropertyChanged();
            }
        }

        public string JsonContent
        {
            get { return _jsonContent; }
            set
            {
                _jsonContent = value;
                OnPropertyChanged();
            }
        }

        // Added LoadedFilePath property
        public string LoadedFilePath
        {
            get { return _loadedFilePath; }
            set
            {
                _loadedFilePath = value;
                OnPropertyChanged();
            }
        }

        // New property for currently pressed button
        public string CurrentButtonPressed
        {
            get { return _currentButtonPressed; }
            set
            {
                _currentButtonPressed = value;
                OnPropertyChanged();
                HandleButtonPress(); // Handle the button press when it changes
            }
        }

        // Joystick detection code
        private DirectInput _directInput;
        private List<JoystickDevice> _joysticks;

        public BindButtonsWindowViewModel(string selectedFilePath)
        {
            SelectedFilePath = selectedFilePath;
            InitializeJoystick();
        }

        private void InitializeJoystick()
        {
            _directInput = new DirectInput();
            _joysticks = JoystickManager.GetConnectedJoysticks(_directInput);
        }

        private void LoadJsonContent()
        {
            if (File.Exists(SelectedFilePath))
            {
                try
                {
                    // Read XML content from the file
                    string xmlContent = File.ReadAllText(SelectedFilePath);

                    Console.WriteLine($"File loaded: {SelectedFilePath}");
                    Console.WriteLine($"File content:\n{xmlContent}");

                    // Convert XML to JSON
                    JsonContent = ConvertXmlToJson(xmlContent);

                    // Set the LoadedFilePath property
                    LoadedFilePath = SelectedFilePath;

                    Console.WriteLine($"JSON content:\n{JsonContent}");  // Print the JSON content to the console
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading JSON content: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Console.WriteLine($"File not found: {SelectedFilePath}");
            }
        }

        private string ConvertXmlToJson(string xmlContent)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);

                // Convert XML to JSON using Newtonsoft.Json
                string jsonContent = JsonConvert.SerializeXmlNode(xmlDoc, Formatting.Indented);

                return jsonContent;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error converting XML to JSON: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public string ConvertJsonToXml(string jsonContent)
        {
            try
            {
                // Convert JSON to XML using Newtonsoft.Json
                XmlDocument xmlDoc = JsonConvert.DeserializeXmlNode(jsonContent);

                // Format the XML content
                StringWriter sw = new StringWriter();
                XmlTextWriter xtw = new XmlTextWriter(sw);
                xmlDoc.WriteTo(xtw);
                string formattedXml = sw.ToString();

                return formattedXml;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error converting JSON to XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public void SaveJsonContent()
        {
            if (String.IsNullOrEmpty(_jsonContent))
            {
                MessageBox.Show("JSON content is empty. Nothing to save.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON Files|*.json|All Files|*.*",
                DefaultExt = "json",
                FileName = Path.GetFileName(_selectedFilePath) // Set the default file name
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                _selectedFilePath = saveFileDialog.FileName;

                try
                {
                    File.WriteAllText(_selectedFilePath, _jsonContent);
                    Debug.WriteLine($"JSON content saved to: {_selectedFilePath}");

                    MessageBox.Show($"JSON content saved to: {_selectedFilePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving JSON content: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Save operation canceled.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void HandleButtonPress()
        {
            // Handle the button press here, if needed
            // For example, you can perform additional actions based on the pressed button
            Debug.WriteLine($"Button Pressed: {CurrentButtonPressed}");

            // Obtain the selected joystick using the joystick name
            _selectedJoystick = JoystickManager.GetJoystickByName(_currentButtonPressed);

            if (_selectedJoystick != null)
            {
                Debug.WriteLine("Selected joystick found");

                // Optionally, you can perform actions with the selected joystick
                // For example, check if a specific button is pressed on the joystick
                bool isButtonPressed = _selectedJoystick.IsButtonDown(0); // Replace 0 with the actual button index

                if (isButtonPressed)
                {
                    Debug.WriteLine("Button 0 is pressed on the selected joystick");
                    
                }
                else
                {
                    Debug.WriteLine("Button 0 is not pressed on the selected joystick");
                }
            }
            else
            {
                Debug.WriteLine("Selected joystick not found");
                // Handle the case where the selected joystick name is not found
                MessageBox.Show("Selected joystick not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
