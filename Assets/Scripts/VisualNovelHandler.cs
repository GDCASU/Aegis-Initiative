using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualNovelHandler : MonoBehaviour
{
    public GameObject historyScrollView;
    public Text historyText;

    private void Start()
    {
        historyText.text = "";
        historyScrollView.SetActive(false);
    }

    /// <summary>
    /// Method called by the flowchart that takes the current dialog text and
    /// updates the history panel with that text
    /// </summary>
    public void UpdateHistoryText()
    {
        SayDialog currentDialog = SayDialog.GetSayDialog();
        string addText = string.Format("<b>{0}</b>\n{1}\n\n", currentDialog.NameText, currentDialog.StoryText);
        historyText.text += addText;
    }

    /// <summary>
    /// Method called by UI to toggle the history text panel
    /// </summary>
    public void ToggleHistory()
    {
        historyScrollView.SetActive(!historyScrollView.activeSelf);
    }
}
