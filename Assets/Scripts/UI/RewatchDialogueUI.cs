using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NoName
{
    public class RewatchDialogueUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _dialogueName;

        private Button _button;

        private Dialogue _dialogue;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(ButtonAction);
        }

        public void InitializeRewatchDialogue(Dialogue dialogue)
        {
            _dialogue = dialogue;
            _dialogueName.text = dialogue.name;
        }

        private void ButtonAction()
        {
            GameUI.DialogueLibrary.ReloadDialogueConfirmation(_dialogue);
        }
    }
}