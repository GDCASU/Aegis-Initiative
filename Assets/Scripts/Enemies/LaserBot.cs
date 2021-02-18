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
using Cinemachine;

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
    private ShipMovement playerMovement;

    private float spread = 0.5f;
    public CinemachineSmoothPath path;
    public CinemachineDollyCart playerCart;

    /// <summary>
    /// Assigns the host and Player transforms, then rotates the Bot in the direction opposite to the direction the Player is facing.
    /// </summary>
    private void Start()
    {
        host = transform.parent;
        if (PlayerInfo.singleton != null)
        {
            player = PlayerInfo.singleton.GetComponent<Transform>();
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
    /// Shoots bullets in a burst. isShooting = true whilst the burst is running.
    /// </summary>
    private IEnumerator ShootBurst()
    {
        isShooting = true;
		Rigidbody dispensedBullet;

        for (int i = 0; i < bulletsInBurst; i++)
        {
            // Do not shoot bullet if the Bot has been destroyed.
            if (gameObject == null)
                yield break;

            //Vector3 bulletVelocity;
            //BulletVectorPrediction.PredictiveAim(bulletSpawnpoint.position, bulletSpeed, player.position, playerMovement.currentVelocity, 0, out bulletVelocity);

            Vector3 bulletVelocity = PredictBulletVelocityToPlayer();

            dispensedBullet = Instantiate(bullet, bulletSpawnpoint.position, transform.rotation).GetComponent<Rigidbody>();
            dispensedBullet.velocity = bulletVelocity;

            yield return new WaitForSeconds(betweenShotTime);
        }

        isShooting = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            playerDetected = true;
        }
    }

    private Vector3 PredictBulletVelocityToPlayer()
    {
        Vector3 initialBulletPosition = bulletSpawnpoint.position;
        Vector3 initialPlayerPosition = player.position;
        Vector3 playerVelocity = playerMovement.currentVelocity;
        Vector3 playerToBulletSpawnpoint = initialPlayerPosition - initialBulletPosition;

        float playerDistanceSquared = (playerToBulletSpawnpoint).sqrMagnitude;
        
        float bulletSpeedSquared = Mathf.Pow(bulletSpeed, 2);

        float playerSpeedSquared = playerVelocity.sqrMagnitude;

        float timeTillImpact;

        if(playerSpeedSquared >= bulletSpeedSquared)
        {
            timeTillImpact = Mathf.Sqrt(playerDistanceSquared / 0.001f);
        }
        else
        {
            timeTillImpact = Mathf.Sqrt(playerDistanceSquared / (bulletSpeedSquared - playerSpeedSquared));
        }

        /*
        //The players position at the given time
        Vector3 targetPosition = path.EvaluatePositionAtUnit(
            playerCart.m_Position + playerCart.m_Speed * timeTillImpact,
            playerCart.m_PositionUnits + (int)(playerCart.m_Speed * timeTillImpact)
        );

        //Normalized tangent vector where the player will be at a given time
        Vector3 targetTangent = Vector3.Normalize(
            path.EvaluateTangentAtUnit(
                playerCart.m_Position + playerCart.m_Speed * timeTillImpact,
                playerCart.m_PositionUnits + (int)(playerCart.m_Speed * timeTillImpact)
            )
        );
        */

        /**
         * Randomly chooses to go left or right of the player
         */
        /*
        if (Random.value < 0.5f)
        {
            targetTangent.x *= -1;
        }
        else
        {
            targetTangent.z *= -1;
        }

        targetTangent.y = 0;
        //Gives random length to the left/right modifier
        targetTangent *= Random.Range(-spread, spread);

        Vector3 interceptPoint = targetPosition + targetTangent;
        Vector3 velocityVector = (interceptPoint - initialPosition) / timeTillImpact;
        */

        Vector3 velocityVector = playerVelocity + (playerToBulletSpawnpoint / timeTillImpact);

        return velocityVector;
    }
}
