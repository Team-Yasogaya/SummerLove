using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    public class StateMachine : MonoBehaviour
    {
        private State _currentState;

        protected virtual void Update()
        {
            _currentState?.Tick(Time.deltaTime);
        }

        public void SwitchState(State newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();

            Debug.Log("Entering " + newState.ToString());
        }
    }
}