using Assets.Scripts.Manager;
using Assets.Scripts.UI;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuildApplyList : UIWindow {
    public Transform Root;
    public UIGuildApplyItem selectitem;
    public GameObject prefabs;
    // Use this for initialization
    void Start () {
        RefreshUI();
    }

    private void RefreshUI()
    {
        ClearItems();
        InitItems();
    }

    private void InitItems()
    {
        foreach (var info in GuildManager.Instance.Guild.Applies)
        {
            GameObject go = Instantiate(prefabs, Root);
            UIGuildApplyItem apply = go.GetComponent<UIGuildApplyItem>();
            apply.Set(info, this);
        }
    }

    private void ClearItems()
    {
        foreach (UIGuildApplyItem go in Root.GetComponentsInChildren<UIGuildApplyItem>())
        {
            Destroy(go.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
