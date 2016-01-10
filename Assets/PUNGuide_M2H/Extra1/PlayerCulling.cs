using UnityEngine;
using System.Collections;

public class PlayerCulling : Photon.MonoBehaviour {


	private NetworkCulling networkCulling;

	public void Awake(){
		networkCulling = GetComponent<NetworkCulling>();  
	}


	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			networkCulling.SetGroup(photonView);
			Vector3 pos = transform.position;
			Quaternion rot = transform.rotation;
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
		}
		else
		{
			Vector3 pos = transform.position;
			Quaternion rot = transform.rotation;
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
			SetPositionRotation(pos,rot); //Your own synchronization code (or just directly set the position/rotation for now)
		}
	}

	void SetPositionRotation(Vector3 pos, Quaternion rot){
		//TODO set position and rotation.
		//Easy&Smooth is to LERP to the latest value
		transform.position = pos;
		transform.rotation = rot;
	}


	void Update(){
		//TODO. Lerp to the latest value?
	}

}
