using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMushroom : MonoBehaviour
{
    //TODO: GeneratePoisonCloud method - create poison clouds every x increments of time

    //TODO: DisableActiveAbilities method - disable player's active abilities when player flies through poision clouds

    //TODO: TakeDamage method - decrease Mushroom health

    //TODO: DestroyMushroom method - destroy mushroom when player collides or shoots it, then call generatePoisonCloud

    //Mushroom stats
    public int health; //Mushroom health
    private float lifeTimer;
    private float generateTimer;

    //Generating clouds using Particle System
    public ParticleSystem cloud;
    public float timeGenerate; //timer for generating the clouds

    //Other object stats
    public int damageNum; //amount of damage applied to Mushroom health

    void Start()
    {
        //health = 10;
        lifeTimer = 5; //
        generateTimer = timeGenerate;
    }

    void Update()
    {
        if (health <= 0)
        {
            DestroyMushroom();
        }

        if (generateTimer > 0)
        {
            generateTimer -= Time.deltaTime;
        }
        else if (generateTimer == 0)
        {
            cloud.Stop();
            cloud.Clear();
        }
        else if (generateTimer <= 0)
        {
            GeneratePoisonCloud();
            generateTimer = timeGenerate;
        }

        // if (dead)
        // {
            
        // }
    }

    public void GeneratePoisonCloud()
    {
        cloud.Simulate(0.0f, true, true); //reset particle system timer
        cloud.Play();
        Debug.Log("Generate clouds");
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    //If player collides with the Mushroom, then destroy this gameObject
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Collided with Player");
            DestroyMushroom();
        }
        else if (collision.gameObject.tag == "Bullet")
        {
            TakeDamage(2);
        }
    }

    //Destroy mushroom when player collides or shoots it, then call GeneratePoisonCloud
    public void DestroyMushroom()
    {
        GeneratePoisonCloud();
        Destroy(gameObject);
    }
}
