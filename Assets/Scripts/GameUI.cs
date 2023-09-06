using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    public class GameUI : MonoBehaviour
    {
        public static GameUI Instance;

        [SerializeField] private DialoguePromptUI _dialoguePromptUI;

        public static DialoguePromptUI DialoguePrompt { get { return Instance._dialoguePromptUI; } }

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
    }
}