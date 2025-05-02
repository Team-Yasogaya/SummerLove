using System.Collections;
using System.Collections.Generic;
using NoName;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace NoName
{
    public class StartMenuUI : BaseMenuUI
    {
        [SerializeField] Button _newGameButton;
        [SerializeField] Button _loadGameButton;

        [Header("Assets")]
        [SerializeField] VideoClip _startMenuVideoClip;

        public override void Open()
        {
            base.Open();
        }

        public override void Close()
        {
            base.Close();
        }

        void Start()
        {
            _newGameButton.onClick.AddListener(OnNewGameButtonClicked);
            _loadGameButton.onClick.AddListener(OnLoadGameButtonClicked);
        }

        private void OnNewGameButtonClicked()
        {  
            StartCoroutine(OnNewGameRoutine());
        }

        private void OnLoadGameButtonClicked()
        {
            StartCoroutine(OnLoadGameRoutine());
        }

        private IEnumerator OnNewGameRoutine() 
        {
            SceneManager.LoadScene(1);

            while (CinematicManager.Instance == null)
            {
                yield return null;
            }

            GameManager.Instance.InitPlayer();

            CinematicManager.Instance.PlayCinematic(_startMenuVideoClip);
            CinematicManager.Instance.OnCinematicEnded += () => {
                Debug.Log("Cinematic ended!");
            };

            Close();
        }

        private IEnumerator OnLoadGameRoutine()
        {
            yield return GameManager.Instance.SavingSystem.LoadLastScene("saveFile");
            
            GameManager.Instance.InitPlayer();

            Close();
        }
    }
}

