using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that holds data for individual profiles as well
/// as some save file management stuff
/// </summary>
public class ProfileData
{
    public readonly static string profileFilepathTemplate = "/profile_{0}.data";
    public readonly static string characterFilepathTemplate = "/profile_{0}_character_{1}.data";

    [Header("Actual Profile Variables")]
    public int profileIndex;
    public string profileName;
    public List<CharacterData> characterList;

    [Header("File stuff")]
    public string profileFilepath;
    public string[] CharacterFileNames
    {
        get
        {
            string[] fileNames = new string[characterList.Count];

            for(int x = 0; x < fileNames.Length; x++)
            {
                fileNames[x] = string.Format(characterFilepathTemplate, profileIndex, characterList[x].id);
            }

            return fileNames;
        }
    }
    public ProfileSaveFormat SaveFormat
    {
        get
        {
            return new ProfileSaveFormat(profileIndex, profileName, CharacterFileNames);
        }
    }

    public ProfileData(int index, string name)
    {
        profileIndex = index;
        profileName = name;
        characterList = new List<CharacterData>();
        profileFilepath = string.Format(profileFilepathTemplate, profileIndex);
    }
}

/// <summary>
/// Serializable class that serves as the format for how
/// profiles are saved and loaded
/// </summary>
[System.Serializable]
public class ProfileSaveFormat
{
    public int profileIndex;
    public string profileName;
    public string[] CharacterFilepaths;

    public ProfileSaveFormat(int index, string name, string[] characters)
    {
        profileIndex = index;
        profileName = name;
        CharacterFilepaths = characters;
    }
}
