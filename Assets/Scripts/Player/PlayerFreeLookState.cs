using System;
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
            HandleInputs();
            HandleMovement(delta);
            CheckForInteractables();
        }

        public override void Exit()
        {

        }

        private void HandleInputs()
        {
            if (InputManager.Instance.DeductionInput)
            {
                GameUI.OpenDeductionTable();
                return;
            }

            if (InputManager.Instance.DialoguesInput)
            {
                GameUI.OpenDialogueLibrary();
                return;
            }

            if (InputManager.Instance.InventoryInput)
            {
                GameUI.OpenInventory();
                return;
            }
        }

        private void HandleMovement(float delta)
        {
            Vector3 movement = CalculateMovement();
            Move(movement * _playerStateMachine.RunningSpeed, delta);

            Vector3 rotation = CalculateRotation();
            Rotate(rotation * _playerStateMachine.RotationSpeed, delta);

            _playerStateMachine.UpdateLocomotionAnimationValues(
                rotation.magnitude * Mathf.Sign(InputManager.Instance.MoveInput.x), 
                movement.magnitude * Mathf.Sign(InputManager.Instance.MoveInput.y) * (_playerStateMachine.IsSprinting ? 1 : .5f)
            );
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