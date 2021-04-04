using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that manages all the profiles within the game. This includes having
/// references to them as well as saving and loading them
/// 
/// I don't think I remember the actual problem but because of it all ProfileData
/// objects within the profiles array is given an object to prevent them from being
/// null. ProfileData's with an id of -1 are considered to be invalid/empty profiles
/// </summary>
public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance;

    public ProfileData[] profiles;
    public int currentProfileIndex = -1;
    public int profileCount = 3;
    public ProfileData CurrentProfile
    {
        get
        {
            if (currentProfileIndex >= 0 && currentProfileIndex < profileCount) return profiles[currentProfileIndex];

            return null;
        }
    }

    [SerializeField]
    private bool saveTestProfiles;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        LoadAllProfiles();

        if (saveTestProfiles) TestSaveProfiles();
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

        profiles[profileIndex] = new ProfileData(profileIndex, profileName);
        return true;
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

            if (profiles[x] == null) profiles[x] = new ProfileData(-1, "");
        }
    }

    public void DeleteProfile(int id)
    {
        profiles[id] = new ProfileData(-1, "");
        SaveProfile(id);
    }

    public List<CopilotData> GetCurrentCopilotList()
    {
        return GetCopilotList(currentProfileIndex);
    }

    public List<CopilotData> GetCopilotList(int index)
    {
        if (index < profileCount && profiles[index] != null) 
            return profiles[index].CopilotList;

        return null;
    }

    /// <summary>
    /// Method that creates some test profiles. This should not actually be used
    /// throughout the game
    /// </summary>
    public void TestSaveProfiles()
    {
        CreateProfile(0, "UITest1");
        profiles[0].AddCopilot(new CopilotData(Copilots.Feebee,0,0,true));
        profiles[0].AddCopilot(new CopilotData(Copilots.Frederick, 0, 0, true));
        profiles[0].currentStage = 0;
        SaveProfile(0);
        CreateProfile(1, "UITest2");
        profiles[1].AddCopilot(new CopilotData(Copilots.DaddyLongLegs, 4, 3, true));
        profiles[1].AddCopilot(new CopilotData(Copilots.Feebee,0,0,true));
        profiles[1].AddCopilot(new CopilotData(Copilots.Frederick, 0, 0, true));
        profiles[1].AddCopilot(new CopilotData(Copilots.MushroomFriend, 0, 0, true));
        profiles[1].AddCopilot(new CopilotData(Copilots.SpaceGirl, 0, 0, true));
        profiles[1].currentStage = 1;
        SaveProfile(1);

    }
}
