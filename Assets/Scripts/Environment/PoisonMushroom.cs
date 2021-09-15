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
    public bool once;

    void Start()
    {
        collider = GetComponent<BoxCollider>();
        mesh = GetComponent<MeshRenderer>();
        cloud = GetComponent<ParticleSystem>();
        StartCoroutine(PlayParticle());
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
    public override void OnPlayerCollision()
    {
        base.OnPlayerCollision();

        //Debug.Log("Deactivate Player's Active Abilities");
    }
    IEnumerator PlayParticle()
    {
        yield return new WaitForSeconds(Random.Range(0, 5));
        cloud.Play();
    }
    //Destroy mushroom when player collides or shoots it, then call GeneratePoisonCloud
    public void DestroyMushroom()
    {
        collider.enabled = false;
        mesh.enabled = false;
    }
}
