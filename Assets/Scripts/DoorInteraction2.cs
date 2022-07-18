using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction2 : MonoBehaviour
{
    private TextAlignment OpenDoorText;
    private bool OpenDoor = false;
    static public bool GameOver, GameClear;

    private void Start()
    {   
        GameOver = false;
        GameClear = false;
    }

   private  void Update()
    {
        // Open door
        if (OpenDoor && Input.GetKeyDown(KeyCode.F) && CharacterControllerLvl2.GotKey)
        {
            GameClear = true;
            
        } else if(OpenDoor && Input.GetKeyDown(KeyCode.F))
        {
            GameOver = true;
        }
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

}
