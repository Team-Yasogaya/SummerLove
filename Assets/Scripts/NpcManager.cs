using UnityEngine;

namespace NoName
{
    public class NpcManager : Interactable
    {
        [SerializeField] private Npc _npc;
        [SerializeField] private Dialogue _npcDialogue;

        private Collider interactionCollider;

        private void Awake()
        {
            interactionCollider = GetComponent<Collider>();
            interactionCollider.isTrigger = true;
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

            DialogueManager.Instance.StartDialogue(_npcDialogue);
        }

        private void FacePlayer(PlayerStateMachine playerManager)
        {
            Vector3 faceDirection = playerManager.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(faceDirection);
        }
    }
}
