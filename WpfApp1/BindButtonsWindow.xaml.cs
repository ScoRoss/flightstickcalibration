using System;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

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
                    // Deserialize JSON content into dynamic
                    dynamic jsonData = JsonConvert.DeserializeObject(_viewModel.JsonContent);

                    if (jsonData != null)
                    {
                        // Assuming your XAML contains a UniformGrid named "UniformGridButtons"
                        foreach (var property in jsonData)
                        {
                            // Create a button for each property
                            Button button = new Button();
                            button.Content = property.Name;
                            button.Click += (sender, e) => HandleButtonClick(property);

                            // Add the button to your UI (UniformGrid in this case)
                            UniformGridButtons.Children.Add(button);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating buttons: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void HandleButtonClick(dynamic property)
        {
            // Handle button click based on the associated action
            MessageBox.Show($"Button clicked: {property.Name}");
        }
    }
}
