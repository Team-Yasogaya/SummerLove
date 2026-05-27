using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;

namespace NoName
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [field: SerializeField] public PlayerStateMachine Player { get; private set; }
        [field: SerializeField] public JsonSavingSystem SavingSystem { get; private set; }

        private static readonly List<IPredicateEvaluator> _evaluatorList = new();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            if (InputManager.Instance.ConsumeSaveInput())
            {
                SavingSystem.Save("saveFile");
            }
        }

        public void InitPlayer() 
        {
            Player = FindObjectOfType<PlayerStateMachine>();
            if (Player == null)
            {
                Debug.LogError("PlayerStateMachine not found in the scene.");
                return;
            }
        }

        #region Conditional Gameplay State
        public static void AddConditionEvaluator(IPredicateEvaluator evaluator)
        {
            if (_evaluatorList.Contains(evaluator))
            {
                Debug.Log("Game Manager already contains this evaluator: " + evaluator);
                return;
            }

            _evaluatorList.Add(evaluator);
        }

        public static void RemoveConditionEvaluator(IPredicateEvaluator evaluator)
        {
            _evaluatorList.Remove(evaluator);
        }

        public static IEnumerable<IPredicateEvaluator> GetEvaluators => _evaluatorList;
        #endregion
    }
}