using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace NoName
{
    public class DeductionClueUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI _clueText;
        [SerializeField] private Button _removeButton;

        public Action OnRemove;

        public void Initialize(string text)
        {
            _clueText.text = text;

            _removeButton.onClick.AddListener(OnRemoveAction);
            _removeButton.gameObject.SetActive(false);
        }

        private void OnRemoveAction()
        {
            if (OnRemove == null)
            {
                Debug.Log("You called remove action with no event defined. This shouldn't happen");
            }

            OnRemove?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _removeButton.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _removeButton.gameObject.SetActive(false);
        }
    }
}