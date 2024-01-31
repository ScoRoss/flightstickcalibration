using Newtonsoft.Json.Linq;
using System;
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
                        // Assuming your XAML contains a UniformGrid named "UniformGridButtons"
                        JArray actionMaps = jsonData["ActionMaps"]?["actionmap"] as JArray;

                        if (actionMaps != null)
                        {
                            foreach (JObject actionMap in actionMaps)
                            {
                                // Assuming each action map has a unique identifier
                                string actionMapName = actionMap["@name"]?.ToString();

                                if (!string.IsNullOrEmpty(actionMapName))
                                {
                                    // Create a button for each action map with some margin for spacing
                                    Button button = new Button();
                                    button.Content = actionMapName;
                                    button.Margin = new Thickness(10); // Adjust the margin as needed
                                    button.Click += (sender, e) => HandleButtonClick(actionMapName);

                                    // Add the button to your UI (UniformGrid in this case)
                                    UniformGridButtons.Children.Add(button);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating buttons: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void HandleButtonClick(string actionMapName)
        {
            // Handle button click based on the associated action map
            MessageBox.Show($"Button clicked for action map: {actionMapName}");
        }
    }
}
