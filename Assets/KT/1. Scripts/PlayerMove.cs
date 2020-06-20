using UnityEngine;
using Photon.Pun;

namespace Gyu
{
    public class PlayerMove : MonoBehaviourPun, IPunObservable
    {
        CharacterController cc;
        AudioSource audioSource;
        public AudioClip jumpClip;

        Vector3 dir;
        public float walkSpeed;
        public float dashSpeed;
        float moveSpeed;
        [HideInInspector] public bool contact;

        [HideInInspector] public float yVelocity = 0f;
        float gravityConstant = -9.8f;
        public float jumpPower;

        PlayerState playerState;

        #region Main
        void Start()
        {
            if (!photonView.IsMine)
                return;

            Init();
        }

        void Update()
        {
            if (!photonView.IsMine)
            {
                float lerpSpeed = 50f;
                //끊김을 보간을 통해 보정
                transform.position = Vector3.Lerp(transform.position, otherPos, Time.deltaTime * lerpSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, otherRot, Time.deltaTime * lerpSpeed);
                return;
            }
                

            SetXZDir();
            Land();
            Jump();
            ApplyGravity();
            Move();
        }
        #endregion

        #region Help
        /// <summary>
        /// initialise
        /// </summary>
        void Init()
        {
            cc = GetComponent<CharacterController>();
            playerState = GetComponent<PlayerState>();
            audioSource = GetComponent<AudioSource>();
            walkSpeed = 8f;
            dashSpeed = 13f;
            jumpPower = 5f;
        }

        void SetXZDir()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            dir = transform.forward * v + transform.right * h;
            dir.Normalize();
        }

        void Land()
        {
            if (cc.isGrounded)
            {
                if (!contact) //지면에 착지하는 순간
                {
                    //print(yVelocity);
                    contact = true;
                }
                yVelocity = 0f;
            }
        }

        void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (cc.isGrounded)
                {
                    yVelocity += jumpPower;
                    contact = false;
                    //print("jumped");
                    //print("Jump velocity : " + yVelocity);

                    audioSource.clip = jumpClip;
                    audioSource.Play();
                }
            }
        }

        void ApplyGravity()
        {
            yVelocity += gravityConstant * Time.deltaTime;
            dir.y = yVelocity / moveSpeed;
        }

        void Move()
        {
            if (playerState.clientState == PlayerState.ClientState.Dash)
            {
                moveSpeed = dashSpeed;
            }
            else
            {
                moveSpeed = walkSpeed;
            }
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }


        Vector3 otherPos;
        Quaternion otherRot;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            // 동기화될 데이터를 서버로부터 받기
            else
            {
                otherPos = (Vector3)stream.ReceiveNext();
                otherRot = (Quaternion)stream.ReceiveNext();
            }
        }
        #endregion

    } 
}
