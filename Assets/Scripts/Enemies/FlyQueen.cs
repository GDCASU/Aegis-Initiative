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

    public Animator animator;
    int spawnIndex = 0;

    public float timer = 0.1f;  // time since the latest larvae drop from the first spawnpoint

    public EnemyMovement enemyMovement;


    private void Start()
    {
        enemyMovement = GetComponentInParent<EnemyMovement>();
    }
    /// <summary>
    /// Initiates a barrage of larvae at the first larvae drop rate.
    /// </summary>
    private void Update()
    {
        if (timer <=  0)
        {
            timer = firstLarvaeDropRate;
            StartCoroutine(DropLarvae());
        }
        if (!enemyMovement.flyingIn && !enemyMovement.isFlyingAway) timer -= Time.deltaTime;
    }

    public void SpawnBullet()
    {
        Instantiate(flyLarvae, larvaeSpawnpoints[spawnIndex].transform.position, transform.rotation);
        spawnIndex = (++spawnIndex % 2);
    }

    /// <summary>
    /// For each spawnpoint, creates a Fly Larvae at the succeeding larvae drop rate.
    /// </summary>
    private IEnumerator DropLarvae()
    {
        animator.SetBool("Shooting", true);
        yield return new WaitForSeconds(12 /24f);
        animator.SetBool("Shooting", false);

    }
}