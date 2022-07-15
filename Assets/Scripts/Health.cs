using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Health : MonoBehaviour
{
    //Character Scalars
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private int healthRegenRate = 1;
    [SerializeField] private int healthRegenDelay = 1;

    //Character Booleans
    private bool isDead;
    private bool isRegenerating;
    private bool isDamaged;
    public bool IsDamagedThisUpdate()
    {
        if (isDamaged)
        {
            isDamaged = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    //Public Getters
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set the current health
        currentHealth = maxHealth;
        //Set the isDead boolean
        isDead = false;
        //Set the isRegenerating boolean
        isRegenerating = false;
    }
    //Damage the Character
    public void Damage(int damage)
    {
        //Check if the character is dead
        if (isDead)
        {
            return;
        }
        //Subtract the damage from the current health
        currentHealth -= damage;
        //Check if the character is dead
        if (currentHealth <= 0)
        {
            //Set the isDead boolean
            isDead = true;
            //Call the Die method
            Die();
        }
        //See if the character is not regenerating
        if (!isRegenerating)
        {
            //Start the regeneration coroutine
            StartCoroutine(Regenerate());
        }
        //Alert the Game Manager
        isDamaged = true;
    }
    // Trigger Death on the character
    private void Die()
    {
        //Send the death message to the player
        SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
    }
    //Heal the Character
    public void Heal(int heal)
    {
        //Check if the character is dead
        if (isDead)
        {
            return;
        }
        //Add the heal to the current health
        currentHealth += heal;
        //clamp the current health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
    //Regen the Character
    private IEnumerator Regenerate()
    {
        //Set the isRegenerating boolean
        isRegenerating = true;
        //while the character is damaged and not dead
        while (currentHealth < maxHealth && !isDead)
        {
            //Wait for the regen delay
            yield return new WaitForSeconds(healthRegenDelay);
            ///Heal the character
            Heal(healthRegenRate);
         }
        //Set the isRegenerating boolean
        isRegenerating = false;
    }
}