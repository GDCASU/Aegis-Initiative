using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSlugEnd : MonoBehaviour
{
    public SpaceSlug slug;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            slug.endReached = true;
            slug.chancesOfAttack = 0;
            slug.maxTime = 4;
            slug.timer = 4;
            slug.outerWidthRange = 75;
        } 
    }
}
