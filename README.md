# Project Documentation

This document provides an overview of the key components within the FlightStick Calibration application, focusing on its core functionality and the roles of specific files within the project.

## Table of Contents

- [BindButtonsWindow.xaml.cs](#bindbuttonswindowxamlcs)
- [BindButtonsWindowViewModel.cs](#bindbuttonswindowviewmodelcs)
- [JsonFileManager.cs](#jsonfilemanagercs)
- [XmlFileHandler.cs](#xmlfilehandlercs)
- [MainWindow.xaml.cs](#mainwindowxamlcs)

---

## BindButtonsWindow.xaml.cs

### Overview

`BindButtonsWindow.xaml.cs` is the code-behind for the BindButtonsWindow UI, responsible for handling user interactions when configuring button bindings on a flight stick.

### Key Methods

- `InitializeComponent`: Initializes the window components.
- `OnBindButtonClick`: Handles the logic when the "Bind" button is clicked.
- `FillInAdditionalMethodsHere`: Other methods specific to this file.

---

## BindButtonsWindowViewModel.cs

### Overview

The ViewModel for `BindButtonsWindow`, facilitating the separation of view logic and business logic. It exposes data and commands to the UI, handling the application's state and responses to user inputs during button binding.

### Key Properties and Commands

- `Commands`: List any ICommand properties that bind user actions from the UI to ViewModel methods.
- `Properties`: Describe any properties that reflect the state of the UI or data being manipulated.
- `FillInAdditionalPropertiesAndCommandsHere`: Other properties and commands specific to this ViewModel.

---

## JsonFileManager.cs

### Overview

Manages the serialization and deserialization of JSON files, providing an interface for reading and writing configuration settings, mappings, or other data structures.

### Key Methods

- `ReadJson<T>`: Generic method for deserializing JSON content from a file into an object of type T.
- `WriteJson<T>`: Serializes an object of type T into a JSON string and writes it to a specified file.
- `FillInAdditionalMethodsHere`: Additional methods for handling JSON data.

---

## XmlFileHandler.cs

### Overview

Similar to `JsonFileManager.cs`, but focused on XML data handling. This class provides methods for reading and writing XML files, supporting configuration, settings, and data persistence.

### Key Methods

- `ReadXml<T>`: Deserializes XML content from a file into an object of type T.
- `WriteXml<T>`: Serializes an object into XML format and writes it to a file.
- `FillInAdditionalMethodsHere`: Additional XML data handling methods.

---

## MainWindow.xaml.cs

### Overview

The code-behind for the application's main window, `MainWindow.xaml.cs` orchestrates the primary user interface, navigation, and interaction logic for the application.

### Key Methods

- `InitializeComponent`: Initializes the components of the main window.
- `FillInEventHandlersAndLogic`: Describe any key event handlers, navigation logic, or other methods that facilitate the main application flow.

---

