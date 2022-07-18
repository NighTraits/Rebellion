using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Death : MonoBehaviour
{
    public AudioSource clip;
   
   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.CompareTag("Player"))
      {
       
       
       clip.Play();

      }


   }
}
