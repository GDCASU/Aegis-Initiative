using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSerpent : MonoBehaviour
{
    private enum AttackPattern
    {
        WaterBarrage,
        HeadBash,
        WaterCannon
    }
    
    private AttackPattern currentPattern;
    private float beginAttackingTime = 2f;

    private float recoveryPatternTime = 3f;
    private int barrageBullets = 5;
    private int bulletDamage = 10;
    private float timeBetweenBarrageBullets = 0.5f;

    private float bashChargeTime = 6f;
    private int bashCancelDamage = 100;
    private int lunges = 3;
    private int lungeDamage = 20;

    private float cannonChargeTime = 8f;
    private int cannonCancelDamage = 100;
    private float laserDuration = 10f;
    private float laserFollowSpeed = 0.5f;

    private IEnumerator StartWaterBarrage()
    {
        yield return null;
    }

    private IEnumerator StartHeadBash()
    {
        yield return null;
    }

    private IEnumerator StartWaterCannon()
    {
        yield return null;
    }
}
