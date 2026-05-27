
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NoName
{
    public class DeductionTimeTransitionUI : TransitionUI
    {
        [SerializeField] TextMeshProUGUI _deductionText;
        [SerializeField] float _charactersPerSeconds;
        
        protected override IEnumerator TransitionRoutine()
        {
            string textToType = "Deduction \n Time";
            _deductionText.text = "";

            foreach (char c in textToType.ToCharArray())
            {
                _deductionText.text += c;
                yield return new WaitForSeconds(1 / _charactersPerSeconds);
            }

            yield return new WaitForSeconds(2);

            yield return base.TransitionRoutine();
        }
    }
}