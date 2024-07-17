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

        private Dialogue _dialogue;
        private Vector3 promptPosition;

        // Text Speech
        private DialogueNode currentNode;
        private bool finished;
        private Coroutine typingRoutine;

        public event Action OnNodeFinished;

        private void Update()
        {
            
        }

        private void OnEnable()
        {
            InputManager.Instance.ConfirmEvent += OnConfirm;
        }

        private void OnDisable()
        {
            InputManager.Instance.ConfirmEvent -= OnConfirm;
        }

        public void StartDialogue(Dialogue dialogue)
        {
            _dialogue = dialogue;
            _dialogueText.text = string.Empty;
        }

        public void AddNode(DialogueNode node, string talkerName = null)
        {
            currentNode = node;
            //SetCharacterText(talker.Name);
            _dialogueText.text = string.Empty;
            StartTypingLine();
        }

        public void SetCharacterText(string text)
        {
            _talkerName.text = text;
        }

        public void OnConfirm()
        {
            if (finished)
            {
                OnNodeFinished?.Invoke();
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
                if (DialogueHistory.Instance.GetRecordByDialogue(_dialogue).collectedClues.Count == _dialogue.MaxCollectableClues)
                {
                    buildText = buildText.Replace(clue.Word, clue.GetDisabledHyperTextClue());
                    _dialogueText.text = buildText;
                }
                else if (DialogueHistory.Instance.GetRecordByDialogue(_dialogue).collectedClues.Contains(clue))
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
            var linkGuid = Guid.Parse(linkId);

            var clue = currentNode.GetDialogueClueByID(linkGuid);

            Debug.Log("Clicked on the Clue: " + clue.Word + " with ID: " + clue.Id);

            CollectClue(clue);
        }

        public void UpdateCluesInkCounter()
        {
            DialogueHistory.DialogueRecord record = DialogueHistory.Instance.GetRecordByDialogue(_dialogue);

            _cluesInkText.text = (_dialogue.MaxCollectableClues - record.collectedClues.Count).ToString();
        }

        private void CollectClue(DialogueClue clue)
        {
            // PLAY COLLECTING ANIMATION

            DialogueHistory.Instance.AddCollectedClueToDialogue(_dialogue, clue);

            UpdateCluesInkCounter();
            BuildDialogueNodeClues();
        }
    }
}
