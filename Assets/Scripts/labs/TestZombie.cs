using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestZombie : MonoBehaviour
{
    Animator anim;
    NavMeshAgent nav;
    Rigidbody rigid;
    public Collider[] enemyDetectZone;
    public GameObject z_AttackPointObj;
    public Vector3 z_AttackPoint;
    public Vector3 offsetSize;
    public Vector3 offsetPos;
    public Vector3 attackBasePos;
    public GameObject enemy;

    public float maxHealth = 30;
    public float curHealth = 30;

    bool enemyDetect;
    bool isWalk;
    bool isFind;
    bool isAttack;
    bool enemyDie = true;
    private void Awake() {
        z_AttackPointObj = GameObject.Find("DefencePoint"); // 처음 시작 시 본진 오브젝트 타겟팅
        z_AttackPoint = z_AttackPointObj.transform.position; // 메인 이동 좌표 변수
        attackBasePos = z_AttackPointObj.transform.position; // 본진 포지션 백업
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }
    private void Update() {
        if (curHealth > 0)
        {
            enemyDetectZone = Physics.OverlapBox(this.transform.position + offsetPos, offsetSize / 2, Quaternion.identity);
            if (enemyDetectZone.Length > 1) // 본인 콜라이더 포함한 갯수가 1개 이상일 경우 enemyDetect = true;
                enemyDetect = true; else enemyDetect = false;
            if (!enemyDetect && !isWalk)
                Walk(attackBasePos);
            else
            {
                if (enemyDie)
                    FindEnemy(enemyDetectZone);
                if (isFind && !enemyDie)
                    Walk(enemy.transform.position);
            }
            FreezeVelocity();
        }
        else
        {
            nav.enabled = false;
            anim.SetTrigger("doDie");
        }
    }
    void Walk(Vector3 walkPoint) {
        isWalk = true;
        nav.SetDestination(walkPoint); // 목표 이동값으로 이동
        Debug.Log("걷는중");
    }
    void FindEnemy(Collider[] enemyDetect) {
        isFind = false;
        Collider closestEnemy = null; // 가장 가까운 콜라이더를 대입할 변수
        float closestDistance = float.MaxValue; // 가까운 거리를 비교해서 대입할 변수
        foreach (Collider enemyCol in enemyDetect) // 콜라이더 내부에 들어온 오브젝트 순회
        {
            if (enemyCol.CompareTag("PlayerTeam")) // 콜라이더 태그가 플레이어 팀이면
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemyCol.transform.position); // 오브젝트와 감지된 오브젝트의 거리 계산
                if(distanceToEnemy < closestDistance) // 이전 콜라이더보다 현재 콜라이더의 거리값이 더 작으면
                {
                    closestDistance = distanceToEnemy; // 더 가까운(float 수치가 더욱  0에 가까운)값을 새로 대입
                    closestEnemy = enemyCol; // 비교 성공 시 더 가까운 콜라이더를 새로 대입
                }
            }
        }
        enemy = closestEnemy.gameObject; // 계산이 완료된 새로 대입할 콜라이더의 게임 오브젝트를 백업
        z_AttackPoint = enemy.transform.position; // 새로 대입한 콜라이더의 포지션을 백업
        isFind = true; // 탐색 루프 후 완료 플래그 선언
        enemyDie = false;
    }
    void FreezeVelocity() {
        rigid.velocity = Vector3.zero; //회전력과 속도를 0으로 설정함으로써 물리 충돌 시 회전과 속도 문제 해결
        rigid.angularVelocity = Vector3.zero;
    }
}
