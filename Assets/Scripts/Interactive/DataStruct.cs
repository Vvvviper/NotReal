using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactive
{
    [System.Serializable]
    public class SoundInfo{
        public SoundType soundType;
        public AudioClip[] audioClip;
        public List<DialogueText> dialogueText;
        public bool needFinish;
    }


    [System.Serializable]
    public class DialogueText
    {
        public string _text;
    }

    public enum SoundType
    {
        playLoop, playOnce, playEnter,
    }
}