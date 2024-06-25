using Assets.Scripts.Manager;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class UIQuest : UIWindow {
	public Text Title;
	public GameObject ItemPrefab;
	public UIQuestInfo questInfo;
	public Transform MainRoot;
	public Transform BranchRoot;


    // Use this for initialization
    void Start () {
		questInfo.gameObject.SetActive(false);
		InitAllQuestItems();
    }

    // Update is called once per frame
    void Update () {
		
	}

	void InitAllQuestItems()
	{
		foreach(var kv in QuestManager.Instance.Quests)
		{
			if(kv.Value.Define.Type == Common.Data.QuestType.Main)
			{
				GameObject go = Instantiate(ItemPrefab, MainRoot);
				
				var ui = go.GetComponent<UIQuestItem>();
				ui.SetItem(kv.Value, questInfo);
			}
            if (kv.Value.Define.Type == Common.Data.QuestType.Branch)
            {
                GameObject go = Instantiate(ItemPrefab, MainRoot);
                var ui = go.GetComponent<UIQuestItem>();
                ui.SetItem(kv.Value,questInfo);
            }
        }
	}

	public void CheckMain()
	{
		
	}
}
