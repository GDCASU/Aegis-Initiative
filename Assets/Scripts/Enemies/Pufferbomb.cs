using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pufferbomb : MonoBehaviour
{
    public int damage; //damage dealt to Player
    public float distance; //distance between Pufferbomb and Player for Pufferbomb to explode

    private ParticleSystem explosion;
    private MeshRenderer mesh;
    private bool isExploding; //check that Pufferbomb is already exploding
    private bool isShot; //check if Player already shot Pufferbomb

    void Start()
    {
        explosion = transform.GetComponent<ParticleSystem>();
        mesh = transform.GetComponent<MeshRenderer>();

        isExploding = false;
        isShot = false;
    }

    void Update()
    {
        if (!isExploding && !isShot && Vector3.Distance(PlayerInfo.singleton.transform.position, transform.position) <= distance)
        {
            StartCoroutine(Explode()); //start explode coroutine 
            PlayerInfo.singleton.TakeDamage(damage); //damage Player
        }
    }

    //Play explosion particle system effect
    private IEnumerator Explode()
    {
        isExploding = true;
        mesh.enabled = false;
        explosion.Play(); //start Particle System explosion effect
        yield return new WaitForSeconds(1.2f); //wait for explosion to finish before destroying Pufferbomb
        Destroy(gameObject);
    }

    //If Player shoots Pufferbomb, destroy Pufferbomb
    private void OnCollisionEnter(Collision collision)
    {
        if (!isExploding && collision.gameObject.CompareTag("Bullet"))
        {
            isShot = true;
            StartCoroutine(Explode()); //start explode coroutine  
        }
    }
}
