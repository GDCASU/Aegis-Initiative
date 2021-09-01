/*
 * Revision Author: Cristion Dominguez
 * Revision Date: 22 Jan. 2021
 * 
 * Modification: Class obtains bullet damage from PlayerInfo script upon prefab creation.
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

    private bool lockedOn;
    private GameObject _target;

    private void Start()
    {
        timer = bulletDespawnTime;
        if (bulletSource == BulletSource.Player) damage = PlayerInfo.singleton.bulletDamage;
    }

    /*private void OnEnable()
    {
        Time.timeScale = 0.1f;
    }*/

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) Destroy(transform.gameObject);

    }

    private void FixedUpdate()
    {

        if (_target != null)
        {
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.magnitude * (_target.transform.position - gameObject.transform.position).normalized;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (bulletSource == BulletSource.Player)
        {

            if (collision.gameObject.tag == "Enemy") collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            else if (collision.gameObject.tag == "BossBodyPart") collision.gameObject.GetComponent<BossBodyPart>().ApplyDamage(damage);
            else if (collision.gameObject.tag == "BreakableEnvironment") collision.gameObject.GetComponent<EnvironmentHealth>().TakeDamage(damage);
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

    public void LockOn(GameObject target)
    {        
        if(target != null)
        {
            lockedOn = true;
            _target = target;
        }
        
    }
}
