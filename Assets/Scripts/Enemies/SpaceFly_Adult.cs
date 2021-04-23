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

    public Transform bulletSpawn; 

    public Animator animator;

    public float framesOfAnimation;

    private bool shooting;


    public override void Start()
    {
        base.Start();
        shootTimer = Random.Range(shootTimerMin, shootTimerMax); //timer within a range
        realRateOfFire = new WaitForSeconds(1.0f/rateOfFire); //for Shoot();
    }

    private void FixedUpdate()
    {
        if(shootTimer <= 0)
        {
            playerPos = PlayerInfo.singleton.transform.localPosition;           
            StartCoroutine(Shoot());
        }
        else
        {
            shootTimer -= Time.fixedDeltaTime;
        }
    }

    IEnumerator Shoot()
    {
        //while(shots < numBullets) //shoot set number of bullets
        //{
        //    GameObject temp = Instantiate(bullet, transform.position, Quaternion.identity, GameObject.Find("PlayerDollyCart").transform);
        //    temp.GetComponent<SalivaBullet>().SetTarget(playerPos);
        //    shots++;

        //    yield return realRateOfFire;
        //}

        shooting = true;
        animator.SetBool("Shooting", shooting);
        yield return new WaitForSeconds(framesOfAnimation / (24f * 2f));
        shooting = false;
        animator.SetBool("Shooting", shooting);
        shootTimer = Random.Range(shootTimerMin, shootTimerMax);
    }

    public void SpawnBullet()
    {
        GameObject temp = Instantiate(bullet, bulletSpawn.position, Quaternion.identity, PlayerInfo.singleton.transform.parent);
        temp.GetComponent<SalivaBullet>().SetTarget(playerPos);

    }

}
