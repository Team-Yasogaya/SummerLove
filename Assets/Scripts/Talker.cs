using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    public class Talker : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private Dialogue _talkerDialogue;

        public void Talk()
        {
            GameUI.DialoguePrompt.gameObject.SetActive(true);
            GameUI.DialoguePrompt.SetDialogue(_talkerDialogue, _name);
            GameUI.DialoguePrompt.StartDialogue();
        }
    }
}
