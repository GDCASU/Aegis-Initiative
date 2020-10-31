using FMOD;
using JetBrains.Annotations;
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
    private List<CopilotData> copilotList;
    public Dictionary<int, int> stageScores;
    public int farthestStage;   //This helps keep tracks of game progess by knowing which stage they last beat
    public int newGamePlusCount;

    public ProfileData(int index, string name)
    {
        this.index = index;
        this.name = name;
        this.copilotList = new List<CopilotData>();
        this.stageScores = new Dictionary<int, int>();
        this.farthestStage = 0;
        this.newGamePlusCount = 0;
    }

    public ProfileData(int index, string name, List<CopilotData> characterList, Dictionary<int, int> stageScores, int farthestStage, int newGamePlusCount)
    {
        this.index = index;
        this.name = name;
        this.copilotList = characterList;
        this.stageScores = stageScores;
        this.farthestStage = farthestStage;
        this.newGamePlusCount = newGamePlusCount;
    }

    public bool AddCopilot(CopilotData newPilot)
    {
        foreach (CopilotData pilot in copilotList)
            if (pilot.name.Equals(newPilot.name)) return false;

        copilotList.Add(newPilot);
        return true;
    }

    public CopilotData GetCopilot(string pilotName)
    {
        foreach(CopilotData pilot in copilotList)
            if (pilot.name.Equals(pilotName)) return pilot;

        return null;
    }
}
