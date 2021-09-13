
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SpaceSlugEnd : MonoBehaviour
{
    [EventRef]
    public string BossMusic = "";
    public SpaceSlug slug;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FMODStartMusic.music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            FMODStartMusic.music = RuntimeManager.CreateInstance(BossMusic);
            FMODStartMusic.music.start();
            FMODStartMusic.music.release();

            slug.endReached = true;
            slug.chancesOfAttack = 0;
            slug.maxTime = 4;
            slug.timer = 4;
            slug.outerWidthRange = 75;
            slug.health = 40;
        } 
    }
}
