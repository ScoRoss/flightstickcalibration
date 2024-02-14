using System;
using System.IO;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace WpfApp1
{
    public class BindButtonsWindowViewModel : INotifyPropertyChanged
    {
        private string _selectedFilePath;
        private string _jsonContent; // Store JSON content as a variable
        private string _loadedFilePath; // Added LoadedFilePath property

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

        public BindButtonsWindowViewModel(string selectedFilePath)
        {
            SelectedFilePath = selectedFilePath;
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

        public void SaveJsonContent()
        {
            if (!String.IsNullOrEmpty(_jsonContent))
            {
                try
                {
                    // Write JSON content back to the file
                    File.WriteAllText(SelectedFilePath, _jsonContent);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving JSON content: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
        }
        
        

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        
    }
    
}
