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
    private float shootTimerMin = 6.0f;
    [SerializeField]
    private float shootTimerMax = 14.0f;
    private float shootTimer;
    [SerializeField]
    private float rateOfFire = 10f;

    Vector3 playerPos;
    private WaitForSeconds rof;

    private int shots;

    private void Start()
    {
        shootTimer = Random.Range(shootTimerMin, shootTimerMax); //timer within a range
        rof = new WaitForSeconds(1.0f/rateOfFire); //for Shoot();

        shots = 0;
    }

    private void FixedUpdate()
    {
        if(shootTimer <= 0)
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
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

            yield return rof;
        }

        yield return null;
    }

}
