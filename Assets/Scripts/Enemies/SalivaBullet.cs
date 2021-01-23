using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BULLET FOR SPACE FLY (ADULT) !!!!!!
/// </summary>

public class SalivaBullet : Bullet
{
    [SerializeField]
    private float bulletSpeed = 0.07f;

    private bool reachedDest = false;
    Vector3 targetPos;

    void Start()
    {

        //face player
        Vector3 dir = transform.TransformDirection(targetPos - transform.localPosition);
        transform.rotation = Quaternion.LookRotation(dir);

        timer = bulletDespawnTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) Destroy(transform.gameObject);

        //move toward grabbed player location
        transform.localPosition += transform.forward * bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player")) //hit player
        {
            collision.gameObject.GetComponent<PlayerInfo>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    //set target position for this burst
    public void SetTarget(Vector3 target)
    {
        targetPos = target;
    }

}
