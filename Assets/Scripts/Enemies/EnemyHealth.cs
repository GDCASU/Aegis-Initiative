using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    int Health;
    public int DamageRate;
    // Start is called before the first frame update
    void Start()
    {
        Health = 50;
        DamageRate = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            //Debug.Log(Health);
            DamageRate = other.gameObject.GetComponent<Bullet>().damage;
            takeDamage(DamageRate);            
        }
    }
    public void takeDamage(int damage)
    {
        Health -= damage;
    }
}
