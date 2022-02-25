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
            if (audioSource == null)
            {
                audioSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
                audioSource.playOnAwake = false;
            }
            else
                audioSource = this.GetComponent<AudioSource>();

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

            //get a random dialogue text from sound event
            string dialogue = "";
            int textIndex = Random.Range(0, currentSound.dialogueText.Count);
            if (currentSound.dialogueText.Count != 0)
                dialogue = currentSound.dialogueText[textIndex];
                

            switch (currentSound.soundType)
            {
                case SoundType.playLoop:
                    PlayLoop(audioClip);
                    DisplayDialogue(dialogue);
                    waitClipFinish = currentSound.needFinish;
                    NextSoundEvent();
                    break;
                case SoundType.playOnce:
                    PlayOnce(audioClip);
                    DisplayDialogue(dialogue);
                    waitClipFinish = currentSound.needFinish;
                    NextSoundEvent();
                    StartCoroutine(TriggerSound());
                    break;
                case SoundType.playEnter:
                    PlayOnce(audioClip);
                    DisplayDialogue(dialogue);
                    waitClipFinish = currentSound.needFinish;
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

        private void DisplayDialogue(string text)
        {
            DialogueController.instance.ShowLine(text);
        }

    }

}

