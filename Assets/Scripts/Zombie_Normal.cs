using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie_Normal : MonoBehaviour
{
    Animator anim;
    public GameObject z_AttackPoint;
    PlayerSystem playerSystem;
    Rigidbody rigid;
    Vector3 attackBasePos;
    Vector3 attackTargetPos;
    Material mat;
    NavMeshAgent nvAgent; // �׺������

    public float maxHealth = 30;
    public float curHealth;

    public Collider[] enemyDetectZone;
    Soldier soldier;
    public Vector3 offsetSize;
    public Vector3 offsetPos;


    bool enemyDetect;
    bool isMove;
    bool isAttack;

    //int playerLayer = LayerMask.NameToLayer("PlayerTeam");
    void Awake() {
        z_AttackPoint = GameObject.Find("DefencePoint");
        attackBasePos = z_AttackPoint.transform.position;
        playerSystem = z_AttackPoint.GetComponent<PlayerSystem>();
        mat = this.GetComponent<Material>();
        rigid = this.GetComponent<Rigidbody>();
        nvAgent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        
    }
    private void FixedUpdate() {
        enemyDetectZone = Physics.OverlapBox(this.transform.position + offsetPos, offsetSize / 2, Quaternion.identity) ;
        //enemyDetect = enemyDetectZone.Length > 0;
        if (enemyDetect == false)
        {
            Walk();
            FreezeVelocity();
        }
        else if (enemyDetect)
        {
            foreach (Collider enemyCol in enemyDetectZone)
            {
                if (enemyCol.gameObject != gameObject) // �ڱ� �ڽ��� ����
                {


                }
            }
            //Attack();
        }
    }
    void Update() {
        if (curHealth > 0)
        {
            
        }
        else
        {
            nvAgent.enabled = false;
            anim.SetTrigger("doDie");
        }

    }
    void Idle() {
    }
    void Walk() {
        nvAgent.destination = attackBasePos;
        anim.SetBool("IsWalk",true);
    }
    void Attack() {
        if (enemyDetect)
        {
            //anim.SetBool("IsWalk", false);
            nvAgent.SetDestination(transform.position);
            anim.SetTrigger("DoAttack");
        }

    }
    void FreezeVelocity() {
        if (isMove)
        {
            rigid.velocity = Vector3.zero; //ȸ���°� �ӵ��� 0���� ���������ν� ���� �浹 �� ȸ���� �ӵ� ���� �ذ� 
            rigid.angularVelocity = Vector3.zero;
        }
    }
    void OnDrawGizmos() {
        //Gizmos.DrawCube(this.transform.position,test);
        Gizmos.DrawWireCube(this.transform.position + offsetPos, offsetSize);
    }
}
