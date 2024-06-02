using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrunt : EnemyHealth
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
    public bool flyOff = false;
    private string Hit = "event:/SFX/Combat/Hit";
    public bool overrideBulletSize;

    System.Random rng = new System.Random();

    public override void Start()
    {
        base.Start();
        animations.Play(flyIn.name);
        animationWaitTime = flyIn.length;
        shootingWaitTime = 5.0f;
        FMODUnity.RuntimeManager.LoadBank("Combat");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            SoundManager.singleton.PlayOneShot(Hit, transform.position, SoundManager.VolumeType.sfx);
        }
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
            if (PlayerInfo.singleton!=null) transform.parent.LookAt(PlayerInfo.singleton.transform.GetChild(0).transform.position);
            Quaternion NewRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, 5f * Time.deltaTime);
            //Debug.Log(1 - Mathf.Abs(Quaternion.Dot(transform.rotation, NewRot)));
            if (1 - Mathf.Abs(Quaternion.Dot(transform.rotation, NewRot)) < 0.0001f)
            {
                shoot = false;
                StartCoroutine("Shoot");
            }
        }

        if (flyOff)     
        {
            // TODO: some animation here instead of MoveTowards
            if (overrideBulletSize)
            {
                transform.localPosition -= transform.parent.forward * .05f;
                animations.Stop();
            }
            else transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 0, -1), 5f * Time.deltaTime);

            if (!GetComponentInChildren<Renderer>().isVisible)
                DestroyEnemy();
        }
    }
    IEnumerator Shoot()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.parent.forward * (overrideBulletSize ? .5f : 0) , transform.rotation, transform);
            if (overrideBulletSize) bullet.transform.localScale = Vector3.one * .05f;
            bullet.GetComponent<Rigidbody>().AddForce( (overrideBulletSize ? transform.parent.forward : transform.forward) * 200f);
            yield return new WaitForSeconds(bulletSpeed);
        }
        flyOff = true;
    }
}