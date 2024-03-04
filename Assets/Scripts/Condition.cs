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

        [SerializeField] private ConditionType _conditionType;

        [SerializeField] private string _itemId;
        [SerializeField] private int _quantity = 1;

        [SerializeField] private int _undeadIndex;
        [SerializeField] private int _appraisalValue;


        [SerializeField] private string _predicate;
        [SerializeField] private string[] _parameters;

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
