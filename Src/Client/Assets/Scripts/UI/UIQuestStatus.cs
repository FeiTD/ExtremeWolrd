using Assets.Scripts.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestStatus : MonoBehaviour {

	public List<Image> images;
	private NpcQuestStatus QuestStatus;

    public void SetQuestStatus(NpcQuestStatus status)
    {
		QuestStatus = status;
		for(int i = 0; i < 4; i++)
		{
			if (images[i] != null)
			{
				images[i].gameObject.SetActive(i == (int)status);
			}
		}
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
