using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingMushroomHealth : MonoBehaviour
{
    int health;
    public int damageRate;
    public GameObject healingOrbPrefab;
    public GameObject healingOrb;
    private Transform orbSpawn;
    // Start is called before the first frame update
    void Start()
    {
        orbSpawn = gameObject.transform;
        health = 30;
        damageRate = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            healingOrb = Instantiate(healingOrbPrefab, orbSpawn.position, orbSpawn.rotation);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Debug.Log(health);
            damageRate = other.gameObject.GetComponent<Bullet>().damage;
            takeDamage(damageRate);
        }
    }
    public void takeDamage(int damage)
    {
        health -= damage*(2);
    }
}
