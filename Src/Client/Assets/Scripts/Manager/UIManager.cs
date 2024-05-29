using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    
    public class UIManager:Singleton<UIManager>
    {
        class UIElement
        {
            public string Resource;
            public bool Cache;
            public GameObject Instance;
        }
        Dictionary<Type,UIElement> UIResource = new Dictionary<Type,UIElement>();
        public UIManager()
        {
            UIResource.Add(typeof(UITips),new UIElement() { Cache = true,Resource="UI/UITips"});
        }
        public T Show<T>()
        {
            Type type = typeof(T);
            if (this.UIResource.ContainsKey(type))
            {
                UIElement info = this.UIResource[type];
                if(info.Instance != null)
                {
                    info.Instance.SetActive(false);
                }
                else
                {
                    UnityEngine.Object prefab = Resources.Load(info.Resource);
                    if(prefab != null)
                    {
                        return default(T);
                    }
                    info.Instance = (GameObject)GameObject.Instantiate(prefab);
                }
                return info.Instance.GetComponent<T>();
            }
            return default(T);
        }

        public void Close(Type type)
        {
            if(UIResource.ContainsKey(type))
            {
                UIElement info = UIResource[type];
                if (info.Cache)
                {
                    info.Instance.SetActive(false);
                }
                else
                {
                    GameObject.Destroy(info.Instance);
                    info.Instance = null;
                }
            }
        }
    }
}
