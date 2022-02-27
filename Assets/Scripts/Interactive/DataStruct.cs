using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Interactive
{
    [System.Serializable]
    public class SoundInfo{
        public SoundType soundType;
        public AudioClip[] audioClip;
        public List<string> lineText;
        public List<string> dialogueText;
        public bool needFinish;
        public UnityEvent action;
    }

    public enum SoundType
    {
        playLoop, playOnce, playEnter,
    }
}