using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Dialogue",
             "Set Important",
             "Set if this dialogue can be interrupted or not")]
public class ImportantDialogue : Command
{
    public bool isImportant = false;

    public override void OnEnter()
    {
        RemarkManager.singleton.SetImportantDialogue(isImportant);
        //base.OnEnter();
        Continue();
    }
}
