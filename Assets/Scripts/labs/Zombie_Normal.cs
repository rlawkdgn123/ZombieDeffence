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
        Collider closestEnemy = null; // ���� ����� �ݶ��̴��� ����
        float closestDistance = float.MaxValue; // ���԰��� �ִ�ġ�� �Ҵ�

        foreach (Collider enemyCol in enemy) // �ݶ��̴� ���ο� ���� ������Ʈ ��ȸ
        {
            if (enemyCol.gameObject != gameObject) // ���� ������Ʈ ����
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemyCol.transform.position); //�� �ݶ��̴����� �Ÿ� ���
                if (distanceToEnemy < closestDistance) // ���� ����� �ݶ��̴���  closestEnemy�� ����
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
            Collider closestEnemy = FindClosestEnemy(enemyDetectZone); // ���� ����� �ݶ��̴��� ����
            if (closestEnemy != null) // �ݶ��̴��� �����Ǿ��� ���
            {
                enemyDetect = true;
                float distanceToEnemy = Vector3.Distance(transform.position, closestEnemy.transform.position); // ���� ����� �ݶ��̴��� ������ �Ÿ��� ���
                Debug.Log(distanceToEnemy);
                float attackThreshold = 2.0f; // �ٰ����� ������ �Ÿ� ����
                if (distanceToEnemy <= attackThreshold) // �ش� �Ÿ����� ������ ��� ���� ���� ����
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
        Debug.Log("�̵��ϰ��־��");
        yield return new WaitForSeconds(1.5f);
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
