using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NoName
{
    public class DialogueLibraryUI : BaseMenuUI
    {
        [SerializeField] private RectTransform _talkersContainer;
        [SerializeField] private RectTransform _dialoguesContainer;
        [SerializeField] private Button _closeButton;

        [SerializeField] private TalkerUI _talkerUIPrefab;
        [SerializeField] private RewatchDialogueUI _rewatchDialoguePrefab;

        private Talker _currentSelectedTalker;

        private void Start()
        {
            _closeButton.onClick.AddListener(Close);
        }

        private void OnEnable()
        {
            FillTalkersList();
        }

        private void OnDisable()
        {
            ClearTalkersList();
            ClearDialogueList();
        }

        public void FillTalkersList()
        {
            ClearTalkersList();

            foreach (Talker talker in DialogueHistory.Instance.GetAllRegisteredTalkers())
            {
                TalkerUI talkerUI = Instantiate(_talkerUIPrefab, _talkersContainer);
                talkerUI.InitializeTalkerUI(talker);
            }
        }

        private void ClearTalkersList()
        {
            foreach (Transform child in _talkersContainer)
            {
                Destroy(child.gameObject);
            }
        }

        public void OpenDialogueList(Talker talker)
        {
            ClearDialogueList();

            _currentSelectedTalker = talker;

            foreach (var record in DialogueHistory.Instance.GetRecordsByTalker(talker))
            {
                RewatchDialogueUI rewarch = Instantiate(_rewatchDialoguePrefab, _dialoguesContainer);
                rewarch.InitializeRewatchDialogue(record.dialogue);
            }
        }

        private void ClearDialogueList()
        {
            foreach (Transform child in _dialoguesContainer)
            {
                Destroy(child.gameObject);
            }
        }

        public void ReloadDialogueConfirmation(Dialogue dialogue)
        {
            GameUI.ConfirmationModal.Show("Rewatch this dialogue with " + _currentSelectedTalker.Name + "?");
            GameUI.ConfirmationModal.OnConfirm += () => ReloadDialogue(dialogue);
        }

        private void ReloadDialogue(Dialogue dialogue)
        {
            Close();

            GameUI.DialoguePrompt.ReloadDialogueFromHistory(dialogue);
            GameUI.DialoguePrompt.OnDialogueFinished = GameUI.OpenDialogueLibrary;
        }
    }
}