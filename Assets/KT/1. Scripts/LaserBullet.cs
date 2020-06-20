using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LaserBullet : MonoBehaviourPun
{
    float launchSpeed = 80f;
    float bulletLifeSpan = 3f;

    private void OnEnable()
    {
        Destroy(gameObject, bulletLifeSpan);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.position += transform.forward * launchSpeed * Time.deltaTime;

    }

    //충돌하면 터진다.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.layer == LayerMask.NameToLayer("Player")) //맞은게 애너미라면
        {
            PhotonNetwork.Destroy(gameObject);
        }

        //모든 게 끝나면 스스로 자신을 파괴한다.
    }
}
