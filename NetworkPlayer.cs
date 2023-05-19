using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer Local { get; private set; }
    private GameObject _camObj;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;

            var camRotZ = transform.rotation.eulerAngles.z;

            _camObj = GameObject.FindWithTag($"MainCamera");
            _camObj.transform.rotation = Quaternion.Euler(0, 0, camRotZ);
        }
        else
        {
            gameObject.transform.TryGetComponent(
                out PlayerInputHandler playerInputHandler);
            playerInputHandler.enabled = false;
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player.IsValid)
        {
            Runner.Despawn(Object);
        }
    }
}
