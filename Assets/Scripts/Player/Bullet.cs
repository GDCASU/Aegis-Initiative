/*
 * Revision Author: Cristion Dominguez
 * Revision Date: 22 Jan. 2021
 * Modification: Class obtains bullet damage from PlayerInfo script upon prefab creation.
 * 
 * Revision Author: Cristion Dominguez
 * Revision Date: 15 March 2021
 * Modification: Bullet ignores collisions with gameobjects from the PassThrough layer.
 */

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
        if (bulletSource == BulletSource.Player) damage = PlayerInfo.singleton.bulletDamage;

        Physics.IgnoreLayerCollision(9, 12);
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
        else
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerInfo>().TakeDamage(damage);
                collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        Destroy(transform.gameObject);
    }
}
