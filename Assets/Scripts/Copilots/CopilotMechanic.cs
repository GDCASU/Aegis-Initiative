using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopilotMechanic : MonoBehaviour
{
    public TypeOfMechanic typeOfMechanic;
    public string abilityName;
    public string description;
    public Sprite icon;

    public virtual void CopyInfo(CopilotMechanic copilotMechanic)
    {
        typeOfMechanic = copilotMechanic.typeOfMechanic;
        abilityName = copilotMechanic.abilityName;
        description = copilotMechanic.description;
        icon = copilotMechanic.icon;
    }
}
