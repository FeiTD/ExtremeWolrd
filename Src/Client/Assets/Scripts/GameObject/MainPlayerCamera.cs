using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerCamera : MonoSingleton<MainPlayerCamera> {
	public Camera Camera;
	public Transform ViewPoint;
	public GameObject Player;

	void LateUpdate()
    {
		if(Player == null)
        {
			if (Users.Instance.CurrentCharacterObject == null)
				return;
			Player = Users.Instance.CurrentCharacterObject.gameObject;
        }

		this.transform.position = Player.transform.position;
		this.transform.rotation = Player.transform.rotation;

    }
}
