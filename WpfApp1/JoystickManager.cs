using SharpDX.DirectInput;

namespace WpfApp1;

public static class JoystickManager
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
                // Add debug statement to check the pressed button
                Console.WriteLine($"Button {i} is pressed");

                // You can return the button name or index here
                return $"Button {i}";
            }
        }

        // No button was pressed
        Console.WriteLine("No button was pressed");
        return string.Empty;
    }
}