using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    Animator DoorAnimation;
    private TextAlignment OpenDoorText;
    private bool OpenDoor = false;
    static public bool GameOver = false, GameClear = false;
    private AudioSource door;
   


    private void Start()
    {
        DoorAnimation = GetComponent<Animator>();
        door = GetComponent<AudioSource>();
    }

   private  void Update()
    {
        StartCoroutine(Wait());
    }

    // Enter door trigger area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Near Door");
            OpenDoor = true;
        }
    }

    // Exit door trigger area
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Away from door");
            OpenDoor = false;
        }
    }

    private IEnumerator  Wait(){
        
        // Open door
        if (OpenDoor && Input.GetKeyDown(KeyCode.F) && CharacterController.GotKey)
        {
            DoorAnimation.SetTrigger("OpeningDoor");
            yield return new WaitForSeconds( 3.0F );
            GameClear = true;
            door.Play();

        } else if(OpenDoor && Input.GetKeyDown(KeyCode.F))
        {
            GameOver = true;
        }
    }
}
