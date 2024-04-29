using UnityEngine;

public class BossBodyPart : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public GameObject visualLocation;
    public float offsetDistance = 33.857f;
    public void ApplyDamage(int damage) => enemyHealth.TakeDamage(damage);

    private void Update()
    {
        if (!enemyHealth.calledResetEnemy)
        {
            Vector3 calculatedLocation = transform.position + transform.right * -offsetDistance;
            visualLocation.transform.position = calculatedLocation;
            Vector3 enemyConversion = Camera.main.WorldToViewportPoint(visualLocation.transform.position);
            if (enemyConversion.z < 0)
            {
                PlayerInfo.singleton.GetComponent<BasicPlayerShooting>().ResetEnemy(gameObject);
            }
            else
            {
                if (PlayerInfo.singleton != null) PlayerInfo.singleton.GetComponent<BasicPlayerShooting>().AimAssist(visualLocation);
            }
        }
    }
}
