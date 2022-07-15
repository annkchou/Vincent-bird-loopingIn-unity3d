using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Collectable : MonoBehaviour
{
    //Collectable types and their values
    public enum CollectableType
    {
        Loop,
        Coin,
        Health,
        Mana,
        Stamina,
        None
    }

    //CollectableType
    [SerializeField] private CollectableType collectableType;
    [SerializeField] private int collectableValue;
    [SerializeField] private AudioSource collectableAudio;
    //Collectable Collider
    private Collider collectableCollider;
    //Renderer
    private Renderer collectableRenderer;
    // Start is called before the first frame update
    void Start()
    {
        //Set the collectableCollider
        collectableCollider = GetComponent<Collider>();
        //Set the collectableCollider to be trigger
        collectableCollider.isTrigger = true;
        //Set the collectableRenderer
        collectableRenderer = GetComponent<Renderer>();
        collectableAudio = GetComponent<AudioSource>();
    }

    //OnTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        //Check if the other is the player
        if (other.gameObject.CompareTag("Player"))
        {
            //Get the Player Controller from the other
            PlayerController controller =
           other.gameObject.GetComponent<PlayerController>();
            if (controller == null) return;
            //Call the Collect method
            Collect(controller);
        }
    }

    //Collection Actions
    private void Collect(PlayerController controller)
    {
        //set the clip
       // collectableAudio.clip = collectableAudio;
        // play the clip
      //  collectableAudioSource.Play(); // watch for update collectale
        //Send the collectable type and value to the player
       controller.Collect(collectableType, collectableValue);
        //disable the renderer
        //collectableRenderer.enabled = false;
        //Disable the collectable collider
        collectableCollider.enabled = false;
        // float timeRemaining = 0;
        if (collectableAudio != null)
        {
            collectableAudio.Play();
        }
        
        //Destroy the collectable
        Destroy(gameObject,collectableAudio.clip.length);
    }
}