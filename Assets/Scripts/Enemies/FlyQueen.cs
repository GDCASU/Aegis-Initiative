/* 
 * Handles the behavior of the Fly Queen, which drops larvae in an attempt to hit the Player.
 * 
 * Author: Cristion Dominguez
 * Date: 2 January 2021
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyQueen : MonoBehaviour
{
    [SerializeField]
    private GameObject flyLarvae;  // prefab of Fly Larvae

    [SerializeField]
    private float firstLarvaeDropRate = 3f;  // the rate at which larvae shall drop from the first spawnpoint

    [SerializeField]
    private float succeedingLarvaeDropRate = 0.3f;  // the rate at which the second and succeeding spawnpoints shall spawn larvae after one another

    [SerializeField]
    private List<GameObject> larvaeSpawnpoints;  // spawnpoints for the Fly Larvae

    private float elapsedTime = 0;  // time since the latest larvae drop from the first spawnpoint

    /// <summary>
    /// Initiates a barrage of larvae at the first larvae drop rate.
    /// </summary>
    private void Update()
    {
        // If the elapsed time reaches the firstLarvaeDropRate, call the DropLarvae coroutine and reset the elapsed time.
        if (elapsedTime >= firstLarvaeDropRate)
        {
            StartCoroutine(DropLarvae());

            elapsedTime = 0;
        }

        // Increment the elapsed time.
        elapsedTime += Time.deltaTime;
    }

    /// <summary>
    /// For each spawnpoint, creates a Fly Larvae at the succeeding larvae drop rate.
    /// </summary>
    private IEnumerator DropLarvae()
    {
        for (int i = 0; i < larvaeSpawnpoints.Count; i++)
        {
            // If the Fly Queen has been destroyed, stop dropping larvae.
            if (gameObject == null)
            {
                yield break;
            }

            Instantiate(flyLarvae, larvaeSpawnpoints[i].transform.position, transform.rotation);
            yield return new WaitForSeconds(succeedingLarvaeDropRate);
        }
    }
    public void SpawnLarvae()
    {
        Instantiate(flyLarvae, larvaeSpawnpoints[0].transform.position, transform.rotation);
    }
}
