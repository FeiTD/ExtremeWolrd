using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElement : MonoBehaviour {
	public Transform Owner;

	public float height = 1.9f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Owner != null)
		{
			this.transform.position = Owner.position + Vector3.up * height;
		}

        if (Camera.main != null)
            this.transform.forward = Camera.main.transform.forward;
    }
}
