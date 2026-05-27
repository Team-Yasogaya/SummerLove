using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NoName;
using UnityEngine;

public class Jukebox : Interactable
{
    [SerializeField] private List<AudioSO> _songs;

    public override async void Interact()
    {
        int selectedIndex = await GameUI.MultichoiceModal.ShowModal("Play something!", _songs.Select(ac => ac.name).ToList());

        if (selectedIndex >= 0 && selectedIndex < _songs.Count)
        {
            AudioManager.Instance.PlayJukeboxTrack(_songs[selectedIndex]);
            
            Debug.Log($"Jukebox: Playing {_songs[selectedIndex].TrackName}");
        }
    }

    public override void HidePrompt()
    {
        
    }

    public override void ShowPrompt()
    {
        
    }
}
