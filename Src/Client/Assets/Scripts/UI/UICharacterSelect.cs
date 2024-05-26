using Assets.Scripts.Manager;
using Assets.Scripts.Models;
using Assets.Scripts.Services;
using Assets.Scripts.UI;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterSelect : MonoBehaviour {
	public GameObject CharacterCreat;
	public GameObject CharacterSelect;
	public UICharacterView CharacterView;
	public InputField charname;
	public Image[] titles;
	public InputField Dscribtion;


	public GameObject CharInfo;
	public List<GameObject> uiChars = new List<GameObject>();
	public Transform uiCharList;

	private CharacterClass charClass;
	private int selectCharIndex = -1;
	// Use this for initialization
	void Start () {
		UserService.Instance.OnCreatCharacter = OnCreatCharacter;
		InitCharList(true);
		OnSelectClass(0);
		UpdateTitle(0);
	}

	void OnCreatCharacter(Result result, string message)
	{
		if (result == Result.Success)
		{
			InitCharList(true);
		}
		else
			MessageBox.Show(message, "错误", MessageBoxType.Error);
	}

	public void OnClickCreatChar()
    {
        if (string.IsNullOrEmpty(charname.text))
        {
			MessageBox.Show("请输入名称！");
			return;
        }

		UserService.Instance.SendCreatCharacter(charname.text, charClass);
	}

	public void OnCreatNewChar()
    {
		CharacterSelect.SetActive(false);
		CharacterCreat.SetActive(true);
	}

	public void OnBackUp()
    {
        OnSelectClass(0);
        CharacterSelect.SetActive(true);
		CharacterCreat.SetActive(false);
	}
	public void OnSelectClass(int charClass)
    {
		this.charClass = (CharacterClass)charClass;
		CharacterView.CurrentRole = charClass;
		UpdateTitle(charClass);
	}
	public void OnSelectCharacter(int idx)
    {
		for (int i = 0; i < Users.Instance.Info.Player.Characters.Count; i++)
		{
			UICharInfo ci = this.uiChars[i].GetComponent<UICharInfo>();
			ci.Selected = idx == i;
		}
		CharacterView.CurrentRole = (int)Users.Instance.Info.Player.Characters[idx].Class - 1;
		selectCharIndex = idx;
	}

	public void UpdateTitle(int charClass)
    {
		for (int i = 0; i < titles.Length; i++)
		{
			titles[i].gameObject.SetActive(i == charClass);
		}
		DataManager.Instance.Load();
		if (DataManager.Instance.Characters.ContainsKey(charClass))
		{
            Dscribtion.text = DataManager.Instance.Characters[charClass].Description;
        }
		else
		{
			Dscribtion.text = "";
        }	
	}

	public void InitCharList(bool init)
	{
		CharacterSelect.SetActive(true);
		CharacterCreat.SetActive(false);
		if (init)
		{
			foreach (var old in uiChars)
			{
				Destroy(old);
			}
			uiChars.Clear();

            for (int i = 0; i < Users.Instance.Info.Player.Characters.Count; i++)
            {
				GameObject go = Instantiate(CharInfo, this.uiCharList);
				UICharInfo chrInfo = go.GetComponent<UICharInfo>();
				chrInfo.info = Users.Instance.Info.Player.Characters[i];
				chrInfo.charName.text = Users.Instance.Info.Player.Characters[i].Name;
				chrInfo.charClass.text = Users.Instance.Info.Player.Characters[i].Class.ToString();

				Button button = go.GetComponent<Button>();
				int idx = i;
                button.onClick.AddListener(() =>
                {
                    OnSelectCharacter(idx);
                });

                uiChars.Add(go);
				go.SetActive(true);
			}
        }
	}

	public void GameEnter()
    {
		if(selectCharIndex >= 0)
        {
			UserService.Instance.SendGameEnter(selectCharIndex);
        }
    }
}
