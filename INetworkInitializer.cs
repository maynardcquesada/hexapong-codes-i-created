using System;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;

public interface INetworkInitializer
{
    Task Initialize(NetworkRunner runner, GameMode gameMode,
        NetAddress address, SceneRef scene, Action<NetworkRunner> initialized);
}
