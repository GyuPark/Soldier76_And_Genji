using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class J_Block : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    J_Shuriken js;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==LayerMask.NameToLayer("shuriken"))
        {
            GetComponentInParent<Genji>().Ultpoint += 20;
            js = other.GetComponent<J_Shuriken>();
            //js.dir = transform.forward;
            //photonView.RPC("Tititing", RpcTarget.All);
            //other.GetComponent<J_Shuriken>().dir = transform.forward;
            print("티티팅");
        }
    }
    [PunRPC]
    public void Tititing()
    {
        if (js != null) {
           // js.dir = transform.forward;
        }
        else 
        {
            print("어림없지");
        }
    }
    */
}
