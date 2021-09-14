using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugRock : EnemyHealth
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") PlayerInfo.singleton.TakeDamage(collisionDamage);
    }

    override protected void Update()
    {
        
    }
}
