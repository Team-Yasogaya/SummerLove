using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine _playerStateMachine;

        // UTILITY VARIABLES
        protected Quaternion targetRotation;
        protected Vector3 lookPos;

        public PlayerBaseState(PlayerStateMachine playerStateMachine)
        {
            _playerStateMachine = playerStateMachine;
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            _playerStateMachine.Controller.Move((motion + _playerStateMachine.ForceReceiver.Movement) * deltaTime);
        }

        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected Vector3 CalculateMovement()
        {
            Vector3 movement = InputManager.Instance.MoveInput.x * _playerStateMachine.MainCameraTransform.right;
            movement += InputManager.Instance.MoveInput.y * _playerStateMachine.MainCameraTransform.forward;
            movement.y = 0f;
            movement.Normalize();

            return movement;
        }

        protected void BackToLocomotion()
        {
            _playerStateMachine.SwitchState(_playerStateMachine.FreeLookState);
        }
    }
}