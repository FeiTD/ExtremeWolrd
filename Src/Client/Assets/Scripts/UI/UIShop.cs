using Assets.Scripts.Manager;
using Assets.Scripts.Models;
using Assets.Scripts.Services;
using Assets.Scripts.UI;
using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIWindow {
	public Text money;
	public Text title;

	public GameObject shopItem;
	ShopDefine shop;
	public Transform[] itemRoot;
	// Use this for initialization
	void Start () {
		StartCoroutine(InitItems());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator InitItems()
	{
		DataManager.Instance.Load();
		foreach(var kv in DataManager.Instance.ShopItems[shop.ID])
		{
			if(kv.Value.Status > 0)
			{
				GameObject go = Instantiate(shopItem, itemRoot[0]);
				UIShopItem ui = go.GetComponent<UIShopItem>();
				ui.SetShopItem(kv.Key,kv.Value,this);
			}
		}
		yield return null;
	}

	public void SetShop(ShopDefine shop)
	{
		this.shop = shop;
		this.title.text = shop.Name;
		this.money.text = Users.Instance.CurrentCharacter.Gold.ToString();
	}

	private UIShopItem selectedItem;
	public void SelectShopItem(UIShopItem item)
	{
		if (selectedItem != null)
			selectedItem.Selected = false;
		selectedItem = item;
	}

	public void OnClickBuy()
	{
		if (selectedItem != null)
            ItemService.Instance.SendBuyItem(shop.ID, selectedItem.ShopItemID);

    }
}
