using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth {get; private set;}
    private Animator anim;
    private bool dead;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float _damage) //
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth); // Trunca la vida en 0 mínimo y vida con la que empieza startingHealth

        if (currentHealth > 0)
        {
            // player vivo
        }
        else
        {
            if (!dead)
            {
                GetComponent<CharacterController>().enabled = false;
                dead = true;
            }
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
    
}
