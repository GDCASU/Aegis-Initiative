/* 
 * Handles the health, damage and units of the Swarm as well as the Swarm interactions with the Player.
 * 
 * Author: Cristion Dominguez
 * Date: 13 Nov. 2020
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

    private GameObject[] units;  // arrays of units

    private List<int> activeUnitIndexes;  // list of indexes corresponding to active units

    private float healthPerUnit;

    private float damagePerUnit;

    public Action OnPlayerCollision;  // delegate holding the Disperse method of all the units

    /// <summary>
    /// If swarm population >= 1, assigns unit values, instantiates unit prefabs in units array, and adds unit indexes in activeUnitIndexes list. Otherwise, destroys
    /// swarm game object.
    /// </summary>
    private void Start()
    {
        if (swarmPopulation >= 1)
        {
            // Assign health and damage values for each unit.
            healthPerUnit = (float) totalHealth / swarmPopulation;
            damagePerUnit = (float) totalDamage / swarmPopulation;
    
            // Initialize units array and activeUnitIndexes list.
            units = new GameObject[swarmPopulation];
            activeUnitIndexes = new List<int>();

            // Instantiate game objects from the unit prefab and insert them into units array.
            // Add indexes to activeUnitIndexes list.
            for (int i = 0; i < swarmPopulation; i++)
            {
                units[i] = Instantiate(unitPrefab, this.transform);
                activeUnitIndexes.Add(i);
            }
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
    /// Decreases Swarm health by damage value. If total health is <= 0, then destroys Swarm game object. If total health is <= to the 
    /// </summary>
    /// <param name="damage"> amount to remove from Swarm health </param>
    public void TakeDamage(int damage)
    {
        totalHealth -= damage;

        if (totalHealth <= 0)
        {
            Destroy(this.transform.parent.gameObject);
            return;
        }

        while (totalHealth <= (swarmPopulation - 1) * healthPerUnit)
        {
            int removeUnitIndex = UnityEngine.Random.Range(0, activeUnitIndexes.Count);
            units[activeUnitIndexes[removeUnitIndex]].GetComponent<SwarmUnit>().DisableUnit();
            activeUnitIndexes.RemoveAt(removeUnitIndex);
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
        int damageDone = (int) Mathf.Round(activeUnitIndexes.Count * damagePerUnit);
        player.TakeDamage(damageDone);
    }

    /// <summary>
    /// Calls for each Swarm unit to disperse until disperse time is reached, at which point the Swarm game object shall be destroyed.
    /// </summary>
    private IEnumerator DisperseSwarm()
    {
        if (OnPlayerCollision != null)
            OnPlayerCollision();

        yield return new WaitForSeconds(disperseTime);
        Destroy(this.transform.parent.gameObject);
    }
}