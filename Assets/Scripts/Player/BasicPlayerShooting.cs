using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerShooting : MonoBehaviour
{
    public float fireRate;
    public float speed;
    public int magSize;
    public GameObject bulletPrefab;
    private GameObject bullet;
    public Transform spawnR;
    public Transform spawnL;
    public bool alternate;

    private float timerOne;
    private float timerTwo;

    private void Start()
    {
        timerOne = fireRate;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (alternate)
            {
                if (timerOne < 0)
                {
                    bullet = Instantiate(bulletPrefab, spawnR.position, spawnR.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = spawnR.forward * speed;
                    timerOne = fireRate;
                    timerTwo = fireRate / 2f;
                }
                if (timerTwo < 0)
                {
                    bullet = Instantiate(bulletPrefab, spawnL.position, spawnL.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = spawnL.forward * speed;
                    timerTwo = fireRate;
                }
                timerTwo -= Time.deltaTime;
            }
            else 
            {
                if (timerOne < 0)
                {
                    bullet = Instantiate(bulletPrefab, spawnR.position, spawnR.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = spawnR.forward * speed;
                    bullet = Instantiate(bulletPrefab, spawnL.position, spawnL.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = spawnL.forward * speed;
                    timerOne = fireRate;
                }
            }          
        }
        timerOne -= Time.deltaTime;
    }
}
