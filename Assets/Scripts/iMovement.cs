using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    //Get Current Speed
    float GetCurrentSpeed();

    //Set up movement
    void SetupMovement();

    //Update Movement with direction and jump
    void UpdateMovement(Vector3 moveDirection, bool isJumping);
    //Manage Death behavior

    void SetActive(bool value);
   //Determine if the character is grounded
    bool GetGrounded();
    //determine if the player is trying to jump
     bool GetJumpState();
     void OnDeath();


}
