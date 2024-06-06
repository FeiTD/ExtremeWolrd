using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabButton : MonoBehaviour {
	public Sprite activeImage;
	private Sprite normalImage;
	public UITabView View;
	public int Index = 0;
	public bool select = false;
	private Image tabImage;
    public void Select(bool flag)
    {
        tabImage.overrideSprite = flag ? activeImage : normalImage;
    }

    // Use this for initialization
    void Start () {
		tabImage = GetComponent<Image>();
		normalImage = tabImage.sprite;
		this.GetComponent<Button>().onClick.AddListener(OnClick);
	}

    private void OnClick()
    {
        View.SelectTab(Index);
    }

}
