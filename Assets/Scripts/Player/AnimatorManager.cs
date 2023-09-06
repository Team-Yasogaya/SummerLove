using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator Animator { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        public void PlayAnimation(int animation, float transitionTime, bool applyRootMotion = true)
        {
            Animator.applyRootMotion = applyRootMotion;
            Animator.CrossFadeInFixedTime(animation, transitionTime);
        }

        public void PlayAnimation(string animation, float transitionTime, bool applyRootMotion = true)
        {
            Animator.applyRootMotion = applyRootMotion;
            Animator.CrossFadeInFixedTime(animation, transitionTime);
        }
    }
}