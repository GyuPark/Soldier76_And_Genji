using UnityEngine;
using Photon.Pun;

namespace Gyu
{
    public class PlayerState : MonoBehaviourPun, IPunObservable
    {

        #region Field
        public enum ClientState
        {
            Idle,
            Walk,
            Dash,
            Rifle,
            Grenade,
            Reload,
            Buttstroke
        }

        public ClientState clientState;

        public Animator clientAnim;

        AudioSource audioSource;
        //public AudioClip runAudioClip;
        
        #endregion

        #region Main

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (!photonView.IsMine)
            {
                //enemy의 animation trigger 받아오기
                if (otherIdleSync)
                {
                    clientAnim.ResetTrigger("isWalking");
                    clientAnim.ResetTrigger("isRunning");

                    clientAnim.SetTrigger("isIdle");
                    otherIdleSync = false;
                }
                if (otherWalkSync)
                {
                    clientAnim.ResetTrigger("isIdle");
                    clientAnim.ResetTrigger("isRunning");

                    clientAnim.SetTrigger("isWalking");
                    otherWalkSync = false;
                }
                if (otherRunSync)
                {
                    clientAnim.ResetTrigger("isIdle");
                    clientAnim.ResetTrigger("isWalking");

                    clientAnim.SetTrigger("isRunning");
                    otherRunSync = false;
                }
                return;
            }

            if (clientAnim.GetCurrentAnimatorStateInfo(0).IsName("ClientReload"))
            {
                clientState = ClientState.Reload;
            }
            if (clientAnim.GetCurrentAnimatorStateInfo(0).IsName("ClientRifle"))
            {
                clientState = ClientState.Rifle;
            }

            switch (clientState)
            {
                case ClientState.Idle:
                    if (IsMoving())
                    {
                        clientAnim.ResetTrigger("isIdle");
                        clientAnim.ResetTrigger("isRunning");

                        clientAnim.SetTrigger("isWalking");
                        //내 sync 변수 true
                        walkSync = true;

                        clientState = ClientState.Walk;
                        //print("state : " + clientState.ToString());
                    }
                    break;
                case ClientState.Walk:
                    if (!IsMoving())
                    {
                        clientAnim.ResetTrigger("isWalking");
                        clientAnim.SetTrigger("isIdle");
                        //내 sync 변수 true
                        idleSync = true;

                        clientState = ClientState.Idle;
                        //print("state : " + clientState.ToString());
                    }
                    else if (IsRunning())
                    {
                        clientAnim.ResetTrigger("isWalking");
                        clientAnim.SetTrigger("isRunning");
                        //내 sync 변수 true
                        runSync = true;

                        clientState = ClientState.Dash;
                        //print("state : " + clientState.ToString());
                    }
                    break;
                case ClientState.Dash:
                    if (!IsRunning() || (IsRunning() && !IsMoving()))
                    {
                        clientAnim.ResetTrigger("isIdle");
                        clientAnim.ResetTrigger("isRunning");
                        clientAnim.SetTrigger("isWalking");
                        //내 sync 변수 true
                        walkSync = true;

                        clientState = ClientState.Walk;
                        //print("state dash1 : " + clientState.ToString());
                    }
                    break;
                case ClientState.Rifle:
                    if (!clientAnim.GetCurrentAnimatorStateInfo(0).IsName("ClientRifle"))
                    {
                        clientState = ClientState.Idle;
                    }
                    break;
                case ClientState.Grenade:
                    break;
                case ClientState.Reload:
                    if (!clientAnim.GetCurrentAnimatorStateInfo(0).IsName("ClientReload"))
                    {
                        clientState = ClientState.Idle;
                    }
                    break;
                case ClientState.Buttstroke:
                    break;
                default:
                    break;
            }
        }

        bool idleSync = false;
        bool walkSync = false;
        bool runSync = false;

        bool otherIdleSync = false;
        bool otherWalkSync = false;
        bool otherRunSync = false;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(idleSync);
                idleSync = false;
                stream.SendNext(walkSync);
                walkSync = false;
                stream.SendNext(runSync);
                runSync = false;
            }
            else
            {
                otherIdleSync = (bool)stream.ReceiveNext();
                otherWalkSync = (bool)stream.ReceiveNext();
                otherRunSync = (bool)stream.ReceiveNext();
            }
        }
        #endregion

        #region Helper
        /// <summary>
        /// Initialise
        /// </summary>
        void Init()
        {
            clientState = ClientState.Idle;
            audioSource = GetComponent<AudioSource>();
        }

        bool IsMoving()
        {
            bool isMoving = false;
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            if (h != 0 || v != 0) //any WASD input at all?
            {
                isMoving = true;
            }
            return isMoving;
        }

        bool IsRunning()
        {
            bool isRunning = false;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
            }
            return isRunning;
        }

        

        #endregion

    } 
}
