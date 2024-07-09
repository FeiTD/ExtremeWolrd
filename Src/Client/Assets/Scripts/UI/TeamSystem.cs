using Assets.Scripts.Manager;
using Assets.Scripts.Models;
using Assets.Scripts.Services;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamSystem : MonoBehaviour {
	public Text title;
	public Transform root;
	public GameObject teamitemPrefab;
	// Use this for initialization
	void Start () {
		TeamService.Instance.OnRefrenshTeam += RefreshUI;

        RefreshUI();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void RefreshUI()
	{
		ClearItem();
		InitUI();
	}

	void ClearItem()
	{
        foreach (UITeamItem go in root.GetComponentsInChildren<UITeamItem>())
        {
            Destroy(go.gameObject);
        }
    }

	void InitUI()
	{
		title.text = string.Format("我的队伍({0}/5)", Users.Instance.TeamInfo.Members.Count); 

        foreach (var member in Users.Instance.TeamInfo.Members)
		{
			GameObject go = Instantiate(teamitemPrefab, root);
            UITeamItem teamitem = go.GetComponent<UITeamItem>();
            teamitem.SetUI(SpriteManager.Instance.GetClassimage((int)member.Class - 1), 1,member.Name,member.Id == Users.Instance.TeamInfo.Leader);
		}
	}

	public void LeaveTeam()
	{
		var message = MessageBox.Show("是否离开队伍？", "", MessageBoxType.Confirm, "确定", "取消");
		message.OnYes = () =>
		{
			TeamService.Instance.SendTeamLeaveReq(Users.Instance.TeamInfo.TeamId, Users.Instance.CurrentCharacter.Id);
		};
	}
}
