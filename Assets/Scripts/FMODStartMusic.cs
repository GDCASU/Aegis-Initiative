using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMODUnity
{
    public class FMODStartMusic : MonoBehaviour
    {
        [EventRef]
        public string Event = "";
        private FMOD.Studio.EventInstance music;

        void Start()
        {
            music = RuntimeManager.CreateInstance(Event);
            music.start();
            music.release();
        }

        void OnDestroy()
        {
            music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}