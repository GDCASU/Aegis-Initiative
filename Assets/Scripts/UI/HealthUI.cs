using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public GameObject boss;
    public GameObject player;
    public GameObject bossHealthBar;
    public GameObject PlayerInfoBar;
    private EnemyHealth bossHealth;
    private PlayerInfo PlayerInfo;
    private Slider bossSlider;
    private Slider playerSlider;

    void Start()
    {
        if (bossHealth != null) bossHealth = boss.GetComponent<EnemyHealth>();
        PlayerInfo = player.GetComponent<PlayerInfo>();
        if (bossSlider != null) bossSlider = bossHealthBar.GetComponent<Slider>();
        playerSlider = PlayerInfoBar.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bossSlider != null) bossSlider.value = bossHealth.health;
        playerSlider.value =  PlayerInfo.health;
    }
}
