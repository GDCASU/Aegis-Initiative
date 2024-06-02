using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundManager : MonoBehaviour
{
    public static SoundManager singleton;

    public enum SFX
    {
        Select,
        Shoot,
        Hit,
        Heal,
        LowHealth,
    }
    public static Dictionary<SFX, string> sfxMap = new Dictionary<SFX, string>
    {
        {SFX.Select,  "event:/SFX/UI/Select"},
        {SFX.Shoot, "event:/SFX/Combat/Shoot"},
        {SFX.Hit, "event:/SFX/Combat/Hit"},
        {SFX.Heal, "event:/SFX/UI/Heal"},
        {SFX.LowHealth, "event:/SFX/UI/Low Health"}
    };

    private static string volumeSettingsPath = "/volume.data";
    public enum VolumeType
    {
        music,
        sfx,
    }

    public static float defaultMusicVolume = 0.1f;
    public static float defaultSfxVolume = 0.1f;

    [HideInInspector]
    public float currentMusicVolume = defaultMusicVolume;
    [HideInInspector]
    public float currentSfxVolume = defaultSfxVolume;

    public static Dictionary<VolumeType, float> volumeMap = new Dictionary<VolumeType, float> {};
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);

        LoadVolumeSettings();
    }

    public void SetMusicVolume(float volume)
    {
        volumeMap[VolumeType.music] = volume;
        FMODStartMusic.music.setVolume(volume);
        SaveVolumeSettings();
    }

    public void SetSFxVolume(float volume)
    {
        volumeMap[VolumeType.sfx] = volume;
        SaveVolumeSettings();
    }

    public void PlayOneShot(string path, Vector3 position, VolumeType type)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, position, volumeMap[type]);
    }

    private void SaveVolumeSettings()
    {
        SaveManager.SaveContent(volumeMap, volumeSettingsPath);
    }

    private void LoadVolumeSettings()
    {
        if (SaveManager.LoadContent(volumeSettingsPath) is Dictionary<VolumeType, float> volumeSettings)
        {
            currentMusicVolume = volumeSettings[VolumeType.music];
            currentSfxVolume = volumeSettings[VolumeType.sfx];
        }
    }

    public float GetVolume(VolumeType type)
    {
        return volumeMap[type];
    }
}
