using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    [System.Serializable]
    public class Condition
    {
        [System.Serializable]
        public enum ConditionType
        {
            None,
            HasItem,
            Custom,
        }

        [SerializeField] ConditionType _conditionType;

        [SerializeField] string _itemId;
        [SerializeField] int _quantity = 1;

        [SerializeField] int _undeadIndex;
        [SerializeField] int _appraisalValue;


        [SerializeField] string _predicate;
        [SerializeField] string[] _parameters;

        public bool Check(IEnumerable<IPredicateEvaluator> evaulators)
        {
            foreach (var evaluator in evaulators)
            {
                bool? result = evaluator.Evaluate(_predicate, _parameters);

                if (result == null) continue;

                if (result == false) return false;
            }

            return true;
        }
    }
}
