using System.Collections.Generic;
using UnityEngine;
//Required Components
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(IMovement))]
public class PlayerController : MonoBehaviour
{
    //Reference
    private IMovement movement;
    private Animator playerAnimator;
    private Inventory playerInventory;
    private Health playerHealth;
    //Player Fields
    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        // Set this as the playerController on the Game Manager
        GameManager.instance.SetPlayerController(this);
        //subscribe StartPlayer to the GameManager's GameStartedEvent
        GameManager.instance.GameStartedEvent += StartPlayer;
        //subscribe StopPlayer to the GameManager's GameOverEvent
        GameManager.instance.GameOverEvent += StopPlayer;
        GameManager.instance.PauseGameEvent += OnPause;
        //Setup Player
        SetupPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the player is active
        if (!isActive)
        {
            return;
        }
        //Get Input
        GetInput();
        //Set Animator Parameters
        SetAnimatorParameters();
    }
    //Player Animator Parameters
    private void SetAnimatorParameters()
    {
        if (playerAnimator == null)
        {
            return;
        }
        //Set Animator Parameters
        playerAnimator.SetFloat("Speed", movement.GetCurrentSpeed());
        playerAnimator.SetBool("IsGrounded", movement.GetGrounded()); //Change this
        if (playerHealth.IsDamagedThisUpdate())
        {
            playerAnimator.SetTrigger("Damaged");
        }
        if (movement.GetJumpState())
        {
            playerAnimator.SetTrigger("Jump");
        }
    }
    //Disable Player
    public void OnDeath()
    {
        //Set Player Booleans
        isActive = false;
        //Set Player Animator Parameters
        //playerAnimator.SetTrigger("Death");
        //Stop Player Movement
        movement.OnDeath();
        //Alert Game Manager
        GameManager.instance.GameOverSequence();
    }
    //Start Player
    void StartPlayer()
    {
        //Set isActive to true
        isActive = true;
        movement.SetActive(isActive);
    }

    //Stop Player
    void StopPlayer()
    {
        //Set isActive to false
        isActive = false;
        movement.SetActive(isActive);
    }
    private void OnPause(bool pause)
    {
        if (!pause)
        {
            StartPlayer();
        }
        else
        {
            StopPlayer();
        }
    }

    //Player Methods
    //Setup Player
    private void SetupPlayer()
    {
        
        //Components
        movement = GetComponent<IMovement>();
        playerInventory = GetComponent<Inventory>();
        playerHealth = GetComponent<Health>();
        playerAnimator = GetComponentInChildren<Animator>();
        //Update Loops
        playerInventory.AddLoop(0);
        //Setup Movement
        movement.SetupMovement();
        StartPlayer();

    }
    //Get The Input
    private void GetInput()
    {
        //Pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.GamePaused(true);
            return;
        }
        //Local Variables
        Vector3 moveDirection = Vector3.zero;

        //Get Player Input
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.z = Input.GetAxis("Vertical");
        //Clean up Movement
        if (moveDirection.magnitude > 1)
        {
            moveDirection = moveDirection.normalized;
        }
        if (moveDirection.magnitude < .01f)
        {
            moveDirection = Vector3.zero;
        }
        //Assign Jump
        bool jump = Input.GetButtonDown("Jump");
        //Move Player
        movement.UpdateMovement(moveDirection, jump);

    }

    //Collectable Pickup
    public void Collect(Collectable.CollectableType collectableType, int value)
    {
        //if collectable type is a loop  //TOFIX
        if (collectableType == Collectable.CollectableType.Loop)
        {
            //if there is an inventory
            if (playerInventory != null)
            {
                //Add Loop
                playerInventory.AddLoop(value);
            }
        }
    }
    //Damage
    public void Damage(int amount)
    {
        playerHealth.Damage(amount);
    }
    //Getters
    //Health
    public Health GetHealth()
    {
        return playerHealth;
    }
    //Position
    public Vector3 GetPosition()
    {
        return transform.position;
    }

}
