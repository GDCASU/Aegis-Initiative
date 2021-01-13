using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceFly_Adult : EnemyHealth
{
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private int numBullets = 3;
    [SerializeField]
    private float shootTimerMin = 6.0f; //min time until next shot
    [SerializeField]
    private float shootTimerMax = 14.0f; //max time until next shot
    private float shootTimer;
    [SerializeField]
    private float rateOfFire = 10f; //RoF for designer

    Vector3 playerPos;
    private WaitForSeconds realRateOfFire; //for Shoot();

    private int shots;

    private void Start()
    {
        shootTimer = Random.Range(shootTimerMin, shootTimerMax); //timer within a range
        realRateOfFire = new WaitForSeconds(1.0f/rateOfFire); //for Shoot();

        shots = 0;
    }

    private void FixedUpdate()
    {
        if(shootTimer <= 0)
        {
            playerPos = PlayerHealth.singleton.transform.localPosition;
            shootTimer = Random.Range(shootTimerMin, shootTimerMax);
            shots = 0;
            StartCoroutine(Shoot());
        }
        else
        {
            shootTimer -= Time.deltaTime;
        }
    }

    IEnumerator Shoot()
    {
        while(shots < numBullets) //shoot set number of bullets
        {
            GameObject temp = Instantiate(bullet, transform.position, Quaternion.identity, GameObject.Find("PlayerDollyCart").transform);
            temp.GetComponent<SalivaBullet>().SetTarget(playerPos);
            shots++;

            yield return realRateOfFire;
        }

        yield return null;
    }

}
