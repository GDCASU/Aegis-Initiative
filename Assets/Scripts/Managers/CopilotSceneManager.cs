using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopilotSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.singleton.RemoveActive();
        GameManager.singleton.RemovePassive();
    }


}
