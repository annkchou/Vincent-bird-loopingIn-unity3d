using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Required Components
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    //Enemy Scalars
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private float stoppingDistance = 1.0f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private int attackDamage = 5;
    [SerializeField] private float updateCooldown = 0.1f;

    //Enemy Components
    private NavMeshAgent agent;
    //Enemy Booleans
    private bool isActive;
    //Look rotations
    private Quaternion lookRotation;

    //Start is called before the first frame update
    void Start()
    {
        SetupEnemy();
    }
    //Update is called once per frame
    void Update()
    {
        //Rotate towards the destination
        RotateTowardsDestination();
    }

    //Rotate towards the destination
    private void RotateTowardsDestination()
    {
        //Get the direction to the destination
        Vector3 direction = agent.destination - transform.position;
        //Check if the direction is not zero
        if (direction != Vector3.zero)
        {
            //Set the look rotation
            lookRotation = Quaternion.LookRotation(direction);
            //Rotation
            float rotation = rotationSpeed * Time.deltaTime;
            //Rotate the enemy towards the direction
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation,
           rotation);
        }
    }

    //Setup the Enemy
    private void SetupEnemy()
    {
        //Set the agent
        agent = GetComponent<NavMeshAgent>();
        //Set the stopping distance
        agent.stoppingDistance = stoppingDistance;
        //Set the speed
        agent.speed = speed;
        //Set the rotation speed
        agent.angularSpeed = rotationSpeed;
        //Set the destination
        agent.destination = transform.position;
        //Start looking for the player
        StartCoroutine(LookForPlayer());
        //Start the Core Behaviour
        StartCoroutine(EnemyBehaviour());
    }
    //Attack the Player
    private void Attack()
    {
        GameManager.instance.playerController.Damage(attackDamage);
    }
    //Enemy Behaviour
    private IEnumerator EnemyBehaviour()
    {
        //Set the destination
        SetDestination();
        yield return null;
        //Check if the enemy is active
        while (isActive)
        {
            //Check if the enemy is in range
            //player position
            Vector3 playerPosition =
           GameManager.instance.playerController.GetPosition();
            float distance = Vector3.Distance(playerPosition, transform.position);

            if (distance <= stoppingDistance)
            {
                //Attack
                Attack();
                //Wait for the attack cooldown
                yield return new WaitForSeconds(attackCooldown);
                continue;
            }
            //Wait for updateCooldown
            yield return new WaitForSeconds(updateCooldown);
            //Set the destination
            SetDestination();
        }
    }

    //Look for the player
    private IEnumerator LookForPlayer()
    {
        //Check if the enemy is active
        while (isActive)
        {
            //Wait for updateCooldown
            yield return new WaitForSeconds(updateCooldown);
            //Set the destination
            SetDestination();
        }
    }
    //Set the destination
    private void SetDestination()
    {
        //Get the player
        PlayerController player = GameManager.instance.playerController;
        if (player == null) return;
        //Set the destination
        agent.destination = player.GetPosition();  
    }
}
