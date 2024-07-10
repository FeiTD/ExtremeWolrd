using Assets.Scripts.Manager;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICreatOrJoinGuild : UIWindow {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreatGuild()
	{
		UIManager.Instance.Show<UICreatGuild>();
	}

	public void JoinGuild()
	{
        UIManager.Instance.Show<UIJoinGuild>();
    }
}
