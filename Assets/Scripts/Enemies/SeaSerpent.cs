using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSerpent : MonoBehaviour
{
    private enum AttackPattern
    {
        WaterBarrage,
        HeadBash,
        WaterCannon
    }
    
    [Header("Basics")]
    public float beginAttackingTime = 2f;
    public float attackCooldown = 2f;
    public float waterBarrageFrequency = 0.5f;
    public float headBashFrequency = 0.35f;
    public float waterCannonFrequency = 0.15f;

    [Header("Water Barrage")]
    public Transform waterBulletSpawnpoint;
    public GameObject waterBullet;
    public int barrageBullets = 5;
    public float waterBulletSpeed = 8f;
    public int bulletDamage = 10;
    public float timeBetweenBarrageBullets = 0.5f;

    [Header("Head Bash")]
    public Transform headWeakpoint;
    public float headWeakpointProtrusion = 0.1f;
    public float headProtrustionTime = 1f;
    public float headRetractionTime = 0.5f;
    public float bashChargeTime = 6f;
    public int bashCancelDamage = 100;
    public int lunges = 3;
    public int lungeDamage = 20;
    public float lungeDistance = 2f;
    public float followSpeed = 1f;
    public float lungeTime = 0.3f;
    public float retractTime = 0.6f;
    public float timeBetweenLunges = 0.5f;

    [Header("Water Cannon")]
    public Transform mouthWeakpoint;
    public float mouthWeakpointProtrusion = 0.3f;
    public float mouthProtrustionTime = 1f;
    public float mouthRetractionTime = 0.5f;
    public Transform waterLaser;
    public float cannonChargeTime = 8f;
    public int cannonCancelDamage = 100;
    public float laserDuration = 10f;
    public float laserFollowSpeed = 0.5f;

    private AttackPattern currentPattern;
    private Transform host;

    private bool isReadyForBattle = false;
    private bool isAttacking = false;
    private bool isHeadBashing = false;
    private bool isCannoning = false;
    private float randomNum;
    private Dictionary<AttackPattern, float> attackFrequencyList = new Dictionary<AttackPattern, float>();
    private float timeSinceAttack;

    public bool headBashDisrupted = false;
    public bool waterCannonDisrupted = false;

    private Transform player;
    private Transform dollyCart;
    private ShipMovement playerMovement;

    Vector3 originalPosition;
    private Vector2 hostPosition_XY;
    private Vector2 playerPosition_XY;
    private Vector3 newHostPosition_XY;

    private void Start()
    {
        host = transform.parent;
        waterLaser = mouthWeakpoint.GetChild(0);
        player = PlayerInfo.singleton.transform;
        dollyCart = player.parent;
        playerMovement = player.GetComponent<ShipMovement>();
        originalPosition = host.localPosition;
        StartCoroutine(WaitToBeginBattle());
    }

    private IEnumerator WaitToBeginBattle()
    {
        yield return new WaitForSeconds(beginAttackingTime);
        isReadyForBattle = true;
    }

    private void Update()
    {
        if (isReadyForBattle && !isAttacking)
        {
            timeSinceAttack += Time.deltaTime;

            if (timeSinceAttack >= attackCooldown)
            {
                isAttacking = true;
                timeSinceAttack = 0;

                StartCoroutine(StartHeadBash());

                /*
                randomNum = Random.value;

                if (randomNum <= waterCannonFrequency)
                {
                    StartCoroutine(StartWaterCannon());
                }
                else if(randomNum <= waterCannonFrequency + headBashFrequency)
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

    private void FixedUpdate()
    {
        if (isHeadBashing)
        {
            hostPosition_XY = new Vector2(host.localPosition.x, host.localPosition.y);
            playerPosition_XY = new Vector2(player.localPosition.x, player.localPosition.y);
            newHostPosition_XY = Vector2.MoveTowards(hostPosition_XY, playerPosition_XY, followSpeed * Time.deltaTime);
            host.localPosition = new Vector3(newHostPosition_XY.x, newHostPosition_XY.y, host.localPosition.z);
        }
        else if(isCannoning)
        {
            Vector3 playerDirection = player.position - mouthWeakpoint.position;
            Vector3 currentDirection = Vector3.RotateTowards(mouthWeakpoint.forward, playerDirection, laserFollowSpeed * Time.deltaTime, 0f);
            mouthWeakpoint.rotation = Quaternion.LookRotation(currentDirection);
        }
        else if(isHeadBashing)
        {
            //transform.localPosition = new Vector3()
        }
    }

    private IEnumerator StartWaterBarrage()
    {
        Vector3 initialBulletPosition, initialPlayerPosition, playerVelocity, futurePlayerPosition, bulletVelocity, positionInCamera, bulletToFuturePlayerPosition, spread;
        Quaternion bulletRotation;
		Rigidbody dispensedBullet;

        for (int i = 0; i < barrageBullets; i++)
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

            // Introduce spread. [NOT FUNCTIONAL]
            //spread = Random.insideUnitSphere * maxSpreadDistance;
            bulletRotation = Quaternion.LookRotation(bulletVelocity, Vector3.up);
            //bulletRotation = Quaternion.Euler(bulletRotation.eulerAngles += spread);

            // Launch the bullet.
            dispensedBullet = Instantiate(waterBullet, waterBulletSpawnpoint.position, bulletRotation, dollyCart).GetComponent<Rigidbody>();
            dispensedBullet.velocity = dispensedBullet.transform.forward * waterBulletSpeed;

            yield return new WaitForSeconds(timeBetweenBarrageBullets);
        }

        isAttacking = false;
    }

    private IEnumerator StartHeadBash()
    {
        isHeadBashing = true;
        

        float timeElapsed = 0f;
        float initialYPosition = headWeakpoint.localPosition.y;
        float finalYPosition = headWeakpoint.localPosition.y + headWeakpointProtrusion;
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
            host.GetComponent<EnemyMovement>().enabled = false;
            headWeakpoint.GetComponent<Collider>().isTrigger = true;

            float initialZPosition = host.localPosition.z;
            float finalZPosition = host.localPosition.z - lungeDistance;
            float currentZPosition;

            for(int i = 0; i < lunges; i++)
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

        Vector3 currentHostPosition = host.localPosition;

        // Return to normal pos
        timeElapsed = 0;
        while (timeElapsed < headRetractionTime)
        {
            currentYPosition = Mathf.Lerp(finalYPosition, initialYPosition, timeElapsed / headRetractionTime);
            headWeakpoint.localPosition = new Vector3(headWeakpoint.localPosition.x, currentYPosition, headWeakpoint.localPosition.z);

            currentXAngles = Mathf.Lerp(finalXAngles, initialXAngles, timeElapsed / headRetractionTime);
            transform.localEulerAngles = new Vector3(currentXAngles, transform.localEulerAngles.y, transform.localEulerAngles.z);

            host.localPosition = Vector3.Lerp(currentHostPosition, originalPosition, timeElapsed / headRetractionTime);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        headWeakpoint.localPosition = new Vector3(headWeakpoint.localPosition.x, initialYPosition, headWeakpoint.localPosition.z);
        transform.localEulerAngles = new Vector3(initialXAngles, transform.localEulerAngles.y, transform.localEulerAngles.z);
        host.localPosition = originalPosition;

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
