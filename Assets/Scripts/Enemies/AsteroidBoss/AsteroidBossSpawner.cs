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

            Vector3 spawnRelativeToPlayer = new Vector3(0, -12.5f, 30);
            Instantiate(asteroidBoss, dollyCart, false).gameObject.transform.localPosition = spawnRelativeToPlayer;
        }
    }
}
