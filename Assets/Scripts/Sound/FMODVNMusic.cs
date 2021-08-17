using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMODUnity
{
    public class FMODVNMusic : MonoBehaviour
    {
        public enum Character
        {
            Mushii
        };

        [EventRef]
        public string[] Events = { "" };
        public static FMOD.Studio.EventInstance music;
        public Character character;

        private float originalVolume;
        private float currentVolume;

        void Start()
        {
            // play theme 1 on scenes 0 and 2
            // play theme 2 on scenes 1 and 3
            int vnNumber = GameManager.singleton.activeCopilot.GetComponent<CopilotInfo>().copilotData.vnScenesCompleted;
            var Event = (vnNumber % 2 == 0) ? Events[0] : Events[1];

            music = RuntimeManager.CreateInstance(Event);
            music.start();
            music.release();

            music.getVolume(out originalVolume);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                music.getVolume(out currentVolume);

                if (currentVolume != 0)
                    music.setVolume(0);
                else
                    music.setVolume(originalVolume);
            }
        }

        void OnDestroy()
        {
            music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}