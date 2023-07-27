using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class SpaceshipMovement : MonoBehaviour
{
    [Header("Spaceship settings")]

    [SerializeField] private float _yawTorque = 500f;
    [SerializeField] private float _pitchTorque = 1000f;
    [SerializeField] private float _rollTorque = 1000f;
    [SerializeField] private float _thrustForce = 100f;
    [SerializeField] private float _maxAngularVelocity = 1f;

    [Tooltip("Ammount of thrust reduction if the ship is not accelerating")]
    [SerializeField, Range(0.001f, 0.999f)] private float _thrustReduction = 0.5f;
    [SerializeField] private float _maxThrust = 100f;

    [SerializeField] GameObject _spaceshipGraphics;
    [SerializeField] float _maxShipRollAngle = 45f;

    private Rigidbody _rb;
    private float _spaceshipThrust = 0f;
    private float _currentRollRotation = 0f;

    // Input values
    private float _thrust1D;
    private float _roll1D;
    private Vector2 _pitchYaw;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.maxAngularVelocity = _maxAngularVelocity;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Roll torque
        _rb.AddRelativeTorque(Vector3.back * _roll1D * _rollTorque * Time.fixedDeltaTime);
        // Pitch torque
        _rb.AddRelativeTorque(Vector3.right * -_pitchYaw.y * _pitchTorque * Time.fixedDeltaTime);
        // Yaw torque
        _rb.AddRelativeTorque(Vector3.up * _pitchYaw.x * _yawTorque * Time.fixedDeltaTime);

        // Apply any rotation to spaceship graphics
        // Roll rotation
        _currentRollRotation = Mathf.LerpAngle(_currentRollRotation, _maxShipRollAngle * -_pitchYaw.x, 0.1f);
        _spaceshipGraphics.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, _currentRollRotation));

        // Thrust
        if (_thrust1D > 0.1f || _thrust1D < -0.1f)
        {
            _spaceshipThrust = Mathf.Clamp(_spaceshipThrust + _thrustForce * Time.fixedDeltaTime, 0f, _maxThrust);
        }
        else
        {
            _spaceshipThrust -= _spaceshipThrust * _thrustReduction * Time.fixedDeltaTime;
            _spaceshipThrust = Mathf.Clamp(_spaceshipThrust, 0f, _maxThrust);
        }

        // Add thrust velocity to the spaceship
        _rb.velocity += transform.forward * _spaceshipThrust * Time.fixedDeltaTime;
    }

    #region Public Methods

    public Vector2 GetDirectionDelta()
    {
        return _pitchYaw;
    }

    public Vector3 GetVelocity()
    {
        return _rb.velocity;
    }

    public float GetThrust()
    {
        return _spaceshipThrust;
    }

    #endregion

    #region Input Methods

    public void OnThrust(InputAction.CallbackContext context)
    {
        _thrust1D = context.ReadValue<float>();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        _roll1D = context.ReadValue<float>();
    }

    public void OnPitchYaw(InputAction.CallbackContext context)
    {
        _pitchYaw = context.ReadValue<Vector2>();
        _pitchYaw.x = Mathf.Clamp(_pitchYaw.x, -1f, 1f);
        _pitchYaw.y = Mathf.Clamp(_pitchYaw.y, -1f, 1f);
    }

    #endregion
}
