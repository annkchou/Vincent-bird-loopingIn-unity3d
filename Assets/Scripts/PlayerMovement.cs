using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour, IMovement
{
    //Move Scalars
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float jumpCooldown = 1.5f;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private float gravity = 5.0f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    //Move Booleans
    public bool isGrounded { get; private set; }
    public bool canDoubleJump = true;
    public bool isJumping { get; private set; }
    private bool isActive;
    //Components
    private Rigidbody moveRb;
    //Move Vectors
    private Vector3 moveDirection;
    //Move Quaternions
    private Quaternion lookRotation;
    //Move Floats
    private float jumpCooldownTimer;
    
    //External References
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        //Set Gravity
        SetGravity();
        //Set the main camera
        mainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        //check if player is grounded
        CheckIfGrounded();
    }
    //fixed update is called at a fixed rate
    void FixedUpdate()
    {
        //if (!isActive) return;
        //Move Player
        MovePlayer();
        //Jump
        if (isJumping)
        {
            Jump();
        }
        //Rotate Player
        RotatePlayer();

    }
    //Getter
    public float GetCurrentSpeed()
    {
        return moveDirection.magnitude;
    }
    //Update isActive
    public void SetActive(bool value)
    {
        isActive = value;
    }

    public bool GetGrounded()
    {
        return isGrounded;
    }

   public bool GetJumpState()
    {
        return isJumping;
    }

    public void SetupMovement()
    {
        SetActive(true);
        //Get Player Components
        moveRb = GetComponent<Rigidbody>();
        //Set Player Booleans
        isGrounded = true;
        isJumping = false;
        //Set Move Vectors
        moveDirection = Vector3.zero;
    }
    public void UpdateMovement(Vector3 directionToMove, bool jump)
    {
       // directionToMove=transform.InverseTransformVector(directionToMove);
        if (directionToMove == Vector3.zero)
        {
            moveRb.angularVelocity = Vector3.zero;
        }
        //Adjust Player Vectors to match camera
       // moveDirection = AdjustedMoveDirection(directionToMove);
        //moveDirection = transform.InverseTransformVector(directionToMove);
        moveDirection = transform.TransformVector(directionToMove);
        //Set Player Booleans
        //Set Player Booleans
        //moveDirection = directionToMove;

        if (jump)
        {
            //if player is grounded and not jumping and jump cooldown is 0
            if (isGrounded && !isJumping && jumpCooldownTimer <= 0)
            {
                isJumping = true;
            }
         //   else if (!isGrounded && canDoubleJump)
         //   {
         //       isJumping = true;
         //       canDoubleJump = false;
         //       Debug.Log("Trying to Double Jump");
         //   }
        }
        //Set Player Quaternions
        if (moveDirection.magnitude > 0)
        {
            lookRotation = Quaternion.LookRotation(moveDirection);
        }
        else
        {
            lookRotation = Quaternion.LookRotation(transform.forward);
        }
    }
    //Set Gravity
    private void SetGravity()
    {
        //Set Gravity
        Physics.gravity = new Vector3(0, -gravity, 0);
    }
    //Check if player is grounded
    private void CheckIfGrounded()

    {
        //Check if player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down,groundCheckDistance);
        if (isGrounded)
        {
            canDoubleJump = true;
        }
    }
    //Player Movement
    private void MovePlayer()
    {
        //Move Player
        moveRb.MovePosition(transform.position + moveDirection * speed *
    Time.deltaTime);
    }
    //Player Rotation
    private void RotatePlayer()
    {
        //Rotate Player
        Quaternion rotation = Quaternion.Slerp(transform.rotation,
        lookRotation, rotationSpeed * Time.deltaTime);
        //Nomalize rotation
        rotation = rotation.normalized;
        //set rotation
        moveRb.MoveRotation(rotation);
    }
    //Player Jump
    private void Jump()
    {
        //Jump Player
        moveRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //Set Player Booleans
        isGrounded = false;
        isJumping = false;
        //Cooldown
        JumpCooldown();
    }
    //Player Jump Cooldown
    private void JumpCooldown()
    {
        //Set Player Floats
        jumpCooldownTimer = jumpCooldown;
        //Start Coroutine
        StartCoroutine(JumpCooldownTimer());
    }
    //Player Jump Cooldown Timer
    private IEnumerator JumpCooldownTimer()
    {
        //Wait for jump cooldown
        while (jumpCooldownTimer > 0)
        {
            //Decrement jump cooldown
            jumpCooldownTimer -= Time.deltaTime;
            //Wait for next frame
            yield return null;
        }
    }
    //Todo: Uncomment this when camera is implemented
    //Adjust Player Vectors to match camera
    private Vector3 AdjustedMoveDirection(Vector3 moveDirection)
    {
        //Camera position
        Vector3 cameraPosition = mainCamera.transform.position;
        //remove the y height from the camera position
         cameraPosition.y = transform.position.y;
        //Direction from the Player to the camera
        Vector3 direction = transform.position - cameraPosition;
        //Adjust Player Vectors to match camera
          moveDirection = Quaternion.LookRotation(direction) * moveDirection;
        //Remove the y height from the move direction
        moveDirection.y = 0;
        //return player vectors
        return moveDirection;
    }
    public void OnDeath()
    {
        moveRb.velocity = Vector3.zero;
        isActive = false;
    }

}



