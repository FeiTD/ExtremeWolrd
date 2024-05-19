using Assets.Scripts.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINameBar : MonoBehaviour {
	public Text Name;

	public Character Character;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UpdateText();
		//this.transform.forward = Camera.main.transform.forward;
	}

	void UpdateText()
    {
		if (this.Character != null)
		{
			string name = Character.Name + " Lv." + Character.Info.Level;
			if(name != Name.text)
            {
				Name.text = name;
            }
		}

	}
}
