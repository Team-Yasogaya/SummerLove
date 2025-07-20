using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NoName
{
    public class DialogueLibraryUI : BaseMenuUI
    {
        [SerializeField] RectTransform _talkersContainer;
        [SerializeField] RectTransform _dialoguesContainer;
        [SerializeField] Button _closeButton;

        [SerializeField] TalkerUI _talkerUIPrefab;
        [SerializeField] RewatchDialogueUI _rewatchDialoguePrefab;

        private Npc currentSelectedTalker;

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

            foreach (Npc talkerNpc in DialogueHistory.Instance.GetAllRegisteredTalkers())
            {
                TalkerUI talkerUI = Instantiate(_talkerUIPrefab, _talkersContainer);
                talkerUI.InitializeTalkerUI(talkerNpc);
            }
        }

        private void ClearTalkersList()
        {
            foreach (Transform child in _talkersContainer)
            {
                Destroy(child.gameObject);
            }
        }

        public void OpenDialogueList(Npc talkerNpc)
        {
            ClearDialogueList();

            currentSelectedTalker = talkerNpc;

            foreach (var record in DialogueHistory.Instance.GetRecordsByTalker(talkerNpc))
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
            GameUI.ConfirmationModal.Show("Rewatch this dialogue with " + currentSelectedTalker.Name + "?");
            GameUI.ConfirmationModal.OnConfirm += () => ReloadDialogue(dialogue);
        }

        private void ReloadDialogue(Dialogue dialogue)
        {
            Close();

            DialogueManager.Instance.RestartDialogueFromHistory(dialogue);
            DialogueManager.Instance.OnEndDialogue += GameUI.OpenDialogueLibrary;
        }
    }
}