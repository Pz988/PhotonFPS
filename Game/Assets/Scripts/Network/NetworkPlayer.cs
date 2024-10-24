using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer local {  get; set; }

    public Transform playerModel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority) // if player object is the player itself / user system  
        {
            local = this;

            //Sets render layer for the local player model
            Utils.SetRenderLayerInChildren(playerModel, LayerMask.NameToLayer("LocalPlayerModel"));

            //disable main camera when using local
            Camera.main.gameObject.SetActive(false);

            Debug.Log("Local Player Spawned");

        }
        else
        {   //disable camera if not local player, unity will switch to latest camera added elsewise.
            Camera localCamera = GetComponentInChildren<Camera>();
            localCamera.enabled = false;

            //disable remote audio listener so unity stops yelling at me. only 1 audio listner allowed
            AudioListener audioListener = GetComponentInChildren<AudioListener>();
            audioListener.enabled = false;


        }    Debug.Log("Remote Player Spawned"); // non local / not the user system 

        //give player prefabs id when spawned to tell apart.
        transform.name = $"P_{Object.Id}";  
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority) // if the player is the local user
        {
            Runner.Despawn(Object); // delete/despawn the player on session/match/game quit
        }
    }

}
