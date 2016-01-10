using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour
{
    NetworkCulling networkCulling;
    Transform mTrans;

    void Awake()
    {
        networkCulling = GetComponent<NetworkCulling>();
        mTrans = GetComponent<Transform>();

        gameObject.name = gameObject.name + photonView.viewID;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this player: send the others our data
            networkCulling.SetGroup(photonView);
            stream.SendNext(mTrans.position);
            stream.SendNext(mTrans.rotation);
        }
        else
        {
            //Network player, receive data
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }

    private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this

    void Update()
    {
        if (!photonView.isMine)
        {
            //Update remote player (smooth this, this looks good, at the cost of some accuracy)
            mTrans.position = Vector3.Lerp(mTrans.position, correctPlayerPos, Time.deltaTime * 5);
            mTrans.rotation = Quaternion.Lerp(mTrans.rotation, correctPlayerRot, Time.deltaTime * 5);
        }
    }

}