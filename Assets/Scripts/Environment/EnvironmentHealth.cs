using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentHealth : MonoBehaviour
{
    public int health;
    public int collisionDamage;
    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInChildren<PlayerHealth>().TakeDamage(collisionDamage);
            TakeDamage(health);
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
