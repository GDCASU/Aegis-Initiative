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

        //TestSaveProfiles();
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
        SaveManager.SaveContent(profiles[profileIndex], string.Format(ProfileData.profileFilepathTemplate, profileIndex));
    }

    public void SaveCurrentProfile()
    {
        SaveProfile(currentProfileIndex);
    }

    public void LoadAllProfiles()
    {
        profiles = new ProfileData[profileCount];

        //For loop to go through each profile
        for (int x = 0; x < profiles.Length; x++)
        {
            profiles[x] = SaveManager.LoadContent(string.Format(ProfileData.profileFilepathTemplate, x)) as ProfileData;
        }
    }

    /// <summary>
    /// Method that creates some test profiles. This should not actually be used
    /// throughout the game
    /// </summary>
    private void TestSaveProfiles()
    {
        CreateProfile(0, "Nick");
        profiles[0].AddCopilot(new CopilotData("Feebee", 10, 50, true));
        CreateProfile(2, "Christian");
        profiles[2].AddCopilot(new CopilotData("Connor", 4, 3, true));
        profiles[2].AddCopilot(new CopilotData("Daddy Long Legs"));
        profiles[2].AddCopilot(new CopilotData("Daniel"));
        SaveCurrentProfile();
        SaveProfile(2);
    }
}
