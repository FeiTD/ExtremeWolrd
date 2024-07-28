using Assets.Scripts.Manager;
using Assets.Scripts.Services;
using Assets.Scripts.UI;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIJoinGuild : UIWindow {
	public Transform root;
	public Text guildname;
	public Text guildnotice;
	public Text guildmember;
	public Text guildid;
    public Text guildowner;
    public GameObject prefabs;
    public GameObject guildInfo;
    public UIJoinGuildItem selectItem;
    // Use this for initialization
    void Start () {
        GuildService.Instance.OnGuildListResult += RefreshUI;
        guildInfo.SetActive(false);
        GuildService.Instance.SendGuildListReq();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void RefreshUI(List<NGuildInfo> AllGuilds)
    {
        ClearList();
        InitList(AllGuilds);
    }

    void ClearList()
    {
        foreach (UIJoinGuildItem go in root.GetComponentsInChildren<UIJoinGuildItem>())
        {
            if(root.gameObject !=  go.gameObject)
            Destroy(go.gameObject);
        }
    }

    void InitList(List<NGuildInfo> AllGuilds)
    {
        foreach (var info in AllGuilds)
        {
            GameObject go = Instantiate(prefabs, root);
            UIJoinGuildItem friendItem = go.GetComponent<UIJoinGuildItem>();
            friendItem.Set(info.GuildName, info.Id.ToString(), info.leaderName, info.memberCount.ToString(), this, info);
        }
    }

    public void Set(string guildname,string guildid,string guildmembercount,string guildnotice,string guildowner)
    {
        this.guildname.text = guildname;
        this.guildid.text = guildid;
        this.guildnotice.text = guildnotice;
        this.guildowner.text = guildowner;
        this.guildmember.text = guildmembercount;
    }

    public void JoinRequest()
	{
        GuildService.Instance.SendGuildJoinRequest(selectItem.guild.Id);
	}
}
