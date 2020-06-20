using UnityEngine;
using System.IO;
using Photon.Pun;
using TMPro;


//탄창 하나에 25발
//장전은 전체에서 현재를 빼면 가능
//
namespace Gyu
{
    public class PlayerWeapon : MonoBehaviourPun, IPunObservable
    {
        public float range = 100f;
        public int bulletsPerMag = 25;
        public int currentBullets;
        public int rifleDamage;

        public float fireRate = 0.1f;
        float fireTimer;
        public float rifleRange = 100f;

        Camera playerFirstPersonCamera;
        public Camera playerThirdPersonCamera;
        Animator anim;
        public TextMeshProUGUI currentBulletUI;

        public Transform shootPos;
        public ParticleSystem muzzleFlash;
        public GameObject helixRocket;

        public LineRenderer bulletTrail;
        public GameObject rifleLaser;
        public GameObject enemyHitUIFactory;
        public GameObject deathSkull;
        public GameObject eliminatedSignal;
        public GameObject iKilledEnemy;

        public Transform achievement;
        public Transform whoKilledWho;
        public Transform uiCenter;


        Soldier76Narrative sn;
        AudioSource _audioSource;
        public AudioClip bootSound;
        public AudioClip shootSound;
        public AudioClip reloadSound1;
        public AudioClip reloadSound2;
        public AudioClip fireHelixRocket;


        public enum TacticalVisorState
        {
            Ready,
            Enabled,
            NotReady
        }
        public TacticalVisorState tvs;
        public TacticalVisor tv;
        public GameObject circleRenderer;
        public float tacticalVisorTimer = 0f;

        

        #region Main
        void Start()
        {
            //if (!photonView.IsMine)
            //{
            //    return;
            //}

            sn = GetComponentInParent<Soldier76Narrative>();
            playerFirstPersonCamera = GetComponentInParent<Camera>();
            anim = GetComponent<Animator>();
            currentBullets = bulletsPerMag;
            _audioSource = GetComponent<AudioSource>();

            _audioSource.PlayOneShot(bootSound);

            grenadeState = GrenadeState.Ready;
            playerGrenadeCounter.SetActive(false);

            tvs = TacticalVisorState.Ready;
        }

        void Update()
        {
            if (!photonView.IsMine)
            {
                if (otherRifleSync)
                {
                    anim.SetBool("FireRifle", true);
                    anim.PlayInFixedTime("ClientRifle");
                }
                if (otherGrenadeSync)
                {
                    //anim.ResetTrigger("isIdle");
                    anim.ResetTrigger("isWalking");
                    anim.ResetTrigger("isRunning");
                    anim.ResetTrigger("isReloading");
                    anim.SetTrigger("isGrenading");
                    otherGrenadeSync = false;
                }
                if (otherReloadSync)
                {
                    anim.ResetTrigger("isIdle");
                    anim.ResetTrigger("isWalking");
                    anim.ResetTrigger("isRunning");
                    anim.ResetTrigger("isGrenading");
                    anim.SetTrigger("isReloading");
                    otherReloadSync = false;
                }
                return;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            if (Input.GetButton("Fire1"))
            {
                if (currentBullets > 0)
                {
                    FireRifle();
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                if (grenadeState == GrenadeState.Ready)
                {
                    FireGrenade();
                }
            }


            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (tvs == TacticalVisorState.Ready)
                {
                    tvs = TacticalVisorState.Enabled;
                    tv.gameObject.SetActive(true);
                    circleRenderer.SetActive(true);
                }
            }

            if (tvs == TacticalVisorState.Enabled)
            {
                tacticalVisorTimer += Time.deltaTime;
                if (tacticalVisorTimer > 6f)
                {
                    tacticalVisorTimer = 0f;
                    tvs = TacticalVisorState.NotReady;
                    tv.gameObject.SetActive(false);
                    circleRenderer.SetActive(false);
                    tvs = TacticalVisorState.Ready;
                }
            }

            //시간이 총 발사 주기보다 못 미친다면
            if (fireTimer < fireRate)
            {
                fireTimer += Time.deltaTime;
            }

            comboTimer += Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (!photonView.IsMine) //내가 아닐 때
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("ClientRifle"))
                {
                    anim.SetBool("FireRifle", false); //매 프레임 켜놓을 수는 없으니 꺼준다.
                    otherRifleSync = false;
                }
                return;
            }

            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("ClientRifle"))
            {
                anim.SetBool("FireRifle", false); //매 프레임 켜놓을 수는 없으니 꺼준다.
                rifleSync = false;
            }
        }

        bool rifleSync = false;
        bool grenadeSync = false;
        bool reloadSync = false;

        bool otherRifleSync = false;
        bool otherGrenadeSync = false;
        bool otherReloadSync = false;


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(rifleSync);

                stream.SendNext(grenadeSync);
                grenadeSync = false;
                stream.SendNext(reloadSync);
                reloadSync = false;
            }
            else
            {
                otherRifleSync = (bool)stream.ReceiveNext();
                otherGrenadeSync = (bool)stream.ReceiveNext();
                otherReloadSync = (bool)stream.ReceiveNext();
            }
        }

        #endregion

        RaycastHit hitinfo2;

        #region Helper
        private void FireRifle()
        {
            //발사주기에 못 미치면 발사 안함
            if (fireTimer < fireRate || currentBullets <= 0) return;

            //발사!
            //-시간 리셋
            fireTimer = 0f;
            //-총알 빼기
            currentBullets--;
            currentBulletUI.text = currentBullets.ToString();

            Transform cameraTransform;
            if (playerFirstPersonCamera.enabled == true) //1인칭 카메라 컴포넌트가 켜져있으면
            {
                cameraTransform = playerFirstPersonCamera.transform;
            }
            else //아니라면 3인칭 카메라를 쓴다
            {
                cameraTransform = playerThirdPersonCamera.transform;
            }

            //ray와 bullet 방향 설정
            Vector3 raycastDir = Vector3.zero;
            Vector3 bulletDir = Vector3.zero;
            if (tvs == TacticalVisorState.Enabled && tv.closestTarget != null)
            {
                raycastDir = tv.closestTarget.targetObject.transform.position - cameraTransform.position;
                bulletDir = tv.closestTarget.targetObject.transform.position - shootPos.position;
            }
            else
            {
                raycastDir = cameraTransform.forward;
                bulletDir = shootPos.transform.forward;
            }

            //레이발사
            Ray ray = new Ray(cameraTransform.position, raycastDir);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rifleRange))
            {
                //GameObject laser = PhotonNetwork.Instantiate(Path.Combine("Soldier76Objects", "vfx_Projectile_LaserBlue"), shootPos.position, Quaternion.identity);
                //laser.transform.forward = bulletDir;

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    //콤보계산기
                    HitCombo();

                    //EnemyHitUI를 생성한다.
                    GameObject enemyHitUI = Instantiate(enemyHitUIFactory, uiCenter);
                    enemyHitUI.transform.localScale = Vector3.one * comboCount;
                    if(hit.transform.GetComponent<J_HP>())
                    {
                        hit.transform.GetComponent<J_HP>().Damaged(30);
                        if (hit.transform.GetComponent<J_HP>().PlayerCurrHp < 0)
                        {
                            sn.PlayAudio(sn.killAudioClip[Random.Range(0, sn.killAudioClip.Length)]);

                            deathSkull.SetActive(true);

                            GameObject eliminated = Instantiate(eliminatedSignal, achievement);
                            eliminated.transform.SetSiblingIndex(0);
                            Destroy(eliminated, 1.5f);

                            GameObject whoKilled = Instantiate(iKilledEnemy, whoKilledWho);
                            whoKilled.transform.SetSiblingIndex(0);
                            Destroy(whoKilled, 1.5f);
                        }
                    }


                    //맞은 놈이 soldier76 라면
                    //맞은 놈이 genji라면

                    //hp를 bulletDamage만큼 깐다.
                    //hit.transform.GetComponent<Enemy>().Damage(rifleDamage);


                    //애너미가 내 총알에 맞고 사망하면
                    
                }
                else if(hit.transform.gameObject.layer == LayerMask.NameToLayer("block"))
                {
                    if (Physics.Raycast(hit.point, hit.transform.forward, out hitinfo2))
                    {
                        print("해치웠나?");
                        hitinfo2.transform.GetComponent<PlayerHP>().Damaged(rifleDamage, hitinfo2.transform.position.x, hitinfo2.transform.position.y, hitinfo2.transform.position.z);
                    }
                }
            }

            anim.PlayInFixedTime("ClientRifle");
            anim.SetBool("FireRifle", true);
            rifleSync = true;

            muzzleFlash.Play();
            PlayShootSound();
        }


        //void SpawnBulletTrail(Vector3 hitPoint)
        //{
        //    GameObject bulletTrailEffect = Instantiate(bulletTrail.gameObject, shootPos.position, Quaternion.identity);
        //    LineRenderer lineR = bulletTrailEffect.GetComponent<LineRenderer>();
        //    lineR.SetPosition(0, shootPos.position);
        //    lineR.SetPosition(1, hitPoint);

        //    Destroy(bulletTrailEffect, 1f);
        //}

        void PlayShootSound()
        {
            //_audioSource.clip = shootSound;
            //_audioSource.Play();
            _audioSource.PlayOneShot(shootSound);
        }

        float comboCount = 0.5f;
        float comboIncrement = 0.08f;
        float comboMax = 1.2f;
        float comboTimer = 0f;
        float comboFreq = 0.8f;

        void HitCombo()
        {
            //print("comboTimer : " + comboTimer);
            //print("comboCount :" + comboCount);

            //특정 시간 안에 enemy를 다시 맞추면 특정 숫자의 combo count가 올라간다.
            if (comboTimer < comboFreq)
            {
                comboCount += comboIncrement;
            }
            //특정 시간 안에 enemy를 다시 맞추지 못하면 comboCount는 1로 돌아간다. 
            if (comboTimer > comboFreq)
            {
                comboCount = 0.5f;
                comboTimer = 0f;
            }

            //combo count는 comboMax까지 올라갈 수 있다.
            comboCount = Mathf.Clamp(comboCount, 0.5f, comboMax);
        }

        void Reload()
        {
            if (currentBullets == 25) return;

            anim.ResetTrigger("isIdle");
            anim.ResetTrigger("isWalking");
            anim.ResetTrigger("isRunning");
            anim.ResetTrigger("isGrenading");
            anim.SetTrigger("isReloading");

            reloadSync = true;
        }

        void PlayReloadSound1() //animation event
        {
            _audioSource.clip = reloadSound1;
            _audioSource.Play();
        }
        void PlayReloadSound2() //animation event
        {
            _audioSource.clip = reloadSound2;
            _audioSource.Play();
        }

        void PlayDashSound() //animation event
        {

        }

        void ReloadAddBullets() //animation event 에서 access
        {
            currentBullets += bulletsPerMag - currentBullets;
            currentBulletUI.text = currentBullets.ToString();
        }

        public enum GrenadeState { Ready, NotReady}
        public GrenadeState grenadeState;
        public GameObject playerGrenadeCounter;

        void FireGrenade()
        {
            //유탄발사 애니메이션
            anim.ResetTrigger("isIdle");
            anim.ResetTrigger("isWalking");
            anim.ResetTrigger("isRunning");
            anim.ResetTrigger("isReloading");
            anim.SetTrigger("isGrenading");

            grenadeSync = true;

            _audioSource.PlayOneShot(fireHelixRocket);

            //유탄발사
            GameObject grenadeObj = PhotonNetwork.Instantiate(Path.Combine("Soldier76Objects", "HelixRocket"), shootPos.position, shootPos.rotation);
            //shootPos.transform.forward = grenadeObj.transform.forward;
            Grenade grenadeComponent = grenadeObj.GetComponent<Grenade>();
            grenadeComponent.deathSkull = deathSkull;
            grenadeComponent.achievement = achievement;
            grenadeComponent.whoKilledWho = whoKilledWho;
            grenadeComponent.uiCenter = uiCenter;

            //카운트 시작
            grenadeState = GrenadeState.NotReady;
            playerGrenadeCounter.SetActive(true);
        }
        #endregion
    } 
}
