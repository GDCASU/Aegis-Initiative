using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth singleton;
    public int health = 50;
    public int maxHealth = 60;
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
        health -= Mathf.RoundToInt((float)damage*(1f-defense));
        if (health <= 0)
        {
            KillPlayer();
        }
    }    
    public void Heal(int heal)
    {
        if (health + heal > maxHealth) health = maxHealth;
        else health += heal;        
    }
    public void KillPlayer()
    {
        //Destroy(gameObject);
    }
}
