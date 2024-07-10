using Assets.Scripts.Manager;
using Assets.Scripts.UI;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuild : UIWindow {
    public Transform root;
    public Text guildname;
    public Text guildnotice;
    public Text guildmember;
    public Text guildid;
    public Text guildowner;
    public GameObject prefabs;


    public UIGuildItem selectItem;
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
        foreach (UIGuildItem go in root.GetComponentsInChildren<UIGuildItem>())
        {
            Destroy(go.gameObject);
        }
    }

    void InitList()
    {
        foreach (var info in GuildManager.Instance.Guild.Members)
        {
            GameObject go = Instantiate(prefabs, root);
            UIGuildItem friendItem = go.GetComponent<UIGuildItem>();
            friendItem.Set(info.Info.Name,info.Info.Class.ToString(),info.joinTime.ToString(),info.Info.Level.ToString(),this);
        }
    }

    public void Leave ()
    {

    }

    public void Talk()
    {

    }

    public void Transfer()
    {

    }

    public void QuestList()
    {

    }

    public void Recall()
    {

    }

    public void Remotions()
    {

    }

    public void Remove()
    {

    }
}
