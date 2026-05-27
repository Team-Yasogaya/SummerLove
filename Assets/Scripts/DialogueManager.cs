using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NoName
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;

        private Dialogue currentDialogue;
        private DialogueNode currentNode;

        private event Action OnEnterNode;
        private event Action OnExitNode;
        public event Action OnStartDialogue;
        public event Action OnEndDialogue;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {

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
            currentDialogue = dialogue;
            currentDialogue.Initialize();

            currentNode = dialogue.RootNode;

            DialogueHistory.Instance.AddDialogueToHistory(dialogue);

            GameUI.DialoguePrompt.Open();
            GameUI.DialoguePrompt.StartDialogue(dialogue);
            GameUI.DialoguePrompt.UpdateCluesInkCounter();
            GameUI.DialoguePrompt.AddNode(currentNode);
            GameUI.DialoguePrompt.OnNodeFinished += Next;

            OnStartDialogue?.Invoke();
            OnStartDialogue = null;
            OnEnterNode?.Invoke();
        }

        public void RestartDialogueFromHistory(Dialogue dialogue, bool saveOnRecords = false)
        {
            currentDialogue = dialogue;
            currentDialogue.Initialize();

            currentNode = dialogue.RootNode;

            GameUI.DialoguePrompt.Open();
            GameUI.DialoguePrompt.StartDialogue(dialogue);
            GameUI.DialoguePrompt.UpdateCluesInkCounter();
            GameUI.DialoguePrompt.AddNode(currentNode);
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
            DialogueNode[] availableNodes = FilterOnConditions(currentDialogue.ChildrenNodes(currentNode)).ToArray();

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
            currentNode = node;
            GameUI.DialoguePrompt.AddNode(currentNode);
            GameUI.DialoguePrompt.OnNodeFinished += Next;

            OnEnterNode?.Invoke();
        }

        private void UpdateCinematicNode(DialogueNode node)
        {
            currentNode = node;
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

            currentDialogue = null;
            currentNode = null;
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
            foreach (var action in currentNode.OnEnterActions)
            {
                DialogueTrigger.Trigger(action, null);
            }
        }

        private void TriggerExitNodeActions()
        {
            foreach (var action in currentNode.OnExitActions)
            {
                DialogueTrigger.Trigger(action, null);
            }
        }

        private void TriggerEnterNodeAnimation()
        {
            if (string.IsNullOrEmpty(currentNode.OnEnterAnimation)) return;
        }

        private void TriggerExitNodeAnimation()
        {
            if (string.IsNullOrEmpty(currentNode.OnExitAnimation)) return;
        }
    }
}