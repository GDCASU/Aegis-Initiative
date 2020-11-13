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
    public int damageNum; //amount of damage applied to Mushroom health
    public float lifeTimer; //timer for destroying the Mushroom
    private float generateTimer; //timer for generating the clouds
    private BoxCollider collider;
    private MeshRenderer mesh;

    //Generating clouds using Particle System
    //public ParticleSystem cloud;
    private ParticleSystem cloud;
    public float timeGenerate; //duration for generating the clouds

    void Start()
    {
        collider = GetComponent<BoxCollider>();
        mesh = GetComponent<MeshRenderer>();
        cloud = GetComponent<ParticleSystem>();
        generateTimer = timeGenerate;
    }

    void Update()
    {
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

        //Start death sequence
        if (health <= 0)
        {
            lifeTimer -= Time.deltaTime;
            DestroyMushroom();
        }
        if (lifeTimer < 0)
        {
            Debug.Log("Destroy Mushroom object");
            Destroy(gameObject);
        }
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
            health = 0;
            //DestroyMushroom();
        }
        else if (collision.gameObject.tag == "Bullet")
        {
            TakeDamage(2);
        }
    }

    //If player collides with Particle System cloud
    void OnParticleCollision(GameObject other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Deactivate Player's Active Abilities");
        }
    }

    //Destroy mushroom when player collides or shoots it, then call GeneratePoisonCloud
    public void DestroyMushroom()
    {
        GeneratePoisonCloud();
        collider.enabled = false;
        mesh.enabled = false;
    }
}
