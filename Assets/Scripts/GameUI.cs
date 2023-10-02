using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    public class GameUI : MonoBehaviour
    {
        public static GameUI Instance;

        [SerializeField] private DialoguePromptUI _dialoguePromptUI;
        [SerializeField] private ConfirmationModalUI _confirmationModalUI;
        [SerializeField] private DeductionTableUI _deductionTableUI;

        public static DialoguePromptUI DialoguePrompt { get { return Instance._dialoguePromptUI; } }
        public static ConfirmationModalUI ConfirmationModal { get { return Instance._confirmationModalUI; } }
        public static DeductionTableUI DeductionTable { get { return Instance._deductionTableUI; } }

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