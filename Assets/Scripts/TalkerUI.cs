using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NoName
{
    public class TalkerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _talkerName;

        private Button _button;

        private Talker _talker;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(ButtonAction);
        }

        public void InitializeTalkerUI(Talker talker)
        {
            _talker = talker;
            _talkerName.text = talker.Name;
        }

        private void ButtonAction()
        {
            GameUI.DialogueLibrary.OpenDialogueList(_talker);
        }
    }
}