using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using SharpDX.DirectInput;

namespace WpfApp1
{
    public partial class BindButtonsWindow : Window
    {
        private readonly BindButtonsWindowViewModel _viewModel;
        private DirectInput _directInput;
        private JoystickDevice _selectedJoystick;
        private JsonFileManager _jsonFileManager; // Add this field
        // BindButtonsWindow constructor
        public BindButtonsWindow(string selectedFilePath, string selectedJoystickName)
        {
            InitializeComponent();
            _viewModel = new BindButtonsWindowViewModel(selectedFilePath);
            DataContext = _viewModel;
            _directInput = new DirectInput();
            _jsonFileManager = new JsonFileManager(_viewModel.JsonContent); // Initialize JsonFileManager

            // Initialize joystick detection and update the ComboBox
            UpdateJoystickList();

            // Call the method to generate buttons based on JSON content
            GenerateButtonsFromJson();
        }

        private void UpdateJoystickList()
        {
            var joystickNames = JoystickManager.GetJoystickNames();
            ComboBoxJoysticks.ItemsSource = joystickNames;

            // Automatically select the first joystick, if available
            if (joystickNames.Count > 0)
            {
                ComboBoxJoysticks.SelectedIndex = 0; // Select the first joystick
            }
        }
        // Method to generate buttons based on JSON content
        private void GenerateButtonsFromJson()
        {
            if (!string.IsNullOrEmpty(_viewModel.JsonContent))
            {
                try
                {
                    // Deserialize JSON content into JObject
                    JObject jsonData = JObject.Parse(_viewModel.JsonContent);

                    if (jsonData != null)
                    {
                        // Create buttons for all @input elements
                        GenerateButtonsForInputs(jsonData);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating buttons: {ex.Message}", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
        // Method to generate buttons based on JSON content
        private void GenerateButtonsForInputs(JObject jsonData)
        {
            JArray actionMaps = jsonData["ActionMaps"]?["actionmap"] as JArray;

            if (actionMaps != null)
            {
                foreach (JObject actionMap in actionMaps)
                {
                    JArray actions = actionMap["action"] as JArray;

                    if (actions != null)
                    {
                        foreach (JObject action in actions)
                        {
                            string inputName = action["rebind"]?["@input"]?.ToString();
                            inputName = inputName?.TrimStart('@'); // Remove @ symbol from the beginning, if present

                            string name = action["@name"]?.ToString();

                            if (!string.IsNullOrEmpty(inputName))
                            {
                                // Add debug output to check if the correct data is being retrieved
                                Debug.WriteLine($"Found Input: {inputName}, Name: {name}");

                                // Create a button for each input with some margin for spacing
                                Button button = new Button();

                                // Make the button content the concatenated input and name
                                button.Content = $"{inputName}\n{name}";

                                // Set the font size and style as needed
                                button.FontSize = 16;
                                button.FontWeight = FontWeights.Bold;

                                // Set other properties as needed (e.g., margin)
                                button.Margin = new Thickness(10); // Adjust the margin as needed
                                button.Click += (sender, e) => PromptUserToPressJoyStickButton(inputName);

                                // Add the button to your UI (UniformGrid in this case)
                                Dispatcher.Invoke(() => UniformGridButtons.Children.Add(button));
                            }
                        }
                    }
                }
            }
        }
        // Prompt the user to select a joystick
        private void PromptUserToPressJoyStickButton(string selectedUiButton)
        {
            // Use MessageBox to prompt the user to select a joystick
            MessageBox.Show("Please select a joystick.", "Select Joystick", MessageBoxButton.OK, MessageBoxImage.Information);

            // Assuming you have a ComboBox named ComboBoxJoysticks
            string selectedJoystickName = ComboBoxJoysticks.SelectedItem?.ToString();

            Debug.WriteLine($"Selected joystick name: {selectedJoystickName}");

            // Obtain the selected joystick using the joystick name
            _selectedJoystick = JoystickManager.GetJoystickByName(selectedJoystickName);

            if (_selectedJoystick != null)
            {
                // Capture the button input from the joystick
                string capturedButton = _selectedJoystick.CapturePressedButton();
                Debug.WriteLine($"Joystick found: {_selectedJoystick.Name}");

                // Update the JSON file with the button that was pressed using JsonFileManager
                _jsonFileManager.UpdateJsonWithButton(selectedUiButton, capturedButton);

                // Optionally, you can update the JsonContent in the ViewModel
                _viewModel.JsonContent = _jsonFileManager.JsonContent;
            }
            else
            {
                Debug.WriteLine("Selected joystick not found");
                // Handle the case where the selected joystick name is not found
                MessageBox.Show("Selected joystick not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ComboBoxJoysticks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle the selection changed event if needed
        }
    }
}
