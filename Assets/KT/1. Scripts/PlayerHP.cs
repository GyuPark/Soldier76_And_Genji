using UnityEngine;
using Photon.Pun;

/// <summary>
/// 플레이어 체력
/// </summary>
namespace Gyu 
{
    public class PlayerHP : MonoBehaviourPun
    {
        #region Field
        int hp;
        public int HP
        {
            get { return hp; }
            set
            {
                hp = value;
                hp = Mathf.Clamp(hp, 0, 200);
            }
        }

        AudioSource audioSource;
        public AudioClip playerHitAudio;

        public GameObject playerHitDir;
        public Transform centerUI;

        public GameObject playerHitBoundary;
        public Transform playerUI;

        CameraFieldOfView cfv;

        #endregion


        #region Main
        private void Start()
        {
            if (!photonView.IsMine)
                return;

            HP = 200;
            cfv = GetComponentInChildren<CameraFieldOfView>();
            audioSource = GetComponent<AudioSource>();
        }
        #endregion
        private void Update()
        {
            if (HP <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        /// <summary>
        /// damage inflicted on Soldier76
        /// </summary>
        /// <param name="damage">damage amount</param>
        /// <param name="x">Vector3.x of WorldPos from which attack occurs</param>
        /// <param name="y">Vector3.y of WorldPos from which attack occurs</param>
        /// <param name="z">Vector3.z of WorldPos from which attack occurs</param>
        public void Damaged(int damage, float x, float y, float z)
        {
            photonView.RPC("DamageFunc", RpcTarget.AllBuffered, damage, x, y, z);
        }

        [PunRPC]
        void DamageFunc(int damage, float x, float y, float z)
        {
            Vector3 location = new Vector3(x, y, z);
            HP -= damage;

            //맞으면 맞는 소리를 내고
            //audioSource.PlayOneShot(playerHitAudio);

            //피격 순간 적의 transform을 받아와 damageindicator를 생성하고
            GameObject damageIndicator = Instantiate(playerHitDir, centerUI);
            damageIndicator.GetComponent<DamageIndicatorUI>().player = transform;
            damageIndicator.GetComponent<DamageIndicatorUI>().enemy = location;

            //화면 blur효과를 준다.
            GameObject damageBlur = Instantiate(playerHitBoundary, playerUI);
        }
    } 
}
