using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using Collision = Fungus.Collision;

public class EnemyGrunt : Enemy
{
    public Animation animations;
    public AnimationClip flying;
    public AnimationClip flyIn;
    public float health;
    private Vector3 start;
    private float waitTime;
    private bool isflying;
    System.Random rng = new System.Random();

    private void Start()
    {
        animations.Play(flyIn.name);
        waitTime = flyIn.length;
        health = 50;
    }
    private void FixedUpdate()
    {
        waitTime -= Time.deltaTime;
        if (waitTime <= 0)
        {
            start = transform.localPosition;
            animations.Play(flying.name);
            isflying = true;
        }
    }
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health -= 2;
        }
    }
    private void Update()
    {
        if(health <= 0) Destroy(gameObject);
        if(isflying)transform.localPosition = Vector3.Lerp(start,start+new Vector3(-25f,0,0),1);
    }
}
