﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
using Assets.Scripts.Manager;
using Assets.Scripts.Entities;
using Assets.Scripts.Models;

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
            GameObject go = (GameObject)Instantiate(obj,this.transform);
            go.name = "Character_" + character.Id + "_" + character.Name;
            Characters[character.Info.Id] = go;


            go.transform.position = GameObjectTool.LogicToWorld(character.position);
            go.transform.forward = GameObjectTool.LogicToWorld(character.direction);
            EntityController ec = go.GetComponent<EntityController>();
            if (ec != null)
            {
                ec.entity = character;
                ec.isPlayer = character.IsCurrentPlayer;
                //ec.Ride(character.Info.Ride);
            }

            PlayerInputController pc = go.GetComponent<PlayerInputController>();
            if (pc != null)
            {

                if (character.IsCurrentPlayer)
                {
                    Users.Instance.CurrentCharacterObject = pc;
                    MainPlayerCamera.Instance.Player = go;
                    pc.enabled = true;
                    pc.character = character;
                    pc.entityController = ec;
                }
                else
                {
                    pc.enabled = false;
                }
            }

            UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);
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
