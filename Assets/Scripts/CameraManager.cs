using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;   // Singleton

    [SerializeField] CinemachineBrain _cinemachineBrain;
    [SerializeField] CinemachineVirtualCamera _mainCamera;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }

    public void OnSpaceshipSpawned(SpaceshipMovement spaceship)
    {
        if (spaceship.Object.HasInputAuthority)
        {
            _mainCamera.Follow = spaceship.transform;
            _mainCamera.LookAt = spaceship.transform;
        }
    }
}
