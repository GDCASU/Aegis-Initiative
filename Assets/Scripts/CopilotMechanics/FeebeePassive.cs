using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeebeePassive : CopilotPassiveMechanic
{
    public float defenseIncrease; 
    private void Start()
    {
        PlayerHealth.singleton.defense = defenseIncrease;
    }
    public override void CopyInfo(CopilotMechanic copilotMechanic)
    {
        base.CopyInfo(copilotMechanic);
        defenseIncrease = ((FeebeePassive)copilotMechanic).defenseIncrease;
    }
}
