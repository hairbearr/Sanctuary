using System.Collections.Generic;
using UnityEngine;
#pragma warning disable CS0649

namespace Sanctuary.Harry.Audio
{
    [CreateAssetMenu(fileName = "AudioArray", menuName = "Sanctuary/Audio Array")]
    public class ScriptableAudioArray : ScriptableObject
    {
        [SerializeField] List<AudioClip> clips = new List<AudioClip>();

        public int Count
        {
            get
            {
                return clips.Count;
            }
        }

        public AudioClip GetClip(int clip)
        {
            if (clips.Count == 0) return null;
            return clips[Mathf.Clamp(clip, 0, clips.Count - 1)];
        }

        public AudioClip PlayAudioClip(int index)
        {
            if (index < 0 || index > clips.Count || clips == null) return null;
            var clipToPlay = clips[index];
            return clipToPlay;
            // if (audioClips != null)
            // {
            //     //int index = UnityEngine.Random.Range(0, audioClips.Length);
            // }  
        }
    }
}
