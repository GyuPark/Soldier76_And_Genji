using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.EventSystems;

public class J_HP : MonoBehaviourPun
{
    // 최대체력
    public int MaxHp;
    // 현재체력
    public int hp;
    public int PlayerCurrHp
    {
        get
        {
            return hp;
        }
        set
        {
            // 체력이 최대체력을 넘으면 다시 최대체력으로
            if (value > MaxHp)
            {
                hp = MaxHp;
            }
            // 체력이 0이하가 되면 0
            else if (value < 0)
            {
                hp = 0;
            }
            // 아니면 체력계산하십쇼
            else
            {
                hp = value;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    // 피해받는 기능
    public void Damaged(int atk)
    {
        // PlayerCurrHp -= atk;
        // print(PlayerCurrHp);
        photonView.RPC("DamageProcess", RpcTarget.AllBuffered, atk);

    }
    [PunRPC]
    void DamageProcess(int dam)
    {
        PlayerCurrHp = PlayerCurrHp - dam < 0 ? 0 : PlayerCurrHp - dam;
    }
}
