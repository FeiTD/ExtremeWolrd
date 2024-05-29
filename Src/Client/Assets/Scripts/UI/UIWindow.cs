using Assets.Scripts.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public abstract class UIWindow:MonoBehaviour
    {
        public enum WindowResult
        {
            None = 0,
            Yes,
            No,
        }
        public delegate void CloseHandler(UIWindow sender,WindowResult result);
        public event CloseHandler OnClose;
        public virtual Type Type { get { return this.GetType(); } }

        public void Close(WindowResult result = WindowResult.None)
        {
            UIManager.Instance.Close(this.Type);
            if(OnClose != null)
                OnClose(this,result);
            OnClose = null;
        }

        public virtual void OnCloseClick()
        {
            Close();
        }

        public virtual void OnYesClick()
        {
            Close(WindowResult.Yes);
        }

        public virtual void OnNoClick()
        {
            Close(WindowResult.No);
        }
    }
}
