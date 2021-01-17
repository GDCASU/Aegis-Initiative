/*
 * Defines the health of the Player.
 * 
 * Author: [REDACTED]
 * Date: [REDACTED]
 * 
 */

/*
 * Revision Author: Cristion Dominguez / Christian Gonzalez
 * Revision Date: 2 January 2021
 * 
 * Modifications: Added a damage multiplier for incoming damage to the Player and a vulnerability effect.
 *                Added a defense value to decrease incoming damage to the Player.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth singleton;
    public int health = 50;
    public int maxHealth = 60;
    public float defense; 

    private float damageMultiplier = 1f;  // amount to multiply incoming damage by
    private IEnumerator vulnerabilityLifecycle;  // event dictating how long a vulnerability effect should last
    
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(0);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= Mathf.RoundToInt((float)damage * damageMultiplier * (1f - defense));
        RemarkManager.singleton.TakingDamage(); //added by Brad for when player takes damage
        if (health <= 0)
        {
            KillPlayer();
        }
    }    
    public void Heal(int heal)
    {
        if (health + heal > maxHealth) health = maxHealth;
        else health += heal;        
    }
    public void KillPlayer()
    {
        //Destroy(gameObject);
    }

    /// <summary>
    /// Changes the amount of damage the Player shall receive and starts the vulnerability effect's life cycle. Resets the effect's life cycle if it is still active.
    /// </summary>
    /// <param name="vulnerabilityDamageMultiplier"> new amount to multiply incoming damage by </param>
    /// <param name="activeTime"> time effect should last for </param>
    public void ApplyVulnerability(float vulnerabilityDamageMultiplier, float activeTime)
    {
        damageMultiplier = vulnerabilityDamageMultiplier;

        if (vulnerabilityLifecycle != null)
            StopCoroutine(vulnerabilityLifecycle);

        vulnerabilityLifecycle = StartVulnerabilityLifecycle(activeTime);
        StartCoroutine(vulnerabilityLifecycle);
    }

    /// <summary>
    /// Removes the vulnerability effect from the Player, restoring the the incoming damage multiplier to 1.
    /// </summary>
    /// <param name="activeTime"> time effect should last for </param>
    private IEnumerator StartVulnerabilityLifecycle(float activeTime)
    {
        yield return new WaitForSeconds(activeTime);
        damageMultiplier = 1f;
        vulnerabilityLifecycle = null;
    }
}
