using Assets.Scripts.Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGuildApplyItem : MonoBehaviour ,IPointerClickHandler{
    public Text Name;
    public Text Class;
    public Text Level;
    public Image BG;
    public Sprite NormalBG;
    public Sprite SelectedBG;
    UIGuildApplyList owner;
    NGuildApplyInfo info;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (owner.selectitem != null)
        {
            owner.selectitem.BG.overrideSprite = NormalBG;
        }
        owner.selectitem = this;
        this.BG.overrideSprite = SelectedBG;
    }

    internal void Set(NGuildApplyInfo item, UIGuildApplyList uIGuildApplyList)
    {
        info = item;
        this.Name.text = item.Name; 
        this.Class.text = ((CharacterClass)item.Class).ToString();
        this.Level.text = item.Level.ToString();
        owner = uIGuildApplyList;
    }

    public void Accept()
    {
        GuildService.Instance.SendGuildJoinApply(true, info);
    }

    public void Refuse()
    {
        GuildService.Instance.SendGuildJoinApply(false, info);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
