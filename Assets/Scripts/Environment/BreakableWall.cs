using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    /* Notes:
     * The wall object with this script must also have a collider component
     */
    public int wallHealth = 10;
    public int playerCollisionDamage= 5;

    // Update is called once per frame
    void Update()
    {
        if (wallHealth <= 0)
        {
            DestroyWall();
        }

    }

    void DestroyWall()
    {
        //Debug.Log("Wall Destroyed");
        Destroy(gameObject);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Hit player");
            //collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(playerCollisionDamage);

            DestroyWall();
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            //Debug.Log("Hit bullet");
            wallHealth -= other.gameObject.GetComponent<Bullet>().damage;

            Destroy(other.gameObject);
        }
    }

}
