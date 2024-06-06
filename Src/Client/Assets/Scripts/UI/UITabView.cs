using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabView : MonoBehaviour {
	public UITabButton[] TabButtons;
	public GameObject[] TabPages;
	public int index = -1;
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Start()
	{
		for(int i = 0; i < TabButtons.Length; i++)
		{
			TabButtons[i].View = this;
			TabButtons[i].Index = i;
        }
		yield return new WaitForEndOfFrame();
		SelectTab(0);
	}

	public void SelectTab(int idx)
	{
		if(idx != index)
		{
			for(int i = 0;i < TabButtons.Length;i++)
			{
				TabButtons[i].Select(i == idx);
				TabPages[i].SetActive(i == idx);
			}
		}
	}
}
