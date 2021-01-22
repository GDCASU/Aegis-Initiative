using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletSource 
{
    Player,
    Enemy
};
public class Bullet : MonoBehaviour
{
    public float bulletDespawnTime;
    public float timer;
    public int damage;
    public BulletSource bulletSource;

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
        if (bulletSource == BulletSource.Player)
        {
            if (collision.gameObject.tag == "Enemy") collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            if (collision.gameObject.tag == "BreakableEnvironment") collision.gameObject.GetComponent<EnvironmentHealth>().TakeDamage(damage);
        }
        //else
        //{
        //    if (collision.gameObject.tag == "Player") collision.gameObject.GetComponent<PlayerInfo>().TakeDamage(damage);
        //}
        Destroy(transform.gameObject);
    }
}
