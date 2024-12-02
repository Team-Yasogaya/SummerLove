using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    public class TransitionUI : MonoBehaviour
    {
        protected Coroutine _transitionRoutine;

        public event Action OnTransitionComplete;

        public virtual void StartTransition()
        {
            gameObject.SetActive(true);
            _transitionRoutine = StartCoroutine(TransitionRoutine());
        }

        protected virtual IEnumerator TransitionRoutine()
        {
            yield return null;

            OnTransitionComplete?.Invoke();
            OnTransitionComplete = null;
            gameObject.SetActive(false);
        }
    }
}
