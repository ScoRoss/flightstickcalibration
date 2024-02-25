using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace WpfApp1
{
    public class JsonFileManager
    {
        public event EventHandler<string> ErrorOccurred;
        private const string JsonFilter = "JSON Files|*.json|All Files|*.*";
        private const string XmlFilter = "XML Files|*.xml|All Files|*.*";


        public string JsonContent
        {
            get { return _jsonContent; }
            set { _jsonContent = value; }
        }
        private string _jsonContent;
        private string _copiedFilePath;
        public string CopiedFilePath
        {
            get { return _copiedFilePath; }
            set { _copiedFilePath = value; }
        }
        public string XmlContent
        {
            get { return _xmlContent; }
            set { _xmlContent = value; }
        }
        private string _xmlContent;

        public JsonFileManager(string jsonContent, string xmlContent)
        {
            JsonContent = jsonContent;
            XmlContent = xmlContent;
        }
        public string GetXmlContent()
        {
            try
            {
                // Check if the copied XML file path is not null or empty
                if (!string.IsNullOrEmpty(CopiedFilePath))
                {
                    // Read the content of the copied XML file
                    string xmlContent = File.ReadAllText(CopiedFilePath);
                    return xmlContent;
                }
                else
                {
                    MessageBox.Show("No XML file has been copied yet.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading XML content: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return string.Empty; // Return an empty string if no content is retrieved or an error occurs
        }
        // Updates the XML content with the selected UI button and captured button
        public void UpdateXmlWithButton(string selectedUiButton, string capturedButton, string xmlContentFromWindow)
        {
            if (!string.IsNullOrEmpty(xmlContentFromWindow))
            {
                try
                {
                    // Load XML content from the window
                    XDocument xmlDoc = XDocument.Parse(xmlContentFromWindow);

                    if (xmlDoc != null)
                    {
                        // Debug statement: Print the XML content to the console
                        Debug.WriteLine($"Original XML Content: {xmlDoc}");

                        // Find the rebind element corresponding to the selectedUiButton
                        XElement rebindElement = FindRebindElement(xmlDoc.Root, selectedUiButton);

                        if (rebindElement != null)
                        {
                            // Print the information in the debug console
                            Debug.WriteLine($"Updating XML for '{selectedUiButton}' - Changing button to '{capturedButton}'");

                            // Update the XML content using the UpdateXmlAttributeValue method
                            xmlContentFromWindow = UpdateXmlAttributeValue(xmlDoc, selectedUiButton, $"js1_{capturedButton}");

                            // Debug statement: Print the modified XML content to the console
                            Debug.WriteLine($"Modified XML Content: {xmlContentFromWindow}");

                            // Optionally, you can save the modified XML content to a temporary file or perform other actions
                        }
                        else
                        {
                            // Handle the case where the rebind element was not found
                            MessageBox.Show($"Rebind element for '{selectedUiButton}' not found in XML data.", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating XML content: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string UpdateXmlAttributeValue(XDocument xmlDoc, string selectedUiButton, string newValue)
        {
            try
            {
                // Find the rebind element corresponding to the selectedUiButton
                var rebindElement = xmlDoc.Root
                    .Elements("actionmap")
                    .Elements("action")
                    .Elements("rebind")
                    .Where(rebind => (string)rebind.Attribute("input") == selectedUiButton)
                    .FirstOrDefault();

                if (rebindElement != null)
                {
                    // Print the information in the debug console
                    Debug.WriteLine($"Updating XML for '{selectedUiButton}' - Changing button to '{newValue}'");

                    // Update the input attribute value
                    rebindElement.SetAttributeValue("input", newValue);

                    // Save the changes back to the XML content
                    return xmlDoc.ToString();
                }
                else
                {
                    // Handle the case where the rebind element was not found
                    MessageBox.Show($"Rebind element for '{selectedUiButton}' not found in XML data.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating XML content: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return xmlDoc.ToString(); // Return the original XML if an error occurs
        }

        // Method to handle the save button press for JSON file
        public void SaveJsonToFile(string filePath = null)
        {
            if (string.IsNullOrEmpty(JsonContent))
            {
                MessageBox.Show("JSON content is empty. Cannot save.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // If filePath is not specified, show the save dialog
                if (string.IsNullOrEmpty(filePath))
                {
                    var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                    {
                        Filter = JsonFilter,
                        DefaultExt = ".json"
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        filePath = saveFileDialog.FileName;
                    }
                }

                // Save JSON content to the specified file
                File.WriteAllText(filePath, JsonContent);
                MessageBox.Show($"JSON content saved to {filePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving JSON content: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to handle the save button press for XML file
        public void SaveXmlToFile(string filePath = null)
        {
            if (string.IsNullOrEmpty(XmlContent))
            {
                MessageBox.Show("XML content is empty. Cannot save.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // If filePath is not specified, show the save dialog
                if (string.IsNullOrEmpty(filePath))
                {
                    var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                    {
                        Filter = XmlFilter,
                        DefaultExt = ".xml"
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        filePath = saveFileDialog.FileName;
                    }
                }

                // Save XML content to the specified file
                File.WriteAllText(filePath, XmlContent);
                MessageBox.Show($"XML content saved to {filePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving XML content: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to find the input token corresponding to the selectedUiButton in JSON
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

        // Method to find the rebind element corresponding to the selectedUiButton in XML
        private XElement FindRebindElement(XElement root, string selectedUiButton)
        {
            // Recursive method to find the rebind element corresponding to the selectedUiButton
            foreach (XElement element in root.Descendants("rebind"))
            {
                XAttribute inputAttribute = element.Attribute("input");
                if (inputAttribute != null && inputAttribute.Value == selectedUiButton)
                {
                    return element;
                }

                XElement childElement = FindRebindElement(element, selectedUiButton);
                if (childElement != null)
                {
                    return childElement;
                }
            }

            return null;
        }

        public void UpdateJsonWithButton(string selectedUiButton, string capturedButton, string viewModelJsonContent)
        {
            throw new NotImplementedException();
        }
    }
}