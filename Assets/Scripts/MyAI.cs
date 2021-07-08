using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Invector.vCharacterController
{
    public class MyAI : MonoBehaviour
    {
        public AudioSource audio;
        public HPBar healthBar;
        public int maxHP;
        public int currentHP;
        public GameObject hpUI;
        public float jumpSpeed;

        NavMeshAgent agent;
        private Transform target;
        Animator anim;
        GameObject player;
        List<Rigidbody> allRbs;
        
        // Start is called before the first frame update
        void Start()
        {

            currentHP = maxHP;
            healthBar.SetMaxHealth(maxHP);

            allRbs = new List<Rigidbody>();
            allRbs.AddRange(GetComponentsInChildren<Rigidbody>());
            player = GameObject.FindGameObjectWithTag("Player");
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            target = GameObject.FindGameObjectWithTag("Player").transform;
            if(gameObject.tag == "AttackZombie")
                StartCoroutine(SetTarget());

            foreach (Rigidbody rb in allRbs)
            {
                rb.isKinematic = true;
            }


        }

        public void HasSight()
        {
            StartCoroutine(SetTarget());
        }
        public void Die()
        {
            foreach (Rigidbody rb in allRbs)
            {
                rb.isKinematic = false;
            }
            anim.enabled = false;
            agent.isStopped = true;
            Destroy(hpUI);
        }

        // Update is called once per frame

        void Update()
        {

            if (agent.isOnOffMeshLink)
            {
                //agent.speed = 1f;
                anim.SetBool("isJumping", true);
            }
            else
            {
                //agent.speed = 3f;
                anim.SetBool("isJumping", false);
            }
                

            if(player.GetComponent<vThirdPersonController>() != null)
            {
                if (player.GetComponent<vThirdPersonController>().currentHP <= 0)
                {
                    agent.isStopped = true;
                    anim.SetBool("isPlayerDead", true);
                }
                   

            }
    
           // anim.speed = agent.velocity.magnitude;

            //if(player.GetComponent<Animator>().)

            if (Vector3.Distance(player.transform.position, transform.position) < 1f)
            {
                anim.SetBool("isAttacking", true);
            }
            else
            {
                anim.SetBool("isAttacking", false);
            }

        }

        public void Damaged(int dmg)
        {
            audio.Play();
            currentHP -= dmg;
            healthBar.SetHealth(currentHP);
            if(currentHP <= 0)
            {
                Die();
            }
            else if(currentHP == 1)
            {
                anim.SetBool("isWounded", true);
                agent.speed /= 2;
            }
        }

        IEnumerator SetTarget()
        {
                yield return new WaitForSeconds(1f);
                agent.SetDestination(target.position);
                StartCoroutine(SetTarget());
            
            

        }
    }
}
