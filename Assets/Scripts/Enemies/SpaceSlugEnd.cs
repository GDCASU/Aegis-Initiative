
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using FMODUnity;

public class SpaceSlugEnd : MonoBehaviour
{
    [EventRef]
    public string BossMusic = "";
    public SpaceSlug slug;
    public CinemachineSmoothPath bossTrack;
    public CinemachineDollyCart playerCart;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FMODStartMusic.music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            FMODStartMusic.music = RuntimeManager.CreateInstance(BossMusic);
            FMODStartMusic.music.start();
            FMODStartMusic.music.release();
            FMODStartMusic.music.setVolume(GameManager.singleton.musicVolume);

            playerCart.m_Path = bossTrack;
            slug.playerPath = bossTrack;
            playerCart.m_Position = 0;

            //slug.endReached = true;
            slug.chancesOfAttack = 100;
            slug.timeAhead = 7;
            slug.maxTime = 4;
            slug.timer = 0;
            slug.outerWidthRange = 75;
            slug.health = 60;
        } 
    }
}
