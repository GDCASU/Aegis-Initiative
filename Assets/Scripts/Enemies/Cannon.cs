using System.Collections;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : EnemyHealth
{
    public float fireRate;
    private float timer;
    private float numSelected;
    public int precision;
    public GameObject projectilePrefab;
    private System.Random rng;
    CinemachineDollyCart cart;

    public override void Start() 
    {
        base.Start();
        rng = new System.Random();
        timer = fireRate;
        cart = PlayerInfo.singleton.GetComponentInParent<CinemachineDollyCart>();
    }

    // Give value to variables

    // Call shooting methods
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = fireRate;
            numSelected = rng.Next(100);
            if (precision < numSelected) ProjectileAttack.ShootProjectileAroundPlayer(cart, (CinemachineSmoothPath)cart.m_Path, projectilePrefab, transform.position);
            else ProjectileAttack.ShootProjectileAtPlayer(cart, (CinemachineSmoothPath)cart.m_Path, projectilePrefab, transform.position);
        }
    }
}
