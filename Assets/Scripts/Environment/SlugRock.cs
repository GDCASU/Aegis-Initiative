using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugRock : EnemyHealth
{
    private void Start()
    {
        base.Start();
       StartCoroutine(ActivateColldier());

    }
    public IEnumerator ActivateColldier()
    {
        float delay = 1;
        while (delay > 0)
        {
            delay -= Time.deltaTime;
            yield return null;
        }
        GetComponent<SphereCollider>().enabled = true;
    }
}
