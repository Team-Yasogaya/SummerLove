using UnityEngine;

namespace NoName
{
    [CreateAssetMenu(fileName = "New NPC", menuName = "Interactables/NPC", order = 1)]

    public class Npc : ScriptableObject
    {
        [Header("Interaction")]
        [SerializeField] private string _promptText;

    }
}
