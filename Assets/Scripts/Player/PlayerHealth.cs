using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;
    void Start()
    {
        health = 50;
        maxHealth = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            KillPlayer();
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        print(health);
    }

    public void Heal(int heal)
    {
        if(health < maxHealth)
        {
            health += heal;
        }
        Debug.Log(health);
    }
    public void KillPlayer()
    {
        //Destroy(gameObject);
    }
}
