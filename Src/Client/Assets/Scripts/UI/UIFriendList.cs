using Assets.Scripts.Manager;
using Assets.Scripts.Models;
using Assets.Scripts.Services;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFriendList : UIWindow {
	public Transform Root;
	public UIFriendItem selectitem;
    public GameObject friendItemPrefab;
	// Use this for initialization
	void Start () {
        FriendService.Instance.OnFriendUpdate += RefreshUI;
        RefreshUI();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnRemoveClick()
    {
        if (selectitem == null)
        {
            MessageBox.Show("请选择要删除的好友");
            return;
        }
        var messagebox = MessageBox.Show("是否删除该好友？");
        messagebox.OnYes = () =>
		{
            FriendService.Instance.SendFriendRemove(selectitem.friend.Id);
		};
    }

    public void OnAddClick()
    {
        var inputbox = InputBox.Show("请输入好友ID", "", "确认", "取消");
        inputbox.OnYes = () =>
        {
            int friendId;
            int.TryParse(inputbox.Input.text, out friendId);
            if (string.IsNullOrEmpty(inputbox.Input.text))
            {
                inputbox.Content.text = "请输入ID";
                return;
            }

            if (friendId == Users.Instance.CurrentCharacter.Id)
            {
                inputbox.Content.text = "不能添加自己";
                return;
            }

            FriendService.Instance.SendFriendAdd(friendId);
            this.OnNoClick();
        };
        inputbox.OnNo = () =>
        {
            this.OnNoClick();
        };
    }

    public void OnTalkClick()
    {

    }

    public void RefreshUI()
    {
        ClearItems();
        InitItems();
    }

    public void ClearItems()
    {
        foreach (UIFriendItem go in Root.GetComponentsInChildren<UIFriendItem>())
        {
                Destroy(go.gameObject);
        }
    }

    public void InitItems()
    {
        foreach(var info in FriendManager.Instance.friends)
        {
            GameObject go = Instantiate(friendItemPrefab, Root);
            UIFriendItem friendItem = go.GetComponent<UIFriendItem>();
            friendItem.Set(info,this);
        }
    }
}
