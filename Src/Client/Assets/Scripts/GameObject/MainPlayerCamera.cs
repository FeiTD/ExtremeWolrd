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
			return;
        }

		this.transform.position = Player.transform.position;
		this.transform.rotation = Player.transform.rotation;

    }
}
