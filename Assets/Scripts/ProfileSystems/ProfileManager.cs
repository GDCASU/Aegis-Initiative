using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that manages all the profiles within the game. This includes having
/// references to them as well as saving and loading them
/// </summary>
public class ProfileManager : MonoBehaviour
{
    public ProfileData[] profiles;
    public int currentProfileIndex = 0;
    public int profileCount = 3;

    /// <summary>
    /// Method used to create a new profile obj
    /// </summary>
    /// <param name="profileIndex">The index within the array of profiles to store the profile</param>
    /// <param name="profileName">Name associated with the new profile</param>
    /// <returns>True if succesful and false otherwise</returns>
    public bool CreateProfile(int profileIndex, string profileName)
    {
        if (profileIndex < 0 || profileIndex >= profiles.Length) return false;

        if (profiles[profileIndex] == null)
        {
            profiles[profileIndex] = new ProfileData(profileIndex, profileName);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Method to save the currently selected profile
    /// </summary>
    public void SaveCurrentProfile()
    {
        //Saves the profile boj
        SaveManager.SaveContent(profiles[currentProfileIndex].SaveFormat, profiles[currentProfileIndex].profileFilepath);

        //Goes through characters within profile and save them all
        string[] characterFileNames = profiles[currentProfileIndex].CharacterFileNames;
        for(int x = 0; x < profiles[currentProfileIndex].characterList.Count; x++)
        {
            SaveManager.SaveContent(profiles[currentProfileIndex].characterList[x].SaveFormat, characterFileNames[x]);
        }
    }

    /// <summary>
    /// Method to load in all available profiles
    /// </summary>
    public void LoadAllProfiles()
    {
        profiles = new ProfileData[profileCount];

        //For loop to go through each profile
        for (int profileIndex = 0; profileIndex < profiles.Length; profileIndex++)
        {
            ProfileSaveFormat profileSaveData = SaveManager.LoadContent(ProfileData.profileFilenameHeader + profileIndex) as ProfileSaveFormat;

            if (profileSaveData != null)
            {
                ProfileData profile = new ProfileData(profileIndex, profileSaveData.profileName);

                //For loop to go through each character within the loaded profile
                for(int characterIndex = 0; characterIndex < profileSaveData.CharacterFilepaths.Length; characterIndex++)
                {
                    CharacterSaveFormat characterSaveData = SaveManager.LoadContent(profileSaveData.CharacterFilepaths[characterIndex]) as CharacterSaveFormat;

                    if (characterSaveData != null)
                    {
                        profile.characterList.Add(new CharacterData(characterSaveData.id, characterSaveData.name, characterSaveData.level, characterSaveData.xp));
                    }
                }
            }
        }
    }
}
