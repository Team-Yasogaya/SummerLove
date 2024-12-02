using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace NoName {
    public class ConfirmationModalUI : BaseMenuUI
    {
        [SerializeField] private TextMeshProUGUI _modalText;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;

        public event Action OnConfirm;
        public event Action OnCancel;

        private void OnEnable()
        {
            _confirmButton.onClick.AddListener(ConfirmAction);
            _cancelButton.onClick.AddListener(CancelAction);
        }

        private void OnDisable()
        {
            _confirmButton.onClick?.RemoveListener(ConfirmAction);
            _cancelButton.onClick?.RemoveListener(CancelAction);
        }

        public void SetModalText(string text)
        {
            _modalText.text = text;
        }

        public void Show(string text) 
        {
            SetModalText(text);
            gameObject.SetActive(true);
        }

        public void CloseModalBox()
        {
            gameObject.SetActive(false);
        }

        private void ConfirmAction()
        {
            OnConfirm?.Invoke();
            OnConfirm = null;

            CloseModalBox();
        }

        private void CancelAction()
        {
            OnCancel?.Invoke();
            OnCancel = null;

            CloseModalBox();
        }
    }
}