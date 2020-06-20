using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_CamRot : MonoBehaviour
{
    public float my,mx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 입력
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        // 회전방향
        mx += h * 200 * Time.deltaTime;
        my += v * 200 * Time.deltaTime;
        // y값 회전 제약
        my = Mathf.Clamp(my, -60, 60);
        // 회전
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
