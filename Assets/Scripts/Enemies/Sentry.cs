using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : Enemy
{
    public GameObject bulletPrefab;
    private GameObject player;
    private Transform gun;
    private float bulletSpeed = 0.25f;

    void Start()
    {
        gun = transform.GetChild(1).transform;
        StartCoroutine("Shoot");
    }

    IEnumerator Shoot()
    {
        while (GetComponent<Renderer>().isVisible)
        {
            GameObject bullet = Instantiate(bulletPrefab, gun.position, gun.rotation, transform);
            bullet.GetComponent<Rigidbody>().AddForce((player.transform.position - gun.position) * 200f);
            yield return new WaitForSeconds(bulletSpeed);
        }
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}