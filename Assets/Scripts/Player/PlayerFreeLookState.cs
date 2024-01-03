using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    public class PlayerFreeLookState : PlayerBaseState
    {


        // UTILITIES
        private Interactable closestInteractable;
        private float closestDistance;


        public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
            _playerStateMachine.Animator.Play(AnimationNames.FREE_LOOK_1H);
        }

        public override void Tick(float delta)
        {
            HandleMovement(delta);
            CheckForInteractables();
        }

        public override void Exit()
        {
            
        }

        private void HandleMovement(float delta)
        {
            Vector3 movement = CalculateMovement();
            Move(movement * _playerStateMachine.RunningSpeed, delta);

            if (movement.sqrMagnitude != 0)
            {
                targetRotation = Quaternion.LookRotation(movement);
                _playerStateMachine.transform.rotation = Quaternion.Slerp(_playerStateMachine.transform.rotation, targetRotation, _playerStateMachine.RotationSmoothTime * delta);
            }

            _playerStateMachine.UpdateLocomotionAnimationValues(0f, movement.magnitude * (_playerStateMachine.IsSprinting ? 2 : 1));
        }

        private void CheckForInteractables()
        {
            InputManager.Instance.ResetInteract();

            Collider[] collisions = Physics.OverlapSphere(_playerStateMachine.transform.position, _playerStateMachine.InteractionRange);

            closestDistance = Mathf.Infinity;
            closestInteractable = null;

            foreach (Collider collision in collisions)
            {
                if (!collision.TryGetComponent(out Interactable interactable)) continue;

                float distance = Vector3.Distance(_playerStateMachine.transform.position, interactable.transform.position);

                if (distance > closestDistance) continue;

                closestInteractable = interactable;
                closestDistance = distance;
            }

            if (closestInteractable == null) return;

            closestInteractable.ShowPrompt();

            InputManager.Instance.InteractEvent += closestInteractable.Interact;
            InputManager.Instance.InteractEvent += FaceInteractable;
        }

        private void FaceInteractable()
        {
            if (closestInteractable == null) return;

            Vector3 targetDirection = closestInteractable.transform.position - _playerStateMachine.transform.position;
            targetDirection.y = 0f;

            _playerStateMachine.transform.rotation = Quaternion.LookRotation(targetDirection);
        }
    }
}