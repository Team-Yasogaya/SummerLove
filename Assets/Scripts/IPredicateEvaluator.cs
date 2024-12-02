using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    public interface IPredicateEvaluator 
    {
        void EnableEvaluator();
        void DisableEvaluator();
        public bool? Evaluate(string predicateFunctionName, string[] parameters);
    }
}
