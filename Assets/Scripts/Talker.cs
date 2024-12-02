using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace NoName
{
    public class Talker : MonoBehaviour
    {
        [SerializeField] string _talkerName;

        private DialogueTrigger _dialogueTrigger;
        private NpcManager _character;

        private Dialogue _currentDialogue;
        private DialogueNode _currentNode;

        private event Action OnEnterNode;
        private event Action OnExitNode;
        public event Action OnStartDialogue;
        public event Action OnEndDialogue;

        public string Name => _talkerName;

        private void Awake()
        {
            _dialogueTrigger = GetComponent<DialogueTrigger>();
            _character = GetComponent<NpcManager>();
        }

        private void OnEnable()
        {
            OnEnterNode += TriggerEnterNodeActions;
            OnEnterNode += TriggerEnterNodeAnimation;

            OnExitNode += TriggerExitNodeActions;
            OnExitNode += TriggerExitNodeAnimation;
        }

        private void OnDisable()
        {
            OnEnterNode -= TriggerEnterNodeActions;
            OnEnterNode -= TriggerEnterNodeAnimation;

            OnExitNode -= TriggerExitNodeActions;
            OnExitNode -= TriggerExitNodeAnimation;
        }

        public void StartDialogue(Dialogue dialogue, bool saveOnRecords = false)
        {
            _currentDialogue = dialogue;
            _currentDialogue.Initialize();

            _currentNode = dialogue.RootNode;

            DialogueHistory.Instance.AddDialogueToHistory(dialogue, this);

            GameUI.DialoguePrompt.Open();
            GameUI.DialoguePrompt.StartDialogue(dialogue);
            GameUI.DialoguePrompt.UpdateCluesInkCounter();
            GameUI.DialoguePrompt.AddNode(_currentNode);
            GameUI.DialoguePrompt.OnNodeFinished += Next;

            OnStartDialogue?.Invoke();
            OnStartDialogue = null;
            OnEnterNode?.Invoke();
        }

        public void RestartDialogueFromHistory(Dialogue dialogue, bool saveOnRecords = false)
        {
            _currentDialogue = dialogue;
            _currentDialogue.Initialize();

            _currentNode = dialogue.RootNode;

            GameUI.DialoguePrompt.Open();
            GameUI.DialoguePrompt.UpdateCluesInkCounter();
            GameUI.DialoguePrompt.AddNode(_currentNode);
            GameUI.DialoguePrompt.OnNodeFinished += Next;

            OnStartDialogue?.Invoke();
            OnStartDialogue = null;
            OnEnterNode?.Invoke();
        }

        private void Next()
        {
            Next(0);
        }

        private void Next(int childIndex)
        {
            GameUI.DialoguePrompt.OnNodeFinished -= Next;
            DialogueNode[] availableNodes = FilterOnConditions(_currentDialogue.ChildrenNodes(_currentNode)).ToArray();

            if (availableNodes.Length > 0)
            {
                OnExitNode?.Invoke();

                var nextNode = availableNodes[childIndex];

                if (nextNode.IsCinematicNode) 
                {
                    UpdateCinematicNode(nextNode);
                }
                else
                {
                    UpdateNode(nextNode);
                }
            }
            else
            {
                QuitDialogue();
            }
        }

        private void UpdateNode(DialogueNode node)
        {
            _currentNode = node;
            GameUI.DialoguePrompt.AddNode(_currentNode);
            GameUI.DialoguePrompt.OnNodeFinished += Next;

            OnEnterNode?.Invoke();
        }

        private void UpdateCinematicNode(DialogueNode node)
        {
            _currentNode = node;
            CinematicManager.Instance.PlayCinematic(node.VideoClip);
            CinematicManager.Instance.OnCinematicEnded += Next;

            OnEnterNode?.Invoke();
        }

        private void QuitDialogue()
        {
            GameUI.DialoguePrompt.OnNodeFinished -= Next;

            OnExitNode?.Invoke();
            OnEndDialogue?.Invoke();
            OnEndDialogue = null;

            GameUI.DialoguePrompt.Close();

            _currentDialogue = null;
            _currentNode = null;
        }

        private IEnumerable<DialogueNode> FilterOnConditions(IEnumerable<DialogueNode> inputList)
        {
            foreach (DialogueNode node in inputList)
            {
                if (node.CheckCondition(GameManager.GetEvaluators))
                {
                    yield return node;
                }
            }
        }

        private void TriggerEnterNodeActions()
        {
            foreach (var action in _currentNode.OnEnterActions)
            {
                DialogueTrigger.Trigger(action, _dialogueTrigger);
            }
        }

        private void TriggerExitNodeActions()
        {
            foreach (var action in _currentNode.OnExitActions)
            {
                DialogueTrigger.Trigger(action, _dialogueTrigger);
            }
        }

        private void TriggerEnterNodeAnimation()
        {
            if (string.IsNullOrEmpty(_currentNode.OnEnterAnimation)) return;

            //_character.PlayOverrideAnimation(_currentNode.OnEnterAnimation);
        }

        private void TriggerExitNodeAnimation()
        {
            if (string.IsNullOrEmpty(_currentNode.OnExitAnimation)) return;

            //_character.PlayOverrideAnimation(_currentNode.OnExitAnimation);
        }
    }
}
