using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that holds data for individual characters
/// </summary>
public class CharacterData
{
    public string id;
    public string name;
    public int level;
    public int xp;

    public CharacterSaveFormat SaveFormat
    {
        get { return new CharacterSaveFormat(id, name, level, xp); }
    }

    public CharacterData(string id, string name, int level = 0, int xp = 0)
    {
        this.id = id;
        this.name = name;
        this.level = level;
        this.xp = xp;
    }

    public CharacterData(CharacterSaveFormat save)
    {
        id = save.id;
        name = save.name;
        level = save.level;
        xp = save.xp;
    }
}

[System.Serializable]
public class CharacterSaveFormat
{
    public string id;
    public string name;
    public int level;
    public int xp;

    public CharacterSaveFormat(string id, string name, int level, int xp)
    {
        this.id = id;
        this.name = name;
        this.level = level;
        this.xp = xp;
    }
}
