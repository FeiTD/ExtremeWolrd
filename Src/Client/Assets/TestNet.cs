using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		NetClient.Instance.Init("127.0.0.1", 8000);
		NetClient.Instance.Connect();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
