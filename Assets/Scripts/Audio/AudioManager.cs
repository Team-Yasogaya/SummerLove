using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace NoName
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Mixer Groups")]
        [SerializeField] private AudioMixerGroup _musicGroup;
        [SerializeField] private AudioMixerGroup _sfxGroup;

        [Header("Sources")]
        [SerializeField] private AudioSource _bgmSource;      
        [SerializeField] private AudioSource _jukeboxSource; 

        private Coroutine _fadeCoroutine;

        private void Awake()
        {
            if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
            else { Destroy(gameObject); }
        }

        public void PlayBackgroundMusic(AudioSO data)
        {
            _bgmSource.clip = data.Clip;
            _bgmSource.volume = data.DefaultVolume;
            _bgmSource.pitch = data.DefaultPitch;
            _bgmSource.loop = data.Loop;
            _bgmSource.Play();
        }

        public void PlayJukeboxTrack(AudioSO newTrackData)
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

            _fadeCoroutine = StartCoroutine(CrossFadeMusic(_bgmSource, _jukeboxSource, newTrackData.Clip));
        }

        private IEnumerator CrossFadeMusic(AudioSource from, AudioSource to, AudioClip nextClip)
        {
            float duration = 1.5f;
            float currentTime = 0;

            to.clip = nextClip;
            to.volume = 0;
            to.Play();

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float percent = currentTime / duration;

                from.volume = 1 - percent;
                to.volume = percent;
                yield return null;
            }

            from.Stop();
        }

        public void PlaySFX(AudioClip clip, float volume = 1.0f)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
        }

        public AudioSource PlayLoopingSFX(AudioSO data)
        {
            GameObject sfxObj = new GameObject("LoopingSFX_" + data.TrackName);
            AudioSource source = sfxObj.AddComponent<AudioSource>();
            source.clip = data.Clip;
            source.loop = true;
            source.outputAudioMixerGroup = _sfxGroup;
            source.Play();
            return source; 
        }
    }
}
