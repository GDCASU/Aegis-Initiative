using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int collisionDamage;
    public bool doNotDespawn;
    public float lifeTime = 20;
    public GameObject deathEffect;
    public float lifeTimer;

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
        if (health <= 0)
        {
            if (deathEffect != null)
            {
                GameObject temp = Instantiate(deathEffect, transform.position, Quaternion.identity);
                temp.transform.localScale = transform.localScale;
            }
            DestroyEnemy();
        }
    }
    public virtual void DestroyEnemy()
    {
        //print(UnityEngine.StackTraceUtility.ExtractStackTrace());
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
