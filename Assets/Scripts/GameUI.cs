using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    public class GameUI : MonoBehaviour
    {
        public static GameUI Instance;

        [SerializeField] private OverworldUI _overworldUI;
        [SerializeField] private DialoguePromptUI _dialoguePromptUI;
        [SerializeField] private ConfirmationModalUI _confirmationModalUI;
        [SerializeField] private DeductionTableUI _deductionTableUI;

        public static OverworldUI OverworldUI { get { return Instance._overworldUI; } }
        public static DialoguePromptUI DialoguePrompt { get { return Instance._dialoguePromptUI; } }
        public static ConfirmationModalUI ConfirmationModal { get { return Instance._confirmationModalUI; } }
        public static DeductionTableUI DeductionTable { get { return Instance._deductionTableUI; } }
        public static List<BaseMenuUI> MenuStack { get { return Instance._menuStack; } }

        private List<BaseMenuUI> _menuStack;

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
            }
        }
        #endregion
    }
}