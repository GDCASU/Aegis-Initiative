using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBodyPart : MonoBehaviour
{
    public EnemyHealth enemyHealth;

    public void ApplyDamage(int damage) => enemyHealth.TakeDamage(damage);

    private void Update()
    {
        PlayerInfo.singleton.GetComponent<BasicPlayerShooting>().AimAssist(gameObject);
    }

}
