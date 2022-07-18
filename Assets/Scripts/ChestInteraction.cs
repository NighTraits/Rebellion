using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    Animator ChestAnimation;
    private TextAlignment OpenChestText;
    private bool OpenChest = false;
    static public bool chestOpen = false;
    public AudioSource sonido;

    private void Start()
    {
        ChestAnimation = GetComponent<Animator>();
        sonido = GetComponent<AudioSource>();
    }

   private  void Update()
    {
        // Can only get one key
        if (OpenChest && Input.GetKeyDown(KeyCode.F) && !CharacterController.GotKey)
        {
            ChestAnimation.SetTrigger("OpeningChest");
            chestOpen = true;
            sonido.Play();

            // Chest 1 has the correct key
            if(this.CompareTag("DoorChest"))
            {
                CharacterController.GotKey = true;
               // sonido.Play();
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            OpenChest = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            OpenChest = false;
        }
    }
}
