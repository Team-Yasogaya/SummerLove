using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NoName
{
    public class OverworldUI : MonoBehaviour
    {
        [SerializeField] private Button _deductionTableButton;

        private void Awake()
        {
            _deductionTableButton.onClick.AddListener(() => GameUI.OpenDeductionTable());
        }
    }
}