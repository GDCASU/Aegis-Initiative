using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth singleton;
    public int health = 50;
    public int maxHealth = 60;
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            KillPlayer();
        }
    }

    public void Heal(int heal)
    {
        if (health < maxHealth)
        {
            health += heal;
            if (health > maxHealth) health = maxHealth;
        }
    }
    public void KillPlayer()
    {
        //Destroy(gameObject);
    }
}
