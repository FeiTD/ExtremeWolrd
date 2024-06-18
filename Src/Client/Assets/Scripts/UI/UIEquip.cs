using Assets.Scripts.Manager;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquip : UIWindow
{
	public Text Title;
	public GameObject ItemPrefab;
	public GameObject ItemEqupedPrefab;
	public Transform ItemListRoot;
	public List<Image> Slots;
	// Use this for initialization
	void Start()
	{
		RefreshUI();

        EquipManager.Instance.OnEquipChanged += RefreshUI;
	}

    private void OnDestroy()
    {
        EquipManager.Instance.OnEquipChanged -= RefreshUI;
    }
    void RefreshUI()
	{
		ClearAllEquipList();
		InitAllEquipItems();

		ClearEquipedList();
		InitEquipedItems();
    }

	/// <summary>
	/// 显示所有装备
	/// </summary>
	void InitAllEquipItems()
	{
		foreach (var kv in ItemManager.Instance.Items)
		{
			if(kv.Value.ItemDefine.Type == SkillBridge.Message.ItemType.Equip)
			{
				if (EquipManager.Instance.Contains(kv.Key))
				{
					continue;
				}
				GameObject go = Instantiate(ItemPrefab, ItemListRoot);
				UIEqipItem ui = go.GetComponent<UIEqipItem>();
				ui.SetEquipItem(kv.Key,kv.Value,this,false);
			}
		}
	}

	/// <summary>
	/// 清空左侧所有装备列表
	/// </summary>
	void ClearAllEquipList()
	{
		foreach(var item in ItemListRoot.GetComponentsInChildren<UIEqipItem>())
		{
			Destroy(item.gameObject);
		}
	}

	/// <summary>
	/// 清空穿戴的装备
	/// </summary>
	void ClearEquipedList()
	{
		foreach(var item in Slots)
		{
			if(item.transform.childCount > 0)
			{
				Destroy(item.transform.GetChild(0).gameObject);
			}
		}
	}

	/// <summary>
	/// 初始化已经装备的列表
	/// </summary>
	void InitEquipedItems()
	{
		for(int i = 0; i < EquipManager.Instance.Equips.Length;i++)
		{
			var item = EquipManager.Instance.Equips[i];
			if(item != null)
			{
				GameObject go = Instantiate(ItemEqupedPrefab, Slots[i].transform);
				UIEqipItem ui = go.GetComponent<UIEqipItem>();
				ui.SetEquipItem(i, item, this, true);
			}
		}
	}

    internal void UnEquip(Item item)
    {
		EquipManager.Instance.UnEquipItem(item);
    }

    internal void DoEquip(Item item)
    {
        EquipManager.Instance.EquipItem(item);
    }
}
