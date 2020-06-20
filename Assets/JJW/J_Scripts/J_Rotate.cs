using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class J_Rotate : MonoBehaviourPun, IPunObservable
{
    // 회전 누적값
    public float mx, my;
    public bool isVerticl;
    public bool isHorizontal;

    Quaternion otherRot;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 포톤 뷰 컴포넌트가 붙어 있다면...
        if (GetComponent<PhotonView>())
        {
            // 나의 오브젝트라면...
            if (photonView.IsMine)
            {
                RotationObject();
            }
            // 남의 오브젝트라면...
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, otherRot, Time.deltaTime * 50);
            }
        }
        // 포톤 뷰 컴포넌트가 없다면...
        else
        {
            RotationObject();
        }


    }

    void RotationObject()
    {
        // 마우스 입력
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        // 회전방향
        mx += h * 200 * Time.deltaTime;
        my += v * 200 * Time.deltaTime;
        // y값 회전 제약
        my = Mathf.Clamp(my, -45, 30);
        // 회전
        if (isVerticl == true)
        {
            transform.localEulerAngles = new Vector3(0, mx, 0);
        }
        if (isHorizontal == true)
        {
            transform.localEulerAngles = new Vector3(-my, 0, 0);
        }
    }

    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.rotation);
            
        }
        else
        {
            otherRot = (Quaternion)stream.ReceiveNext();
            
        }
    }
}
