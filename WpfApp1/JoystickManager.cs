using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX.DirectInput;

public static class JoystickManager
{
    private static DirectInput _directInput;
    private static List<JoystickDevice> _joysticks;
    // Initializes the DirectInput object
    public static void Initialize()
    {
        _directInput = new DirectInput();
        _joysticks = GetConnectedJoysticks();
    }
    // Returns a list of connected joysticks
    private static List<JoystickDevice> GetConnectedJoysticks()
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
            return new List<JoystickDevice>();
        }

        return joystickGuids.Select(guid => new JoystickDevice(_directInput, guid)).ToList();
    }
    // Returns a list of joystick names
    public static List<string> GetJoystickNames()
    {
        return _joysticks.Select(joystick => joystick.Name).ToList();
    }
    // Returns a joystick by its name
    public static JoystickDevice GetJoystickByName(string joystickName)
    {
        return _joysticks.FirstOrDefault(joystick => joystick.Name == joystickName);
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
        return (state.Buttons.Length > button) && state.Buttons[button];
    }
    // Captures the button that was pressed
    public string CapturePressedButton()
    {
        // Poll the joystick for the current state
        _joystick.Poll();
        var state = _joystick.GetCurrentState();

        // Check the state for button presses
        var buttons = state.Buttons;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i])
            {
                // Button i is pressed
                // You can return the button name or index here
                return $"Button {i}";
            }
        }

        // No button was pressed
        return string.Empty;
    }
}
