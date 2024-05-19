using Assets.Scripts.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager> {
	public GameObject NameBarPrefab;

	public Dictionary<Transform, GameObject> Elements = new Dictionary<Transform, GameObject>();
	// Use this for initialization
	public void AddCharacterNameBar(Transform owner,Character character)
    {
		GameObject goNameBar = Instantiate(NameBarPrefab, this.transform);
		goNameBar.name = "NameBar" + character.entityId;
		goNameBar.GetComponent<UIWorldElement>().Owner = owner;
		goNameBar.GetComponent<UINameBar>().Character = character;
		goNameBar.SetActive(true);
		Elements[owner] = goNameBar;
    }

	public void RemoveCharacterNameBar(Transform owner)
    {
		if (Elements.ContainsKey(owner))
        {
			Destroy(Elements[owner]);
			Elements.Remove(owner);
		}
    }
}
