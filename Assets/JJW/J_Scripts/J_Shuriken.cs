using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Gyu;

public class J_Shuriken : MonoBehaviourPun, IPunObservable
{
    SphereCollider sp;

    // Start is called before the first frame update
    void Start()
    {
     
        sp = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            GoFoward();
            shootRay();
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, otherPos, Time.deltaTime * 50.0f);
        }

        Destroy(gameObject, 5);
    }
    public GameObject shu_Eff;
    public float speed = 7f;
    
    public Vector3 dir2;

    public void GoFoward()
    {
        transform.position += transform.forward * speed* Time.deltaTime;
    }

    public float rayDis = 0.2f;
    void shootRay()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hitInfo, rayDis))
        {

            if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if(hitInfo.transform.GetComponent<J_HP>())
                {
                    hitInfo.transform.GetComponent<J_HP>().Damaged(30);
                }
                //GameObject.Find("GenJI").GetComponent<Genji>().Ultpoint += 10;
                GetComponentInParent<Genji>().Ultpoint += 10;
                //hitInfo.transform.GetComponent<PhotonView>().RPC("OnHitEffect", RpcTarget.All);
                if (hitInfo.transform.GetComponent<PlayerHP>())
                {
                    Vector3 v3 = gameObject.transform.parent.transform.position;
                    hitInfo.transform.GetComponent<PlayerHP>().Damaged(30,v3.x,v3.y,v3.z);
                }
                PhotonView.Destroy(gameObject);
                //Destroy(gameObject);
            }
            else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                GameObject eff = PhotonNetwork.Instantiate(Path.Combine("Shurikens", "Shuriken_Eff (1)"), hitInfo.point, hitInfo.transform.rotation);

                //GameObject eff = Instantiate(shu_Eff);
                shu_Eff.transform.position = hitInfo.point;
                shu_Eff.transform.up = hitInfo.normal;
                PhotonView.Destroy(gameObject);

                sp.enabled=false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==LayerMask.NameToLayer("block"))
        {
            

            //GoFoward(dir2, speed * 2);
            transform.forward = -other.transform.forward;
            //transform.position += hitInfo.transform.forward * speed * Time.deltaTime;
            print("제발요 좀");
            //GameObject srk = Instantiate(shu_Eff);
            //srk.transform.position = transform.position;
            //Destroy(gameObject);
        }
    }
    Vector3 otherPos;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            otherPos = (Vector3)stream.ReceiveNext();
        }
    }
}
