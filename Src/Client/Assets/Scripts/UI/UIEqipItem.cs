using Assets.Scripts.Manager;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEqipItem : MonoBehaviour,IPointerClickHandler {
	public Image Icon;
	public Text Title;
	public Text Level;
	public Text LimitClass;
	public Text Category;
	public Image Background;
	public Sprite NormalBg;
	public Sprite SelectedBg;
	UIEquip owner;
	public int index { get;set;}
	bool isEquiped;
	Item item;
    public bool Selected { get; set; }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetEquipItem(int idx, Item item, UIEquip uIEquip, bool isEquiped)
    {
		this.owner = uIEquip;
		this.index = idx;
		this.item = item;
		this.isEquiped = isEquiped;
		if (Title != null) Title.text = item.ItemDefine.Description;
		if (Level != null) Level.text = item.ItemDefine.Level.ToString();
		if (LimitClass != null) LimitClass.text = item.ItemDefine.LimitClass;
		if (Category != null) Category.text = item.ItemDefine.Category;
		if (Icon != null) Icon.overrideSprite = Resloader.Load<Sprite>(this.item.ItemDefine.Icon);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isEquiped)
		{
			UnEquip();
		}
		else
		{
			if (Selected)
			{
				DoEquip();
				Selected = false;
			}
			else
			{
				Selected = true;
			}
		}
    }

    private void DoEquip()
    {
        var msg = MessageBox.Show(string.Format("要装备【{0}】吗？", item.ItemDefine.Name), "确认", MessageBoxType.Confirm);
        msg.OnYes = () =>
        {
			var oldEquip = EquipManager.Instance.GetEquip(item.EquipDefine.Slot);
			if(oldEquip != null)
			{
                var newmsg = MessageBox.Show(string.Format("要替换掉【{0}】吗？", oldEquip.ItemDefine.Name), "确认", MessageBoxType.Confirm);
				newmsg.OnYes = () =>
				{
                    this.owner.DoEquip(this.item);
                };
            }
			else
			{
                this.owner.DoEquip(this.item);
            }
        };
    }

    private void UnEquip()
    {
		var msg = MessageBox.Show(string.Format("要取下【{0}】吗？",item.ItemDefine.Name), "确认", MessageBoxType.Confirm);
		msg.OnYes = () =>
		{
			this.owner.UnEquip(this.item);
		};

    }
}
