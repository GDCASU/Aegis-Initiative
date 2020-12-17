using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class CopilotInfo : MonoBehaviour
{
    public Character character;
    public CopilotData copilotData;
    public Sprite portrait;

    [Header("Active")]
    #region Active
    public string active;
    public string activeDescription;
    public Sprite activeIcon;
    #endregion

    [Header("Passive")]
    #region Passive
    public string passive;
    public string passiveDescription;
    public Sprite passiveIcon;
    #endregion
}
