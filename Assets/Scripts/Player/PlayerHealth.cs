using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth singleton;
    public float health = 50;
    public float defense;
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
        health -= (float)damage*(1-defense);
        if (health <= 0)
        {
            KillPlayer();
        }
    }
    public void KillPlayer()
    {
        //Destroy(gameObject);
    }
}
