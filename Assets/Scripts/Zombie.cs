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
    NavMeshAgent nvAgent; // 네비게이터
    Vector3 moveVec;
    float zomVec_X, zomVec_Z;
    public List<GameObject> detectedObjects = new List<GameObject>();
   // public Collider enemyDetectZone;
    public Vector3 test;
    public Vector3 offset;

    public bool NormalZombie=true;
    public bool RunningZombie;
    bool enemyDetect;
    //find로 베이스 오브젝트 찾기  - 캐릭터 움직이기 - 적 콜라이더 발견 - 가장 가까운 거리 계산 - 적 오브젝트에게 이동 - 공격 - 오브젝트가 사라지면 다시 반복
    void Awake()
    {
        z_AttackPoint = GameObject.Find("DefencePoint");
        attackBasePos = z_AttackPoint.transform.position;
        playerSystem = z_AttackPoint.GetComponent<PlayerSystem>();
        nvAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //anim.wrapMode = WrapMode.Loop;// 애니메이션 반복
        
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
                if (enemyCol.gameObject != gameObject) // 자기 자신은 제외
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
