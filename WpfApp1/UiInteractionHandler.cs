using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public class UiInteractionHandler
    {
        private readonly JsonFileManager _jsonFileManager;
        private readonly BindButtonsWindowViewModel _viewModel;

        public UiInteractionHandler(JsonFileManager jsonFileManager, BindButtonsWindowViewModel viewModel)
        {
            _jsonFileManager = jsonFileManager ?? throw new ArgumentNullException(nameof(jsonFileManager));
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }

        public void HandleButtonClick(string xmlLocation, ComboBox comboBox)
        {
            // Handle button click logic here
            PromptUserToPressJoyStickButton(xmlLocation, comboBox);
        }

        private void PromptUserToPressJoyStickButton(string selectedUiButton, ComboBox comboBox)
        {
            MessageBox.Show($"Please press the joystick button for {selectedUiButton} and confirm the binding.", "Press Joystick Button",
                MessageBoxButton.OK, MessageBoxImage.Information);

            string selectedJoystickName = comboBox.SelectedItem?.ToString();

            Console.WriteLine($"Selected joystick name: {selectedJoystickName}");

            JoystickDevice selectedJoystick = JoystickManager.GetJoystickByName(selectedJoystickName);

            if (selectedJoystick != null)
            {
                string capturedButton = selectedJoystick.CapturePressedButton();

                MessageBoxResult result = MessageBox.Show($"Do you want to bind '{selectedUiButton}' to joystick button '{capturedButton}'?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Update XML content using JsonFileManager logic
                    _jsonFileManager.UpdateXmlWithButton(selectedUiButton, capturedButton, _viewModel.XmlContent);
                    _viewModel.XmlContent = _jsonFileManager.GetXmlContent(); // Get updated XML content
                }
                else
                {
                    MessageBox.Show("Binding change canceled.", "Canceled", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                Console.WriteLine("Selected joystick not found");
                MessageBox.Show("Selected joystick not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
