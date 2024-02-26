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
        private JsonFileManager _jsonFileManager;
        private JoystickManager _joystickManager;
        private JoystickDevice _selectedJoystickDevice;
        public BindButtonsWindow(string selectedFilePath, string selectedJoystickName)
        {
            InitializeComponent();
            _viewModel = new BindButtonsWindowViewModel(selectedFilePath);
            DataContext = _viewModel;
            _directInput = new DirectInput();
            _jsonFileManager = new JsonFileManager(_viewModel.JsonContent, _viewModel.XmlContent);
            _joystickManager = new JoystickManager();

            // Initialize joystick detection and update the ComboBox
            JoystickManager.Initialize();
            UpdateJoystickList();

            // Call the method to generate buttons based on JSON content
            GenerateButtonsFromJson();
        }


        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            // Call the SaveJsonContent and ConvertJsonToXml methods
            SaveAndConvertToJson();

            // Optionally, you can display a message to the user or perform additional actions
            MessageBox.Show("JSON content saved and converted to XML.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveAndConvertToJson()
        {
            // Call the SaveJsonContent and ConvertJsonToXml methods
            _viewModel.SaveJsonContent();
            string xmlContent = _viewModel.ConvertJsonToXml(_viewModel.JsonContent);

            // Display the XML content (for testing purposes)
            if (!string.IsNullOrEmpty(xmlContent))
            {
                Debug.WriteLine($"Converted XML content:\n{xmlContent}");
            }
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
                                Debug.WriteLine($"Found Input: {inputName}, Name: {name}");

                                Button button = new Button
                                {
                                    Content = $"{inputName}\n{name}",
                                    FontSize = 16,
                                    FontWeight = FontWeights.Bold,
                                    Margin = new Thickness(10),
                                    Tag = inputName // Use Tag to store the inputName for later retrieval
                                };

                                // Register the Click event handler
                                button.Click += Button_Click;

                                string category = GetButtonCategory(inputName);
                                AddButtonToCategory(button, category);
                            }
                        }
                    }
                }
            }
        }
        private void PromptUserToPressJoyStickButton(string selectedUiButton)
        {
            // Assume ComboBoxJoysticks holds the name of the selected joystick
            string selectedJoystickName = ComboBoxJoysticks.SelectedItem.ToString();

            // Retrieve the JoystickDevice instance for the selected joystick
            var selectedJoystick = JoystickManager.GetJoystickByName(selectedJoystickName);

            if (selectedJoystick != null)
            {
                string capturedButton = selectedJoystick.CapturePressedButton();

                if (!string.IsNullOrEmpty(capturedButton))
                {
                    // Assuming _jsonFileManager is correctly initialized and can update the JSON/XML
                    _jsonFileManager.UpdateXmlWithButton(selectedUiButton, capturedButton, _viewModel.XmlContent);

                    // Refresh UI or notify the user as needed...
                    MessageBox.Show($"Button '{capturedButton}' captured for action '{selectedUiButton}'.", "Capture Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No button press detected.", "Capture Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("No joystick device selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is string selectedUiButton)
            {
                // Directly call PromptUserToPressJoyStickButton with the selected UI button's input name
                PromptUserToPressJoyStickButton(selectedUiButton);
            }
        }



        private string GetButtonCategory(string inputName)
        {
            // Determine the category of the button based on its input name prefix
            if (inputName.StartsWith("js1_"))
            {
                return "JS1 Buttons"; // Category for js1_ buttons
            }
            else if (inputName.StartsWith("js2_"))
            {
                return "JS2 Buttons"; // Category for js2_ buttons
            }
            else
            {
                return "Other Buttons"; // Category for buttons with unknown prefixes
            }
        }

        private void AddButtonToCategory(Button button, string category)
        {
            // Find or create a stack panel for the category
            StackPanel categoryPanel = UniformGridButtons.Children
                                            .OfType<StackPanel>()
                                            .FirstOrDefault(panel => panel.Tag?.ToString() == category);

            if (categoryPanel == null)
            {
                // Create a new stack panel for the category
                categoryPanel = new StackPanel();
                categoryPanel.Tag = category; // Tag the panel with the category name

                // Set orientation and other properties of the stack panel
                categoryPanel.Orientation = Orientation.Vertical;
                categoryPanel.Margin = new Thickness(10); // Adjust the margin as needed

                // Create a label for the category
                Label categoryLabel = new Label();
                categoryLabel.Content = category;
                categoryLabel.FontSize = 18;
                categoryLabel.FontWeight = FontWeights.Bold;
                categoryLabel.Margin = new Thickness(0, 0, 0, 5); // Add margin below the label

                // Add the label to the category panel
                categoryPanel.Children.Add(categoryLabel);

                // Add the category panel to the UniformGrid
                UniformGridButtons.Children.Add(categoryPanel);
            }

            // Add the button to the category panel
            categoryPanel.Children.Add(button);
        }




        private void ComboBoxJoysticks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle the selection changed event if needed
        }
    }
}