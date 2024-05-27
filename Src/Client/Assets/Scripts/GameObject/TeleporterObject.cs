using Assets.Scripts.Manager;
using Assets.Scripts.Services;
using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterObject : MonoBehaviour {
	public int ID;
	Mesh mesh;
	// Use this for initialization
	void Start () {
		mesh = GetComponent<Mesh>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        PlayerInputController playerInputController = other.GetComponent<PlayerInputController>();
		if(playerInputController != null && playerInputController.isActiveAndEnabled)
		{
			TeleporterDefine td = DataManager.Instance.Telepoters[this.ID];
			if (td == null)
			{
				Debug.LogErrorFormat("TeleporterObject: Character[{0}] Enter Teleporter [{1}], But TeleporterDefine not existed",playerInputController.character.Info.Name,ID);
                return;
            }
			if(td.LinkTo > 0)
			{
				if (DataManager.Instance.Telepoters.ContainsKey(td.LinkTo))
					MapService.Instance.SendMapTeleport(ID);			
			}

		}
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
		if(this.mesh != null )
		{
			Gizmos.DrawWireMesh( mesh ,this.transform.position + Vector3.up * this.transform.localPosition.y * .5f,this.transform.rotation,this.transform.localScale);
			UnityEditor.Handles.color = Color.red;
			UnityEditor.Handles.ArrowHandleCap(0,transform.position,this.transform.rotation,1f,EventType.Repaint);
		}
    }
#endif
}
