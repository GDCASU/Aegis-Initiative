using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int collisionDamage;
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)Destroy(gameObject);
    }
}
