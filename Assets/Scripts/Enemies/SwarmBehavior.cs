/* 
 * Handles the health, damage and units of the Swarm as well as the Swarm interactions with the Player.
 * 
 * Author: Cristion Dominguez
 * Date: 13 Nov. 2020
 */

 /*
  * Revision Author: Cristion Dominguez
  * Revision Date: 16 Nov. 2020
  * 
  * Modifications: Removed a list of indexes corresponding to active units in an array and replaced the units array with a units list. Updated documentation as well.
  */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwarmBehavior : MonoBehaviour
{
    [Header("Swarm Health and Damage")]
    [SerializeField]
    private int totalHealth = 100;  // health of full swarm

    [SerializeField]
    private int totalDamage = 20;  // damage done by a full swarm

    [Header("Swarm Units")]
    [SerializeField]
    private GameObject unitPrefab;  // prefab of a swarm resident

    [SerializeField]
    private int swarmPopulation = 100;  // unit count of a full swarm

    [Header("On Player Collision")]
    [SerializeField]
    private float disperseTime = 1.5f;

    private List<GameObject> units;  // arrays of units

    private float healthPerUnit;

    private float damagePerUnit;

    public Action OnPlayerCollision;  // delegate holding the Disperse method of all the units

    /// <summary>
    /// If swarm population >= 1, assigns unit values and instantiates unit prefabs in units list. Otherwise, destroys Swarm game object's parent.
    /// </summary>
    private void Start()
    {
        if (swarmPopulation >= 1)
        {
            // Assign health and damage values for each unit.
            healthPerUnit = (float) totalHealth / swarmPopulation;
            damagePerUnit = (float) totalDamage / swarmPopulation;
    
            // Initialize units list.
            units = new List<GameObject>();

            // Instantiate game objects from the unit prefab and insert them into units list.
            for (int i = 0; i < swarmPopulation; i++)
                units.Add(Instantiate(unitPrefab, this.transform));
        }
        else
            Destroy(this.transform.parent.gameObject);
    }

    /// <summary>
    /// If a bullet passes through Swarm Collider, accesses the damage of the bullet and applies damage to Swarm. Else if a player passes through Swarm Collider,
    /// accesses the player's PlayerHealth script to deal damage to player and then disperses the Swarm.
    /// </summary>
    /// <param name="other"> the Collider that entered the Swarm Collider </param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            int bulletDamage = other.gameObject.GetComponent<Bullet>().damage;
            TakeDamage(bulletDamage);
        }
        else if (other.tag == "Player")
        {
            DealDamage(other.GetComponent<PlayerHealth>());
            StartCoroutine(DisperseSwarm());
        }
    }

    /// <summary>
    /// Decreases Swarm health by damage value. If total health is <= 0, then destroys Swarm game object's parent. Otherwise, checks if the total health is not proportional
    /// to the Swarm population and if so, reduces the Swarm population until it is proportional to the health again.
    /// </summary>
    /// <param name="damage"> amount to remove from Swarm health </param>
    public void TakeDamage(int damage)
    {
        totalHealth -= damage;

        // If total health <= 0, then destroy parent game object and break out of function.
        if (totalHealth <= 0)
        {
            Destroy(this.transform.parent.gameObject);
            return;
        }

        // Whilst total health is not proportional to Swarm population, remove a unit randomly and reduce Swarm population by 1.
        while (totalHealth <= (swarmPopulation - 1) * healthPerUnit)
        {
            int removeUnitIndex = UnityEngine.Random.Range(0, units.Count);
            units[removeUnitIndex].GetComponent<SwarmUnit>().DisableUnit();
            units.RemoveAt(removeUnitIndex);
            swarmPopulation--;
        }
    }

    /// <summary>
    /// Calculates damage by rounding the product of active unit count times damage per unit and deals damage to player.
    /// </summary>
    /// <param name="player"> the PlayerHealth script belonging to the player </param>
    private void DealDamage(PlayerHealth player)
    {        
        // Rounding is required because the product could be a value such as 0.9999, which would default to 0 even though a 1 is desired.
        int damageDone = (int) Mathf.Round(units.Count * damagePerUnit);
        player.TakeDamage(damageDone);
    }

    /// <summary>
    /// Calls for each Swarm unit to disperse until disperse time is reached, at which point the Swarm game object's parent shall be destroyed.
    /// </summary>
    private IEnumerator DisperseSwarm()
    {
        if (OnPlayerCollision != null)
            OnPlayerCollision();

        yield return new WaitForSeconds(disperseTime);
        Destroy(this.transform.parent.gameObject);
    }
}