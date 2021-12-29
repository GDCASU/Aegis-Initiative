/*
 * Handles the behavior of the Fly Larvae from the Fly Queen, which deals damage and applies a vulnerability effect when colliding with the Player.
 * NOTE: Turn on isTrigger for the larvae collider if the gameobject shall spawn inside the Queen.
 * 
 * Author: Cristion Dominguez
 * Date: 2 January 2021
 * 
 */

 /*
  * Revision Author: Cristion Dominguez
  * Revision Date: 22 Jan. 2021
  * 
  * Modification: The vulnerability effect is invoked by referencing the PlayerInfo script.
  * 
  */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyLarvae : EnemyHealth
{
    [SerializeField]
    private float vulnerabilityDamageMultiplier = 1.25f;  // amount to multiply Player incoming damage by temporarily

    [SerializeField]
    private float vulnerabilityActiveTime = 5f;  // how long the vulnerability effect last

    [SerializeField]
    private float forwardSpeed;  // speed the larvae should be ejected from the Fly Queen's front

    private float elapsedTime = 0;  // time since the larvae's birth

    private Rigidbody larvaeBody;

    private Collider larvaeCollider;

    /// <summary>
    /// Obtains the Fly Larvae's Rigidbody and Collider as well as grant it a velocity in the direction of the Fly Queen's front.
    /// </summary>
    override public void Start()
    {
        base.Start();
        larvaeBody = GetComponent<Rigidbody>();
        larvaeCollider = GetComponent<Collider>();
        if (PlayerInfo.singleton) transform.LookAt(PlayerInfo.singleton.transform);
        larvaeBody.velocity = transform.forward * forwardSpeed;
    }

    /// <summary>
    /// If the larvae collider is a trigger (for when its gameobject spawns inside the Queen), then sets the collider to no longer be a trigger once the larvae exits the Queen.
    /// </summary>
    /// <param name="other"> the collider interacting with the larvae (presumably the Queen) </param>
    private void OnTriggerExit(Collider other)
    {
        if (larvaeCollider.isTrigger)
            larvaeCollider.isTrigger = false;
    }

    /// <summary>
    /// If the larvae collides with the Player, deals damage and applies a vulnerability effect to the Player. Destroys the larvae gameobject afterwards.
    /// </summary>
    /// <param name="collision"> info about the collision </param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            PlayerInfo.singleton.TakeDamage(collisionDamage);
            PlayerInfo.singleton.AddStatusEffect(StatusEffects.VULNERABILITY, vulnerabilityDamageMultiplier, vulnerabilityActiveTime);
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(this.gameObject);
        }
    }
}
