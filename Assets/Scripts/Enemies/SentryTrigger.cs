using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "GameDollyCart")
        {
            transform.parent.GetComponent<Sentry>().enabled = true;
            transform.parent.GetComponent<Sentry>().SetPlayer(collider.transform.GetChild(1).gameObject);
        }
    }
}
