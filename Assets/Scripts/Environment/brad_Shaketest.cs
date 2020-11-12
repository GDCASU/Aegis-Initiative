using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brad_Shaketest : ScreenShake
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ShakeTest")
        {
            ShakeCamera();
        }
    }
}
