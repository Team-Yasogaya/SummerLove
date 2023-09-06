using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    public class NpcManager : Interactable
    {
        [SerializeField] private Npc _npc;

        private Collider _collider;
        private SpriteRenderer _spriteRenderer;
        private Talker _talker;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;

            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _talker = GetComponent<Talker>();
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
            FacePlayer(GameManager.Instance.Player);

            _talker.Talk();
        }

        private void FacePlayer(PlayerStateMachine playerManager)
        {
            Vector3 faceDirection = playerManager.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(faceDirection);
        }
    }
}
