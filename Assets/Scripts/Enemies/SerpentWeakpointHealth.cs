using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerpentWeakpointHealth : EnemyHealth
{
    private enum WeakpointType
    {
        Head,
        Mouth
    }

    [SerializeField]
    private WeakpointType weakpoint;

    private int maxHealth;
    private SeaSerpent serpent;
    private EnemyHealth serpentHealth;

    private void Awake()
    {
        serpent = transform.GetComponentInParent<SeaSerpent>();
        serpentHealth = transform.GetComponentInParent<EnemyHealth>();
        if (weakpoint == WeakpointType.Head)
            maxHealth = serpent.BashCancelDamage;
        else if (weakpoint == WeakpointType.Mouth)
            maxHealth = serpent.CannonCancelDamage;
    }

    private void OnEnable()
    {
        health = maxHealth;
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        serpentHealth.TakeDamage(damage);

        if(health <= 0)
        {
            if (weakpoint == WeakpointType.Head)
                serpent.CancelHeadBash();
            else
                serpent.CancelWaterCannon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (weakpoint == WeakpointType.Head && other.tag.Equals("Player"))
        {
            other.GetComponent<PlayerInfo>().TakeDamage(serpent.LungeDamage);
        }
    }
}
