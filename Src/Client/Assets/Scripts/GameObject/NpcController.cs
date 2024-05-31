using Assets.Scripts.Manager;
using Assets.Scripts.Models;
using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour {
	public int ID;
	Animator anim;
	NpcDefine npc;
	bool interactive;
	Color origncolor;
	Color render;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		npc = NpcManager.Instance.GetNpcDefine(ID);
		//origncolor = GetComponent<Renderer>().sharedMaterial.color;
		//renderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		StartCoroutine(Actions());
    }
	
    private void OnMouseDown()
    {
		Interactive();
    }
  //  private void OnMouseOver()
  //  {
		//HighLight(true);
  //  }
  //  private void OnMouseEnter()
  //  {
  //      HighLight(true);
  //  }
  //  private void OnMouseExit()
  //  {
  //      HighLight(false);
  //  }

    private void Interactive()
    {
		if (!interactive)
		{
			interactive = true;
			StartCoroutine(DoInteractive());
		}
    }

	IEnumerator Actions()
	{
		while (true)
		{
			if (interactive)
				yield return new WaitForSeconds(2f);
			else
				yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
			Relax();
		}
	}

    private void Relax()
    {
		anim.SetTrigger("Relax");
    }

    IEnumerator DoInteractive()
    {
		yield return FaceToPlayer();
		if (NpcManager.Instance.Interactive(ID))
		{
			anim.SetTrigger("Talk");
		}
		yield return new WaitForSeconds(3f);
		interactive = false;
    }

	IEnumerator FaceToPlayer()
	{
		Vector3 faceTo = (Users.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
		while(Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward, faceTo)) > 5)
		{
			this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
	}
	public void HighLight(bool hightlight)
	{
		if (hightlight)
		{
            if (GetComponent<Renderer>().sharedMaterial.color != Color.white)
                GetComponent<Renderer>().sharedMaterial.color = Color.white;
        }			
		else
		{
            if (GetComponent<Renderer>().sharedMaterial.color != origncolor)
                GetComponent<Renderer>().sharedMaterial.color = origncolor;
        }

    }
}
