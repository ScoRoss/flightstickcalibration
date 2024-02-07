using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class BindButtonsWindow : Window
    {
        private readonly BindButtonsWindowViewModel _viewModel;

        public BindButtonsWindow(string selectedFilePath)
        {
            InitializeComponent();
            _viewModel = new BindButtonsWindowViewModel(selectedFilePath);
            DataContext = _viewModel;

            // Call the method to generate buttons based on JSON content
            GenerateButtonsFromJson();
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
                    MessageBox.Show($"Error generating buttons: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

                                button.Click += (sender, e) => HandleButtonClickForInput(inputName);

                                // Add the button to your UI (UniformGrid in this case)
                                Dispatcher.Invoke(() => UniformGridButtons.Children.Add(button));
                            }
                        }
                    }
                }
            }
        }

        private JArray GetAllInputs(JObject jsonData)
        {
            // Retrieve all @input elements across all devices
            JArray allInputs = new JArray();

            GetAllInputsRecursive(jsonData, allInputs);

            return allInputs;
        }

        private void GetAllInputsRecursive(JToken token, JArray allInputs)
        {
            if (token.Type == JTokenType.Object)
            {
                foreach (JProperty property in token.Children<JProperty>())
                {
                    if (property.Name == "@input")
                    {
                        allInputs.Add(property.Value);
                    }

                    GetAllInputsRecursive(property.Value, allInputs);
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                foreach (JToken arrayItem in token.Children())
                {
                    GetAllInputsRecursive(arrayItem, allInputs);
                }
            }
        }

        private void HandleButtonClickForInput(string inputName)
        {
            // Handle button click based on the associated input
            MessageBox.Show($"Button clicked for input: {inputName}");
        }
    }
}
