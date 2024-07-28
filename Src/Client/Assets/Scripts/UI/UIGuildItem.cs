using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGuildItem : MonoBehaviour,IPointerClickHandler {
	public Text membername;
    public Text memberclass;
    public Text memberoffice;
    public Text memberjointime;
    public Text memberlevel;
	UIGuild owner;
    Sprite NormalBG;
    public Image BG;
	public Sprite selectBG;
    public NGuildMemberInfo info;
    // Use this for initialization
    void Start () {
        NormalBG = BG.sprite;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Set(string membername, string memberclass,string memberjointime,string memberlevel,string title, UIGuild owner,NGuildMemberInfo info)
	{
		this.membername.text = membername;
		this.memberclass.text = memberclass;
		this.memberjointime.text = memberjointime; 
		this.memberlevel.text = memberlevel;
        this.memberoffice.text = title;
		this.owner = owner;
        this.info = info;
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        if(owner.selectItem != null)
        {
            owner.selectItem.BG.overrideSprite = NormalBG;
        }
        owner.selectItem = this;
        this.BG.overrideSprite = selectBG;
    }
}
