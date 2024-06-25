using Assets.Scripts.Models;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestInfo : UIWindow {
	public Text QuestDecription;
	public Text QuestTarget;
	public List<Transform> QuestReward;
	public Text QuestMoney;
	public Text QuestExp;
    public UIQuestItem QuestItem;
    public Button ComPeleted;
    public Button Accept;
    public Button Cancel;
    public Quest quest;
    // Use this for initialization
    void Start () {
		//this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetQuest(Quest quest)
	{
        this.gameObject.SetActive(true);
		this.quest = quest;
        UpdateUI();
        if (quest.Info == null)
		{
            ComPeleted.gameObject.SetActive(false);
            Accept.gameObject.SetActive(true);
			Cancel.gameObject.SetActive(true);
        }
		else
		{
			if(quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
			{
                ComPeleted.gameObject.SetActive(false);
                Accept.gameObject.SetActive(true);
                Cancel.gameObject.SetActive(true);
            }
			else
			{
                ComPeleted.gameObject.SetActive(false);
                Accept.gameObject.SetActive(false);
                Cancel.gameObject.SetActive(false);
            }
		}
    }
    private void UpdateUI()
    {
        QuestDecription.text = quest.Define.Name;
        QuestMoney.text = quest.Define.RewardGold.ToString();
        QuestExp.text = quest.Define.RewardExp.ToString();
        QuestTarget.text = quest.Define.Overview;
    }
}
