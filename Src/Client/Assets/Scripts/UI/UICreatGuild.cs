using Assets.Scripts.Manager;
using Assets.Scripts.Models;
using Assets.Scripts.Services;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICreatGuild : UIWindow {
	public InputField guildname;
	public InputField guildnotice;
	// Use this for initialization
	void Start () {
		GuildService.Instance.OnGuildCreat += OnGuildCreat;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreatGuild()
	{
		var message = InputBox.Show("是否花费0创建工会？","","确定","取消");
		message.OnYes = () =>
		{
			if(Users.Instance.CurrentCharacter.Gold < 0)
			{
				MessageBox.Show("金币不足");
			}
			else
			{
				GuildService.Instance.SendGuildCreat(guildname.text,guildnotice.text);
			}
            message.Close();
        };
	}

	public void OnGuildCreat(bool success)
	{
		if (success)
		{
			
		}
	}
}
