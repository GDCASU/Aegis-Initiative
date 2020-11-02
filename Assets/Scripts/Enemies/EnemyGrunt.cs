using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrunt : Enemy
{
    public GameObject bulletPrefab;
    public Animation animations;
    public AnimationClip flying;
    public AnimationClip flyIn;
    public int bulletCount;
    private Vector3 start;
    private float animationWaitTime;
    private float shootingWaitTime;
    private float bulletSpeed = 0.25f;
    private bool isflying;
    private bool shoot = true;
    private bool flyOff = false;
    System.Random rng = new System.Random();

    private void Start()
    {
        animations.Play(flyIn.name);
        animationWaitTime = flyIn.length;
        shootingWaitTime = 5.0f;
    }
    private void FixedUpdate()
    {
        animationWaitTime -= Time.deltaTime;
        shootingWaitTime -= Time.deltaTime;

        if (animationWaitTime <= 0)
        {
            start = transform.localPosition;
            animations.Play(flying.name);
            isflying = true;
        }

        if (shootingWaitTime <= 0 && shoot)
        {
            Quaternion OriginalRot = transform.rotation;
            transform.LookAt(gameObject.transform.parent.GetChild(1).transform.position);
            Quaternion NewRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, 5f * Time.deltaTime);
            if (Mathf.Abs(1 - Quaternion.Dot(transform.rotation, NewRot)) < 0.01f )
            {
                shoot = false;
                StartCoroutine("Shoot");
            }
        }

        if (flyOff)
        {
            // TODO: some animation here instead of MoveTowards
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 0, -1), 5f*Time.deltaTime);
            
            if (!transform.GetChild(0).GetComponent<Renderer>().isVisible)
                Destroy(gameObject);
        }
    }

    IEnumerator Shoot()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation, transform);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 200f);
            yield return new WaitForSeconds(bulletSpeed);
        }
        flyOff = true;
    }
}
