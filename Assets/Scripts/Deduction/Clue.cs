using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    [Serializable]
    public enum ClueType
    {
        None = 0,
        Weapon = 1,
        Victim = 2,
        Time = 3,
        Culprit = 4
    }
    
    [CreateAssetMenu(fileName = "New Clue", menuName = "Dialogue/Clue")]
    public class Clue : ScriptableObject
    {
        [SerializeField] string _id;
        [SerializeField] ClueType _type;
        [SerializeField] string _word;
        [SerializeField] string _linkId;


        public string Id => _id;
        public string Word => _word;
        public string LinkId => _linkId;

        void OnValidate()
        {
            if (string.IsNullOrEmpty(_id))
            {
                _id = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrEmpty(_linkId))
            {
                _linkId = GenerateUniqueLinkId();
            }
        }

        private string GenerateUniqueLinkId()
        {
            return Guid.NewGuid().ToString("N")[..16];
        }

        public string GetHyperTextClue()
        {
            return _type switch
            {
                ClueType.None => null,
                ClueType.Weapon => "<link=\"" + LinkId + "\"><color=red>" + Word + "</color></link>",
                ClueType.Victim => "<link=\"" + LinkId + "\"><color=green>" + Word + "</color></link>",
                ClueType.Time => "<link=\"" + LinkId + "\"><color=blue>" + Word + "</color></link>",
                ClueType.Culprit => "<link=\"" + LinkId + "\"><color=orange>" + Word + "</color></link>",
                _ => null,
            };
        }

        public string GetDisabledHyperTextClue()
        {
            return "<color=grey>" + Word + "</color>";
        }
    }
}

