using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingOrb : MonoBehaviour
{
    public int healPoints = 5;
    // Start is called before the first frame update

    // Update is called once per frame

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerInfo>().Heal(healPoints);
            Destroy(gameObject);
        }
    }
   
}
