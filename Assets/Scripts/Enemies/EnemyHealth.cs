using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int collisionDamage;
    public float lifeTime = 20;
    public float lifeTimer;

    private void Start()
    {
        lifeTimer = lifeTime;
    }
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) DestroyEnemy();
    }
    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0) DestroyEnemy();
    }
    public void DestroyEnemy()
    {
        Destroy(GetComponentInParent<EnemyMovement>()?.gameObject ?? gameObject);
    }
}
