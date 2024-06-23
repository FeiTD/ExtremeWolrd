using Assets.Scripts.Entities;
using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager> {
	public GameObject NameBarPrefab;
	public GameObject NpcStatusPrefab;
	public Dictionary<Transform, GameObject> NameBarElements = new Dictionary<Transform, GameObject>();
    public Dictionary<Transform, GameObject> NpcStatuElements = new Dictionary<Transform, GameObject>();
    // Use this for initialization
    public void AddCharacterNameBar(Transform owner,Character character)
    {
		GameObject goNameBar = Instantiate(NameBarPrefab, this.transform);
		goNameBar.name = "NameBar" + character.entityId;
		goNameBar.GetComponent<UIWorldElement>().Owner = owner;
		goNameBar.GetComponent<UINameBar>().Character = character;
		goNameBar.SetActive(true);
        NameBarElements[owner] = goNameBar;
    }

	public void RemoveCharacterNameBar(Transform owner)
    {
		if (NameBarElements.ContainsKey(owner))
        {
			Destroy(NameBarElements[owner]);
            NameBarElements.Remove(owner);
		}
    }


    public void AddNpcStatu(Transform owner, NpcQuestStatus status)
    {
        if(NpcStatuElements.ContainsKey(owner))
        {
            NpcStatuElements[owner].GetComponent<UIQuestStatus>().SetQuestStatus(status);
        }
        else
        {
            GameObject NpcStatus = Instantiate(NpcStatusPrefab, this.transform);
            NpcStatus.name = "NpcQuestStatu" + owner.name;
            NpcStatus.GetComponent<UIWorldElement>().Owner = owner;
            NpcStatus.GetComponent<UIQuestStatus>().SetQuestStatus(status);
            NpcStatus.SetActive(true);
            NpcStatuElements[owner] = NpcStatus;
        }
        
    }

    public void RemoveNpcStatu(Transform owner)
    {
        if (NpcStatuElements.ContainsKey(owner))
        {
            Destroy(NpcStatuElements[owner]);
            NpcStatuElements.Remove(owner);
        }
    }
}
