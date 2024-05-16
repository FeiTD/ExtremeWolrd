using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
using Assets.Scripts.Manager;
using Assets.Scripts.Entities;

public class GameObjectManager : MonoBehaviour {
	Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();
	// Use this for initialization
	void Start () {
		StartCoroutine(InitGameObject());
		CharacterManager.Instance.OnCharacterEnter = OnCharacterEnter;
	}

	private void OnDestory()
    {
		CharacterManager.Instance.OnCharacterEnter = null;
	}
    private void OnCharacterEnter(Character ch)
    {
		CreatCharacterObject(ch);
	}

	public void CreatCharacterObject(Character character)
    {

        if (!Characters.ContainsKey(character.entityId) || Characters[character.entityId] == null)
        {
            Object obj = Resloader.Load<Object>(character.Define.Resource);
            if (obj == null)
            {
                Debug.LogErrorFormat("Character[{0}] Resource[{1}] not existed.", character.Define.TID, character.Define.Resource);
                return;
            }
            GameObject go = (GameObject)Instantiate(obj);
            go.name = "Character_" + character.Id + "_" + character.Name;
            go.transform.position = GameObjectTool.LogicToWorld(character.position);
            go.transform.forward = GameObjectTool.LogicToWorld(character.direction);

            Characters[character.Info.Id] = go;

            
        }
    }

	IEnumerator InitGameObject()
    {
		foreach(var ch in CharacterManager.Instance.Characters.Values)
        {
			CreatCharacterObject(ch);
			yield return null;
		}
    }
}
