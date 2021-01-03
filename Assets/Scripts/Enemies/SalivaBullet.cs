using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BULLET FOR SPACE FLY (ADULT)
public class SalivaBullet : Bullet
{
    [SerializeField]
    private float bulletSpeed = 0.10f;

    private bool reachedDest = false;
    Vector3 targetPos;

    void Start()
    {
        Vector3 dir = transform.TransformDirection(targetPos - transform.localPosition);
        transform.rotation = Quaternion.LookRotation(dir);
        timer = bulletDespawnTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) Destroy(transform.gameObject);

        
        if (Vector3.Distance(targetPos, transform.localPosition) < 0.01f) //reached target
        {
            //transform.SetParent(null); //remove from dolly
            reachedDest = true; //don't allow it to continue using "MoveTowards"
        }
        else
        {
            transform.localPosition += transform.forward * bulletSpeed;

            /*if(!reachedDest) //hasn't reached set targetPos yet
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, bulletSpeed);*/
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player")) //hit player
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void SetTarget(Vector3 target) //set target position for this burst
    {
        targetPos = target;
    }

}
