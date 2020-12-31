using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCopilots : MonoBehaviour
{
    public CopilotPassiveMechanic passive;
    public CopilotActiveMechanic active;
    void Start()
    {
        gameObject.AddComponent(GameManager.singleton.active.GetType());
        gameObject.AddComponent(GameManager.singleton.passive.GetType());
        active = GetComponent<CopilotActiveMechanic>();
        passive = GetComponent<CopilotPassiveMechanic>();
        active.CopyInfo(GameManager.singleton.active);
        passive.CopyInfo(GameManager.singleton.passive);
        //switch (GameManager.singleton.passive.typeOfMechanic)
        //{
        //    case GameManager.TypeOfMechanic.Action:
        //        break;
        //    case GameManager.TypeOfMechanic.AOE:
        //        break;
        //    case GameManager.TypeOfMechanic.Buff:
        //        break;
        //    case GameManager.TypeOfMechanic.Create:
        //        break;
        //    case GameManager.TypeOfMechanic.Event:
        //        break;
        //    case GameManager.TypeOfMechanic.Transformative:
        //        break;
        //    case GameManager.TypeOfMechanic.Vision:
        //        break;
        //}
        //switch (GameManager.singleton.active.typeOfMechanic)
        //{
        //    case GameManager.TypeOfMechanic.Action:
        //        break;
        //    case GameManager.TypeOfMechanic.AOE:
        //        break;
        //    case GameManager.TypeOfMechanic.Buff:
        //        break;
        //    case GameManager.TypeOfMechanic.Create:
        //        break;
        //    case GameManager.TypeOfMechanic.Event:
        //        break;
        //    case GameManager.TypeOfMechanic.Transformative:
        //        break;
        //    case GameManager.TypeOfMechanic.Vision:
        //        break;
        //}
    }
}
