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

    private void Start()
    {
        LoadAllProfiles();

        /**
         * Test code
         */
        CreateProfile(0, "Nick");
        profiles[0].characterList.Add(new CharacterData("0", "Feebee", 10, 50));
        CreateProfile(2, "Christian");
        profiles[2].characterList.Add(new CharacterData("0", "Connor", 4, 3));
        profiles[2].characterList.Add(new CharacterData("1", "Daddy Long Legs"));
        profiles[2].characterList.Add(new CharacterData("23", "Daniel"));
        //SaveCurrentProfile();
        //SaveProfile(2);
    }

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

    public void SaveProfile(int profileIndex)
    {
        //Saves the profile boj
        SaveManager.SaveContent(profiles[profileIndex].SaveFormat, profiles[profileIndex].profileFilepath);

        //Goes through characters within profile and save them all
        string[] characterFileNames = profiles[profileIndex].CharacterFileNames;
        for (int x = 0; x < profiles[profileIndex].characterList.Count; x++)
        {
            SaveManager.SaveContent(profiles[profileIndex].characterList[x].SaveFormat, characterFileNames[x]);
        }
    }

    /// <summary>
    /// Method to save the currently selected profile
    /// </summary>
    public void SaveCurrentProfile()
    {
        SaveProfile(currentProfileIndex);
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
            ProfileSaveFormat profileSaveData = SaveManager.LoadContent(string.Format(ProfileData.profileFilepathTemplate, profileIndex)) as ProfileSaveFormat;

            if (profileSaveData != null)
            {
                ProfileData loadedProfile = new ProfileData(profileIndex, profileSaveData.profileName);

                //For loop to go through each character within the loaded profile
                for(int characterIndex = 0; characterIndex < profileSaveData.CharacterFilepaths.Length; characterIndex++)
                {
                    CharacterSaveFormat characterSaveData = SaveManager.LoadContent(profileSaveData.CharacterFilepaths[characterIndex]) as CharacterSaveFormat;

                    if (characterSaveData != null)
                    {
                        loadedProfile.characterList.Add(new CharacterData(characterSaveData.id, characterSaveData.name, characterSaveData.level, characterSaveData.xp));
                    }
                }

                profiles[profileIndex] = loadedProfile;
            }
        }
    }
}
