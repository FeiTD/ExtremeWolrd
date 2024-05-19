using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour {
	public GameObject[] roles;
	private int currentRole = 0;
	public int CurrentRole
    {
        get
        {
			return currentRole;
		}
        set
        {
			currentRole = value;
			UpdateRole();

		}
	}
	// Use this for initialization
	void Start () {
		UpdateRole();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateRole()
    {
		for(int i = 0; i < roles.Length; i++)
        {
			roles[i].SetActive(i == CurrentRole);
        }
    }
}
