using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactive
{
    public class InteractiveObject : MonoBehaviour
    {
        public SoundInfo[] SoundInfo;
        public bool PlayOnAwake;

        private AudioSource audioSource;
        private int soundEventIndex = 0;
        private bool waitClipFinish;
        private bool isWaiting = false;

        private void Awake()
        {
            audioSource = this.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
                audioSource.playOnAwake = false;
            }
            
            
        }

        private void Start()
        {
            if (PlayOnAwake)
            {
                StartCoroutine(TriggerSound());
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(TriggerSound());
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                StartCoroutine(TriggerSound());
            }
        }

        public void TriggerSoundStart()
        {
            StartCoroutine(TriggerSound());
        }

        IEnumerator TriggerSound()
        {
            // Check if there is sound event ready for play
            if (soundEventIndex >= SoundInfo.Length)
                yield break;
            SoundInfo currentSound = SoundInfo[soundEventIndex];

            // Wait for previous clip finish play
            if (isWaiting)
                yield break;
            if (waitClipFinish)
            {
                float currentTime = audioSource.time;
                isWaiting = true;
                while (currentTime < audioSource.clip.length)
                {
                    currentTime += Time.deltaTime;
                    yield return null;
                }
                isWaiting = false;
            }

            // get a random audio clip from the sound event
            AudioClip audioClip = null;
            int clipIndex = Random.Range(0, currentSound.audioClip.Length);
            if (currentSound.audioClip.Length != 0)
                audioClip = currentSound.audioClip[clipIndex];

            string line = "";
            List<string> dialogue = new List<string>();
            bool usingDialogeu = false;
            //If this object involves a dialgue
            if (currentSound.dialogueText.Count != 0)
            {
                dialogue = currentSound.dialogueText;
                usingDialogeu = true;
            }
            else
            {
                //get a random dialogue text from sound event
                int textIndex = Random.Range(0, currentSound.lineText.Count);
                if (currentSound.lineText.Count != 0)
                {
                    line = currentSound.lineText[textIndex];
                    usingDialogeu = false;
                }
            }
            
                

            switch (currentSound.soundType)
            {
                case SoundType.playLoop:
                    PlayLoop(audioClip);
                    DiaplayDialogue(dialogue, line, usingDialogeu);
                    waitClipFinish = currentSound.needFinish;
                    NextSoundEvent();
                    currentSound.action.Invoke();
                    break;
                case SoundType.playOnce:
                    PlayOnce(audioClip);
                    DiaplayDialogue(dialogue, line, usingDialogeu);
                    waitClipFinish = currentSound.needFinish;
                    NextSoundEvent();
                    StartCoroutine(TriggerSound());
                    currentSound.action.Invoke();
                    break;
                case SoundType.playEnter:
                    PlayOnce(audioClip);
                    DiaplayDialogue(dialogue, line, usingDialogeu);
                    waitClipFinish = currentSound.needFinish;
                    currentSound.action.Invoke();
                    break;
            }
        }

        private void NextSoundEvent()
        {
            soundEventIndex++;
        }

        private void PlayLoop(AudioClip clip)
        {
            if (!clip) return;
            audioSource.Stop();
            audioSource.loop = true;
            audioSource.clip = clip;
            audioSource.Play();
        }

        private void PlayOnce(AudioClip clip)
        {
            if (!clip) return;
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = clip;
            audioSource.Play();
        }

        private void DiaplayDialogue(List<string> dialogue, string text, bool usingDialogue)
        {
            if (usingDialogue && dialogue.Count > 0)
                DialogueController.instance.ShowDialogue(dialogue);
            else if(text.Length > 0)
                DialogueController.instance.ShowLine(text);
        }

    }

}

