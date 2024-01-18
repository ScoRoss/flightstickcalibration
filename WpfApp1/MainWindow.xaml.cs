using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Do something with the selected file path, for example:
                string selectedFilePath = openFileDialog.FileName;
                MessageBox.Show($"Selected File: {selectedFilePath}");
            }
        }

        private void BindButtonsButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the BindButtonsWindow and show it
            BindButtonsWindow bindButtonsWindow = new BindButtonsWindow();
            bindButtonsWindow.Show();
        }


    }
}