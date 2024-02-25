using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;
using SharpDX.DirectInput;
using static WpfApp1.RelayCommand;

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
        public string XmlContent
        {
            get { return _xmlContent; }
            set
            {
                _xmlContent = value;
                OnPropertyChanged(); // Make sure to raise the PropertyChanged event when the property changes
            }
        }
        private string _xmlContent;
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
        public ICommand SaveCommand { get; }

        public BindButtonsWindowViewModel(string selectedFilePath)
        {
            SelectedFilePath = selectedFilePath;
            InitializeJoystick();
            SaveCommand = new RelayCommand(SaveCommandExecute, SaveCommandCanExecute);
        }

        private void InitializeJoystick()
        {
            _directInput = new DirectInput();
            _joysticks = JoystickManager.GetConnectedJoysticks(_directInput);
        }

        private void SaveCommandExecute()
        {
            SaveJsonContent();
        }

        private bool SaveCommandCanExecute()
        {
            // You can add conditions here to determine if the command can be executed
            return true;
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
                XmlDocument xmlDoc = JsonConvert.DeserializeXmlNode(jsonContent, "root");
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

        // Method to save JSON content as XML to a file, handling file selection, conversion, sorting, and potential exceptions
        public void SaveJsonContent()
        {
            if (String.IsNullOrEmpty(_jsonContent))
            {
                MessageBox.Show("JSON content is empty. Nothing to save.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "XML Files|*.xml|All Files|*.*",
                DefaultExt = "xml",
                FileName = Path.GetFileNameWithoutExtension(_selectedFilePath) // Set the default file name without extension
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                _selectedFilePath = saveFileDialog.FileName;

                try
                {
                    // Convert JSON to XML
                    string xmlContent = ConvertJsonToXml(_jsonContent);

                    if (String.IsNullOrEmpty(xmlContent))
                    {
                        MessageBox.Show("Error converting JSON to XML. Cannot save.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Load the XML content into an XmlDocument
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlContent);

                    // Sort the XML document
                    SortXml(xmlDoc.DocumentElement);

                    // Save the sorted XML content to the selected file
                    xmlDoc.Save(_selectedFilePath);

                    Debug.WriteLine($"XML content saved to: {_selectedFilePath}");

                    MessageBox.Show($"XML content saved to: {_selectedFilePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving XML content: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Save operation canceled.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private static void SortXml(XmlNode node)
        {
            if (node == null || node.ChildNodes.Count == 0)
            {
                return;
            }

            // Sort child nodes alphabetically by name
            var sortedNodes = node.ChildNodes.Cast<XmlNode>()
                                  .OrderBy(n => n.Name, StringComparer.OrdinalIgnoreCase)
                                  .ToList();

            // Remove existing child nodes
            node.RemoveAll();

            // Add sorted child nodes back to the parent node
            foreach (var sortedNode in sortedNodes)
            {
                node.AppendChild(sortedNode);
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
