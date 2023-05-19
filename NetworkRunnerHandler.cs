using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NetworkRunner))]
public class NetworkRunnerHandler : MonoBehaviour
{
    [SerializeField] private NetworkRunner _networkRunner;
    private INetworkInitializer _networkInitializer;

    private void Start()
    {
        _networkInitializer = new NetworkInitializer();
        TryGetComponent(out _networkRunner);
        _networkInitializer.Initialize(_networkRunner,
            GameMode.AutoHostOrClient, NetAddress.Any(),
            SceneManager.GetActiveScene().buildIndex, null);
    }
}
