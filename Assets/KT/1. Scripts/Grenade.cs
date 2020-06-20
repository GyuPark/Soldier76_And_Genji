using System.IO;
using UnityEngine;
using Photon.Pun;

namespace Gyu
{
    //꼬리에 trail vfx가 붙어있다.
    //충돌하면 터진다.
    //터지면 explosion effect를 생성한다.
    //일정 반경 내에 enemy 들이 존재한다면, 데미지를 깎는다. 그리고 explosionforce를 적용한다..
    // 어떻게 하면 character controller를 잡아낼 것인가?
    //모든 게 끝나면 스스로 자신을 파괴한다.

    //생성되면 날아간다
    //z방향으로 launchSpeed 만큼

    public class Grenade : MonoBehaviourPun, IPunObservable
    {
        public GameObject explosionEffectFactory; //폭발 vfx
        int grenadeDamage = 80; //grenade 위력

        float explosionRangeAsRadius = 1f;

        float launchSpeed = 80f;
        float grenadeLifespan = 5f;

        public GameObject enemyHitUIFactory;
        public GameObject eliminatedSignal;
        public GameObject iKilledEnemy;

        //grenade 생성하면 지정 필
        public GameObject deathSkull;
        public Transform achievement;
        public Transform whoKilledWho;
        public Transform uiCenter;

        private void OnEnable()
        {
            Destroy(gameObject, grenadeLifespan);
        }

        private void Update()
        {
            shootRay();
        }

        private void FixedUpdate()
        {
            
        }

        void LateUpdate()
        {
            if (photonView.IsMine)
            {
                transform.position += transform.forward * launchSpeed * Time.deltaTime;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, otherPos, Time.deltaTime * 50.0f);
            }
        }

        //충돌하면 터진다.
        private void OnCollisionEnter(Collision collision)
        {
            //explosion effect를 생성한다.
            //- grenade의 위치에
            GameObject explosionFX = PhotonNetwork.Instantiate(Path.Combine("Soldier76Objects", "HelixRocketExplosion"), transform.position, Quaternion.identity);
            //- 4초 후 파괴한다.
            Destroy(explosionFX, 4f);

            if (collision.transform.gameObject.layer == LayerMask.NameToLayer("Player")) //맞은게 애너미라면
            {
                print("tlqkf");
                if(collision.gameObject.GetComponent<J_HP>())
                {
                    collision.gameObject.GetComponent<J_HP>().Damaged(grenadeDamage);
                }
                if(collision.gameObject.GetComponent<PlayerHP>())
                {
                    collision.gameObject.GetComponent<PlayerHP>().Damaged(grenadeDamage, collision.transform.position.x, collision.transform.position.y, collision.transform.position.z);
                }
                //EnemyHitUI를 생성한다.
                GameObject enemyHitUI = Instantiate(enemyHitUIFactory, uiCenter);
                enemyHitUI.transform.localScale = Vector3.one;

                //애너미가 내 grenade에 맞고 사망하면
                if (collision.transform.GetComponent<J_HP>().PlayerCurrHp < 0)
                {
                    deathSkull.SetActive(true);

                    GameObject eliminated = Instantiate(eliminatedSignal, achievement);
                    eliminated.transform.SetSiblingIndex(0);
                    Destroy(eliminated, 1.5f);

                    GameObject whoKilled = Instantiate(iKilledEnemy, whoKilledWho);
                    whoKilled.transform.SetSiblingIndex(0);
                    Destroy(whoKilled, 1.5f);
                } 
            }
            

            //모든 게 끝나면 스스로 자신을 파괴한다.
        }
        public float rayDis = 0.2f;
        void shootRay()
        {
            RaycastHit hitInfo;
            Ray ray = new Ray(transform.position, transform.forward);
            if(Physics.Raycast(ray, out hitInfo, rayDis))
            {
                if(hitInfo.transform.gameObject.layer==LayerMask.NameToLayer("block"))
                {
                    transform.forward = hitInfo.transform.forward;
                    //hitInfo.transform.gameObject.GetComponent<PlayerHP>().Damaged(grenadeDamage, hitInfo.transform.position.x, hitInfo.transform.position.y, hitInfo.transform.position.z);
                }
            }
        }


        Vector3 otherPos;
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
            }
            else
            {
                otherPos = (Vector3)stream.ReceiveNext();
            }
        }
    }
}
