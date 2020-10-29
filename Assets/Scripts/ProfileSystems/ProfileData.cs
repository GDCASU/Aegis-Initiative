using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that holds data for individual profiles as well
/// as some save file management stuff
/// </summary>
public class ProfileData
{
    public readonly static string profileFilenameHeader = "p";
    public readonly static string characterFilenameHeader = "c";

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
                fileNames[x] = "/" + profileFilenameHeader + "_" + characterFilenameHeader + "_" + characterList[x].id;
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
        profileFilepath = "/" + profileFilenameHeader + "_" + profileIndex;
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
