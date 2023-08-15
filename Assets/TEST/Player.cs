using Fusion;

public class Player : NetworkBehaviour
{
    private NetworkCharacterControllerPrototype _cc;

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterControllerPrototype>();
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority)
            return;

        if (GetInput(out NetworkInputData data))
        {
            data.pitchYaw.Normalize();
            _cc.Move(5 * data.pitchYaw * Runner.DeltaTime);
        }
    }
}
