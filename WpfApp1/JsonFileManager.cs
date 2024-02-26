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
                    string xmlContentFromWindow = File.ReadAllText(CopiedFilePath);
                    return xmlContentFromWindow;
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

                    // Locate the specific rebind element to update
                    var rebindElementToUpdate = xmlDoc.Descendants("rebind")
                        .FirstOrDefault(el => (string)el.Attribute("input") == selectedUiButton);

                    if (rebindElementToUpdate != null)
                    {
                        // Update the 'input' attribute with the captured button
                        rebindElementToUpdate.SetAttributeValue("input", $"js1_{capturedButton}");

                        // Debug print or log
                        Debug.WriteLine($"Updated XML with new button: {capturedButton} for selected UI button: {selectedUiButton}");

                        // Convert updated XML back to string and possibly save or use as needed
                        xmlContentFromWindow = xmlDoc.ToString();
                        
                    }
                    else
                    {
                        MessageBox.Show($"Rebind element for '{selectedUiButton}' not found in XML data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating XML content: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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


    }
}