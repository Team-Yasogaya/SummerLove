using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NoName
{

    [CreateAssetMenu(fileName = "New Audio SO", menuName = "Audio/AudioSO")]
    public class AudioSO : ScriptableObject
    {
        [field: Header("Identity")]
        [field: SerializeField] public string TrackName { get; private set; }   
        [field: SerializeField] public string Author { get; private set; }     
        [field: SerializeField] public string AudioID { get; private set; }       

        [field: Header("File")]
        [field: SerializeField] public AudioClip Clip { get; private set; }

        [field: Header("Settings")]
        [field: SerializeField, Range(0, 1)] public float DefaultVolume { get; private set; } = 1f;
        [field: SerializeField, Range(0.5f, 1.5f)] public float DefaultPitch { get; private set; } = 1f;
        [field: SerializeField] public bool Loop { get; private set; } = false;

        public void PlayAsSFX()
        {
            AudioManager.Instance.PlaySFX(Clip, DefaultVolume);
        }
    }
}
