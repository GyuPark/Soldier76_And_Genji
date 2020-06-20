using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.IO;
using UnityEngine.EventSystems;
using Gyu;


public class Genji : J_PlayerMovement,IPunObservable
{
    AudioSource audio;

    CharacterController controller;
    bool isClimbimg = false;
    Animator[] anim;
    public GameObject handSword;
    // 기본공격 체크
    bool isbaseAttack;
    // 궁극기 체크
    bool isUltimate;
    float isUltAtkTime;
    public bool atk1;
    public bool atk2;
    public int UltOver;
    public float Ultpoint = 0;
    //체력
    public int mmm = 200;
    public int nnn = 200;
    // 체력 게이지
    public GameObject[] HpBars;
    // 최대 표창 개수
    int shurikencount = 24;
    bool isReloading = false;
    float reloadT;
    public TextMeshProUGUI countShuriken;
    public TextMeshProUGUI hpUi;
    public AudioClip[] aClips;

    int[] HpValue;
    protected override void Start()
    {
        UILimit();
        HpValue =new int[8]{ 25,50,75,100,125,150,175,200};
        audio = GetComponentInParent<AudioSource>();
        //SetHp(mmm, nnn);
        base.Start();
        // 내가 필요한 거 코등
        speed = 10;
        maxJumpcount = 3;
        controller = GetComponent<CharacterController>();
        anim = GetComponentsInChildren<Animator>();
        
        //chest = anim.GetBoneTransform(HumanBodyBones.Chest);
        handSword.SetActive(false);
        isbaseAttack = true;
        isUltimate = true;
        atk1 = true;
        atk2 = false;
        UltOver = 4;
    }
    protected override void Update()
    {
        if (photonView.IsMine)
        {
            
                //photonView.RPC("PlayAni", RpcTarget.All, "THROW");
            
            Climb();
            Reload();
            HpManage();
            if (Ultpoint >= 100)
            {
                Ultpoint = 100;
            }
            if (isClimbimg != true)
            {
                base.Update();
            }
            //LookDir();
            currT += Time.deltaTime;
            DashCurrT += Time.deltaTime;
            isUltAtkTime += Time.deltaTime;
            blockCoolT += Time.deltaTime;
            countShuriken.text = shurikencount.ToString();
            hpUi.text = GetComponent<J_HP>().PlayerCurrHp.ToString() + " / 200";
            shiftIcon.fillAmount = DashCurrT * 0.33f;
            blockIcon.fillAmount = blockCoolT * 0.25f;
        }
        else
        {
            base.Update();
        }
    }


    // 체력게이지
    void HpManage()
    {
        int hp = GetComponent<J_HP>().PlayerCurrHp;
        /*if(hp)
        for(int i =7; i<HpBars.Length; i--)
        {
            HpBars[i].SetActive(false);
        }*/
        /*for(int i =7; i<HpBars.Length; i--)
        {
            if (200>hp-(25*i))
            {
                HpBars[i].SetActive(false);
            }
        }*/
        for (int i = 0; i < HpValue.Length; i++)
        {
            if (hp < HpValue[i])
            {
                HpBars[i].SetActive(false);
            }
        }

    }

    // 장전
    void Reload()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            shurikencount = 24;
        }
        if(shurikencount<=0)
        {
            shurikencount = 0;
            isReloading = true;
            reloadT += Time.deltaTime;

            if (reloadT>=2)
            {
                shurikencount = 24;
                isReloading = false;
                reloadT = 0;
            }
        }
    }

    // 상하체 구분
    // 바라볼 대상
    public Transform justLook;
    Transform chest;
    public Vector3 relativeVec;
    /*public void LookDir()
    {
        chest.LookAt(justLook.position);
        chest.rotation = chest.rotation * Quaternion.Euler(relativeVec);
    }*/

    // 기본공격
    // 표창
    public GameObject Shuriken;
    // 표창쏘는 곳
    public Transform shurikenPos;
    // 표창 날리는 쿨타임
    public float shurikenTime = 0.5f;
    // 경과시간 
    float currT = 0.5f;

    void SetAnimTriger(string triggerName)
    {
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetTrigger(triggerName);
        }

    }

    [PunRPC]
    void PlayAni(string trigger)
    {
        SetAnimTriger(trigger);
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
        // 표창생성
        if(currT>=shurikenTime&&isbaseAttack==true&&isReloading==false)
        {
            photonView.RPC("PlayAni", RpcTarget.All, "THROW");
            audio.PlayOneShot(aClips[0]);
            StartCoroutine(ShurikenShot(shurikenTerm));
            currT = 0;
        }
        // 궁극기를 썼을때
        if(UltOver<=0)
        {
            photonView.RPC("PlayAni", RpcTarget.All, "SwordInput");
            //SetAnimTriger("SwordInput");
            audio.PlayOneShot(aClips[8]);

            isbaseAttack = true;
            isUltimate = true;
            UltOver = 4;
        }
        if (atk1 == true && atk2 == false && isbaseAttack == false)
        {
            if (isUltAtkTime >= 1.333f)
            {
                audio.PlayOneShot(aClips[6]);
                photonView.RPC("PlayAni", RpcTarget.All, "ULTATTACK1");

                //SetAnimTriger("ULTATTACK1");
                SwordAttack(swordDis, 120);
                atk1 = false;
                atk2 = true;
                isUltAtkTime = 0;
            }
        }
        if (atk2 == true && atk1 == false && isbaseAttack == false)
        {
            if (isUltAtkTime >= 1.333f)
            {
                audio.PlayOneShot(aClips[6]);
                photonView.RPC("PlayAni", RpcTarget.All, "ULTATTACK2");

                //SetAnimTriger("ULTATTACK2");
                SwordAttack(swordDis, 120);
                atk1 = true;
                atk2 = false;
                isUltAtkTime = 0;
            }
        }
    }
    
    public float swordDis = 5f;
    // 스피어레이 발사
    public void SwordAttack(float DisDis, int atkDamage)
    {
        RaycastHit hitInfo;
        // 괄호 안 순서대로 (발사위치, 원 크기, 방향, 정보, 발사 거리)
        if(Physics.SphereCast(transform.position,transform.localScale.x*2f,transform.forward,out hitInfo, DisDis))
        {
            if (hitInfo.transform.gameObject.layer==LayerMask.NameToLayer("test"))
            {
                Ultpoint += 20;
                if(hitInfo.transform.gameObject.GetComponent<J_HP>())
                {
                    hitInfo.transform.gameObject.GetComponent<J_HP>().Damaged(atkDamage);
                }
            }
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Ultpoint += 20;
                if (hitInfo.transform.gameObject.GetComponent<J_HP>())
                {
                    hitInfo.transform.gameObject.GetComponent<J_HP>().Damaged(atkDamage);
                }
                if (hitInfo.transform.GetComponent<PlayerHP>())
                {
                    Vector3 v3 = transform.position;
                    hitInfo.transform.GetComponent<PlayerHP>().Damaged(atkDamage, v3.x, v3.y, v3.z);
                }
            }
        }
    }

    // 기본공격2(마우스 오른쪽)
    // 각도조절변수
    public int increaseAngle = 10;
    
    public override void NormalAttack2()
    {
        if (currT >= shurikenTime && isbaseAttack == true&&isReloading==false)
        {
            audio.PlayOneShot(aClips[1]);
            photonView.RPC("PlayAni", RpcTarget.All, "THROW");

            //SetAnimTriger("THROW");
            shurikencount -= 3;
            // 생성
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                for (int i = -5; i<5; i+= increaseAngle)
                {
                    GameObject shuriken = PhotonNetwork.Instantiate(Path.Combine("Shurikens", "Shuriken"),shurikenPos.position,shurikenPos.rotation);
                    shuriken.transform.SetParent(gameObject.transform);
                    print(shuriken.transform.parent);
                    //GameObject shuriken = Instantiate(Shuriken);
                    shuriken.transform.forward = Quaternion.Euler(0, i, 0) * shurikenPos.forward;
                    //shuriken.transform.position = shurikenPos.position;
                    currT = 0;
                }
            }
        }
    }
    // 쉬프트키
    float DashCurrT = 3f;
    public float dashCoolT = 3f;
    public float dashDis = 5f;
    public float dashSpeed = 5f;
    public Image shiftIcon;
    public override void ShiftAttack()
    {
        if(DashCurrT>=dashCoolT)
        {
            StartCoroutine("ShiftDashDes");
        }
    }
    // 근접공격
    public override void PunchAttack()
    {
        audio.PlayOneShot(aClips[3]);
        photonView.RPC("PlayAni", RpcTarget.All, "SLASH");

        //SetAnimTriger("SLASH");
        SwordAttack(2, 30);
    }
    // e키
    float blockCoolT = 4f;
    public float blockT = 4f;
    public Image blockIcon;
    public override void E_Func()
    {
        if(blockCoolT>=blockT)
        {
            audio.PlayOneShot(aClips[2]);

            print("막을게");
            photonView.RPC("PlayAni", RpcTarget.All, "BLOCK");

            //SetAnimTriger("BLOCK");
            blockCoolT = 0;
        }
    }
    // 궁극기
    public override void Ultimate()
    {
        if(isUltimate==true&&Ultpoint>=100)
        {
            audio.PlayOneShot(aClips[5]);

            Ultpoint = 0;
            isUltimate = false;
            isbaseAttack = false;
            // 칼을 뽑는다.
            photonView.RPC("PlayAni", RpcTarget.All, "ULTIMATE");

            //SetAnimTriger("ULTIMATE");
        }
    }
    public float climbingRay = 0.5f;
    public float climbingSpeed = 5f;
    public bool fuckClimb;
    void Climb()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            RaycastHit hitInfo;

            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, climbingRay))
            {
                if (hitInfo.transform.gameObject.tag == "Climbable")
                {
                    print("등산 중");
                    isClimbimg = true;
                    ClimbAnime();
                    Vector3 dist = Vector3.up * climbingSpeed * Time.deltaTime;                    
                    controller.Move(dist);                    
                }
            }
            else if(isClimbimg)
            {
                print("등산 아님");
                isClimbimg = false;
                ChangeState(eState.MOVE);
                fuckClimb = false;
                controller.Move(Vector3.up * 2);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space) && isClimbimg)
        {
            print("등산 아님");
            isClimbimg = false;
            ChangeState(eState.MOVE);

            fuckClimb = false;
        }
    }
    void ClimbAnime()
    {
        if(fuckClimb==false)
        {
            photonView.RPC("PlayAni", RpcTarget.All, "CLIMB");

            //SetAnimTriger("CLIMB");
            fuckClimb = true;
        }

    }
    // 표창생성코루틴
    // 표창 날리는 간격
    public float shurikenTerm = 0.3f;
    IEnumerator ShurikenShot(float sk)
    {
        shurikencount -= 3;
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            for (int i = 0; i<3; i++)
            {
                GameObject shuriken = PhotonNetwork.Instantiate(Path.Combine("Shurikens", "Shuriken"), shurikenPos.position, shurikenPos.rotation);
                shuriken.transform.SetParent(gameObject.transform);
                print(shuriken.transform.parent);
                //GameObject shuriken = Instantiate(Shuriken);
                //shuriken.transform.position = shurikenPos.position;
                shuriken.transform.forward = shurikenPos.forward;
                yield return new WaitForSeconds(sk);
                currT = 0;
            }
        }
    }
    // 겐지의 질풍참
    float ForDashcurrT;
    IEnumerator ShiftDashDes()
    {
        audio.PlayOneShot(aClips[4]);

        state = eState.DASH;
        photonView.RPC("PlayAni", RpcTarget.All, "DASH");

        //SetAnimTriger("DASH");
        //isGravity = false;
        Vector3 asd = shurikenPos.forward * dashDis+transform.position;
        // 0.5동안
        SwordAttack(15,40);
        while(ForDashcurrT <= 0.3)
        {
            ForDashcurrT += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            controller.Move((asd-transform.position).normalized * dashSpeed * Time.deltaTime);    
        }
        state = eState.MOVE;
        //isGravity = true;
        ForDashcurrT = 0;
        DashCurrT = 0;
    }

    public GameObject[] Uies;
    // UI 남 이 먹 게 하 지 않 게 하 겠 다.
    public void UILimit()
    {
        if (!photonView.IsMine)
        {
            for (int i = 0; i < Uies.Length; i++)
            {
                Uies[i].SetActive(false);
            }
        }
    }

}
