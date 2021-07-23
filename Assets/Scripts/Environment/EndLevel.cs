using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") PlayerInfo.singleton.GetComponent<StageTriggers>().EndLevel();
    }
}
