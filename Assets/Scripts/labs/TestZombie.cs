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
        z_AttackPointObj = GameObject.Find("DefencePoint"); // ó�� ���� �� ���� ������Ʈ Ÿ����
        z_AttackPoint = z_AttackPointObj.transform.position; // ���� �̵� ��ǥ ����
        attackBasePos = z_AttackPointObj.transform.position; // ���� ������ ���
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }
    private void Update() {
        if (curHealth > 0)
        {
            enemyDetectZone = Physics.OverlapBox(this.transform.position + offsetPos, offsetSize / 2, Quaternion.identity);
            if (enemyDetectZone.Length > 1) // ���� �ݶ��̴� ������ ������ 1�� �̻��� ��� enemyDetect = true;
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
        nav.SetDestination(walkPoint); // ��ǥ �̵������� �̵�
        Debug.Log("�ȴ���");
    }
    void FindEnemy(Collider[] enemyDetect) {
        isFind = false;
        Collider closestEnemy = null; // ���� ����� �ݶ��̴��� ������ ����
        float closestDistance = float.MaxValue; // ����� �Ÿ��� ���ؼ� ������ ����
        foreach (Collider enemyCol in enemyDetect) // �ݶ��̴� ���ο� ���� ������Ʈ ��ȸ
        {
            if (enemyCol.CompareTag("PlayerTeam")) // �ݶ��̴� �±װ� �÷��̾� ���̸�
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemyCol.transform.position); // ������Ʈ�� ������ ������Ʈ�� �Ÿ� ���
                if(distanceToEnemy < closestDistance) // ���� �ݶ��̴����� ���� �ݶ��̴��� �Ÿ����� �� ������
                {
                    closestDistance = distanceToEnemy; // �� �����(float ��ġ�� ����  0�� �����)���� ���� ����
                    closestEnemy = enemyCol; // �� ���� �� �� ����� �ݶ��̴��� ���� ����
                }
            }
        }
        enemy = closestEnemy.gameObject; // ����� �Ϸ�� ���� ������ �ݶ��̴��� ���� ������Ʈ�� ���
        z_AttackPoint = enemy.transform.position; // ���� ������ �ݶ��̴��� �������� ���
        isFind = true; // Ž�� ���� �� �Ϸ� �÷��� ����
        enemyDie = false;
    }
    void FreezeVelocity() {
        rigid.velocity = Vector3.zero; //ȸ���°� �ӵ��� 0���� ���������ν� ���� �浹 �� ȸ���� �ӵ� ���� �ذ�
        rigid.angularVelocity = Vector3.zero;
    }
}
