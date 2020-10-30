using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that holds data for individual characters
/// </summary>
[System.Serializable]
public class CharacterData
{
    public string id;
    public string name;
    public int level;
    public int xp;

    public CharacterData(string id, string name, int level = 0, int xp = 0)
    {
        this.id = id;
        this.name = name;
        this.level = level;
        this.xp = xp;
    }
}
