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
    private float shootTimerMin = 2.0f;
    [SerializeField]
    private float shootTimerMax = 6.0f;
    private float shootTimer;
    [SerializeField]
    private float rateOfFire = 10f;

    Vector3 playerPos;
    private WaitForSeconds rof;

    private void Start()
    {
        shootTimer = Random.Range(shootTimerMin, shootTimerMax);
        rof = new WaitForSeconds(1/rateOfFire);
    }

    private void FixedUpdate()
    {
        if(shootTimer <= 0)
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
            StartCoroutine(Shoot());
            shootTimer = Random.Range(shootTimerMin, shootTimerMax);
        }
        else
        {
            shootTimer -= Time.deltaTime;
        }
    }

    IEnumerator Shoot()
    {
        for(int i = 0; i < numBullets; i++)
        {
            GameObject temp = Instantiate(bullet, transform.position, Quaternion.identity, GameObject.Find("PlayerDollyCart").transform);
            temp.GetComponent<SalivaBullet>().SetTarget(playerPos);

            yield return rof;
        }

        yield return null;
    }

}
