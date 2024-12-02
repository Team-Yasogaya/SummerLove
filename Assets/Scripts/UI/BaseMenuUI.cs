using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName {
    public class BaseMenuUI : MonoBehaviour
    {
        public virtual void Open()
        {
            GameUI.Instance.AddMenuToStack(this);

            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            GameUI.Instance.RemoveMenuFromStack(this);

            gameObject.SetActive(false);
        }
    }
}