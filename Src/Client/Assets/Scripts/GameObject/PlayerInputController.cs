using Assets.Scripts.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour {

	public Rigidbody rb;
	SkillBridge.Message.CharacterState state;

	public Character character;

	public float rotateSpeed = 2.0f;

	public float turnAngle = 10;

	public int speed;

	public EntityController entityController;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
