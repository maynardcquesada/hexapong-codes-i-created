using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class NetworkSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private GameObject _playerPrefab;
    private byte _playerCount;

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer) return;
        
        print($"Player {player.PlayerId} Joined");
        _playerCount++;
        var playerTransform = _playerPrefab.transform;
        if (_playerCount > 1)
        {
            runner.Spawn(_playerPrefab, new Vector3(-4.2f, 0, 0),
                Quaternion.Euler(0, 0, 90), player.PlayerId);
        }
        else
        {
            runner.Spawn(_playerPrefab, playerTransform.position,
                playerTransform.rotation, player.PlayerId);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (NetworkPlayer.Local == null) return;

        NetworkPlayer.Local.TryGetComponent(
            out PlayerInputHandler localPlayerInputHandler);

        if (localPlayerInputHandler != null)
        {
            input.Set(localPlayerInputHandler.GetNetworkInput());
        }
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player,
        NetworkInput input){ }

    public void OnShutdown(NetworkRunner runner, ShutdownReason 
    shutdownReason){ }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        print("Connected");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner){ }

    public void OnConnectRequest(NetworkRunner runner, 
        NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){ }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress,
        NetConnectFailedReason reason)
    {
        print("Connection Failure");
    }

    public void OnUserSimulationMessage(NetworkRunner runner,
        SimulationMessagePtr message){ }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo>
     sessionList){ }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, 
    Dictionary<string, object> data){ }

    public void OnHostMigration(NetworkRunner runner,
        HostMigrationToken hostMigrationToken){ }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player,
        ArraySegment<byte> data){ }

    public void OnSceneLoadDone(NetworkRunner runner){ }

    public void OnSceneLoadStart(NetworkRunner runner){ }
}
