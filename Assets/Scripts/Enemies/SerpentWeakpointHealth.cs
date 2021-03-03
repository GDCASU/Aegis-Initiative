/*
 * Defines the health for the Sea Serpent's Head and Mouth Weakpoints.
 * 
 * Author: Cristion Dominguez
 * Date: 26 Feb. 2021
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerpentWeakpointHealth : EnemyHealth
{
    private enum WeakpointType
    {
        Head,
        Mouth
    }

    [Header("Serpent Variables")]
    [SerializeField]
    private WeakpointType weakpoint;  // the type of weakpoint the script is attached to
    [SerializeField]
    [Tooltip("Health required to cancel the attack associated with this weakpoint")]
    private int maxHealth;

    private SeaSerpent serpent;
    private EnemyHealth serpentHealth;

    /// <summary>
    /// Obtains SeaSerpent and EnemyHealth scripts from the Sea Serpent transform.
    /// </summary>
    private void Awake()
    {
        serpent = transform.parent.GetComponent<SeaSerpent>();
        serpentHealth = transform.parent.GetComponent<EnemyHealth>();
    }

    /// <summary>
    /// Resets health everytime the weakpoint is enabled.
    /// </summary>
    private void OnEnable() => health = maxHealth;

    /// <summary>
    /// Decreases health of weakpoint and Sea Serpent. If the health of the weakpoint is diminished, the attack pattern associated with the weakpoint is cancelled.
    /// </summary>
    /// <param name="damage"></param>
    public override void TakeDamage(int damage)
    {
        health -= damage;
        serpentHealth.TakeDamage(damage);

        if (health <= 0)
        {
            if (weakpoint == WeakpointType.Head)
                serpent.CancelHeadBash();
            else
                serpent.CancelWaterCannon();
        }
    }

    /// <summary>
    /// Deals damage to Player if the Head Weakpoint collides with Player.
    /// </summary>
    /// <param name="other"> info on collision </param>
    private void OnTriggerEnter(Collider other)
    {
        if (weakpoint == WeakpointType.Head && other.tag.Equals("Player"))
            other.GetComponent<PlayerInfo>().TakeDamage(serpent.LungeDamage);
    }
}
