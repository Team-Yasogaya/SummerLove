using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NoName
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private bool _isPlayerSpeaking;
        [SerializeField] private string _text;
        [SerializeField] private List<string> _children = new ();
        public Rect Rect = new Rect(0, 0, 200, 150);

        public bool IsPlayerSpeaking { get { return _isPlayerSpeaking; } }
        public string Text { get { return _text; } }
        public List<string> Children { get { return _children; } }

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