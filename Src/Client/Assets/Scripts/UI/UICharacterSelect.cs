using Assets.Scripts.Manager;
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

	private CharacterClass charClass;
	private int selectCharIndex = -1;
	
	// Use this for initialization
	void Start () {
		CharacterSelect.SetActive(true);
		CharacterCreat.SetActive(false);
		OnSelectClass(1);
		UpdateTitle(1);
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
		CharacterSelect.SetActive(true);
		CharacterCreat.SetActive(false);
	}
	public void OnSelectClass(int charClass)
    {
		this.charClass = (CharacterClass)charClass;
		CharacterView.CurrentRole = charClass - 1;
		UpdateTitle(charClass);
	}
	public void OnSelectCharacter(int idx)
    {
		CharacterView.CurrentRole = idx;
	}

	public void UpdateTitle(int charClass)
    {
		for (int i = 0; i < titles.Length; i++)
		{
			titles[i].gameObject.SetActive(i == charClass - 1);
		}
		Dscribtion.text = DataManager.Instance.Characters[charClass].Description;
	}
}
