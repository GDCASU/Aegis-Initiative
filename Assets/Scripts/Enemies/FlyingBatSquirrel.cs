using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBatSquirrel : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Speed at which Squirrel shall approach the Player.")]
    private float approachSpeed = 1f;

    [SerializeField]
    [Tooltip("Intensity at which Squirrel shall rotate towards Player.")]
    private float followIntensity = 0.3f;

    [SerializeField]
    [Tooltip("Speed at which Squirrel shall spin around its host.")]
    private float spinSpeed = 700f;

    private Transform host, player;

    Vector3 targetDirection, newDirection;

    private void Start()
    {
        host = transform.parent;
        player = PlayerInfo.singleton.transform;
    }

    private void Update()
    {
        transform.RotateAround(host.position, host.up, spinSpeed * Time.deltaTime);
        host.eulerAngles = RotateToObject(player);
        host.Translate(Vector3.forward * approachSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("Player"))
        {
            int damage = transform.GetComponent<EnemyHealth>().collisionDamage;
            player.GetComponent<PlayerInfo>().TakeDamage(damage);
            transform.GetComponent<EnemyHealth>().DestroyEnemy();
        }
    }

    /// <summary>
    /// Returns a 3D Vector (Worldspace) of euler angles for the Squirrel host rotating towards a target on the next frame.
    /// </summary>
    /// <param name="target"> transform Shark should rotate to </param>
    /// <returns> 3D Vector of euler angles for next frame </returns>
    private Vector3 RotateToObject(Transform target)
    {
        targetDirection = target.position - host.position;
        newDirection = Vector3.RotateTowards(host.forward, targetDirection, followIntensity * Time.deltaTime, 0);
        return Quaternion.LookRotation(newDirection).eulerAngles;
    }
}
