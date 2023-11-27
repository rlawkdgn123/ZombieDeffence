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
    NavMeshAgent nvAgent; // 네비게이터

    public float maxHealth = 30;
    public float curHealth = 30;

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
        //mat = this.GetComponent<Material>();
        rigid = this.GetComponent<Rigidbody>();
        nvAgent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
    }
    private void FixedUpdate() {
        if (curHealth > 0)
        {
            if (!isAttack)
            {
                //StartCoroutine("Wait");
                Walk();
            }
            FreezeVelocity();
        }
        else
        {
            nvAgent.enabled = false;
            anim.SetTrigger("doDie");
        }
    }
    Collider FindClosestEnemy(Collider[] enemy) {
        Collider closestEnemy = null; // 가장 가까운 콜라이더를 대입
        float closestDistance = float.MaxValue; // 대입값을 최대치로 할당

        foreach (Collider enemyCol in enemy) // 콜라이더 내부에 들어온 오브젝트 순회
        {
            if (enemyCol.gameObject != gameObject) // 본인 오브젝트 제외
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemyCol.transform.position); //적 콜라이더와의 거리 계산
                if (distanceToEnemy < closestDistance) // 가장 가까운 콜라이더를  closestEnemy에 대입
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = enemyCol;
                }
            }
        }
        return closestEnemy;
    }
    void Update() {
        enemyDetectZone = Physics.OverlapBox(this.transform.position + offsetPos, offsetSize / 2, Quaternion.identity);
        enemyDetect = enemyDetectZone.Length > 1;
        if (enemyDetect)
        {
            Collider closestEnemy = FindClosestEnemy(enemyDetectZone); // 가장 가까운 콜라이더를 대입
            if (closestEnemy != null) // 콜라이더가 감지되었을 경우
            {
                enemyDetect = true;
                float distanceToEnemy = Vector3.Distance(transform.position, closestEnemy.transform.position); // 가장 가까운 콜라이더의 값과의 거리를 계산
                Debug.Log(distanceToEnemy);
                float attackThreshold = 2.0f; // 다가가서 정지할 거리 지정
                if (distanceToEnemy <= attackThreshold) // 해당 거리보다 좁혀질 경우 어택 로직 실행
                {
                    StartCoroutine("Attack", closestEnemy);
                }
                else
                {
                    nvAgent.speed = 5;
                }
            }
        }
    }
    void Walk() {
        if (!isAttack)
        {
            nvAgent.SetDestination(attackBasePos);
        }
        anim.SetBool("IsWalk", true);
    }
    IEnumerator Attack(Collider closestEnemy) {
        isAttack = true;
        anim.SetBool("IsWalk", false);
        nvAgent.SetDestination(closestEnemy.transform.position);
        nvAgent.speed = 0;

        anim.SetTrigger("DoAttack");
        yield return new WaitForSeconds(1.0f);
        isAttack = false;
    }
    IEnumerator Wait() {
        Walk();
        Debug.Log("이동하고있어요");
        yield return new WaitForSeconds(1.5f);
    }
    void FreezeVelocity() {
        if (isMove)
        {
            rigid.velocity = Vector3.zero; //회전력과 속도를 0으로 설정함으로써 물리 충돌 시 회전과 속도 문제 해결
            rigid.angularVelocity = Vector3.zero;
        }
    }
    void OnDrawGizmos() {
        //Gizmos.DrawCube(this.transform.position,test);
        Gizmos.DrawWireCube(this.transform.position + offsetPos, offsetSize);
    }
}
