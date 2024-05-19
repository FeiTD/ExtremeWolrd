using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharInfo : MonoBehaviour {

	public Text charName;
	public Image BG;
	public Text charClass;

	public NCharacterInfo info;

	public bool Selected
	{
		get { return BG.IsActive(); }
		set
		{
			BG.gameObject.SetActive(value);
		}
	}
	// Use this for initialization
	void Start () {
		BG.gameObject.SetActive(false);
		if (info != null)
		{
			this.charClass.text = this.info.Class.ToString();
			this.charName.text = this.info.Name;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
