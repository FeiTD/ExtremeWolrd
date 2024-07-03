using Assets.Scripts.Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIFriendItem : MonoBehaviour,IPointerClickHandler {
	public Text Name;
	public Text Class;
	public Text Level;
	public Text State;
	public Image BG;
    public Sprite NormalBG;
    public Sprite SelectedBG;
	public NCharacterInfo friend;
	UIFriendList owner;

	// Use this for initialization
	void Start () {
        NormalBG = BG.sprite;
		//owner.AddButton.onClick.AddListener(OnAddClick); 
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Set(NFriendInfo info, UIFriendList friendList)
	{
		Name.text = info.friendInfo.Name;
		Class.text = info.friendInfo.Class.ToString();
		Level.text = info.friendInfo.Level.ToString();
		State.text = info.Status == 0 ? "离线" : "在线";
		owner = friendList;
		friend = info.friendInfo;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
		if(owner.selectitem != null)
		{
            owner.selectitem.BG.overrideSprite = NormalBG;
        }
        owner.selectitem = this;
        this.BG.overrideSprite = SelectedBG;
    }
}
