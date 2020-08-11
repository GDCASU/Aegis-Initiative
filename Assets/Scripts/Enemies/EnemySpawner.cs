using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int grunts;
    public GameObject gruntPrefab;
    private void OnTriggerEnter(Collider other)
    {
        //print(other.name);
        if (other.gameObject.name == "GameDollyCart")
        {
            //if (//other.GetComponent<PlayerInfo>().enemiesOnScreen == 0)
            //{
            //other.GetComponent<PlayerInfo>().enemiesOnScreen--;
            for (int x = 0; x < grunts; x++)
                {
                    Instantiate(gruntPrefab, other.gameObject.transform.position, Quaternion.identity, other.gameObject.transform);
                }
            //}
        }
    }
}
