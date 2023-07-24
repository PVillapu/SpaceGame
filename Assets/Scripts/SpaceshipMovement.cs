using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class SpaceshipMovement : MonoBehaviour
{
    [Header("Spaceship settings")]
    [SerializeField]
    private float _yawTorque = 500f;
    [SerializeField]
    private float _pitchTorque = 1000f;
    [SerializeField]
    private float _rollTorque = 1000f;
    [SerializeField]
    private float _thrust = 100f;
    [SerializeField]
    private float _upthrust = 50f;
    [SerializeField]
    private float _strafeThrust = 50f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float _thrustGlideReduction = 0.999f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float _upDownGlideReduction = 0.111f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float _leftRigtGlideReduction = 0.111f;

    private Rigidbody _rb;
    private float _glide = 0f;
    private float _verticalGlide = 0f;
    private float _horizontalGlide = 0f;

    // Input values
    private float _thrust1D;
    private float _upDown1D;
    private float _strafe1D;
    private float _roll1D;
    private Vector2 _pitchYaw;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
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
        _rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(-_pitchYaw.y, -1f, 1f) * _pitchTorque * Time.fixedDeltaTime);
        // Yaw torque
        _rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(_pitchYaw.x, -1f, 1f) * _yawTorque * Time.fixedDeltaTime);

        // Thrust
        if(_thrust1D > 0.1f || _thrust1D < -0.1f)
        {
            _rb.AddRelativeForce(Vector3.forward * _thrust1D * _thrust * Time.fixedDeltaTime);
            _glide = _thrust;
        }
        else
        {
            _rb.AddRelativeForce(Vector3.forward * _glide * Time.fixedDeltaTime);
            _glide *= _thrustGlideReduction;
        }

        // Up/down 
        if (_upDown1D > 0.1f || _upDown1D < -0.1f)
        {
            float verticalGlide = _upDown1D * _upthrust;
            _rb.AddRelativeForce(Vector3.up * verticalGlide * Time.fixedDeltaTime);
            _verticalGlide = verticalGlide;
        }
        else
        {
            _rb.AddRelativeForce(Vector3.up * _verticalGlide * Time.fixedDeltaTime);
            _verticalGlide *= _upDownGlideReduction;
        }

        // Strafing
        if (_strafe1D > 0.1f || _strafe1D < -0.1f)
        {
            float horizontalGlide = _strafe1D * _strafeThrust;
            _rb.AddRelativeForce(Vector3.right * horizontalGlide * Time.fixedDeltaTime);
            _horizontalGlide = horizontalGlide;
        }
        else
        {
            _rb.AddRelativeForce(Vector3.right * _horizontalGlide * Time.fixedDeltaTime);
            _horizontalGlide *= _leftRigtGlideReduction;
        }
    }

    #region Input Methods

    public void OnThrust(InputAction.CallbackContext context)
    {
        _thrust1D = context.ReadValue<float>();
    }

    public void OnStrafe(InputAction.CallbackContext context)
    {
        _strafe1D = context.ReadValue<float>();
    }

    public void OnUpDown(InputAction.CallbackContext context)
    {
        _upDown1D = context.ReadValue<float>();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        _roll1D = context.ReadValue<float>();
    }

    public void OnPitchYaw(InputAction.CallbackContext context)
    {
        _pitchYaw = context.ReadValue<Vector2>();
    }

    #endregion
}
