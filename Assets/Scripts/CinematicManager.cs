using NoName;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CinematicManager : MonoBehaviour
{
    public static CinematicManager Instance { get; private set; }

    public event Action OnCinematicEnded;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayCinematic(VideoClip clip)
    {
        GameUI.VideoPlayer.Open();
        GameUI.VideoPlayer.PlayClip(clip);

        StartCoroutine(WaitForVideoRoutine());
    }

    private IEnumerator WaitForVideoRoutine()
    {
        yield return new WaitForSeconds(.5f);

        while (GameUI.VideoPlayer.VideoPlayer.isPlaying)
        {
            yield return null;
        }

        if (OnCinematicEnded == null)
        {
            Debug.LogWarning("OnCinematicEnded is not assigned!");
            yield break;
        }

        GameUI.VideoPlayer.Close();
        OnCinematicEnded?.Invoke();
    }
}
