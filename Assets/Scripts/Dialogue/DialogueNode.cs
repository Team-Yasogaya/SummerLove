using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NoName
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private bool _isPlayerSpeaking;
        [TextArea]
        [SerializeField] private string _text;
        [SerializeField] private List<string> _children = new ();
        public Rect Rect = new Rect(0, 0, 200, 150);

        [SerializeField] private List<DialogueClue> _dialogueClues = new();

        public bool IsPlayerSpeaking { get { return _isPlayerSpeaking; } }
        public string Text { get { return _text; } }
        public List<string> Children { get { return _children; } }
        public List<DialogueClue> DialogueClues { get { return _dialogueClues; } }

        [System.Serializable]
        public enum ClueType
        {
            None = 0,
            Weapon = 1,
            Victim = 2, 
            Time = 3,
            Culprit = 4
        }

        [System.Serializable]
        public class DialogueClue
        {
            
            public string ID;
            public string Word;

            [SerializeField] private ClueType _type;

            public DialogueClue(ClueType type, string id)
            {
                ID = id;
                _type = type;
            }

            public string GetHyperTextClue()
            {
                switch (_type)
                {
                    case ClueType.None:
                        return null;
                    case ClueType.Weapon:
                        return "<link=" + ID + "><color=red>" + Word + "</color></link>";
                    case ClueType.Victim:
                        return "<link=" + ID + "><color=green>" + Word + "</color></link>";
                    case ClueType.Time:
                        return "<link=" + ID + "><color=blue>" + Word + "</color></link>";
                    case ClueType.Culprit:
                        return "<link=" + ID + "><color=orange>" + Word + "</color></link>";
                    default:
                        return null;
                }
            }

            public string GetDisabledHyperTextClue() {
                return "<color=gray>" + Word + "</color>";
            }
        }

        [Button]
        private void AddWeaponClue()
        {
            _dialogueClues.Add(new DialogueClue(ClueType.Weapon, "WEAPON_" + _dialogueClues.Count));
        }

        [Button]
        private void AddVictimClue()
        {
            _dialogueClues.Add(new DialogueClue(ClueType.Victim, "VICTIM_" + _dialogueClues.Count));
        }

        [Button]
        private void AddTimeClue()
        {
            _dialogueClues.Add(new DialogueClue(ClueType.Time, "TIME_" + _dialogueClues.Count));
        }

        [Button]
        private void AddCulpritClue()
        {
            _dialogueClues.Add(new DialogueClue(ClueType.Culprit, "CULPRIT_" + _dialogueClues.Count));
        }

        public DialogueClue GetDialogueClueByID(string id)
        {
            foreach (var clue in _dialogueClues)
            {
                if (clue.ID.Equals(id))
                {
                    return clue;
                }
            }

            return null;
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