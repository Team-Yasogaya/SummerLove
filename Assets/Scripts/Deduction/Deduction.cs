using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    [CreateAssetMenu(fileName = "New Deduction", menuName = "Deduction")]
    public class Deduction : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string[] _correctClues;

        public string Id => _id;
        public IEnumerable<string> CorrectClues => _correctClues;

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(_id))
            {
                _id = Guid.NewGuid().ToString();
            }
        }
    }
}