# Project Documentation

This document provides an overview of the key components within the FlightStick Calibration application, focusing on its core functionality and the roles of specific files within the project.

## Table of Contents

- [BindButtonsWindow.xaml.cs](#bindbuttonswindowxamlcs)
- [BindButtonsWindowViewModel.cs](#bindbuttonswindowviewmodelcs)
- [JsonFileManager.cs](#jsonfilemanagercs)
- [XmlFileHandler.cs](#xmlfilehandlercs)
- [MainWindow.xaml.cs](#mainwindowxamlcs)
- [JoystickManager.cs](#joystickmanagercs)
- [NuGet Packages](#nugetpackages)
---

## BindButtonsWindow.xaml.cs

### Overview

`BindButtonsWindow.xaml.cs` is the code-behind for the BindButtonsWindow UI, responsible for handling user interactions when configuring button bindings on a flight stick. It initializes the UI components, manages joystick selection, and processes button binding actions.

### Key Methods

- `InitializeComponent()`: Called to initialize the window's components, setting up the UI based on the XAML definitions.

- `FileButton_Click(object sender, RoutedEventArgs e)`: Executed when the user clicks the file button. This method triggers the process of saving JSON content and converting it to XML, followed by a confirmation message to the user.

- `SaveAndConvertToJson()`: Saves the current JSON content to a file and converts it to XML. It also logs the converted XML content for debugging purposes.

- `UpdateJoystickList()`: Updates the list of available joysticks in the UI. It queries connected joysticks and populates a ComboBox with their names, automatically selecting the first joystick if available.

- `GenerateButtonsFromJson()`: Dynamically generates buttons based on the JSON content describing button bindings. This method parses the JSON, creates buttons for each action, and adds them to the UI.

- `GenerateButtonsForInputs(JObject jsonData)`: Helper method for `GenerateButtonsFromJson()`, responsible for creating and configuring buttons based on the parsed JSON data.

- `Button_Click(object sender, RoutedEventArgs e)`: Handles click events for dynamically generated buttons. It prompts the user to press a joystick button to bind the selected action.

- `PromptUserToPressJoyStickButton(string selectedUiButton)`: Prompts the user to press a button on the joystick for binding and updates the XML configuration accordingly.

- `GetButtonCategory(string inputName)`: Determines the category of a button based on its input name, aiding in the organization of buttons within the UI.

- `AddButtonToCategory(Button button, string category)`: Adds a button to a specific category within the UI, creating a new category section if necessary.

- `ComboBoxJoysticks_SelectionChanged(object sender, SelectionChangedEventArgs e)`: Optionally handles selection changes in the joystick ComboBox, enabling further customization or response to user input.

### Additional Considerations

This code-behind manages crucial interactions for the application's button binding functionality. It integrates with the ViewModel to reflect and update application data based on user actions. Developers should ensure synchronization between UI changes and underlying data models, particularly when handling dynamic content like joystick selection and button bindings.


## BindButtonsWindowViewModel.cs

### Overview

The ViewModel for `BindButtonsWindow`, facilitating the separation of view logic and business logic. It exposes data and commands to the UI, handling the application's state and responses to user inputs during button binding.

### Key Properties

- `SelectedFilePath`: Tracks the file path selected by the user. Loading JSON content when this path changes.
- `JsonContent`: Stores the JSON content loaded from the selected file.
- `XmlContent`: Holds XML content, likely related to button binding configurations.
- `LoadedFilePath`: Indicates the file path from which content was successfully loaded.
- `CurrentButtonPressed`: Represents the currently pressed button, used to handle button press events within the UI.
- `XmlFileHandler`: An instance of `XmlFileHandler` used for reading and writing XML files.

### Commands

- `SaveCommand`: An ICommand that triggers saving the current JSON or XML content to a file. It's enabled regardless of specific conditions in this implementation but can be modified to be conditionally enabled.

### Key Methods and Functions

- `InitializeJoystick()`: Initializes the joystick by detecting connected joystick devices.
- `SaveCommandExecute()`: The execution logic for `SaveCommand`, responsible for saving JSON content.
- `SaveCommandCanExecute()`: Determines whether the `SaveCommand` can be executed. Currently, it always returns `true`.
- `UpdateXmlWithButton(string selectedUiButton, string capturedButton)`: Updates XML content with new button configurations.
- `SaveUpdatedXmlContent(string xmlContent)`: Saves the updated XML content to a file.
- `LoadJsonContent()`: Loads JSON content from the file at `SelectedFilePath`.
- `ConvertXmlToJson(string xmlContent)`: Converts XML content to JSON format.
- `ConvertJsonToXml(string jsonContent)`: Converts JSON content back to XML format.
- `SaveJsonContent()`: Saves the JSON content as XML to a file, after conversion and potential sorting.
- `SortXml(XmlNode node)`: Sorts XML nodes alphabetically by name for organized storage or display.
- `HandleButtonPress()`: Handles actions based on the currently pressed button, including joystick detection and button state checking.
- `OnPropertyChanged([CallerMemberName] string propertyName = null)`: Notifies listeners about property value changes, facilitating UI updates in response to data changes.

### Event Handlers

- `PropertyChanged`: Event raised whenever a property value changes, supporting the INotifyPropertyChanged interface to enable automatic UI updates.

This ViewModel is a central component of the application's architecture, bridging the UI and the application's data logic, particularly in managing button bindings and device configurations.


## JsonFileManager.cs

### Overview

Manages the serialization and deserialization of JSON files, providing an interface for reading and writing configuration settings, mappings, or other data structures.

### Key Methods

- `ReadJson<T>(string filePath)`: Generic method for deserializing JSON content from a file into an object of type T. It takes the path of the JSON file as an argument and returns an instance of type T populated with the data from the file.

- `WriteJson<T>(T object, string filePath)`: Serializes an object of type T into a JSON string and writes it to the specified file. This method is essential for saving modifications back to disk, ensuring that configuration changes or data updates are persisted.

- `SaveJsonToFile(string filePath = null)`: Saves the current state of a JSON-serializable object to a file. If `filePath` is provided, it saves to that location; otherwise, it may use a default or previously specified path.

- `SaveXmlToFile(string filePath = null)`: This method appears to be misclassified under `JsonFileManager.cs` and should be reviewed for correct placement or naming, as it suggests XML file handling.

- `FindInputToken(JToken token, string selectedUiButton)`: Searches a JSON structure for a specific token related to UI button configuration, potentially used in scenarios where JSON data models UI elements or configurations.

Please review and adjust the method descriptions and the inclusion of `SaveXmlToFile` as it seems to be out of place. Ensure each method's description accurately reflects its purpose and implementation in your `JsonFileManager.cs`.

## XmlFileHandler.cs

### Overview

Similar to `JsonFileManager.cs`, but focused on XML data handling. This class provides methods for reading and writing XML files, supporting configuration, settings, and data persistence.

### Key Methods

- `GetXmlContent()`: Retrieves the content of an XML file as a string. This method is typically used to load XML data into memory for processing or manipulation.

- `UpdateXmlWithButton(string selectedUiButton, string capturedButton, string xmlContent)`: Updates an XML document with new button bindings based on user selection and input. The `selectedUiButton` parameter identifies the UI button being configured, `capturedButton` represents the physical button on the device, and `xmlContent` contains the current XML configuration to be updated.

- `UpdateXmlAttributeValue(XDocument xmlDoc, string selectedUiButton, string newValue)`: Modifies an attribute value within an XML document. This method is used to change specific configuration settings, where `xmlDoc` is the document to update, `selectedUiButton` identifies the attribute to change, and `newValue` is the new value to be assigned.

- `FindRebindElement(XElement root, string selectedUiButton)`: Searches for and returns an XML element responsible for a particular button rebinding, using the `root` element as the starting point and `selectedUiButton` to identify the specific element. This function is essential for locating configuration settings within an XML structure to read or modify them.


## MainWindow.xaml.cs

### Overview

The code-behind for the application's main window, `MainWindow.xaml.cs` orchestrates the primary user interface, navigation, and interaction logic for the application.

### Key Methods

- `InitializeComponent`: Initializes the components of the main window.
- `SelectFileButton_Click(object sender, RoutedEventArgs e)`: This method is triggered when the user clicks the "Select File" button. It typically opens a file dialog allowing the user to select a file. After a file is selected, the method may handle the file (e.g., opening or loading it into the application).
- `BindButtonsButton_Click(object sender, RoutedEventArgs e)`: Activated when the "Bind Buttons" button is clicked by the user. This function is responsible for opening a new window or dialog where the user can configure and bind actions to buttons on a flight stick. It's a key part of the application's functionality, enabling customization of device controls.

---

## JoystickManager

### Overview

`JoystickManager` is a static class responsible for managing joystick devices. It initializes joystick detection, retrieves connected joysticks, and allows querying joysticks by name.

### Key Methods

- `Initialize()`: Initializes the `DirectInput` object and populates a list of connected joystick devices. It should be called at the application startup to prepare the joystick management infrastructure.

- `GetConnectedJoysticks(DirectInput directInput)`: Returns a list of `JoystickDevice` instances representing all joysticks currently connected to the system. It combines gamepad and joystick devices under a unified interface.

- `GetJoystickNames()`: Returns a list of names of all connected joysticks. This method is useful for displaying joystick options in the UI.

- `GetJoystickByName(string joystickName)`: Retrieves a `JoystickDevice` instance by its name. This allows for easy selection and interaction with specific joysticks based on user input or saved configurations.

## JoystickDevice

### Overview

`JoystickDevice` represents an individual joystick. It encapsulates the functionality for acquiring joystick input, checking button states, and capturing button presses.

### Key Properties

- `Name`: A read-only property that returns the name of the joystick. Useful for identifying and displaying the joystick in the UI.

### Key Methods

- `IsButtonDown(int button)`: Checks if a specific button on the joystick is currently pressed. This method is crucial for real-time input handling in interactive applications.

- `CapturePressedButton()`: Analyzes the current state of the joystick and returns a string representation of the first pressed button it finds, excluding certain mode buttons. This method is particularly useful for binding UI actions to joystick buttons.

### Usage Scenario

The `JoystickManager` and `JoystickDevice` classes work together to simplify joystick handling in WPF applications. For example, upon application startup, `JoystickManager.Initialize()` is called to detect connected joysticks. The UI can then use `GetJoystickNames()` to populate a selection control, allowing the user to choose a joystick. Once a joystick is selected, `GetJoystickByName()` can retrieve the corresponding `JoystickDevice`, which then can be used to check button states or capture button presses for action bindings or game controls.

### Additional Considerations

- Ensure `JoystickManager.Initialize()` is called early in the application lifecycle to accurately detect and manage connected joysticks.
- When using `CapturePressedButton()`, consider the application context to appropriately handle or ignore the return value, especially when no button press is detected or certain buttons are intentionally ignored.
- Regular polling with `IsButtonDown()` may be necessary for real-time applications, such as games or interactive simulations, to continuously monitor joystick input.
---


## External Packages Overview

The application utilizes several external NuGet packages to enhance its functionality, particularly in handling JSON data and joystick input. Below is an overview of these packages:

### Newtonsoft.Json

- **Description**: Newtonsoft.Json, also known as Json.NET, is a popular high-performance JSON framework for .NET. It provides functionality for converting between .NET objects and JSON without needing to write custom serialization code.
- **Usage in Project**: This package is used for serializing and deserializing JSON data, such as configurations and mappings for joystick buttons. It enables easy storage and retrieval of application settings in JSON format.

### SharpDX

- **Description**: SharpDX is a managed wrapper around the DirectX API, offering access to graphics, audio, and input capabilities. It provides a .NET-friendly way to use DirectX's extensive features.
- **Usage in Project**: Serves as the foundation for other SharpDX packages in the project, enabling direct access to lower-level DirectX functions from managed code.

### SharpDX.DirectInput

- **Description**: A part of the SharpDX suite, SharpDX.DirectInput is specifically focused on handling input from various devices, including joysticks and gamepads. It wraps DirectX DirectInput interfaces to be used in .NET applications.
- **Usage in Project**: Utilized for detecting connected joysticks, reading their input states, and processing button presses. It's essential for implementing the joystick configuration and interaction logic within the application.

### SharpDX.XInput

- **Description**: SharpDX.XInput is another subset of the SharpDX project, targeting the XInput API, which is designed for Xbox 360 controller input but also works with other compatible controllers.
- **Usage in Project**: While not directly mentioned in the provided code, this package could be used alongside SharpDX.DirectInput for handling input from XInput-compatible devices, offering an alternative or supplementary input handling method, especially for applications requiring support for a wide range of controller types.

### Implementation Notes

- To integrate these packages into your project, ensure they are installed via NuGet Package Manager in Visual Studio. This ensures the project has access to the latest versions and functionalities offered by each package.
- The choice between SharpDX.DirectInput and SharpDX.XInput can depend on the specific requirements of your application and the types of devices you intend to support. DirectInput is more general and supports a wider range of input devices, while XInput is optimized for Xbox controllers and provides a simpler API for common game controller features.


