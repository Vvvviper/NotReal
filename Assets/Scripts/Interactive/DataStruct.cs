using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactive
{
    [System.Serializable]
    public class SoundInfo{
        public SoundType soundType;
        public AudioClip[] audioClip;
        public List<string> lineText;
        public Dialogue dialogueText;
        public bool needFinish;
    }

    public enum SoundType
    {
        playLoop, playOnce, playEnter,
    }
}