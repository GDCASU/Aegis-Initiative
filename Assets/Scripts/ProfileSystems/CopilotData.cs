﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// Class that holds data for individual copilot characters
/// </summary>
[System.Serializable]
public class CopilotData
{
    public Copilots name;
    public int level;
    public int experience;
    public bool isUnlocked;

    public CopilotData(Copilots name, int level = 0, int experience = 0, bool isUnlocked = false)
    {
        this.name = name;
        this.level = level;
        this.experience = experience;
        this.isUnlocked = isUnlocked;
    }
}
