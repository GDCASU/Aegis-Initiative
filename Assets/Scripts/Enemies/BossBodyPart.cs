using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBodyPart : MonoBehaviour
{
    public EnemyHealth enemyHealth;

    public void ApplyDamage(int damage) => enemyHealth.TakeDamage(damage);

    private void Update()
    {
        if (!enemyHealth.calledResetEnemy)
        {
            Vector3 enemyConversion = Camera.main.WorldToViewportPoint(transform.position);
            if (enemyConversion.z < 0)
            {
                PlayerInfo.singleton.GetComponent<BasicPlayerShooting>().ResetEnemy(gameObject);
                enemyHealth.calledResetEnemy = true;
            }
            else
            {
                if (PlayerInfo.singleton != null) PlayerInfo.singleton.GetComponent<BasicPlayerShooting>().AimAssist(gameObject);
            }
        }
    }

}
