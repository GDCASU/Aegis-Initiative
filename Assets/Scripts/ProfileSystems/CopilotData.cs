using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// Class that holds data for individual characters
/// </summary>
[System.Serializable]
public class CopilotData
{
    public string name;
    public int level;
    public int experience;
    public bool isUnlocked;

    public CopilotData(string name, int level = 0, int experience = 0, bool isUnlocked = false)
    {
        this.name = name;
        this.level = level;
        this.experience = experience;
        this.isUnlocked = isUnlocked;
    }
}
