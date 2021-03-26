/*
 * Revision Author: Cristion Dominguez
 * Revision Date: 1 March 2021
 * 
 * Modfications: Added the isPermanent variable to allow some Enemies to ignore the life time.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int collisionDamage;
    public float lifeTime = 20;
    public float lifeTimer;
    [Tooltip("Shall the Enemy ignore their life time?")]
    public bool isPermanent = false;

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
        if (!isPermanent && lifeTimer <= 0) DestroyEnemy();
    }
    public void DestroyEnemy()
    {
        Destroy(GetComponentInParent<EnemyMovement>()?.gameObject ?? gameObject);
    }
}
