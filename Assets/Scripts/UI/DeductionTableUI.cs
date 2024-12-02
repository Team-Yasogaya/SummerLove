using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NoName {
    public class DeductionTableUI : BaseMenuUI
    {
        [SerializeField] private RectTransform _deductionClueContainer;
        [SerializeField] private Button _closeButton;

        [Header("Assets")]
        [SerializeField] private GameObject _deductionCluePrefab;

        private void Awake()
        {
            
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(Close);
        }

        private void OnEnable()
        {
            InitializeTable();
        }

        private void OnDisable()
        {
            ClearTable();
        }

        private void InitializeTable()
        {
            ClearTable();

            foreach (var record in DialogueHistory.Instance.RegisteredDialogues)
            {
                foreach (var clue in record.collectedClues)
                {
                    var clueObject = Instantiate(_deductionCluePrefab, _deductionClueContainer);

                    var clueUI = clueObject.GetComponent<DeductionClueUI>();
                    clueUI.Initialize(clue.GetHyperTextClue());
                    clueUI.OnRemove = () => RemoveClueFromDeductionTableConfirmation(clue);
                }
            }
        }

        private void ClearTable()
        {
            foreach (Transform child in _deductionClueContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private void RemoveClueFromDeductionTableConfirmation(DialogueClue clue) 
        {
            GameUI.ConfirmationModal.Show("Are you sure you want to remove this clue?");
            GameUI.ConfirmationModal.OnConfirm += () => RemoveClueFromDeductionTable(clue);
        }

        private void RemoveClueFromDeductionTable(DialogueClue clue) 
        {
            DialogueHistory.Instance.RemoveCollectedClueFromOwnerDialogue(clue);

            // TODO OPTIMIZE
            InitializeTable();
        }
    }
}