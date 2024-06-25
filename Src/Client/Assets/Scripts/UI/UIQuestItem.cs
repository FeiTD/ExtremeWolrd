using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIQuestItem : MonoBehaviour , IPointerClickHandler
{
	public Text QuestDescription;
	public Image Bg;
	private bool selected;

	Quest quest;
	UIQuestInfo info;
	public bool Selected
	{
		get { return selected; }
		set 
		{
			selected = value;
			if (selected)
			{
				Bg.color = Color.green;
			}
			else
			{
				Bg.color = Color.yellow;
			}
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetItem(Quest quest,UIQuestInfo info)
	{
		this.quest = quest;
		this.info = info;
        QuestDescription.text = quest.Define.Name;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (info.QuestItem != null)
        {
            info.QuestItem.Selected = false;
        }
        info.QuestItem = this;
        info.SetQuest(quest);
        Selected = true;
    }
}
