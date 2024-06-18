using Assets.Scripts.Manager;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class UIBag : UIWindow
{
	public Text money;
	public Transform[] pages;
	public GameObject bagItem;
	List<Image> slots;
	// Use this for initialization
	void Start () {
		if(slots == null)
		{
			slots = new List<Image> ();
			for(int page = 0; page< pages.Length; page++)
			{
				slots.AddRange(this.pages[page].GetComponentsInChildren<Image>(true));
			}
		}
		StartCoroutine(InitBags());
	}
	public IEnumerator InitBags()
	{
        money.text = Users.Instance.CurrentCharacter.Gold.ToString();
        for (int i =0;i < BagManager.Instance.Items.Length;i++)
		{
			var item = BagManager.Instance.Items[i];
			if(item.ItemId > 0)
			{
				GameObject go = Instantiate(bagItem, slots[i].transform);
				var ui = go.GetComponent<UIIconBagItem>();
				var def = ItemManager.Instance.Items[item.ItemId].ItemDefine;
				ui.SetMainIcon(def.Icon, item.Count.ToString());
			}
		}
		for(int i = BagManager.Instance.Items.Length; i < slots.Count; i++)
		{
			slots[i].color = Color.gray;
		}
		yield return null;
	}

    public void Reset()
    {
        Clear();
        //BagManager.Instance.Reset();
    }

	public void Clear()
	{
		foreach (var item in slots)
		{
			if(item.transform.childCount > 0)
			Destroy(item.transform.GetChild(0).gameObject);
		}
	}
}
