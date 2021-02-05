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

    void Start()
    {
        explosion = this.transform.GetComponent<ParticleSystem>();
        mesh = this.transform.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (isExploding)
        {
            PlayerInfo.singleton.TakeDamage(damage); //damage to Player
            Destroy(this.gameObject);
        }

        if (!isExploding && Vector3.Distance(PlayerInfo.singleton.transform.position, transform.position) <= distance)
        {
            StartCoroutine(Explode()); //start explode coroutine 
        }
    }

    //Play explosion particle system effect
    private IEnumerator Explode()
    {
        mesh.enabled = false;
        explosion.Play(); //start Particle System explosion effect
        yield return new WaitForSeconds(1.2f); //wait for explosion to finish before destroying Pufferbomb
        isExploding = true;
    }

    //If Player shoots Pufferbomb, destroy Pufferbomb
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            StartCoroutine(Explode()); //start explode coroutine
        }
    }
}
