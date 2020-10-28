using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float health;
    GameObject prefab;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //health -= collision.gameObject.GetComponent<PlayerShot>().damage;
        }
    }

}