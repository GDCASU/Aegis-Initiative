using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopilotSceneManager : MonoBehaviour
{
    /// <summary>
    /// Prevents this scene from fading in.
    /// </summary>
    private void Awake()
    {
        LevelChanger.singleton.PreventFadeIn();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.singleton.RemoveActive();
        GameManager.singleton.RemovePassive();
    }


}
