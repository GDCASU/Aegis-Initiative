using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General parent class for environmental objects that can be destroyed and/or
/// deal damage to the player. 
/// 
/// Some components taken from BreakableWall.cs by Matt Corey
/// </summary>
/// 
public class EnvironmentHealth : MonoBehaviour
{
    public int health;
    public int collisionDamage;
    public GameObject[] destroyReplacements;
    public bool destroyOnContact = true;

    private void Start()
    {
        StartCoroutine(despawn());
    }
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            for(int x = 0; x < destroyReplacements.Length; x++)
            {
                if (destroyReplacements[x] != null)
                {
                    Instantiate(destroyReplacements[x], transform.position, transform.rotation);
                }
            }

            if (destroyOnContact) Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) OnPlayerCollision();
    }

    public virtual void OnPlayerCollision()
    {        
        PlayerInfo.singleton.TakeDamage(collisionDamage);
        PlayerInfo.singleton.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        TakeDamage(health);
        
    }

    IEnumerator despawn()
    {
        float despawnTime = 15;
        while (despawnTime < 0)
        {
            despawnTime -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
