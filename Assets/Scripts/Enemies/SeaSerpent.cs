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
    public float waterBulletSpeed = 10f;
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
    private ShipMovement playerMovement;

    private void Start()
    {
        host = transform.parent;
        waterLaser = mouthWeakpoint.GetChild(0);
        player = PlayerInfo.singleton.transform;
        playerMovement = player.GetComponent<ShipMovement>();
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

                StartCoroutine(StartWaterBarrage());

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
            //transform.LookAt()
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
        Rigidbody dispensedBullet;
        Vector3 bulletVelocity;
        Vector3 bulletSpawnpointPositionInTrack;
        for (int i = 0; i < barrageBullets; i++)
        {
            bulletSpawnpointPositionInTrack = transform.parent.parent.InverseTransformPoint(waterBulletSpawnpoint.position);
            AimPrediction.PredictiveAim(bulletSpawnpointPositionInTrack, waterBulletSpeed, player.localPosition, playerMovement.currentVelocity , 0, out bulletVelocity);
            dispensedBullet = Instantiate(waterBullet, waterBulletSpawnpoint.position, waterBulletSpawnpoint.rotation, transform.parent.parent).GetComponent<Rigidbody>();
            dispensedBullet.velocity = bulletVelocity;

            yield return new WaitForSeconds(timeBetweenBarrageBullets);
        }
        isAttacking = false;
    }

    private IEnumerator StartHeadBash()
    {
        float timeElapsed = 0f;
        float initialZPosition = headWeakpoint.localPosition.z;
        float finalZPosition = headWeakpoint.localPosition.z + headWeakpointProtrusion;
        float currentZPosition;

        Vector3 playerDirection = player.position - headWeakpoint.position;
        float rotateSpeed = Vector3.Angle(headWeakpoint.position, playerDirection) / headProtrustionTime;

        // Display head weakpoint and rotate Serpent.
        while (timeElapsed < headProtrustionTime)
        {
            currentZPosition = Mathf.Lerp(initialZPosition, finalZPosition, timeElapsed / headProtrustionTime);
            headWeakpoint.localPosition = new Vector3(headWeakpoint.localPosition.x, headWeakpoint.localPosition.y, currentZPosition);

            playerDirection = player.position - headWeakpoint.position;
            Vector3 currentDirection = Vector3.RotateTowards(transform.up, playerDirection, rotateSpeed, 0f);
            transform.rotation = Quaternion.LookRotation(currentDirection);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        headWeakpoint.localPosition = new Vector3(headWeakpoint.localPosition.x, headWeakpoint.localPosition.y, finalZPosition);

        // Charge up head bash attack whilst checking if head weakpoint sustained too much damage.
        timeElapsed = 0;
        while (timeElapsed < bashChargeTime)
        {
            if (headBashDisrupted)
                yield break;

            timeElapsed += Time.deltaTime;
            yield return null;
        }


        if (!headBashDisrupted)
        {

        }

        while (timeElapsed < headRetractionTime)
        {
            currentZPosition = Mathf.Lerp(finalZPosition, initialZPosition, timeElapsed / headProtrustionTime);
            headWeakpoint.position = new Vector3(headWeakpoint.position.x, headWeakpoint.position.y, currentZPosition);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        headWeakpoint.position = new Vector3(headWeakpoint.position.x, headWeakpoint.position.y, initialZPosition);
        headBashDisrupted = false;
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
