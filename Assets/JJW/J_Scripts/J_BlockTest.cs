using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_BlockTest : MonoBehaviour
{
    public float shotTerm = 2f;
    float shotT;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*currT += Time.deltaTime;
        shotT += Time.deltaTime;*/
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(ShurikenShot(shurikenTerm));
            //StartCoroutine("shotRay");
        }
    }
    // 표창
    public GameObject Shuriken;
    // 표창쏘는 곳
    public Transform shurikenPos;
    // 표창 날리는 쿨타임
    public float shurikenTime = 0.5f;
    // 경과시간 
    public float shurikenTerm = 0.3f;
    IEnumerator ShurikenShot(float sk)
    {
            GameObject shuriken = Instantiate(Shuriken);
            shuriken.transform.position = shurikenPos.position;
            shuriken.transform.forward = shurikenPos.forward;
            yield return new WaitForSeconds(sk);
            /*currT = 0;
            shotT = 0;*/
    }
    public GameObject CUBE;

    IEnumerator shotRay()
    {
        RaycastHit hitinfo;
        RaycastHit hitinfo2;
        // 테스트용 애새끼가 레이저를 쏨
        if (Physics.Raycast(shurikenPos.position,transform.forward,out hitinfo))
        {
            // 겐지가 막으면
            if(hitinfo.transform.name=="BlockFunc")
            {
                // 맞은 부분에서 다시 나감
                if(Physics.Raycast(hitinfo.point,hitinfo.transform.forward,out hitinfo2))
                {
                    // 다시 나간 레이저가 벽에 맞으면
                    if (hitinfo2.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    {
                        // 이펙트 생성
                        GameObject test = Instantiate(CUBE);
                        test.transform.position = hitinfo2.point;
                    }
                }
            }
            if(hitinfo.transform.gameObject.layer==LayerMask.NameToLayer("Wall"))
            {
                GameObject test = Instantiate(CUBE);
                test.transform.position = hitinfo.point;
            }
        }

        yield return new WaitForSeconds(2);
    }
}
