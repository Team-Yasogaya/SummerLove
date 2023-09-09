using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace NoName
{
    public class DialoguePromptUI : MonoBehaviour, IPointerClickHandler
    {
        [Header("Prompt")]
        [SerializeField] private TextMeshProUGUI _dialogueText;
        [SerializeField] private TextMeshProUGUI _talkerName;
        [SerializeField] private float _typingSpeed;

        private Dialogue dialogue;
        private string talkerName;

        private Vector3 promptPosition;

        // Text Speech
        private DialogueNode currentNode;
        private bool finished;
        private Coroutine typingRoutine;

        private void Update()
        {
            
        }

        public void SetDialogue(Dialogue dialogue, string talkerName)
        {
            this.dialogue = dialogue;
            this.talkerName = talkerName;
        }

        public void StartDialogue()
        {
            InputManager.Instance.DisablePlayerControls();

            currentNode = dialogue.RootNode;
            SetCharacterText(talkerName);
            _dialogueText.text = string.Empty;
            StartTypingLine();
            InputManager.Instance.ConfirmEvent += OnConfirm;
        }

        public void EndDialogue()
        {
            InputManager.Instance.EnablePlayerControls();

            gameObject.SetActive(false);
            InputManager.Instance.ConfirmEvent -= OnConfirm;
        }

        public void SetCharacterText(string text)
        {
            _talkerName.text = talkerName;
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
                // TODO
                StopAllCoroutines();
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
            foreach (var clue in currentNode.DialogueClues)
            {
                _dialogueText.text = _dialogueText.text.Replace(clue.Word, clue.GetHyperTextClue());
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var linkIndex = TMP_TextUtilities.FindIntersectingLink(_dialogueText, Mouse.current.position.ReadValue(), null);

            if (linkIndex == -1) return;

            var linkId = _dialogueText.textInfo.linkInfo[linkIndex].GetLinkID();

            var clue = currentNode.GetDialogueClueByID(linkId);

            Debug.Log("Clicked on the Clue: " + clue.Word + " with ID: " + clue.ID);
        }
    }
}
