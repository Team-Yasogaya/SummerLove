using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    public class ForceReceiver : MonoBehaviour
    {
        [SerializeField] private float _drag = .3f;

        private CharacterController _controller;

        private float _verticalVelocity;
        private Vector3 _impact;
        private Vector3 _dampingVelocity;

        public Vector3 Movement => _impact + Vector3.up * _verticalVelocity;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (_controller.isGrounded && _verticalVelocity < 0f)
            {
                _verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                _verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, _drag);
        }

        public void AddForce(Vector3 force)
        {
            _impact += force;
        }
    }
}
