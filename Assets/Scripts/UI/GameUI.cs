using EasyButtons;
using NoName.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    public class GameUI : MonoBehaviour
    {
        public static GameUI Instance { get; private set; }

        [SerializeField] private OverworldUI _overworldUI;
        [SerializeField] private DialoguePromptUI _dialoguePromptUI;
        [SerializeField] private ConfirmationModalUI _confirmationModalUI;
        [SerializeField] private DeductionTableUI _deductionTableUI;
        [SerializeField] private DialogueLibraryUI _dialogueLibraryUI;
        [SerializeField] private InventoryUI _inventoryUI;
        [SerializeField] private VideoPlayerUI _videoPlayerUI;

        [Header("Transitions")]
        [SerializeField] TransitionUI _deductionTimeTransition;

        public static OverworldUI OverworldUI { get { return Instance._overworldUI; } }
        public static DialoguePromptUI DialoguePrompt { get { return Instance._dialoguePromptUI; } }
        public static ConfirmationModalUI ConfirmationModal { get { return Instance._confirmationModalUI; } }
        public static DeductionTableUI DeductionTable { get { return Instance._deductionTableUI; } }
        public static DialogueLibraryUI DialogueLibrary { get { return Instance._dialogueLibraryUI; } }
        public static InventoryUI Inventory { get { return Instance._inventoryUI; } }
        public static VideoPlayerUI VideoPlayer { get { return Instance._videoPlayerUI; } } 
        public static List<BaseMenuUI> MenuStack { get { return Instance._menuStack; } }

        private List<BaseMenuUI> _menuStack;

        public static event Action OnBackToGame;

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

        private void Start()
        {
            InitializeGameUI();
        }

        private void InitializeGameUI()
        {
            _menuStack = new();
        }

        public static void OpenDeductionTable()
        {
            DeductionTable.Open();
        }

        public static void OpenDialogueLibrary()
        {
            DialogueLibrary.Open();
        }

        public static void OpenInventory()
        {
            Inventory.Open();
        }

        [Button("Start Deduction Time")]
        private void StartDeductionTime()
        {
            _deductionTimeTransition.OnTransitionComplete += OpenDeductionTable;
            _deductionTimeTransition.StartTransition();
        }

        #region Menu Stack
        public void AddMenuToStack(BaseMenuUI menu)
        {
            if (MenuStack.Contains(menu) == true)
            {
                Debug.Log("You're opening two times the same menu. This shouldn't happen");
                return;
            }

            MenuStack.Add(menu);

            InputManager.Instance.DisablePlayerControls();
            OverworldUI.gameObject.SetActive(false);
        }

        public void RemoveMenuFromStack(BaseMenuUI menu)
        {
            if (MenuStack.Contains(menu) == false)
            {
                Debug.Log("You're closing a menu which is not in the open stack. This shouldn't happen");
                return;
            }

            MenuStack.Remove(menu);

            if (MenuStack.Count == 0)
            {
                InputManager.Instance.EnablePlayerControls();
                OverworldUI.gameObject.SetActive(true);

                OnBackToGame?.Invoke();
            }
        }
        #endregion
    }
}