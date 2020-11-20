using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            transform.parent.GetComponent<Sentry>().enabled = true;
            transform.parent.GetComponent<Sentry>().SetPlayer(collider.gameObject);
        }
    }
}
