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

        public BindButtonsWindow(string selectedFilePath)
        {
            InitializeComponent();
            _viewModel = new BindButtonsWindowViewModel(selectedFilePath);
            DataContext = _viewModel;
            _directInput = new DirectInput();



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
                                button.Click += (sender, e) => PromptUserToPressJoyStickButton(inputName, "knownJoystickNameValue");

                                

                                // Add the button to your UI (UniformGrid in this case)
                                Dispatcher.Invoke(() => UniformGridButtons.Children.Add(button));
                            }
                        }
                    }
                }
            }
        }



        // get all updates is still not in use 
        private JArray GetAllInputs(JObject jsonData)
        {
            // Retrieve all @input elements across all devices
            JArray allInputs = new JArray();

            GetAllInputsRecursive(jsonData, allInputs);

            return allInputs;
        }

// collecting all the values from the json file and removing the @
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

        // button click currentlu to be made into a modify input file 
        // this will be the tricky part and may be put into another class 
        private void PromptUserToPressJoyStickButton(string selectedUiButton, string knownJoystickName)
        {
            // Use MessageBox to prompt the user to press a joystick button
            MessageBox.Show("Please press the joystick button for " + selectedUiButton, "Press Joystick Button",
                MessageBoxButton.OK, MessageBoxImage.Information);

            // Obtain the joystickGuid using the known joystick name
            var joystickGuid = _directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices)
                .Concat(_directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                .Where(deviceInstance =>
                {
                    return deviceInstance.Type == SharpDX.DirectInput.DeviceType.Joystick &&
                           deviceInstance.ProductName == knownJoystickName;
                })
                .Select(deviceInstance => deviceInstance.InstanceGuid)
                .FirstOrDefault();

            // Create an instance of JoystickDevice using the obtained joystickGuid
            if (joystickGuid != Guid.Empty)
            {
                var joystickDevice = new JoystickDevice(_directInput, joystickGuid);

                // Capture the button input from the joystick
                string capturedButton = joystickDevice.CapturePressedButton();

                // Update the JSON file with the button that was pressed using the Newtonsoft.Json library
                // For example:
                // _viewModel.UpdateJsonWithButton(selectedUiButton, capturedButton);
            }
            else
            {
                // Handle the case where the selected joystick name is not found or no joystick is connected
                MessageBox.Show("Selected joystick not found or no joystick connected", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}    
