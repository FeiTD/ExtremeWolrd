using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeamItem : MonoBehaviour {
	public Image Class;
	public Slider HP;
	public Text chname;
	public Image isHeader;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetUI(Sprite type,float hp,string name,bool isheader)
	{
		Class.overrideSprite = type;
		HP.value = hp;
		chname.text = name;
        isHeader.gameObject.SetActive(isheader);
    }
}
