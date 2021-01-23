/*
 * Revision Author: Cristion Dominguez
 * Revision Date: 22 Jan. 2021
 * 
 * Modification: Class obtains bullet damage from PlayerInfo script upon prefab creation.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletDespawnTime;
    public float timer;

    [System.NonSerialized]
    public int damage;

    private void Start()
    {
        timer = bulletDespawnTime;
        damage = PlayerInfo.singleton.bulletDamage;
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
