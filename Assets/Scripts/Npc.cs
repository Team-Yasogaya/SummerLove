using System;
using UnityEngine;

namespace NoName
{
    [CreateAssetMenu(fileName = "New NPC", menuName = "Interactables/NPC", order = 1)]

    public class Npc : ScriptableObject
    {
        [Header("Data")]
        [SerializeField] string _id;
        [SerializeField] string _name;

        [Header("Interaction")]
        [SerializeField] string _promptText;

        public string Id => _id;
        public string Name => _name;

        void OnValidate()
        {
            if (string.IsNullOrEmpty(_id))
            {
                _id = Guid.NewGuid().ToString();
            }
        }
    }
}
