using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    [RequireComponent(typeof(Collider))]
    public abstract class Interactable : MonoBehaviour
    {
        public abstract void ShowPrompt();
        public abstract void HidePrompt();
        public abstract void Interact();
    }
}
