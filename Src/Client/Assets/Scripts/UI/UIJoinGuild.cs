using Assets.Scripts.Manager;
using Assets.Scripts.UI;
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
        RefreshUI();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void RefreshUI()
    {
        ClearList();
        InitList();
    }

    void ClearList()
    {
        foreach (UIJoinGuildItem go in root.GetComponentsInChildren<UIJoinGuildItem>())
        {
            Destroy(go.gameObject);
        }
    }

    void InitList()
    {
        foreach (var info in GuildManager.Instance.AllGuilds)
        {
            GameObject go = Instantiate(prefabs, root);
            UIJoinGuildItem friendItem = go.GetComponent<UIJoinGuildItem>();
            friendItem.Set(info.GuildName,info.Id.ToString(),info.leaderName,info.memberCount.ToString(),this);
        }
    }
    public void JoinRequest()
	{

	}
}
