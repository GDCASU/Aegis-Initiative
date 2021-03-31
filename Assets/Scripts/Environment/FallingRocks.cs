using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRocks : EnemyHealth
{
    public float timeToFall = 0.0f; //timer for dropping rock

    private bool collided = false; //check if rock collided with Player
    private Rigidbody rigidbody;

    public override void Start()
    {
        base.Start();
        rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(StartFall(timeToFall));
    }

    IEnumerator StartFall(float timeToFall)
    {
        yield return new WaitForSeconds(timeToFall);
        rigidbody.constraints = RigidbodyConstraints.None;
    }

    //If rock collides with Player, then deal damage to Player once
    private void OnCollisionEnter(Collision collision)
    {
        if (!collided && collision.gameObject.CompareTag("Player"))
        {
            PlayerInfo.singleton.TakeDamage(collisionDamage); //damage Player
            collided = true;
        }
    }
}
