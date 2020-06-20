using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 궁극기 모션 구현
// 칼을 등에서 뽑고 다시 집어넣을것임

public class J_AnimeEvent : MonoBehaviour
{
    public GameObject dragonHead;
    // 등에 붙은 칼
    public GameObject backsword;
    // 손에 붙일 칼
    public GameObject handSword;
    // 평소 쓰던 칼
    public GameObject leftSword;
    public Genji gj;

    public void UltEvent()
    {
        print("뽑는다");
        // 등에붙은 칼을 비활성화
        backsword.SetActive(false);
        // 손에 붙을 칼을 활성화
        handSword.SetActive(true);
        // 쓰던칼을 비활성화
        leftSword.SetActive(false);
    }
    public void UltEventEnd()
    {
        print("집어넣는다");
        // 등에붙은 칼을 비활성화
        backsword.SetActive(true);
        // 손에 붙을 칼을 활성화
        handSword.SetActive(false);
        // 쓰던칼을 비활성화
        leftSword.SetActive(true);
    }
    public void AttackCount()
    {
        gj = GetComponentInParent<Genji>();
        gj.UltOver--;
        print(gj.UltOver);
    }
    public GameObject blockField;
    public void BlockFunc()
    {
        if(blockField.activeSelf==false)
        {
            blockField.SetActive(true);
        }
    }
    public void BlockFunc2()
    {
        if (blockField.activeSelf == true)
        {
            blockField.SetActive(false);
        }
    }

    public void SpawnDragon()
    {
        if(dragonHead.activeSelf==false)
        {
            dragonHead.SetActive(true);
        }
    }
    public void SpawnDragon2()
    {
        if (dragonHead.activeSelf == true)
        {
            float dh = GetComponentInChildren<J_DragonEff>().rotAngle = 360f;
            float dy = GetComponentInChildren<J_DragonEff>().rotateY = 0;
            dragonHead.SetActive(false);
        }
    }
    public GameObject SwordTrail;
    public void TrailOn()
    {
        if(SwordTrail.activeSelf==false)
        {
            SwordTrail.SetActive(true);
        }
    }
    public void TrailOff()
    {
        if (SwordTrail.activeSelf == true)
        {
            SwordTrail.SetActive(false);
        }
    }
    public GameObject DashTrail;
    public void DashTrailOn()
    {
        if (DashTrail.activeSelf == false)
        {
            DashTrail.SetActive(true);
        }
    }
    public void DashTrailOff()
    {
        if (DashTrail.activeSelf == true)
        {
            DashTrail.SetActive(false);
        }
    }
    public GameObject fpsSword;
    public GameObject fpsSwordTrail;
    public void FpsView()
    {
        fpsSword.SetActive(true);
        fpsSwordTrail.SetActive(true);
    }
    public void FpsView2()
    {
        fpsSword.SetActive(false);
        fpsSwordTrail.SetActive(false);
    }
}
