using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using TMPro; // Se usi TextMeshPro

namespace NoName
{
    public class MultichoiceModalUI : BaseMenuUI
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Transform _buttonContainer;
        [SerializeField] private GameObject _buttonPrefab;

        private TaskCompletionSource<int> _choiceTask;
        private List<GameObject> _instantiatedButtons = new();

        public async Task<int> ShowModal(string title, List<string> options)
        {
            _titleText.text = title;
            _choiceTask = new TaskCompletionSource<int>();

            ClearButtons();
            
            for (int i = 0; i < options.Count; i++)
            {
                int index = i; 
                var btnObj = Instantiate(_buttonPrefab, _buttonContainer);
                
                btnObj.GetComponentInChildren<TextMeshProUGUI>().text = options[i];
                
                btnObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectChoice(index));
                
                _instantiatedButtons.Add(btnObj);
            }

            Open(); 
            
            int result = await _choiceTask.Task;
            
            Close(); 
            return result;
        }

        private void SelectChoice(int index)
        {
            _choiceTask.TrySetResult(index);
        }

        private void ClearButtons()
        {
            foreach (var btn in _instantiatedButtons) Destroy(btn);
            _instantiatedButtons.Clear();
        }

        public void OnConfirm()
        {
            
        }
    }
}