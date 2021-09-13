/*
 * Rotates Flying Bat-Squirrel around Host on a horizontal axis whilst moving Host towards the Player until it passes the Player.
 * 
 * Author: Cristion Dominguez
 * Date: 26 March 2021
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBatSquirrel : EnemyHealth
{
    [SerializeField]
    [Tooltip("Speed at which Squirrel shall approach the Player.")]
    private float approachSpeed = 1f;

    [SerializeField]
    [Tooltip("Intensity at which Squirrel shall rotate towards Player.")]
    private float followIntensity = 0.3f;

    [SerializeField]
    [Tooltip("Speed at which Squirrel shall spin around its host.")]
    private float spinSpeed = 700f;

    [SerializeField]
    [Tooltip("Does the Squirrel spawn in the Player Dolly Cart?")]
    private bool spawnsInDollyCart = true;

    private Transform host, player;
    private bool isFollowingPlayer = true;  // Is the Squirrel (Host) following the Player?

    Vector3 targetDirection, newDirection;  // for rotation calculation

    /// <summary>
    /// Initializes variables and turns the Flying Bat-Squirrel around towards the Player's general direction if it spawns in Player Dolly Cart (Cart orients enemies away from the Player).
    /// </summary>
    private void Start()
    {
        host = transform.parent;
        player = PlayerInfo.singleton.transform;

        if (spawnsInDollyCart)
        {
            float yAngles = host.localEulerAngles.y;
            if (yAngles < 90 || yAngles > 270)
                yAngles += 180;
            host.localEulerAngles = new Vector3(host.localEulerAngles.x, yAngles, host.localEulerAngles.z);
        }
    }

    /// <summary>
    /// Rotates Squirrel around Host and translates Host forwards. If the Squirrel is following the Player, then the Host rotates towards the Player.
    /// </summary>
    private void Update()
    {
        base.Update();
        if (isFollowingPlayer)
            host.eulerAngles = RotateToObject(player);

        transform.RotateAround(host.position, host.forward, spinSpeed * Time.deltaTime);
        host.Translate(Vector3.forward * approachSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Deals damage to Player upon collision between Squirrel and Player.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("Player"))
        {
            int damage = transform.GetComponent<EnemyHealth>().collisionDamage;
            player.GetComponent<PlayerInfo>().TakeDamage(damage);
            transform.GetComponent<EnemyHealth>().DestroyEnemy();
        }
    }

    /// <summary>
    /// Returns a 3D Vector (Worldspace) of euler angles for the Squirrel host rotating towards a target on the next frame.
    /// </summary>
    /// <param name="target"> transform Shark should rotate to </param>
    /// <returns> 3D Vector of euler angles for next frame </returns>
    private Vector3 RotateToObject(Transform target)
    {
        targetDirection = target.position - host.position;
        newDirection = Vector3.RotateTowards(host.forward, targetDirection, followIntensity * Time.deltaTime, 0);
        return Quaternion.LookRotation(newDirection).eulerAngles;
    }

    /// <summary>
    /// Notifies Host to stop following the Player.
    /// </summary>
    public void StopFollowingPlayer() => isFollowingPlayer = false;

    public override void DestroyEnemy()
    {
        GetComponent<EnemySpawner>().SpawnEnemy();
        base.DestroyEnemy();
    }
}
