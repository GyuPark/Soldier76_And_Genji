using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class J_ShurikenDestroy : MonoBehaviourPun, IPunObservable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            //Destroy(gameObject, 3);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, otherPos, Time.deltaTime * 50.0f);
        }

        Destroy(gameObject, 3);
        //Destroy(gameObject, 3);
    }

    Vector3 otherPos;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            otherPos = (Vector3)stream.ReceiveNext();
        }
    }
}
