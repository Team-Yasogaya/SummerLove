using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace NoName
{
    public class VideoPlayerUI : BaseMenuUI
    {
        [SerializeField] VideoPlayer _videoPlayer;
        [SerializeField] CanvasGroup _canvasGroup;
        [SerializeField] float _fadeDuration = 0.5f;

        public void PlayClip(VideoClip clip)
        {
            _videoPlayer.Stop();

            _videoPlayer.clip = clip;
            _videoPlayer.Play();
        }

        public bool IsVideoPlaying()
        {
            return _videoPlayer.isPlaying;
        }

        public Coroutine FadeOut()
        {
            return StartCoroutine(FadeOutRoutine());
        }

        private IEnumerator FadeOutRoutine()
        {
            float startAlpha = _canvasGroup.alpha;
            float endAlpha = 0f;
            float elapsedTime = 0f;

            while (elapsedTime < _fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / _fadeDuration);
                yield return null;
            }

            _canvasGroup.alpha = endAlpha;
        }
    }
}