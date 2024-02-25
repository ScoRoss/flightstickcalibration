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

        public BindButtonsWindow(string selectedFilePath, string selectedJoystickName)
        {
            InitializeComponent();
            _viewModel = new BindButtonsWindowViewModel(selectedFilePath);
            DataContext = _viewModel;
            _directInput = new DirectInput();
            _jsonFileManager = new JsonFileManager(_viewModel.JsonContent, _viewModel.XmlContent); 


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

                        // Determine the category of the button based on its input name
                        string category = GetButtonCategory(inputName);

                        // Add the button to the corresponding section based on category
                        AddButtonToCategory(button, category);
                    }
                }
            }
        }
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


        private void PromptUserToPressJoyStickButton(string selectedUiButton)
        {
            JoystickManager.PromptUserToPressJoyStickButton(selectedUiButton, ComboBoxJoysticks, _jsonFileManager, _viewModel);
        }

        private void ComboBoxJoysticks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle the selection changed event if needed
        }
    }
}
