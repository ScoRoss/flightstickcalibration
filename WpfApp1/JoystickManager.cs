using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public class JoystickManager
    {
        private static DirectInput _directInput;
        private static List<JoystickDevice> _joysticks;

        // Initializes the DirectInput object
        public static void Initialize()
        {
            _directInput = new DirectInput();
            _joysticks = GetConnectedJoysticks(_directInput);
            Console.WriteLine("JoystickManager initialized.");
        }

        // Returns a list of connected joysticks
        public static List<JoystickDevice> GetConnectedJoysticks(DirectInput directInput)
        {
            var joystickGuids = _directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices)
                .Concat(_directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                .Where(deviceInstance =>
                {
                    // Check if the device subtype indicates a Joystick
                    return deviceInstance.Type == SharpDX.DirectInput.DeviceType.Joystick;
                })
                .Select(deviceInstance => deviceInstance.InstanceGuid)
                .ToList();

            // Check if there are no connected joysticks
            if (joystickGuids.Count == 0)
            {
                Console.WriteLine("No connected joysticks.");
                return new List<JoystickDevice>();
            }

            var joysticks = joystickGuids.Select(guid => new JoystickDevice(_directInput, guid)).ToList();
            Console.WriteLine($"Connected Joysticks: {string.Join(", ", joysticks.Select(j => j.Name))}");
            return joysticks;
        }

        // Returns a list of joystick names
        public static List<string> GetJoystickNames()
        {
            var joystickNames = _joysticks.Select(joystick => joystick.Name).ToList();
            Console.WriteLine($"Joystick names: {string.Join(", ", joystickNames)}");
            return joystickNames;
        }

        // Returns a joystick by its name
        public static JoystickDevice GetJoystickByName(string joystickName)
        {
            var joystick = _joysticks.FirstOrDefault(j => j.Name == joystickName);
            if (joystick != null)
            {
                Console.WriteLine($"Joystick found by name: {joystickName}");
            }
            else
            {
                Console.WriteLine($"Joystick not found by name: {joystickName}");
            }
            return joystick;
        }



    }

    // Represents a joystick
    public class JoystickDevice
{
    private readonly Joystick _joystick;

    // Initializes the joystick
    public JoystickDevice(DirectInput directInput, Guid joystickGuid)
    {
        _joystick = new Joystick(directInput, joystickGuid);
        _joystick.Acquire();
    }


    public string Name => _joystick.Information.InstanceName;

    // Checks if a button is pressed
    public bool IsButtonDown(int button)
    {
        _joystick.Poll();
        var state = _joystick.GetCurrentState();
        var isPressed = (state.Buttons.Length > button) && state.Buttons[button];
        Console.WriteLine($"Button {button} is{(isPressed ? " " : " not ")}pressed");
        return isPressed;
    }

    // Captures the button that was pressed

    // Debug option to register button presses continuously
    public void DebugMonitorButtonPresses()
    {
        Console.WriteLine($"Monitoring button presses for joystick: {Name}");

        // Create a background thread to continuously monitor button presses
        Task.Run(() =>
        {
            while (true)
            {
                // Poll the joystick for the current state
                _joystick.Poll();
                var state = _joystick.GetCurrentState();

                // Check the state for button presses
                var buttons = state.Buttons;

                // Print the entire array of button states
                Console.WriteLine("Button States: " + string.Join(", ", buttons.Select((value, index) => $"Button {index}: {value}")));

                // Introduce a delay to avoid high CPU usage
                Thread.Sleep(100); // Adjust the delay as needed
            }
        });
    }

    public string CapturePressedButton()
    {
        // Poll the joystick for the current state
        _joystick.Poll();
        var state = _joystick.GetCurrentState();

        // Print the entire state information
        Console.WriteLine($"Joystick State: {state}");

        // Check the state for button presses
        var buttons = state.Buttons;

        // Check if buttons 14, 15, and 16 are pressed and ignore them
        if (buttons.Length >= 16 && (buttons[13] || buttons[14] || buttons[15]))
        {
            Console.WriteLine("Ignored mode buttons (14, 15, 16) are pressed");
            return string.Empty; // Ignore the buttons
        }

        // Process other buttons normally
        for (int i = 0; i < buttons.Length; i++)
        {
            // Ignore buttons beyond index 13
            if (i >= 13)
            {
                continue;
            }

            // If the current button is pressed
            if (buttons[i])
            {
                // Format the button input directly
                string buttonInput = $"js1_button{i + 1}";

                // Add debug statement to check the pressed button along with its index
                Console.WriteLine($"Button {i} (Index: {i + 1}) is pressed");
                return buttonInput;
            }
        }

        // No valid button was pressed
        Console.WriteLine("No valid button was pressed");
        return string.Empty;
    }




    }
}
