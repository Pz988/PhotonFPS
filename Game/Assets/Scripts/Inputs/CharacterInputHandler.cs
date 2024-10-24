using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;
    bool isFireButtonPressed = false;

    //other components
    LocalCameraHandler localCameraHandler;
    CharacterMovementHandler characterMovementHandler;
    

    private void Awake()
    {
       
        localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
        characterMovementHandler = GetComponent<CharacterMovementHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // can be optimized in future 

        if (!characterMovementHandler.Object.HasInputAuthority)
           return;

        //view input 
        viewInputVector.x = Input.GetAxis("Mouse X");
        viewInputVector.y = Input.GetAxis("Mouse Y") * -1; // invert mouse look

        // move input 
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");
       
        //jump 
        if (Input.GetButtonDown("Jump"))
            isJumpButtonPressed = true;

        //fire
        if(Input.GetButtonDown("Fire1"))
            isFireButtonPressed = true;
        
        //set view
        localCameraHandler.SetViewInputVector(viewInputVector);
        
        
    }

    public NetworkInputData GetNetworkInputData()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        //aim data
        networkInputData.aimForwardVector = localCameraHandler.transform.forward;



        //move data
        networkInputData.movementInput = moveInputVector;

        //jump data
        networkInputData.isJumpPressed = isJumpButtonPressed;

        //fire data
        networkInputData.isFireButtonPressed = isFireButtonPressed;

        //reset variables after read state
        isJumpButtonPressed = false;
        isFireButtonPressed = false;

        return networkInputData;
    }
}
