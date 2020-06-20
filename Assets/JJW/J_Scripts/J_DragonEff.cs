using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 위로상승하면서 주위를 도는 구체
public class J_DragonEff : MonoBehaviour
{
    public float rotateY = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        //rotAngle -= Time.deltaTime* 0.05f;
        //if(rotAngle<=0)
        //{
        //    rotAngle = 0;
        //    upSpeed = 0;
        //}

        // 상승함수
        // 공전함수
        if (rotAngle > 0)
        {
            Upward();
            RotateAroundGenji();
        }
    }
    // 상승함수
    public float upSpeed = 2f;
    void Upward()
    {
        transform.position += transform.up * upSpeed * Time.deltaTime;
    }
    // 공전함수
    // 겐지
    public GameObject center;
    // 도는 속도
    public float rotSpeed = 2f;
    public float rotAngle = 360f;
    void RotateAroundGenji()
    {
        rotateY -= Time.deltaTime * rotSpeed;
        rotAngle -= Time.deltaTime * rotSpeed;
        if(rotAngle<=0)
        {
            rotateY = 0;
        }
        Vector3 angle = transform.localEulerAngles;
        angle.y = rotateY;
        transform.localEulerAngles = angle;

        // transform.Rotate(Vector3.up * rotAngle);
        //transform.RotateAround(center.transform.position, center.transform.up*-1, rotAngle);
    }
}
