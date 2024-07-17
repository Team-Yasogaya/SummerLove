using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName {
    public class BaseMenuUI : MonoBehaviour
    {
        public void Open()
        {
            GameUI.Instance.AddMenuToStack(this);

            gameObject.SetActive(true);
        }

        public void Close()
        {
            GameUI.Instance.RemoveMenuFromStack(this);

            gameObject.SetActive(false);
        }
    }
}