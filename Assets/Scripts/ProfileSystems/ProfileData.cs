using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class that holds data for individual profiles as well
/// as some save file management stuff
/// </summary>
public class ProfileData
{
    public readonly static string profileFilepathTemplate = "/profile_{0}.data";

    public int index;
    public string name;
    public List<CharacterData> characterList;
    public ProfileSaveFormat SaveFormat
    {
        get
        {
            CharacterSaveFormat[] characterSaves = new CharacterSaveFormat[characterList.Count];
            for(int x = 0; x < characterSaves.Length; x++)
            {
                characterSaves[x] = characterList[x].SaveFormat;
            }

            return new ProfileSaveFormat(index, name, characterSaves);
        }
    }

    public ProfileData(int index, string name)
    {
        this.index = index;
        this.name = name;
        characterList = new List<CharacterData>();
    }

    public ProfileData(ProfileSaveFormat save)
    {
        index = save.index;
        name = save.name;

        characterList = new List<CharacterData>();
        for(int x = 0; x < save.characterList.Length; x++)
        {
            characterList.Add(new CharacterData(save.characterList[x]));
        }
    }
}

/// <summary>
/// Serializable class that serves as the format for how
/// profiles are saved and loaded
/// </summary>
[System.Serializable]
public class ProfileSaveFormat
{
    public int index;
    public string name;
    public CharacterSaveFormat[] characterList;

    public ProfileSaveFormat(int index, string name, CharacterSaveFormat[] characters)
    {
        this.index = index;
        this.name = name;
        characterList = characters;
    }
}
