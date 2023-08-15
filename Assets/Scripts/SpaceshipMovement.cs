using Cinemachine;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceshipMovement : NetworkBehaviour
{
    [Header("Spaceship settings")]

    [SerializeField] private float _yawTorque = 500f;
    [SerializeField] private float _pitchTorque = 1000f;
    [SerializeField] private float _rollTorque = 1000f;
    [SerializeField] private float _thrustForce = 100f;
    [SerializeField] private float _maxAngularVelocity = 1f;

    [Tooltip("Ammount of thrust reduction if the ship is not accelerating")]
    [SerializeField, Range(0.001f, 20f)] private float _thrustReduction = 0.5f;
    [SerializeField] private float _maxThrust = 100f;

    [SerializeField] GameObject _spaceshipGraphics;
    [SerializeField] float _maxShipRollAngle = 45f;

    private Rigidbody _rb;
    private float _spaceshipThrust = 0f;
    private float _currentRollRotation = 0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.maxAngularVelocity = _maxAngularVelocity;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public override void Spawned()
    {
        if (CameraManager.Instance)
        {
            CameraManager.Instance.OnSpaceshipSpawned(this);
        }       
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            HandleMovement(data.thrust, data.roll, data.pitchYaw);
        }
    }

    private void HandleMovement(float thrustValue, float roll, Vector2 pitchYaw)
    {
        pitchYaw.Normalize();
        thrustValue = Mathf.Clamp01(thrustValue);
        roll = Mathf.Clamp(roll, -1f, 1f);

        // Roll torque
        _rb.AddRelativeTorque(Vector3.back * roll * _rollTorque * Runner.DeltaTime);
        // Pitch torque
        _rb.AddRelativeTorque(Vector3.right * -pitchYaw.y * _pitchTorque * Runner.DeltaTime);
        // Yaw torque
        _rb.AddRelativeTorque(Vector3.up * pitchYaw.x * _yawTorque * Runner.DeltaTime);

        // Apply any rotation to spaceship graphics
        // Roll rotation
        _currentRollRotation = Mathf.LerpAngle(_currentRollRotation, _maxShipRollAngle * -pitchYaw.x, 0.1f);
        _spaceshipGraphics.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, _currentRollRotation));

        // Thrust
        if (thrustValue > 0.1f || thrustValue < -0.1f)
        {
            _spaceshipThrust = Mathf.Clamp(_spaceshipThrust + _thrustForce * Runner.DeltaTime, 0f, _maxThrust);
        }
        else
        {
            _spaceshipThrust -= _spaceshipThrust * _thrustReduction * Runner.DeltaTime;
            _spaceshipThrust = Mathf.Clamp(_spaceshipThrust, 0f, _maxThrust);
        }

        // Add thrust velocity to the spaceship
        _rb.velocity += transform.forward * _spaceshipThrust * Runner.DeltaTime;
    }

    #region Public Methods

    public Vector3 GetVelocity()
    {
        return _rb.velocity;
    }

    public float GetThrust()
    {
        return _spaceshipThrust;
    }

    #endregion
}
