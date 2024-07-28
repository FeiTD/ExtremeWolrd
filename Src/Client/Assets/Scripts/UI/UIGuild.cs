using Assets.Scripts.Manager;
using Assets.Scripts.Models;
using Assets.Scripts.Services;
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

    public GameObject leaderview;
    public GameObject memberview;
    public GameObject vleaderview;
    public UIGuildItem selectItem;
    // Use this for initialization
    void Start () {
        RefreshUI();
        GuildService.Instance.OnGuildUpdate += RefreshUI;
        leaderview.SetActive(false); 
        //memberview.SetActive(false);
        vleaderview.SetActive(false);
        GuildService.Instance.SendGuildRequest();
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
        if (!GuildManager.Instance.HasGuild)
            return;
        if (GuildManager.Instance.Guild.LeaderId == Users.Instance.CurrentCharacter.Id)
            leaderview.SetActive(true);
        else
        {
            var info = GuildManager.Instance.GetMemberInfo(Users.Instance.CurrentCharacter);
            if (info != null && info.Title == GuildTitle.VicePresident)
            {
                vleaderview.SetActive(true);
            }
        }
        guildname.text = GuildManager.Instance.Guild.GuildName;
        guildnotice.text = "宣言：" + GuildManager.Instance.Guild.Notice;
        guildmember.text = "成员数量：" + GuildManager.Instance.Guild.memberCount.ToString();
        guildowner.text = "会长：" + GuildManager.Instance.Guild.leaderName;
        guildid.text = "公会ID：" + GuildManager.Instance.Guild.Id.ToString();
        foreach (var info in GuildManager.Instance.Guild.Members)
        {
            GameObject go = Instantiate(prefabs, root);
            UIGuildItem friendItem = go.GetComponent<UIGuildItem>();
            friendItem.Set(info.Info.Name,info.Info.Class.ToString(),info.joinTime.ToString(),info.Info.Level.ToString(),info.Title.ToString(),this,info);
        }
    }

    public void Leave ()
    {
        var mes = MessageBox.Show("确定要离开工会吗？", "", MessageBoxType.Confirm, "确定", "取消");
        mes.OnYes = () =>
        {
            GuildService.Instance.SendGuildLeaveRequest();
            this.Close();
        };
    }

    public void Talk()
    {

    }

    public void Transfer()
    {
        GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, selectItem.info.Id);
    }

    public void QuestList()
    {
        UIManager.Instance.Show<UIGuildApplyList>();
    }

    public void Recall()
    {
        GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost, selectItem.info.Id);
    }

    public void Remotions()
    {
        GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, selectItem.info.Id);
    }

    public void Remove()
    {
        GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout, selectItem.info.Id);
    }
}
