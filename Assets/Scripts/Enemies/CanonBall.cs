using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBall : ProjectileAttack
{
    public float time;
    public float spread;
    public float locationMod;
    Vector3 startingPos;
    // Give value to variables
    void Start()
    {
        startingPos = new Vector3();
    }

    // Call shooting methods
    void Update()
    {
        ShootRockAtPlayer(time, spread, locationMod, startingPos);
        ShootRocksAroundPlayer(time, spread, locationMod, startingPos);
    }
}
