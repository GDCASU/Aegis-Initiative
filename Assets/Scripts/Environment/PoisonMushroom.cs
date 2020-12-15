using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMushroom : EnvironmentHealth
{
    //Mushroom stats
    public float lifeTimer; //timer for destroying the Mushroom
    private float generateTimer; //timer for generating the clouds
    private BoxCollider collider;
    private MeshRenderer mesh;

    //Generating clouds using Particle System
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
        else if (generateTimer < 0)
        {
            GeneratePoisonCloud();
            generateTimer = timeGenerate;
        }

        if (lifeTimer < 0)
        {
            Destroy(gameObject);
        }
    }

    public void GeneratePoisonCloud()
    {
        cloud.Simulate(0.0f, true, true); //reset particle system timer
        cloud.Play();
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        //Start death sequence
        if (health <= 0)
        {
            lifeTimer -= Time.deltaTime;
            DestroyMushroom();
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
        collider.enabled = false;
        mesh.enabled = false;
    }
}
