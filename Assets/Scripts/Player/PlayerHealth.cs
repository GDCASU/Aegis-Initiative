using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    void Start()
    {
        health = 50;
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
    public void KillPlayer()
    {
        //Destroy(gameObject);
    }
}
