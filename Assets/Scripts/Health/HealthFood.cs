using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFood : MonoBehaviour
{
    [SerializeField] private float healthValue;
    public AudioSource sounded;

    private void start()
    {

       sounded = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().AddHealth(healthValue);
            Destroy(gameObject, 0.5f);
            sounded.Play();
            
            
        }
    }
}
