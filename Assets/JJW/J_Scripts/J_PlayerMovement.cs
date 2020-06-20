using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class J_PlayerMovement : MonoBehaviourPun, IPunObservable
{
    public enum eState
    {
        // 정지상태
        // 이동상태
        MOVE,
        // 대쉬
        DASH,
        // 사망
        DIE,
    }
    public eState state;

    CharacterController cc;

    public bool isGravity = true;

    Animator[] anime;

    AudioSource ads;
    public AudioClip acs;
    // 중력가속도
    float gravity = -9.8f;
    // 수직에 대한 속도
    float yVelocity;
    // 점프 힘
    public float jumpPower = 5f;
    // 이동속도
    public float speed = 5f;
    // 점프 카운트
    public int maxJumpcount = 2;
    public int jumpCount;

    // 최대체력
    /*public int MaxHp;
    // 현재체력
    int hp;

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
            else if(value<0)
            {
                hp = 0;
            }
            // 아니면 체력계산하십쇼
            else
            {
                hp = value;
            }
        }
    }*/
    // 피해받는 기능
    /*public void Damaged(int atk)
    {
        PlayerCurrHp -= atk;
        print(PlayerCurrHp);
    }*/
    /*private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }*/

    // Start is called before the first frame update
    protected virtual void Start()
    {
        ads = GetComponent<AudioSource>();
        //print(MaxHp);
        //print(PlayerCurrHp);
        cc = GetComponent<CharacterController>();
        anime = GetComponentsInChildren<Animator>();
    }
    void SetAnimTrigger(string triggerName)
    {
        for (int i = 0; i < anime.Length; i++)
        {
            anime[i].SetTrigger(triggerName);
        }
    }
    //[PunRPC]
    //void PlayAnime(string trigger)
    //{
    //    SetAnimTrigger(trigger);
    //}
    // Update is called once per frame
    protected virtual void Update()
    {
        Die();

        if(photonView.IsMine)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            AttackFunc();
        }

        switch (state)
        {
            
            case eState.MOVE:
                Move();
                break;
            case eState.DASH:
                break;
            /*case eState.DIE:
                Die();
                break;*/
        }

        

    }
    public void ChangeState(eState s)
    {
        state = s;
        switch(state)
        {
            case eState.MOVE:
                photonView.RPC("PlayAni", RpcTarget.All, "RUN");

                //SetAnimTrigger("RUN");
                print("MOVE");
                break;
        }
    }

    // 체력세팅
    /*public void SetHp(int curr, int max)
    {
        MaxHp = max;
        PlayerCurrHp = curr;
    }*/

    // 이동상태

    float dirH;
    float dirV;
    public void Move()
    {
        // 나의 캐릭터일 때 이동
        if(photonView.IsMine)
        {
            Vector3 dir = Gravity();
            dirH = Input.GetAxis("Horizontal");
            dirV = Input.GetAxis("Vertical");
            cc.Move(dir * speed * Time.deltaTime);
        
            for(int i = 0; i<anime.Length; i++)
            {
                anime[i].SetFloat("h", dirH);
                anime[i].SetFloat("v", dirV);
            }
        
            Jump();
        }
        // 아닐때는 받은 위치좌표로 이동
        else
        {

            // 1. 이동 좌표 동기화
            transform.position = Vector3.Lerp(transform.position, otherPos, Time.deltaTime * 50);

            // 2. 이동 애니메이션 동기화
            for (int i = 0; i < anime.Length; i++)
            {
                anime[i].SetFloat("h", animHorizontal);
                anime[i].SetFloat("v", animVertical);
            }
        }
    }
    bool isDie = false;
    public void Die()
    {
        if(GetComponent<J_HP>().hp<=0&&isDie==false)
        {
            print("사망");
            isDie = true;
            ChangeRegDoll();
        }
    }

    // 원본
    public GameObject original;
    // 렉돌
    public GameObject regDoll;
    // 렉돌 전환 코루틴
    public void ChangeRegDoll()
    {
        ChangeRegDollFunc(original.transform, regDoll.transform);
        //SetRegDollInfo();

        regDoll.SetActive(true);
        original.SetActive(false);
        var jr = GetComponentInChildren<J_Rotate>();
        jr.enabled = false;
        Camera.main.transform.LookAt(regDoll.transform.position);
        //Camera.main.transform.SetParent(null);*/
        cc.enabled = false;
    }

    void ChangeRegDollFunc(Transform origin, Transform reg)
    {
        for(int i = 0; i < origin.transform.childCount; i++)
        {
            if(origin.transform.childCount!=0)
            {
                ChangeRegDollFunc(origin.transform.GetChild(i), reg.transform.GetChild(i));
            }
            reg.transform.GetChild(i).localPosition = origin.transform.GetChild(i).localPosition;
            reg.transform.GetChild(i).localRotation = origin.transform.GetChild(i).localRotation;
        }
    }

    void SetRegDollInfo()
    {
        List<Transform> origin = new List<Transform>();
        GetChildTr(original.transform, origin);

        List<Transform> reg = new List<Transform>();
        GetChildTr(regDoll.transform, reg);

        for(int i = 0; i < origin.Count; i++)
        {
            reg[i].localPosition = origin[i].localPosition;
            reg[i].localRotation = origin[i].localRotation;
        }
    }

    void GetChildTr(Transform tr, List<Transform> list)
    {
        Transform childTr;
        for(int i = 0; i < tr.childCount; i++)
        {
            childTr = tr.transform.GetChild(i);
            list.Add(childTr);

            GetChildTr(childTr, list);
        }
    }
    
    protected bool isJump;
    // 점프!
    void Jump()
    {
        // 케릭터가 바닥에 닿고 있으면
        if (cc.isGrounded)//((cc.collisionFlags & CollisionFlags.Below) != 0)
        {
            isJump = false;
            yVelocity = 0;
            if (jumpCount > 0)
            {
                jumpCount = 0;
            }            
        }

        // 점프키를 누르고 점프횟수가 최대 점프 회수보다 적고 케릭터가 바닥에 닿지 않은 상태일 때
        if (Input.GetButtonDown("Jump") && (jumpCount < maxJumpcount))
        {
            isJump = true;
            //ads.PlayOneShot(acs);
            //anime.SetInteger("CHANGE", 2);
            photonView.RPC("PlayAni", RpcTarget.All, "JUMP");

            //SetAnimTrigger("JUMP");
            yVelocity = jumpPower;
            jumpCount++;
            // 점프에서 다른 상태 전이
        }        
        //StartCoroutine(JumpParameter());

    }

    // 점프 상태전이
    void JumpParameter()
    {        
        //yield return new WaitForSeconds(0.0f);
        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            //if (anime.GetInteger("CHANGE") != 2)
            ChangeState(eState.MOVE);
        }
    }

    // 입력값받기
    Vector3 GetAxisVec3XZ()
    {
        Vector3 v = Vector3.zero;
        v.x = Input.GetAxis("Horizontal");
        v.z = Input.GetAxis("Vertical");
        return v;
    }

    // 중력
    public Vector3 Gravity()
    {
        Vector3 dir = GetAxisVec3XZ();
        dir = Camera.main.transform.TransformDirection(dir);
        if(isGravity==false)
        {
            return dir;
        }
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
        return dir;
    }

    void AttackFunc()
    {
            // 기본공격(마우스 왼쪽)
            if (Input.GetMouseButtonDown(0))
            {
                NormalAttack();
            }
            // 기본공격2(마우스 오른쪽)
            if (Input.GetMouseButtonDown(1))
            {
                NormalAttack2();
            }
            // 쉬프트키
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ShiftAttack();
            }
            // 근접공격
            if (Input.GetMouseButtonDown(2))
            {
                PunchAttack();
            }
            // e키
            if(Input.GetKeyDown(KeyCode.E))
            {
                E_Func();
            }
            // 궁극기
            if (Input.GetKeyDown(KeyCode.R))
            {
                Ultimate();
            }
    }
    // 기본공격(마우스 왼쪽)
    public virtual void NormalAttack()
    {
        print("기본공격");
    }
    // 기본공격2(마우스 오른쪽)
    public virtual void NormalAttack2()
    {
        print("기본공격2");
    }
    // 쉬프트키
    public virtual void ShiftAttack()
    {
        print("SHIFT");
    }
    // 근접공격
    public virtual void PunchAttack()
    {
        print("주먹!");
    }
    // e키
    public virtual void E_Func()
    {
        print("eeee");
    }
    // 궁극기
    public virtual void Ultimate()
    {
        print("궁극기");
    }



    // 내가 아닌 다른 캐릭터의 위치를 동기화하기 위한 벡터 변수
    Vector3 otherPos;
    float animHorizontal = 0;
    float animVertical = 0;
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 동기화할 데이터를 서버에 넘겨주기
        // 만일, 스트림이 쓰기 모드 상태라면...
        if (stream.IsWriting)
        {
            // 나의 위치 좌표 벡터를 서버에 보낸다.
            stream.SendNext(transform.position);
            stream.SendNext(dirH);
            stream.SendNext(dirV);            
            /*Vector3 dir = Gravity();
            cc.Move(dir * speed * Time.deltaTime);*/
        }

        // 동기화될 데이터를 서버로부터 받기
        else
        {
            otherPos = (Vector3)stream.ReceiveNext();

            // 서버에 저장된 위치 좌표 벡터를 받는다.
            animHorizontal = (float)stream.ReceiveNext();
            animVertical = (float)stream.ReceiveNext();
        }
    }
}
