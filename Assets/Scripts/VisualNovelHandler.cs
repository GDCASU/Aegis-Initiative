using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualNovelHandler : MonoBehaviour
{
    public Text historyText;

    private void Start()
    {
        historyText.text = "";
    }

    public void UpdateHistoryText()
    {
        SayDialog currentDialog = SayDialog.GetSayDialog();
        string addText = string.Format("<b>{0}</b>\n{1}\n\n", currentDialog.NameText, currentDialog.StoryText);
        historyText.text += addText;
    }
}
