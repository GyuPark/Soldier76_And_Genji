using System.IO;
using UnityEngine;
using Photon.Pun;

namespace Gyu
{
    //Heal State : ready notready
    public class PlayerHeal : MonoBehaviourPun
    {
        public GameObject playerHealCounter;
        public GameObject soldierHealer;
        public AudioClip healAudio;

        public enum HealState
        {
            Ready,
            NotReady
        }

        public HealState healState;


        private void Start()
        {
            healState = HealState.Ready;
            playerHealCounter.SetActive(false);
        }

        private void Update()
        {
            if (!photonView.IsMine)
                return;

            switch (healState)
            {
                case HealState.Ready:
                    //heal skill 발동하면 not ready로
                    if (Input.GetKeyDown(KeyCode.E) && healState == HealState.Ready)
                    {
                        healState = HealState.NotReady;

                        //쿨타임 count 시작
                        playerHealCounter.SetActive(true);
                        GetComponent<AudioSource>().PlayOneShot(healAudio);

                        //Heal Object 생성
                        GameObject soldierHeal = PhotonNetwork.Instantiate(Path.Combine("Soldier76Objects", "SoldierHealer"), transform.position, Quaternion.identity);
                        int instantiatorID = transform.parent.GetComponentInParent<PhotonView>().ViewID;
                        soldierHeal.GetComponent<PhotonView>().RPC("SetInstantiatorID", RpcTarget.All, instantiatorID);
                    }
                    break;
                case HealState.NotReady:
                    //쿨타임 차면 다시 ready로.. (HealCounterUI에서)
                    break;
                default:
                    break;
            }
        }
    } 
}
