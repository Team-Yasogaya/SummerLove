using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    public class ItemLootManager : Interactable
    {
        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
        }

        private void Start()
        {
            
        }

        public override void ShowPrompt()
        {
            
        }

        public override void HidePrompt()
        {
            
        }

        public override void Interact()
        {

        }
    }
}
