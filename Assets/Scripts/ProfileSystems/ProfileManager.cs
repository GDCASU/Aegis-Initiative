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
        SaveManager.SaveContent(profiles[profileIndex].SaveFormat, string.Format(ProfileData.profileFilepathTemplate, profileIndex));
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
        for (int x = 0; x < profiles.Length; x++)
        {
            ProfileSaveFormat profileSaveData = SaveManager.LoadContent(string.Format(ProfileData.profileFilepathTemplate, x)) as ProfileSaveFormat;

            if (profileSaveData != null)
            {
                ProfileData loadedProfile = new ProfileData(profileSaveData);
                profiles[x] = loadedProfile;
            }
        }
    }
}
