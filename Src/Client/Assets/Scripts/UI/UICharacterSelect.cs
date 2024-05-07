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

	private CharacterClass charClass;
	private int selectCharIndex = -1;
	
	// Use this for initialization
	void Start () {
		UpdateTitle((int)charClass+1);
	}
	
	// Update is called once per frame
	void Update () {
		
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
	}
}
