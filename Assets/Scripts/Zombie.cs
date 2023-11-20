using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
public class Zombie : MonoBehaviour
{
    Animator anim;
    GameObject z_AttackPoint;
    PlayerSystem playerSystem;
    Soldier soldier;
    Vector3 attackBasePos;
    Vector3 attackTargetPos;
    NavMeshAgent nvAgent; // �׺������
    Vector3 moveVec;
    float zomVec_X, zomVec_Z;
    public List<GameObject> detectedObjects = new List<GameObject>();
   // public Collider enemyDetectZone;
    public Vector3 test;
    public Vector3 offset;

    public bool NormalZombie=true;
    public bool RunningZombie;
    bool enemyDetect;
    //find�� ���̽� ������Ʈ ã��  - ĳ���� �����̱� - �� �ݶ��̴� �߰� - ���� ����� �Ÿ� ��� - �� ������Ʈ���� �̵� - ���� - ������Ʈ�� ������� �ٽ� �ݺ�
    void Awake()
    {
        z_AttackPoint = GameObject.Find("DefencePoint");
        attackBasePos = z_AttackPoint.transform.position;
        playerSystem = z_AttackPoint.GetComponent<PlayerSystem>();
        nvAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //anim.wrapMode = WrapMode.Loop;// �ִϸ��̼� �ݺ�
        
    }
    private void FixedUpdate() {
        if(enemyDetect==false) { 
            if(NormalZombie == true)
            {
                Walk();
            }else if (RunningZombie == true)
            {
                Run();
            }
        }

        Collider[] enemyDetectZone = Physics.OverlapBox(this.transform.position + offset, test/2, Quaternion.identity);
        enemyDetect = enemyDetectZone.Length > 0;
        if (!enemyDetect && NormalZombie && !RunningZombie) {
            Walk();
        }
        else if (!enemyDetect && !NormalZombie && RunningZombie) {
            Run();
        }
        if (enemyDetect && NormalZombie && !RunningZombie) {
            foreach (Collider enemyCol in enemyDetectZone)
            {
                if (enemyCol.gameObject != gameObject) // �ڱ� �ڽ��� ����
                {
                    

                }
            }
            Attack();
        }
        else if(enemyDetect && !NormalZombie && RunningZombie) {
            Attack();
        }
        
    }
    void Update()
    {


    }
    void Idle() {
        
        //anim.Play("Z_Idle");
    }
    void Walk() {
        moveVec = new Vector3(zomVec_X, 0, zomVec_Z).normalized;
        nvAgent.destination = attackBasePos;
        if (playerSystem.HP > 0)
            anim.SetBool("IsWalk",true);
    }
    void Run() {
        if (playerSystem.HP > 0)
            anim.SetBool("IsRun", true);
    }
    void Attack() {
        if (enemyDetect)
        {
            nvAgent.SetDestination(transform.position);
            anim.SetTrigger("Attack");
        }

    }
    
    void OnDrawGizmos() {
        //Gizmos.DrawCube(this.transform.position,test);
        Gizmos.DrawWireCube(this.transform.position+offset, test);
    }
}
