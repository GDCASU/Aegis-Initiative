/* 
 * Handles the Laser Bot's movement towards the Player and its burst shooting.
 * 
 * Author: Cristion Dominguez
 * Date: 5 February 2021
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBot : MonoBehaviour
{
    [Header("Shooting")]
    [Tooltip("Time the next burst shall activate after the last bullet from the previous burst.")]
    [SerializeField]
    private float burstRecoveryTime = 2f;

    [Tooltip("Time between bullets in a single burst.")]
    [SerializeField]
    private float betweenShotTime = 0.3f;

    [Tooltip("Amount of bullets in a single burst.")]
    [SerializeField]
    private float bulletsInBurst = 3f;

    [Tooltip("Speed of bullet.")]
    [SerializeField]
    private float bulletSpeed = 10;

    [Tooltip("Damage a bullet deals.")]
    [SerializeField]
    private float bulletDamage;

    [Tooltip("The maximum distance a bullet's trajectory can deviate from a predetermined, future Player position.")]
    [SerializeField]
    private float maxSpreadDistance = 5.5f;

    [Tooltip("Gameobject of bullet.")]
    [SerializeField]
    private GameObject bullet;

    [Tooltip("Transform of an empty gameobject where the bullet shall spawn.")]
    [SerializeField]
    private Transform bulletSpawnpoint;

    [Header("Approaching Player")]
    [Tooltip("Speed the Bot shall approach the Player before detecting the Player.")]
    [SerializeField]
    private float approachSpeed = 4f;

    private float elapsedTime = 0;  // time after the last bullet of the previous burst
    private bool isShooting = false;  // Is the Bot shooting?

    private bool playerDetected = false;  // Has the Bot detected the Player in front of it at one point?

    private Transform host;  // Transform of parent gameobject
    private Transform player;  // Transform of the Player
    private Transform dollyCart;  // Transform of PlayerDollyCart
    private ShipMovement playerMovement;  // movement script of Player

    /// <summary>
    /// Assigns the host, Player and Dolly Cart transforms, then rotates the Bot in the direction opposite to the direction the Player is facing.
    /// </summary>
    private void Start()
    {
        host = transform.parent;
        if (PlayerInfo.singleton != null)
        {
            player = PlayerInfo.singleton.GetComponent<Transform>();
            dollyCart = player.parent;
            transform.Rotate(0, player.localEulerAngles.y + 180, 0);
            playerMovement = PlayerInfo.singleton.GetComponent<ShipMovement>();
        }
    }

    /// <summary>
    /// For every frame, moves the Bot towards the Player if the Player has not been detected yet. Afterward, shoots bursts of bullets at Player when the recovery time
    /// is reached.
    /// </summary>
    private void Update()
    {
        // If the Player exists and has not been detected yet, move the Bot host towards the Player on the XY-plane at approach speed.
        if (player != null && !playerDetected)
        {
            Vector2 hostXYPos = new Vector2(host.localPosition.x, host.localPosition.y);
            Vector2 playerXYPos = new Vector2(player.localPosition.x, player.localPosition.y);
            Vector2 newXYPos = Vector2.MoveTowards(hostXYPos, playerXYPos, approachSpeed * Time.deltaTime);
            host.localPosition = new Vector3(newXYPos.x, newXYPos.y, host.localPosition.z);
        }

        // If the Player has been detected, shoot at Player if the time elapsed after the previous burst is >= burstRecoveryTime and then set elapsedTime to 0. Only
        // increase elapsedTime if the Bot is not shooting.
        if (playerDetected)
        {
            if (elapsedTime >= burstRecoveryTime)
            {
                StartCoroutine(ShootBurst());
                elapsedTime = 0;
            }

            if (!isShooting)
                elapsedTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// For every fixed frame-rate frame, casts a ray in front of the Bot. If the ray hits a Player, set playerDetected to true and enable the EnemyMovement script on host.
    /// </summary>
    private void FixedUpdate()
    {
        RaycastHit hit;
        if (!playerDetected && Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.transform.name.Equals("Player"))
            {
                playerDetected = true;
                host.GetComponent<EnemyMovement>().enabled = true;
            }
        }
    }

    /// <summary>
    /// Shoots bullets in a burst at a predicted player position. isShooting = true whilst the burst is running.
    /// </summary>
    private IEnumerator ShootBurst()
    {
        isShooting = true;

        Vector3 initialBulletPosition, initialPlayerPosition, playerVelocity, futurePlayerPosition, bulletVelocity, positionInCamera, bulletToFuturePlayerPosition, spread;
        Quaternion bulletRotation;
		Rigidbody dispensedBullet;

        for (int i = 0; i < bulletsInBurst; i++)
        {
            // Determine bullet velocity to predicted player position.
            initialBulletPosition = dollyCart.InverseTransformPoint(bulletSpawnpoint.position);
            initialPlayerPosition = player.localPosition;
            playerVelocity = playerMovement.currentVelocity;
            bulletVelocity = DollyCartAimPrediction.GetBulletVelocityToTarget(initialBulletPosition, bulletSpeed, initialPlayerPosition, playerVelocity, dollyCart, out futurePlayerPosition, out _);

            // Constrain the prediced player position to the main camera viewpoint.
            futurePlayerPosition = dollyCart.TransformPoint(futurePlayerPosition);
            positionInCamera = Camera.main.WorldToViewportPoint(futurePlayerPosition);
            positionInCamera.x = Mathf.Clamp01(positionInCamera.x);
            positionInCamera.y = Mathf.Clamp01(positionInCamera.y);
            futurePlayerPosition = Camera.main.ViewportToWorldPoint(positionInCamera);

            // Project the bullet velocity onto the vector from bullet spawnpoint to predicted player position.
            bulletToFuturePlayerPosition = futurePlayerPosition - bulletSpawnpoint.position;
            bulletVelocity = Vector3.Project(bulletVelocity, bulletToFuturePlayerPosition);

            // Introduce spread.
            spread = Random.insideUnitSphere * maxSpreadDistance;
            bulletRotation = Quaternion.LookRotation(bulletVelocity, Vector3.up);
            bulletRotation = Quaternion.Euler(bulletRotation.eulerAngles += spread);

            // Launch the bullet.
            dispensedBullet = Instantiate(bullet, bulletSpawnpoint.position, bulletRotation, dollyCart).GetComponent<Rigidbody>();
            dispensedBullet.velocity = dispensedBullet.transform.forward * bulletSpeed;

            yield return new WaitForSeconds(betweenShotTime);
        }

        isShooting = false;
    }

    /// <summary>
    /// Stops Laser Bot movement if a collision with an Enemy is detected.
    /// </summary>
    /// <param name="collision"> collision info </param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Enemy")
            playerDetected = true;
    }
}
