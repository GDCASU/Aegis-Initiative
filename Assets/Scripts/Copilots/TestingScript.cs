using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    public CopilotPassiveMechanic pass;
    public CopilotActiveMechanic act;
    void Start()
    {
        gameObject.AddComponent(GameManager.singleton.active.GetType());
        gameObject.AddComponent(GameManager.singleton.passive.GetType());
        act = GetComponent<CopilotActiveMechanic>();
        pass = GetComponent<CopilotPassiveMechanic>();
        act.CopyInfo(GameManager.singleton.active);
        pass.CopyInfo(GameManager.singleton.passive);
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
