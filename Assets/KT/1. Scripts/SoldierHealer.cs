using UnityEngine;
using Photon.Pun;

namespace Gyu
{
    //생성될 때 커지는 애니메이션
    //일정 반경 안에 player가 있으면, 1초에 한 번씩 40씩 체력 증가
    //한 healer 최대량은 200
    //lifeSpan은 5seconds 

    //instantiate된다
    //animation sync한다 RPC?
    //animation event에 따라 PhotonNetwork.Destroy 한다.

    //체력추가는 rpc

    public class SoldierHealer : MonoBehaviourPun
    {
        #region Field
        public int instantiatorID;
        public GameObject instantiator;

        Animator anim;
        float healerLifespan = 5f;

        float lifeClock = 0f;
        float healClock = 0f;

        bool isDestroying = false;
        #endregion

        #region Main
        private void OnEnable()
        {
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            //scene에 photonID가 InstantiatorID와 동일한 오브젝트를 찾아서, instantiator에 할당한다.
            GameObject[] rootObjectsInTheScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var item in rootObjectsInTheScene)
            {
                var pv = item.GetComponent<PhotonView>();
                if (pv != null && pv.ViewID == instantiatorID)
                {
                    instantiator = item;
                }
            }
        }

        private void Update()
        {
            //내가 아니면
            if (!photonView.IsMine)
                return;

            //나라면

            lifeClock += Time.deltaTime;
            if (lifeClock > healerLifespan && !isDestroying)
            {
                isDestroying = true;
                anim.SetTrigger("destroy");
            }

            //일정 반경 안에 player가 있으면, 1초에 한 번씩 40씩 체력 증가
            healClock += Time.deltaTime;
            if (healClock > 1f)
            {
                healClock = 0f;
                if (instantiator != null && Vector3.Distance(transform.position, instantiator.transform.position) < 4.5f)
                    photonView.RPC("Heal", RpcTarget.All);
            }
        }
        #endregion

        #region Helper
        void DestroyHealer() //animation event로 access
        {
            PhotonNetwork.Destroy(gameObject);
        }

        [PunRPC]
        void SetInstantiatorID(int photonID)
        {
            //scene에 있는 게임 오브젝트 중 해당 photonId를 갖고 있는 오브젝트를 instantiator로 지정한다.
            instantiatorID = photonID;
        }

        [PunRPC]
        void Heal()
        {
            instantiator.GetComponentInParent<PlayerHP>().HP += 40;
        }
        #endregion
    } 
}
