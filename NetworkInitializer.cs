using System;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;

public class NetworkInitializer : INetworkInitializer
{
    public async Task Initialize(NetworkRunner runner, GameMode gameMode,
        NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {
        runner.ProvideInput = true;
        await runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = "SessionName",
            Initialized = initialized
        });
    }
}
