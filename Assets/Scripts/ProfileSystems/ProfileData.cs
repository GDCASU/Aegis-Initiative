using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class that holds data for individual profiles as well
/// as some save file management stuff
/// </summary>
[System.Serializable]
public class ProfileData
{
    public readonly static string profileFilepathTemplate = "/profile_{0}.data";

    public int index;
    public string name;
    public List<CharacterData> characterList;

    public ProfileData(int index, string name, List<CharacterData> characterList = null)
    {
        this.index = index;
        this.name = name;

        if (characterList == null) this.characterList = new List<CharacterData>();
        else this.characterList = characterList;
    }
}
