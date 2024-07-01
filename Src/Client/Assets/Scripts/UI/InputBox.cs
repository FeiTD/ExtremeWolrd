using UnityEditor.VersionControl;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class InputBox
    {
        static Object cacheObject = null;
        public static UIInputBox Show(string tips = "提示",string content = "", string btnOK = "确认", string btnCancel = "取消")
        {
            if (cacheObject == null)
            {
                cacheObject = Resloader.Load<Object>("UI/UIInputBox");
            }
            GameObject go = (GameObject)GameObject.Instantiate(cacheObject);
            UIInputBox msgbox = go.GetComponent<UIInputBox>();
            msgbox.Init(tips, content, btnOK, btnCancel);
            return msgbox;
        }
    }
}
