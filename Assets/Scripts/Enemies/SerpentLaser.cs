/*
 * Deals damage to Player in bursts when Water Laser collides with Player.
 * 
 * Author: Cristion Dominguez
 * Date: 1 March 2021 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerpentLaser : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Damage dealt in a burst")]
    private int damage;
    [SerializeField]
    [Tooltip("Time between bursts of damage")]
    private float damageCooldown;

    private bool canDealDamage;  // Can the laser deal damage at the moment?
    private float elapsedTime = 0f;  // time since dealing damage 

    /// <summary>
    /// Allows laser to deal damage when it is enabled.
    /// </summary>
    private void OnEnable() => canDealDamage = true;

    /// <summary>
    /// When the laser can not deal damage, elapsed time increases until it reaches the damage cooldown, at which it no longer increases and the laser can deal damage.
    /// </summary>
    private void Update()
    {
        if(!canDealDamage)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= damageCooldown)
                canDealDamage = true;
        }
    }

    /// <summary>
    /// Upon Player collision, if the laser can deal damage, deals damage to Player whilst also disabling the laser's ability to deal damage and setting the elapsed time to 0.
    /// </summary>
    /// <param name="other"> foreign collider </param>
    private void OnTriggerStay(Collider other)
    {
        if (canDealDamage && other.tag.Equals("Player"))
        {
            PlayerInfo.singleton.TakeDamage(damage);
            canDealDamage = false;
            elapsedTime = 0;
        }
    }
}
