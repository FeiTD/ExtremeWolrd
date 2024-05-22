using Assets.Scripts.Manager;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiniMap : MonoBehaviour {
	public Image MiniMap;
	public Image Arrow;
	public Text MapName;
	public Collider MiniMapBox;
	private Transform PlayerTransform;
	// Use this for initialization
	void Start () {
		Init();
	}
	
	// Update is called once per frame
	void Update () {

		if (PlayerTransform == null)
		{
			if (Users.Instance.CurrentCharacterObject == null)
				return;
            PlayerTransform = Users.Instance.CurrentCharacterObject.transform;
        }
			

        float realWidth = MiniMapBox.bounds.size.x;
		float realHeight = MiniMapBox.bounds.size.z;

		float relativeX = PlayerTransform.position.x - MiniMapBox.bounds.min.x;
		float relativeY = PlayerTransform.position.z - MiniMapBox.bounds.min.z;

		float pivotX = relativeX / realWidth;
		float pivotY = relativeY / realHeight;

		MiniMap.rectTransform.pivot = new Vector2(pivotX, pivotY);
		MiniMap.rectTransform.localPosition = Vector2.zero;
		Arrow.transform.eulerAngles = new Vector3(0,0,-PlayerTransform.eulerAngles.y);
	}

	void Init()
    {
		MapName.text = Users.Instance.CurrentMapData.Name;
		if (MiniMap.overrideSprite == null)
		{
			MiniMap.overrideSprite = MiniMapManager.Instance.LoadCurrentMiniMap();
		}
		MiniMap.SetNativeSize();
		MiniMap.transform.localPosition = Vector3.zero;
	}
}
