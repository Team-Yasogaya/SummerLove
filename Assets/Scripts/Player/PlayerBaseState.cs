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

        protected void Rotate(Vector3 rotation, float deltaTime)
        {
            _playerStateMachine.transform.Rotate(rotation * deltaTime);
        }

        protected Vector3 CalculateMovement()
        {
            Vector3 movement = InputManager.Instance.MoveInput.y * _playerStateMachine.transform.forward;
            movement.y = 0f;
            movement.Normalize();

            return movement;
        }

        protected Vector3 CalculateRotation()
        {
            float rotation = InputManager.Instance.MoveInput.x;

            return new Vector3(0, rotation, 0).normalized;
        }

        protected void BackToLocomotion()
        {
            _playerStateMachine.SwitchState(_playerStateMachine.FreeLookState);
        }
    }
}