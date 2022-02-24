using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactive
{
    [System.Serializable]
    public class SoundInfo{
        public SoundType soundType;
        public AudioClip[] audioClip;
        public string dialogueText;
        public bool needFinish;
    }

    public enum SoundType
    {
        playLoop, playOnce, playEnter,
    }
}