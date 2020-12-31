using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public CopilotMechanic active;
    public CopilotMechanic passive;
    public string selectedUIActive;
    public string selectedUIPassive;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
    }
    public enum TypeOfMechanic
    {
        Buff,
        AOE,
        Action,
        Vision,
        Create,
        Transformative,
        Event,
    };
    public enum Copilots
    {
        Feebee = 0
    };
    public void AssignPassive(string abilityName, GameManager.Copilots copilot, CopilotMechanic mechanic)
    {
    }
    public void AssignActive(string abilityName, GameManager.Copilots copilot, CopilotMechanic mechanic)
    {
    }
    //public struct
}
