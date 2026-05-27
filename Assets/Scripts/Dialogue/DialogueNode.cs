using EasyButtons;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using static NoName.Clue;

namespace NoName
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private bool _isPlayerSpeaking;
        [SerializeField] private bool _isCinematicNode;
        [TextArea]
        [SerializeField] private string _text;
        [SerializeField] private List<string> _children = new ();
        public Rect Rect = new Rect(0, 0, 200, 150);

        [SerializeField] List<Clue> _clues;

        [Header("Trigger Actions")]
        [SerializeField] private DialogueAction[] _onEnterActions;
        [SerializeField] private DialogueAction[] _onExitActions;

        [Header("Animations")]
        [SerializeField] private string _onEnterAnimation;
        [SerializeField] private string _onExitAnimation;

        [Header("Conditions")]
        [SerializeField] private Condition _condition;

        [Header("Cinematic Elements")]
        [SerializeField] private VideoClip _videoClip;

        public bool IsPlayerSpeaking { get { return _isPlayerSpeaking; } }
        public string Text { get { return _text; } }
        public List<string> Children { get { return _children; } }
        public List<Clue> Clues { get { return _clues; } }
        public bool IsCinematicNode { get { return _isCinematicNode; } }
        public VideoClip VideoClip { get { return _videoClip; } }

        public IEnumerable<DialogueAction> OnEnterActions
        {
            get
            {
                _onEnterActions ??= new DialogueAction[0];

                foreach (var action in _onEnterActions)
                {
                    yield return action;
                }
            }
        }
        public IEnumerable<DialogueAction> OnExitActions
        {
            get
            {
                _onExitActions ??= new DialogueAction[0];

                foreach (var action in _onExitActions)
                {
                    yield return action;
                }
            }
        }
        public string OnEnterAnimation => _onEnterAnimation;
        public string OnExitAnimation => _onExitAnimation;

        public Clue GetClueByLinkID(string linkId)
        {
            foreach (var clue in _clues)
            {
                if (clue.LinkId.Equals(linkId))
                {
                    return clue;
                }
            }

            return null;
        }

        public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators)
        {
            return _condition.Check(evaluators);
        }

#if UNITY_EDITOR
        public void UpdateText(string text)
        {
            Undo.RecordObject(this, "Node Text Updated");
            _text = text;
        }

        public void AddChild(DialogueNode node)
        {
            if (_children.Contains(node.name)) { return; }

            Undo.RecordObject(this, "Node Linked");

            _children.Add(node.name);
        }

        public void RemoveChild(DialogueNode node)
        {
            Undo.RecordObject(this, "Node Unlinked");

            _children.Remove(node.name);
        }
#endif
    }
}