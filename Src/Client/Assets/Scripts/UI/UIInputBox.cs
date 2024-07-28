using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInputBox : MonoBehaviour {
	public Text Tips;
	public Text Content;
	public InputField Input;
    public Text Yes;
    public Text No;
    public UnityAction OnYes;
    public UnityAction OnNo;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickYes()
    {
        Destroy(this.gameObject);
        if (this.OnYes != null)
            this.OnYes();
    }

    public void OnClickNo()
    {
        Destroy(this.gameObject);
        if (this.OnNo != null)
            this.OnNo();
    }

    public void Close()
    {
        Destroy(this.gameObject);
    }
    public void Init(string tips, string content, string btnOK, string btnCancel)
    {
        Tips.text = tips;
        Content.text = content;
        Yes.text = btnOK;
        No.text = btnCancel;
    }
}
