using UnityEngine;

namespace Gyu
{
    public class Enemy : MonoBehaviour
    {
        float attackTimer;
        float attackFreq = 6f;
        int enemyAttackDamage = 10;

        int hp;
        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }

        private void Start()
        {
            HP = 200;
        }

        private void Update()
        {
            attackTimer += Time.deltaTime;
            Attack();
        }

        public void Damage(int damage)
        {
            HP -= damage;
        }

        public void Attack()
        {
            //3초에 한 번 player를 공격한다.
            if (attackTimer > attackFreq)
            {
                attackTimer = 0f;
                GameObject player = GameObject.Find("Soldier76");
                //player.GetComponent<PlayerHP>().Damaged(enemyAttackDamage, transform.position);
            }
        }
    } 
}
