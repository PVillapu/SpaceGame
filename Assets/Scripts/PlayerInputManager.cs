using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;

    [SerializeField] private InputActionAsset _inputActions;

    private InputAction _thrustButton;
    private InputAction _pitchYaw;
    private InputAction _roll;

    private readonly string THRUST_BUTTON = "Thrust";
    private readonly string PITCH_YAW = "PitchYaw";
    private readonly string ROLL = "Roll";

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        _thrustButton = FindInputAction(THRUST_BUTTON);
        _pitchYaw = FindInputAction(PITCH_YAW);
        _roll = FindInputAction(ROLL);

        _inputActions.Enable();
    }

    private InputAction FindInputAction(string name)
    {
        InputAction inputAction = _inputActions.FindAction(name);
       
        if(inputAction == null)
        {
            Debug.LogError("No action called " + name + " was found!");
        }

        return inputAction;
    }

    public float GetThrustValue()
    {
        return _thrustButton.ReadValue<float>();
    }

    public float GetRollValue()
    {
        return _roll.ReadValue<float>();
    }

    public Vector2 GetPitchYawValue()
    {
        return _pitchYaw.ReadValue<Vector2>();
    }
}
