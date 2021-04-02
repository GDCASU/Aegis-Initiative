﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int collisionDamage;
    public bool doNotDespawn;
    public float lifeTime = 20;
    private float lifeTimer;

    public virtual void Start()
    {
        if (!doNotDespawn)
        {
            lifeTimer = lifeTime;
            StartCoroutine(waitToDespawn());
        }
    }
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) DestroyEnemy();
    }
    public void DestroyEnemy()
    {
        Destroy(GetComponentInParent<EnemyMovement>()?.gameObject ?? gameObject);
    }
    IEnumerator waitToDespawn()
    {
        while (lifeTimer > 0)
        {
            lifeTimer -= Time.deltaTime;
            yield return null;
        }
        DestroyEnemy();
    }
}
