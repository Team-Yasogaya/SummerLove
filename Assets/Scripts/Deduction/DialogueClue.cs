using static NoName.DialogueNode;
using UnityEngine;
using System;

namespace NoName
{
    [System.Serializable]
    public class DialogueClue
    {
        [System.Serializable]
        public enum ClueType
        {
            None = 0,
            Weapon = 1,
            Victim = 2,
            Time = 3,
            Culprit = 4
        }

        [SerializeField] private ClueType _type;

        private Guid _id;
        private string _word;

        public Guid Id => _id;
        public string Word => _word;

        public DialogueClue(ClueType type, Guid id)
        {
            _id = id;
            _type = type;
        }

        public string GetHyperTextClue()
        {
            switch (_type)
            {
                case ClueType.None:
                    return null;
                case ClueType.Weapon:
                    return "<link=" + Id + "><color=red>" + Word + "</color></link>";
                case ClueType.Victim:
                    return "<link=" + Id + "><color=green>" + Word + "</color></link>";
                case ClueType.Time:
                    return "<link=" + Id + "><color=blue>" + Word + "</color></link>";
                case ClueType.Culprit:
                    return "<link=" + Id + "><color=orange>" + Word + "</color></link>";
                default:
                    return null;
            }
        }

        public string GetDisabledHyperTextClue()
        {
            return "<color=grey>" + Word + "</color>";
        }
    }
}