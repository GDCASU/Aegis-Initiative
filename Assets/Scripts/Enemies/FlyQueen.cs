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

public class FlyQueen : EnemyHealth
{
    [SerializeField]
    private GameObject flyLarvae;  // prefab of Fly Larvae

    [SerializeField]
    private float firstLarvaeDropRate = 3f;  // the rate at which larvae shall drop from the first spawnpoint

    [SerializeField]
    private List<GameObject> larvaeSpawnpoints;  // spawnpoints for the Fly Larvae

    public Animator animator;
    int spawnIndex = 0;

    public float timer = 0.1f;  // time since the latest larvae drop from the first spawnpoint

    public EnemyMovement enemyMovement;

    public float framesOfAnimation;

    private bool shooting;


    public override void Start()
    {
        base.Start();
        enemyMovement = GetComponentInParent<EnemyMovement>();
    }
    /// <summary>
    /// Initiates a barrage of larvae at the first larvae drop rate.
    /// </summary>
    override protected void Update()
    {
        base.Update();
        if (timer <=  0 && !shooting)
        {
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
        shooting = true;
        animator.SetBool("Shooting", shooting);       
        yield return new WaitForSeconds(framesOfAnimation /24f);
        shooting = false;
        animator.SetBool("Shooting", shooting);
        timer = firstLarvaeDropRate;

    }
}