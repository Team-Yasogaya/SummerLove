using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace NoName
{
    public class VideoPlayerUI : BaseMenuUI
    {
        [SerializeField] private VideoPlayer _videoPlayer;

        public VideoPlayer VideoPlayer { get { return _videoPlayer; } }

        public void PlayClip(VideoClip clip)
        {
            VideoPlayer.Stop();

            VideoPlayer.clip = clip;
            VideoPlayer.Play();
        }
    }
}