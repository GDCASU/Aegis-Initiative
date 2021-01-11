using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletDespawnTime;
    public float timer;
    public int damage;

    private void Start()
    {
        timer = bulletDespawnTime;
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) Destroy(transform.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy") collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        if (collision.gameObject.tag == "BreakableEnvironment") collision.gameObject.GetComponent<EnvironmentHealth>().TakeDamage(damage);
    }
}
