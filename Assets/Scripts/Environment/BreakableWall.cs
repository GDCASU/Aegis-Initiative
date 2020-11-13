using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    /* Notes:
     * The wall object with this script must also have the rigidbody and collider components
     * Within the rigidbody component of the wall object, "is Kinematic" must NOT be checked for collision with player to work
     * Within the rigidbody component of the wall object, check all the boxes under the "Constraints" tab so that it will not be moved by the bullets
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

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Hit player");
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(playerCollisionDamage);

            DestroyWall();
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            //Debug.Log("Hit bullet");
            wallHealth -= collision.gameObject.GetComponent<Bullet>().damage;

            Destroy(collision.gameObject);
        }

    }
}
