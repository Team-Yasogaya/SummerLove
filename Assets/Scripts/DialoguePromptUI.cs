using System.Collections;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace NoName
{
    public class DialoguePromptUI : BaseMenuUI, IPointerClickHandler
    {
        [Header("Prompt")]
        [SerializeField] private TextMeshProUGUI _dialogueText;
        [SerializeField] private TextMeshProUGUI _talkerName;
        [SerializeField] private float _typingSpeed;

        [Header("Clues")]
        [SerializeField] private TextMeshProUGUI _cluesInkText;

        private Dialogue dialogue;
        private Talker talker;

        private Vector3 promptPosition;

        // Text Speech
        private DialogueNode currentNode;
        private bool finished;
        private Coroutine typingRoutine;

        // Dialogue Events
        public Action OnDialogueFinished;

        private void Update()
        {
            
        }

        public void StartDialogue(Dialogue dialogue, Talker talker)
        {
            this.dialogue = dialogue;
            this.talker = talker;

            Open();

            DialogueHistory.Instance.AddDialogueToHistory(dialogue, talker);

            UpdateCluesInkCounter();

            currentNode = dialogue.RootNode;
            SetCharacterText(talker.Name);
            _dialogueText.text = string.Empty;
            StartTypingLine();

            InputManager.Instance.ConfirmEvent += OnConfirm;
        }

        public void ReloadDialogueFromHistory(Dialogue dialogue)
        {
            DialogueHistory.DialogueRecord dialogueRecord = DialogueHistory.Instance.GetRecordByDialogue(dialogue);
            this.talker = dialogueRecord.talker;
            this.dialogue = dialogue;

            Open();

            UpdateCluesInkCounter();

            currentNode = dialogue.RootNode;
            SetCharacterText(talker.Name);
            _dialogueText.text = string.Empty;
            StartTypingLine();

            InputManager.Instance.ConfirmEvent += OnConfirm;
        }

        public void EndDialogue()
        {
            Close();
            InputManager.Instance.ConfirmEvent -= OnConfirm;

            OnDialogueFinished?.Invoke();
            OnDialogueFinished = null;
        }

        public void SetCharacterText(string text)
        {
            _talkerName.text = text;
        }

        public void OnConfirm()
        {
            if (finished)
            {
                if (currentNode.Children.Count > 0)
                {
                    currentNode = dialogue.GetNode(currentNode.Children[0]);
                    StartTypingLine();
                }
                else
                {
                    EndDialogue();
                }
            }
            else
            {
                StopCoroutine(typingRoutine);
                
                _dialogueText.text = currentNode.Text;

                BuildDialogueNodeClues();

                finished = true;
            }
        }

        public void StartTypingLine()
        {
            finished = false;
            _dialogueText.text = string.Empty;
            typingRoutine = StartCoroutine(TypeLine());
        }

        private IEnumerator TypeLine()
        {
            foreach (char c in currentNode.Text.ToCharArray())
            {
                _dialogueText.text += c;
                yield return new WaitForSeconds(1 / _typingSpeed);
            }

            BuildDialogueNodeClues();
            finished = true;
        }

        private void BuildDialogueNodeClues()
        {
            string buildText = currentNode.Text;

            foreach (var clue in currentNode.DialogueClues)
            {
                if (DialogueHistory.Instance.GetRecordByDialogue(dialogue).collectedClues.Count == dialogue.MaxCollectableClues)
                {
                    buildText = buildText.Replace(clue.Word, clue.GetDisabledHyperTextClue());
                    _dialogueText.text = buildText;
                }
                else if (DialogueHistory.Instance.GetRecordByDialogue(dialogue).collectedClues.Contains(clue))
                {
                    buildText = buildText.Replace(clue.Word, clue.GetDisabledHyperTextClue());
                    _dialogueText.text = buildText;
                }
                else 
                {
                    buildText = buildText.Replace(clue.Word, clue.GetHyperTextClue());
                    _dialogueText.text = buildText;
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var linkIndex = TMP_TextUtilities.FindIntersectingLink(_dialogueText, Mouse.current.position.ReadValue(), null);

            if (linkIndex == -1) return;

            var linkId = _dialogueText.textInfo.linkInfo[linkIndex].GetLinkID();

            var clue = currentNode.GetDialogueClueByID(linkId);

            Debug.Log("Clicked on the Clue: " + clue.Word + " with ID: " + clue.ID);

            CollectClue(clue);
        }

        private void UpdateCluesInkCounter()
        {
            DialogueHistory.DialogueRecord record = DialogueHistory.Instance.GetRecordByDialogue(dialogue);

            _cluesInkText.text = (dialogue.MaxCollectableClues - record.collectedClues.Count).ToString();
        }

        private void CollectClue(DialogueNode.DialogueClue clue)
        {
            // PLAY COLLECTING ANIMATION

            DialogueHistory.Instance.AddCollectedClueToDialogue(dialogue, clue);

            UpdateCluesInkCounter();
            BuildDialogueNodeClues();
        }
    }
}
