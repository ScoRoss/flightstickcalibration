using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Xml.Linq;

namespace WpfApp1
{
    public class XmlFileHandler
    {
        public event EventHandler<string> ErrorOccurred;
        private const string XmlFilter = "XML Files|*.xml|All Files|*.*";
        public readonly string _copiedFilePath;
        private string _xmlContent;

        public string XmlContent
        {
            get { return _xmlContent; }
            set { _xmlContent = value; }
        }

        public XmlFileHandler(string copiedFilePath, string xmlContent = null)
        {
            _copiedFilePath = copiedFilePath;
            XmlContent = xmlContent;
        }

        public string GetXmlContent()
        {
            try
            {
                if (!string.IsNullOrEmpty(_copiedFilePath))
                {
                    XmlContent = File.ReadAllText(_copiedFilePath);
                    return XmlContent;
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

            return string.Empty;
        }

        public void UpdateXmlWithButton(string selectedUiButton, string capturedButton, string xmlContent)
        {
            if (!string.IsNullOrEmpty(XmlContent))
            {
                try
                {
                    XDocument xmlDoc = XDocument.Parse(XmlContent);

                    if (xmlDoc != null)
                    {
                        Debug.WriteLine($"Original XML Content: {xmlDoc}");

                        XElement rebindElement = FindRebindElement(xmlDoc.Root, selectedUiButton);

                        if (rebindElement != null)
                        {
                            Debug.WriteLine($"Updating XML for '{selectedUiButton}' - Changing button to '{capturedButton}'");

                            XmlContent = UpdateXmlAttributeValue(xmlDoc, selectedUiButton, $"js1_{capturedButton}");

                            Debug.WriteLine($"Modified XML Content: {XmlContent}");
                        }
                        else
                        {
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
                var rebindElement = xmlDoc.Root
                    .Elements("actionmap")
                    .Elements("action")
                    .Elements("rebind")
                    .Where(rebind => (string)rebind.Attribute("input") == selectedUiButton)
                    .FirstOrDefault();

                if (rebindElement != null)
                {
                    Debug.WriteLine($"Updating XML for '{selectedUiButton}' - Changing button to '{newValue}'");

                    rebindElement.SetAttributeValue("input", newValue);

                    return xmlDoc.ToString();
                }
                else
                {
                    MessageBox.Show($"Rebind element for '{selectedUiButton}' not found in XML data.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating XML content: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return xmlDoc.ToString();
        }

        private XElement FindRebindElement(XElement root, string selectedUiButton)
        {
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
