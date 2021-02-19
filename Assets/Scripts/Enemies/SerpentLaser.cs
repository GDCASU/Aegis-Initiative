using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerpentLaser : MonoBehaviour
{
    [SerializeField]
    private int damagePerFrame;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerInfo.singleton.TakeDamage(damagePerFrame);
        }
    }
}
