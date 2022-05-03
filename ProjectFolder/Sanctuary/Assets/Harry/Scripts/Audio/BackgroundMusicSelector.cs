using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Audio
{
    public class BackgroundMusicSelector : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip[] audioClipArray;

        void Update()
        {
            if(!audioSource.isPlaying)
            {
                audioSource.clip = GetRandomClip();
                audioSource.Play();
            }
        }

        private AudioClip GetRandomClip()
        {
            int v = Random.Range(0, audioClipArray.Length);
            AudioClip tempClip = audioClipArray[v];
            if(tempClip!=audioSource.clip)
            {
                return tempClip;
            }
            else return audioClipArray[v+1];
        }
    }
}
