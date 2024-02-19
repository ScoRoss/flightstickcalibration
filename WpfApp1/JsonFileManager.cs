using System.Windows;
using Newtonsoft.Json.Linq;
using System;

namespace WpfApp1
{
    public class JsonFileManager
    {
        public string JsonContent { get; private set; }

        public JsonFileManager(string jsonContent)
        {
            JsonContent = jsonContent;
        }

        // Updates the JSON content with the selected UI button and captured button
        public void UpdateJsonWithButton(string selectedUiButton, string capturedButton)
        {
            if (!string.IsNullOrEmpty(JsonContent))
            {
                try
                {
                    // Deserialize JSON content into JObject
                    JObject jsonData = JObject.Parse(JsonContent);

                    if (jsonData != null)
                    {
                        // Find the input token corresponding to the selectedUiButton
                        JToken inputToken = FindInputToken(jsonData, selectedUiButton);

                        if (inputToken != null)
                        {
                            // Check if it's an object or an array
                            if (inputToken is JObject inputObject)
                            {
                                // If it's an object, update the specific property value
                                JProperty buttonProperty = inputObject.Property("@value");
                                if (buttonProperty != null)
                                {
                                    buttonProperty.Value = capturedButton;
                                }
                                else
                                {
                                    // If the property doesn't exist, create a new one
                                    inputObject.Add(new JProperty("@value", capturedButton));
                                }
                            }
                            else if (inputToken is JArray inputArray)
                            {
                                // If it's an array, update the first item's value
                                if (inputArray.Count > 0)
                                {
                                    JToken firstItem = inputArray[0];
                                    if (firstItem is JObject itemObject)
                                    {
                                        JProperty buttonProperty = itemObject.Property("@value");
                                        if (buttonProperty != null)
                                        {
                                            buttonProperty.Value = capturedButton;
                                        }
                                        else
                                        {
                                            // If the property doesn't exist, create a new one
                                            itemObject.Add(new JProperty("@value", capturedButton));
                                        }
                                    }
                                }
                            }

                            // Save the changes back to the JSON content
                            JsonContent = jsonData.ToString();
                        }
                        else
                        {
                            // Handle the case where the input was not found
                            MessageBox.Show($"Input '{selectedUiButton}' not found in JSON data.", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating JSON content: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Method to handle the save button press

        private JToken FindInputToken(JToken token, string selectedUiButton)
        {
            // Recursive method to find the token corresponding to the selectedUiButton
            if (token.Type == JTokenType.Object)
            {
                foreach (JProperty property in token.Children<JProperty>())
                {
                    if (property.Name == "@input" && property.Value.ToString() == selectedUiButton)
                    {
                        return property;
                    }

                    JToken result = FindInputToken(property.Value, selectedUiButton);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                foreach (JToken arrayItem in token.Children())
                {
                    JToken result = FindInputToken(arrayItem, selectedUiButton);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }
    }
}
