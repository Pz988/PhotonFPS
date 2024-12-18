using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharacterMovementHandler : NetworkBehaviour
{
    bool isRespawnRequested = false;

    // other components
    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;
    Camera localCamera;
    HPHandler hpHandler;



    private void Awake()
    {
        networkCharacterControllerPrototypeCustom = GetComponent<NetworkCharacterControllerPrototypeCustom>();
        localCamera = GetComponentInChildren<Camera>();
        hpHandler = GetComponent<HPHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority)
        {
            if (isRespawnRequested) 
            {
                Respawn();
                return;
            }

            //dont update player pos when dead
            if (hpHandler.isDead)
                return;
        }
        // get the input from the network
        if(GetInput(out NetworkInputData networkInputData))
        {
            //rotate the transform according to the client aim vector
            transform.forward = networkInputData.aimForwardVector;

            //cancel out rotation on x axis character will tilt otherwise
            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z);
            transform.rotation = rotation;


            // move
            Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            moveDirection.Normalize();

            networkCharacterControllerPrototypeCustom.Move(moveDirection);

            //jump 
            if (networkInputData.isJumpPressed)
                networkCharacterControllerPrototypeCustom.Jump();

            //check if we fallen off world
            CheckFallRespawn();

        }
    }

    void CheckFallRespawn()
    {
        if (transform.position.y < -12)
        {
            if (Object.HasInputAuthority)
            {
                Debug.Log($"{Time.time} Respawn due to fall outside of map bounds at {transform.position}");
                Respawn();
            }
            
             
        }

         
    }

    public void RequestRespawn()
    {
        isRespawnRequested = true;
    }

    void Respawn()
    {
        networkCharacterControllerPrototypeCustom.TeleportToPosition(Utils.GetRandomSpawnPoint());

        hpHandler.OnRespawned(); //this can be done better with events or other case in future, im just lazy, will make better in future when framework updated.

        isRespawnRequested = false;


    }

    public void SetCharacterControllerEnabled(bool isEnabled)
    {
        networkCharacterControllerPrototypeCustom.Controller.enabled = isEnabled;
    }
  

}
