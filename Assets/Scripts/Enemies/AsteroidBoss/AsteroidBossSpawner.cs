using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBossSpawner : MonoBehaviour
{
    [EventRef]
    public string BossMusic = "";
    public GameObject asteroidBoss;
    public Transform dollyCart;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FMODStartMusic.music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            FMODStartMusic.music = RuntimeManager.CreateInstance(BossMusic);
            FMODStartMusic.music.start();
            FMODStartMusic.music.release();
            FMODStartMusic.music.setVolume(GameManager.singleton.musicVolume);

            Vector3 spawnRelativeToPlayer = new Vector3(0, -50f, 30);
            GameObject boss = Instantiate(asteroidBoss, dollyCart, false);
            boss.transform.localPosition = spawnRelativeToPlayer;
            boss.transform.forward = PlayerInfo.singleton.transform.forward;
        }
    }
}
