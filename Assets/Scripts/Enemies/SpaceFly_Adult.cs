using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceFly_Adult : EnemyHealth
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float shootTimerMin = 6.0f; //min time until next shot
    [SerializeField]
    private float shootTimerMax = 14.0f; //max time until next shot
    private float shootTimer;

    Vector3 playerPos;

    public Transform bulletSpawn; 

    public Animator animator;

    public float framesOfAnimation;

    private bool shooting;


    public override void Start()
    {
        base.Start();
        shootTimer = Random.Range(shootTimerMin, shootTimerMax); //timer within a range
    }

    private void FixedUpdate()
    {
        if (GetComponentInParent<EnemyMovement>().isFlyingAway)
        {
            animator.SetBool("Shooting", false);
        }
        else 
        {
            if (shootTimer <= 0)
            {
                if (PlayerInfo.singleton != null) playerPos = PlayerInfo.singleton.transform.localPosition;
                StartCoroutine(Shoot());
            }
            else
            {
                shootTimer -= Time.fixedDeltaTime;
            }
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
        if (PlayerInfo.singleton != null)
        {
            GameObject temp = Instantiate(bullet, bulletSpawn.position, Quaternion.identity, PlayerInfo.singleton.transform.parent);
            temp.GetComponent<SalivaBullet>().SetTarget(playerPos);
        }
    }

}
