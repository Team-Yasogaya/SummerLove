
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NoName
{
    public class TalkerUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _talkerName;

        private Button button;
        private Npc talker;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Start()
        {
            button.onClick.AddListener(ButtonAction);
        }

        public void InitializeTalkerUI(Npc talker)
        {
            this.talker = talker;
            _talkerName.text = talker.Name;
        }

        private void ButtonAction()
        {
            GameUI.DialogueLibrary.OpenDialogueList(talker);
        }
    }
}