// JoystickManager.cs
using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX.DirectInput;

public static class JoystickManager
{
    private static DirectInput _directInput;
    private static List<JoystickDevice> _joysticks;

    public static void Initialize()
    {
        _directInput = new DirectInput();
        _joysticks = GetConnectedJoysticks();
    }

    private static List<JoystickDevice> GetConnectedJoysticks()
    {
        var joystickGuids = _directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices)
            .Concat(_directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
            .Select(deviceInstance => deviceInstance.InstanceGuid)
            .ToList();

        // Check if there are no connected joysticks
        if (joystickGuids.Count == 0)
        {
            return new List<JoystickDevice>();
        }

        return joystickGuids.Select(guid => new JoystickDevice(_directInput, guid)).ToList();
    }

    public static List<string> GetJoystickNames()
    {
        return _joysticks.Select(joystick => joystick.Name).ToList();
    }
}

public class JoystickDevice
{
    private readonly Joystick _joystick;

    public JoystickDevice(DirectInput directInput, Guid joystickGuid)
    {
        _joystick = new Joystick(directInput, joystickGuid);
        _joystick.Acquire();
    }

    public string Name => _joystick.Information.InstanceName;

    public bool IsButtonDown(int button)
    {
        _joystick.Poll();
        var state = _joystick.GetCurrentState();
        return (state.Buttons.Length > button) && state.Buttons[button];
    }
}