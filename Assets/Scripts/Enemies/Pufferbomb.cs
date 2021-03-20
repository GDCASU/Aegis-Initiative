using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pufferbomb : MonoBehaviour
{
    public int damage; //damage dealt to Player
    public float distance; //distance between Pufferbomb and Player for Pufferbomb to explode
    public float explosionTime; //time it takes for explosion sequence to finish before destroying the gameObject

    private ParticleSystem explosion;
    private MeshRenderer mesh;
    private bool isExploding; //check that Pufferbomb is already exploding
    private bool isShot; //check if Player already shot Pufferbomb

    void Start()
    {
        explosion = transform.GetComponent<ParticleSystem>();
        mesh = transform.GetComponent<MeshRenderer>();
        //Set distance value to the sphere collider that has trigger enabled
        foreach (SphereCollider collider in transform.GetComponents<SphereCollider>())
        {
            if (collider.isTrigger)
            {
                collider.radius = distance;
            }
        }

        isExploding = false;
        isShot = false;
    }

    //Play explosion particle system effect
    private IEnumerator Explode()
    {
        isExploding = true;
        mesh.enabled = false;
        explosion.Play(); //start Particle System explosion effect
        yield return new WaitForSeconds(explosionTime); //wait for explosion to finish before destroying Pufferbomb
        Destroy(gameObject);
    }

    //If Player shoots Pufferbomb, destroy Pufferbomb
    private void OnCollisionEnter(Collision collision)
    {
        if (mesh != null && explosion != null)
        {
            if (!isExploding && collision.gameObject.CompareTag("Bullet"))
            {
                isShot = true;
                StartCoroutine(Explode()); //start explode coroutine  
            }
        }
    }

    //If Player collides with sphere collider trigger, then explode Pufferbomb and deal damage to Player
    private void OnTriggerEnter(Collider other)
    {
        if (!isExploding && !isShot && other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Explode()); //start explode coroutine 
            PlayerInfo.singleton.TakeDamage(damage); //damage Player
        }
    }
}
