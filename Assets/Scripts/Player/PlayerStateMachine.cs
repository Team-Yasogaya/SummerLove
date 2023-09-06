using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    public class PlayerStateMachine : StateMachine
    {
        [Header("Character Parameters")]
        [SerializeField] private float _runningSpeed;
        [SerializeField] private float _walkingSpeed;
        [SerializeField] private float _sprintSpeed;
        [SerializeField] private float _rotationSmoothTime = .3f;
        [SerializeField] private float _animationDampTime = .1f;
        [SerializeField] private float _interactionRange;

        [Header("Utilities")]
        [SerializeField] private Transform _cameraTarget;

        public CharacterController Controller { get; private set; }
        public Animator Animator { get; private set; }
        public AnimatorManager AnimatorManager { get; private set; }
        public Transform MainCameraTransform { get; private set; }
        public ForceReceiver ForceReceiver { get; private set; }


        public float RunningSpeed { get { return _runningSpeed; } }
        public float WalkingSpeed { get { return _walkingSpeed; } }
        public float SprintSpeed { get { return _sprintSpeed; } }
        public float RotationSmoothTime { get { return _rotationSmoothTime; } }
        public float InteractionRange { get { return _interactionRange; } }
        public Transform CameraTarget { get { return _cameraTarget; } }


        // PLAYER STATE DEFINITION
        public PlayerFreeLookState FreeLookState;

        // STATUS VARIABLES
        public bool IsSprinting { get; set; }

        private void Awake()
        {
            Controller = GetComponent<CharacterController>();
            Animator = GetComponent<Animator>();
            AnimatorManager = GetComponent<AnimatorManager>();
            ForceReceiver = GetComponentInChildren<ForceReceiver>();
        }

        private void Start()
        {
            MainCameraTransform = Camera.main.transform;

            FreeLookState = new PlayerFreeLookState(this);

            SwitchState(FreeLookState);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        protected override void Update()
        {
            base.Update();
        }

        public void UpdateLocomotionAnimationValues(float horizontal, float vertical, bool snap = false)
        {
            if (snap)
            {
                AnimatorManager.Animator.SetFloat(AnimationNames.HORIZONTAL, horizontal);
                AnimatorManager.Animator.SetFloat(AnimationNames.VERTICAL, vertical);
            }
            else
            {
                AnimatorManager.Animator.SetFloat(AnimationNames.HORIZONTAL, horizontal, _animationDampTime, Time.deltaTime);
                AnimatorManager.Animator.SetFloat(AnimationNames.VERTICAL, vertical, _animationDampTime, Time.deltaTime);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _interactionRange);
        }
#endif
    }
}