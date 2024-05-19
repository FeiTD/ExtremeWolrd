using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainCity : MonoBehaviour {

	public Text Name;
	public Text Level;
	// Use this for initialization
	void Start () {
		UpdateAvatar();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateAvatar()
    {
		this.Name.text = Users.Instance.CurrentCharacter.Name;
		this.Level.text = Users.Instance.CurrentCharacter.Level.ToString();
    }
}
