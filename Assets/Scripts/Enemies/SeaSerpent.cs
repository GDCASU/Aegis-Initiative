/*
 * Defines and invokes the attack patterns of the Sea Serpent Boss.
 * 
 * Author: Cristion Dominguez
 * Date: 26 Feb. 2021
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSerpent : MonoBehaviour
{
    // Attack patterns of the Sea Serpent.
    private enum AttackPattern
    {
        WaterBarrage,
        HeadBash,
        WaterCannon
    }

    [Header("Basics")]

    [SerializeField]
    [Tooltip("Time Sea Serpent shall remain idle after spawning")]
    private float beginAttackingTime = 2f;
    [SerializeField]
    [Tooltip("Cooldown between attacks")]
    private float attackCooldown = 2f;
    [SerializeField]
    [Tooltip("Frequency of the Water Barrage attack (MUST BE LARGEST VALUE)")]
    private float waterBarrageFrequency = 0.5f;
    [SerializeField]
    [Tooltip("Frequency of the Head Bash attack (MUST BE MIDDLE VALUE)")]
    private float headBashFrequency = 0.35f;
    [SerializeField]
    [Tooltip("Frequency of the Water Cannon attack (MUST BE SMALLEST VALUE)")]
    private float waterCannonFrequency = 0.15f;

    [Header("Water Barrage")]

    [SerializeField]
    [Tooltip("Prefab of Water Bullet")]
    private GameObject waterBullet;
    [SerializeField]
    [Tooltip("Transform of the Water Bullet spawn location")]
    private Transform waterBulletSpawnpoint;
    [SerializeField]
    [Tooltip("Water Bullets in a barrage attack")]
    private int waterBulletsInBarrage = 5;
    [SerializeField]
    [Tooltip("Time between the launchings of individual Water Bullets")]
    private float timeBetweenWaterBullets = 0.5f;
    [SerializeField]
    [Tooltip("Speed of Water Bullet")]
    private float waterBulletSpeed = 8f;
    [SerializeField]
    [Tooltip("Damage of Water Bullet")]
    private int waterBulletDamage = 10;
    [SerializeField]
    [Tooltip("Max distance a Water Bullet can be from a predetermined, future Player position (higher value = lower accuracy)")]
    private float maxSpreadDistance = 0f;

    [Header("Head Bash")]

    [SerializeField]
    [Tooltip("Transform of Head Weakpoint")]
    private Transform headWeakpoint;
    [SerializeField]
    [Tooltip("Distance Head Weakpoint should protude out of Serpent")]
    private float headProtrusionDistance = 0.1f;
    [SerializeField]
    [Tooltip("Time Head Weakpoint should protude out of Serpent")]
    private float headProtrustionTime = 1f;
    [SerializeField]
    [Tooltip("Time Head Weakpoint should retract into Serpent")]
    private float headRetractionTime = 0.5f;
    [SerializeField]
    [Tooltip("Time to charge the bash attack (Head Weakpoint is vulnerable during this time.)")]
    private float bashChargeTime = 6f;
    [field:SerializeField]
    [field:Tooltip("Amount of damage required to cancel bash attack")]
    public int BashCancelDamage { get; private set; }
    [SerializeField]
    [Tooltip("Amount of times the Serpent shall lunge during bash attack")]
    private int totalLunges = 3;
    [field:SerializeField]
    [field:Tooltip("Damage dealt to Player on each lunge")]
    public int LungeDamage { get; private set; }
    [SerializeField]
    [Tooltip("Distance of lunge")]
    private float lungeDistance = 2f;
    [SerializeField]
    [Tooltip("Time to reach lunge distance")]
    private float lungeTime = 0.3f;
    [SerializeField]
    [Tooltip("Time to complete retract from lunge")]
    private float retractTime = 0.6f;
    [SerializeField]
    [Tooltip("Time between individual lunges")]
    private float timeBetweenLunges = 0.5f;
    [SerializeField]
    [Tooltip("Speed the Serpent shall follow Player before lunging")]
    private float followSpeed = 1f;

    [Header("Water Cannon")]

    [SerializeField]
    [Tooltip("Transform of Mouth Weakpoint")]
    private Transform mouthWeakpoint;
    [SerializeField]
    [Tooltip("Distance Mouth Weakpoint should protude out of Serpent")]
    private float mouthWeakpointProtrusion = 0.3f;
    [SerializeField]
    [Tooltip("Time Mouth Weakpoint should protude out of serpent")]
    private float mouthProtrustionTime = 1f;
    [SerializeField]
    [Tooltip("Time Mouth Weakpoint should retract into serpent")]
    private float mouthRetractionTime = 0.5f;
    [SerializeField]
    [Tooltip("Transform of Water Laser")]
    private Transform waterLaser;
    [SerializeField]
    [Tooltip("Time to charge the cannon attack (Mouth Weakpoint is vulnerable during this time.)")]
    private float cannonChargeTime = 8f;
    [field: SerializeField]
    [field: Tooltip("Amount of damage required to cancel cannon attack")]
    public int CannonCancelDamage { get; private set; }
    [SerializeField]
    [Tooltip("Duration of cannon laser")]
    private float laserDuration = 10f;
    [SerializeField]
    [Tooltip("Speed laser shall follow the Player")]
    private float laserFollowSpeed = 0.5f;

    private AttackPattern currentPattern;  // current attack pattern of Serpent
    private Transform host;  // host of Serpent controlling movement

    private bool isReadyForBattle = false;  // Is the Serpent ready for battle?
    private bool isAttacking = false;  // Is the Serpent performing an attack pattern?
    private bool isHeadBashing = false;  // Is the Serpent performing the Head Bash attack?
    private bool isCannoning = false;  // Is the Serpent performing the Water Cannon attack?

    private float randonNumber;  // random number between 0 and 1 utilized to determine the next attack
    private float headBashInterval;  // interval for invoking the Head Bash attack; if randomNumber is in the interval, Head Bash shall be performed
    private float timeSinceAttack;  // time since the last attack

    public bool headBashDisrupted = false;  // Has the Head Bash been disrupted?
    public bool waterCannonDisrupted = false;  // Has the Water Cannon been disrupted?

    private Transform player;  // Transform of Player
    private Transform dollyCart;  // Transform of Player Dolly Cart
    private ShipMovement playerMovement;  // movement script attached to Player

    private Vector3 originalHostPosition;  // position of Host upon spawning
    private Vector2 currentHostPosition_XY;  // current position of Host on the X,Y plane
    private Vector3 newHostPosition_XY;  // new position of Host on the X,Y plane
    private Vector2 playerPosition_XY;  // position of the Player on the X,Y plane

    /// <summary>
    /// Assigns variables.
    /// </summary>
    private void Start()
    {
        host = transform.parent;
        originalHostPosition = host.localPosition;
        waterLaser = mouthWeakpoint.GetChild(0);
        headBashInterval = waterCannonFrequency + headBashFrequency;

        player = PlayerInfo.singleton.transform;
        dollyCart = player.parent;
        playerMovement = player.GetComponent<ShipMovement>();
        
        StartCoroutine(WaitToBeginBattle());
    }

    /// <summary>
    /// Prevents the Serpent from attacking for beginAttackingTime.
    /// </summary>
    private IEnumerator WaitToBeginBattle()
    {
        yield return new WaitForSeconds(beginAttackingTime);
        isReadyForBattle = true;
    }

    /// <summary>
    /// Increases timeSinceAttack when the Serpent is ready for battle and is not attacking. Once timeSinceAttack reaches the attackCooldown, a random number is generated
    /// and an attack pattern is selected based off that number.
    /// </summary>
    private void Update()
    {
        if (isReadyForBattle && !isAttacking)
        {
            timeSinceAttack += Time.deltaTime;

            if (timeSinceAttack >= attackCooldown)
            {
                isAttacking = true;
                timeSinceAttack = 0;

                randonNumber = Random.value;

                StartCoroutine(StartHeadBash());

                /*
                // Choose the attack pattern by determining which interval randomNumber is in.
                if (randonNumber <= waterCannonFrequency)
                {
                    StartCoroutine(StartWaterCannon());
                }
                else if(randonNumber <= headBashInterval)
                {
                    StartCoroutine(StartHeadBash());
                }
                else
                {
                    StartCoroutine(StartWaterBarrage());
                }
                */
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void FixedUpdate()
    {
        if (isHeadBashing)
        {
            currentHostPosition_XY = new Vector2(host.localPosition.x, host.localPosition.y);
            playerPosition_XY = new Vector2(player.localPosition.x, player.localPosition.y);
            newHostPosition_XY = Vector2.MoveTowards(currentHostPosition_XY, playerPosition_XY, followSpeed * Time.deltaTime);
            host.localPosition = new Vector3(newHostPosition_XY.x, newHostPosition_XY.y, host.localPosition.z);
        }
        else if(isCannoning)
        {
            Vector3 playerDirection = player.position - mouthWeakpoint.position;
            Vector3 currentDirection = Vector3.RotateTowards(mouthWeakpoint.forward, playerDirection, laserFollowSpeed * Time.deltaTime, 0f);
            mouthWeakpoint.rotation = Quaternion.LookRotation(currentDirection);
        }
    }

    private IEnumerator StartWaterBarrage()
    {
        Vector3 initialBulletPosition, initialPlayerPosition, playerVelocity, futurePlayerPosition, bulletVelocity, positionInCamera, bulletToFuturePlayerPosition, spread;
        Quaternion bulletRotation;
		Rigidbody dispensedBullet;

        for (int i = 0; i < waterBulletsInBarrage; i++)
        {
            // Do not shoot bullet if the Bot has been destroyed.
            if (gameObject == null)
                yield break;

            // Determine bullet velocity to predicted player position.
            initialBulletPosition = dollyCart.InverseTransformPoint(waterBulletSpawnpoint.position);
            initialPlayerPosition = player.localPosition;
            playerVelocity = playerMovement.currentVelocity;
            bulletVelocity = DollyCartAimPrediction.GetBulletVelocityToTarget(initialBulletPosition, waterBulletSpeed, initialPlayerPosition, playerVelocity, dollyCart, out futurePlayerPosition, out _);

            // Constrain the prediced player position to the main camera viewpoint.
            futurePlayerPosition = dollyCart.TransformPoint(futurePlayerPosition);
            positionInCamera = Camera.main.WorldToViewportPoint(futurePlayerPosition);
            positionInCamera.x = Mathf.Clamp01(positionInCamera.x);
            positionInCamera.y = Mathf.Clamp01(positionInCamera.y);
            futurePlayerPosition = Camera.main.ViewportToWorldPoint(positionInCamera);

            // Project the bullet velocity onto the vector from bullet spawnpoint to predicted player position.
            bulletToFuturePlayerPosition = futurePlayerPosition - waterBulletSpawnpoint.position;
            bulletVelocity = Vector3.Project(bulletVelocity, bulletToFuturePlayerPosition);

            // Introduce spread.
            spread = Random.insideUnitSphere * maxSpreadDistance;
            bulletRotation = Quaternion.LookRotation(bulletVelocity, Vector3.up);
            bulletRotation = Quaternion.Euler(bulletRotation.eulerAngles += spread);

            // Launch the bullet and assigned dmg.
            dispensedBullet = Instantiate(waterBullet, waterBulletSpawnpoint.position, bulletRotation, dollyCart).GetComponent<Rigidbody>();
            dispensedBullet.GetComponent<Bullet>().damage = waterBulletDamage;
            dispensedBullet.velocity = dispensedBullet.transform.forward * waterBulletSpeed;

            yield return new WaitForSeconds(timeBetweenWaterBullets);
        }

        isAttacking = false;
    }

    private IEnumerator StartHeadBash()
    {
        host.GetComponent<EnemyMovement>().enabled = false;
        isHeadBashing = true;
        
        float timeElapsed = 0f;
        float initialYPosition = headWeakpoint.localPosition.y;
        float finalYPosition = headWeakpoint.localPosition.y + headProtrusionDistance;
        float currentYPosition;

        float initialXAngles = transform.localRotation.x;
        float finalXAngles = 90f;
        float currentXAngles;
        float rotateSpeed = 90f / headProtrustionTime;

        // Display head weakpoint and rotate Serpent.
        headWeakpoint.gameObject.SetActive(true);
        while (timeElapsed < headProtrustionTime)
        {
            currentYPosition = Mathf.Lerp(initialYPosition, finalYPosition, timeElapsed / headProtrustionTime);
            headWeakpoint.localPosition = new Vector3(headWeakpoint.localPosition.x, currentYPosition, headWeakpoint.localPosition.z);

            currentXAngles = Mathf.Lerp(initialXAngles, finalXAngles, timeElapsed / headProtrustionTime);
            transform.localEulerAngles = new Vector3(currentXAngles, transform.localEulerAngles.y, transform.localEulerAngles.z);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        headWeakpoint.localPosition = new Vector3(headWeakpoint.localPosition.x, finalYPosition, headWeakpoint.localPosition.z);
        transform.localEulerAngles = new Vector3(finalXAngles, transform.localEulerAngles.y, transform.localEulerAngles.z);

        // Charge up head bash attack whilst checking if head weakpoint sustained too much damage.
        timeElapsed = 0;
        while (timeElapsed < bashChargeTime)
        {
            if (headBashDisrupted)
                timeElapsed = bashChargeTime;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        if (!headBashDisrupted)
        {
            headWeakpoint.GetComponent<Collider>().isTrigger = true;

            float initialZPosition = host.localPosition.z;
            float finalZPosition = host.localPosition.z - lungeDistance;
            float currentZPosition;

            for(int i = 0; i < totalLunges; i++)
            {
                isHeadBashing = false;
                timeElapsed = 0;
                while(timeElapsed < lungeTime)
                {
                    currentZPosition = Mathf.Lerp(initialZPosition, finalZPosition, timeElapsed / lungeTime);
                    host.localPosition = new Vector3(host.localPosition.x, host.localPosition.y, currentZPosition);

                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
                host.localPosition = new Vector3(host.localPosition.x, host.localPosition.y, finalZPosition);

                timeElapsed = 0;
                while(timeElapsed < retractTime)
                {
                    currentZPosition = Mathf.Lerp(finalZPosition, initialZPosition, timeElapsed / retractTime);
                    host.localPosition = new Vector3(host.localPosition.x, host.localPosition.y, currentZPosition);

                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
                host.localPosition = new Vector3(host.localPosition.x, host.localPosition.y, initialZPosition);

                isHeadBashing = true;
                yield return new WaitForSeconds(timeBetweenLunges);
            }   
        }

        isHeadBashing = false;
        Vector3 currentHostPosition = host.localPosition;

        // Return to normal pos
        timeElapsed = 0;
        while (timeElapsed < headRetractionTime)
        {
            currentYPosition = Mathf.Lerp(finalYPosition, initialYPosition, timeElapsed / headRetractionTime);
            headWeakpoint.localPosition = new Vector3(headWeakpoint.localPosition.x, currentYPosition, headWeakpoint.localPosition.z);

            currentXAngles = Mathf.Lerp(finalXAngles, initialXAngles, timeElapsed / headRetractionTime);
            transform.localEulerAngles = new Vector3(currentXAngles, transform.localEulerAngles.y, transform.localEulerAngles.z);

            host.localPosition = Vector3.Lerp(currentHostPosition, originalHostPosition, timeElapsed / headRetractionTime);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        headWeakpoint.localPosition = new Vector3(headWeakpoint.localPosition.x, initialYPosition, headWeakpoint.localPosition.z);
        transform.localEulerAngles = new Vector3(initialXAngles, transform.localEulerAngles.y, transform.localEulerAngles.z);
        host.localPosition = originalHostPosition;

        host.GetComponent<EnemyMovement>().enabled = true;
        headWeakpoint.GetComponent<Collider>().isTrigger = false;
        headWeakpoint.gameObject.SetActive(false);
        headBashDisrupted = false;
        isAttacking = false;
    }

    public void CancelHeadBash() => headBashDisrupted = true;

    private IEnumerator StartWaterCannon()
    {
        float timeElapsed = 0f;
        float initialZPosition = mouthWeakpoint.localPosition.z;
        float finalZPosition = mouthWeakpoint.localPosition.z + mouthWeakpointProtrusion;
        float currentZPosition;

        // Display mouth weakpoint.
        mouthWeakpoint.gameObject.SetActive(true);
        while (timeElapsed < mouthWeakpointProtrusion)
        {
            currentZPosition = Mathf.Lerp(initialZPosition, finalZPosition, timeElapsed / mouthProtrustionTime);
            mouthWeakpoint.localPosition = new Vector3(mouthWeakpoint.localPosition.x, mouthWeakpoint.localPosition.y, currentZPosition);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        mouthWeakpoint.localPosition = new Vector3(mouthWeakpoint.localPosition.x, mouthWeakpoint.localPosition.y, finalZPosition);

        
        // Call for mouth weakpoint to rotate towards Player.
        isCannoning = true;

        // Charge up water cannon attack whilst checking if mouth weakpoint sustained too much damage.
        timeElapsed = 0;
        while (timeElapsed < cannonChargeTime)
        {
            if (waterCannonDisrupted)
                timeElapsed = cannonChargeTime;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // If water cannon was not disrupted, then enable waterLaser.
        if (!waterCannonDisrupted)
        {
            mouthWeakpoint.GetComponent<Collider>().isTrigger = true;
            waterLaser.gameObject.SetActive(true);
            yield return new WaitForSeconds(laserDuration);
        }

        // Retract mouth weakpoint.
        waterLaser.gameObject.SetActive(false);
        timeElapsed = 0;
        while (timeElapsed < mouthRetractionTime)
        {
            currentZPosition = Mathf.Lerp(finalZPosition, initialZPosition, timeElapsed / mouthRetractionTime);
            mouthWeakpoint.localPosition = new Vector3(mouthWeakpoint.localPosition.x, mouthWeakpoint.localPosition.y, currentZPosition);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        mouthWeakpoint.localPosition = new Vector3(mouthWeakpoint.localPosition.x, mouthWeakpoint.localPosition.y, initialZPosition);

        mouthWeakpoint.GetComponent<Collider>().isTrigger = false;
        mouthWeakpoint.gameObject.SetActive(false);
        waterCannonDisrupted = false;
        isAttacking = false;
    }

    public void CancelWaterCannon() => waterCannonDisrupted = true;
}
