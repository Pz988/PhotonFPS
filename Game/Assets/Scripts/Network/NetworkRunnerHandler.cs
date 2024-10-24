using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Linq;


public class NetworkRunnerHandler : MonoBehaviour
{
    public NetworkRunner networkRunnerPrefab;
    NetworkRunner networkRunner;

    // Start is called before the first frame update
    void Start()
    {
        networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.name = "Network runner";

        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.AutoHostOrClient, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
        Debug.Log("Server NetworkRunner Started");


    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gamemode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {
        var SceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (SceneManager == null) 
        { 
            //Handle network objects that already exist in scene
            SceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();

        }

        runner.ProvideInput = true;
        return runner.StartGame(new StartGameArgs // game/lobby/session/match settings/configuration 
        {
            GameMode = gamemode,
            Address = address,
            Scene = scene,
            SessionName = "Test Room",
            Initialized = initialized,
            SceneManager = SceneManager,

        });

    }

}
