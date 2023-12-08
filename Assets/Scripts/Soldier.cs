using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    NavMeshAgent nav;
    Animator anim;

    [SerializeField] GameObject Center;
    [SerializeField] Transform Main_Center;     //������ ��ġ ��

    [SerializeField] Collider now_target;       //���� ����� �÷��̾��� Collider�� �����ϱ� ���� ����
    [SerializeField] Collider temp_target;      //������ Collider (���� �÷��� �߿� ������ ���� / ���� Collider)

    [SerializeField] float default_speed;         //NavMeshAgent�� ó�� �ӵ� ���� �޾Ƴ��� ���� ����
    [SerializeField] float min_distance;        //�÷��̾���� �ּ� �Ÿ� ���� ��

    [SerializeField] public float curHealth;
    [SerializeField] public float maxHealth;
    [SerializeField] public float attackDamage;

    [SerializeField] bool isAttack;
    [SerializeField] bool isDie;

    [SerializeField] Enemy enemyHealth;

    #region ����� ����
    [SerializeField] Vector3 size;              //������ �������ڽ��� ������
    [SerializeField] Vector3 offset;            //������ �������ڽ��� ��ġ
    [SerializeField] Collider[] cols;           //������ �������ڽ��� ������ Collider �����
    [SerializeField] LayerMask target_layer;    //Ư�� ���̾ ���� ������Ʈ�� �����ϱ� ���� ���̾� ����ũ
    #endregion

    private void Awake() {
        Center = GameObject.Find("DefencePointEnemy"); // ó�� ���� �� ���� ������Ʈ Ÿ����
        Main_Center = Center.transform; // ���� Ʈ������ ���� �޾Ƴ���
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        default_speed = nav.speed;               //NavMeshAgent�� ó�� �ӵ� ���� �޾Ƴ���
        anim.SetBool("IsWalk", true);
    }

    void FixedUpdate() 
        {
        if (curHealth > 0 && !isDie)
            {
                cols = Physics.OverlapBox(transform.position + offset, size / 2, Quaternion.identity, target_layer);        //target_layer ���̾� ����ũ�� ���� ������Ʈ ����

            if (now_target == null)             //���� ���� ����� �÷��̾ ���ų�, ����� �÷��̾��� Collider�� ���ٸ�
                now_target = temp_target;       //���Ƿ� ���� �� ���� temp_target�� ��ó�Ѵ�


            if (cols.Length != 0)               //������ �������ڽ����� ������ ��ü�� ���� 0���� �ƴϸ�
            {
                foreach (var t in cols)          //������ �������ڽ����� ������ 1�� �̻��� ��ü�� foreach�������� �����ų t ������ �ϳ��� �����Ѵ�
                {
                    float temp_distance = Vector3.Distance(transform.position, now_target.transform.position);      //���� �ڽŰ� ����� �÷��̾��� �Ÿ� �� (�Ǵ� ���� �ڽŰ� temp_target�� �Ÿ� ��)
                    float now_distance = Vector3.Distance(transform.position, t.transform.position);                //���� �ڽŰ� t ������ ����ִ� ��ü�� ��

                    if (temp_distance > now_distance)        //temp_distance�� �Ÿ� �� ���� now_distance�� �Ÿ� ���� �� �۴ٸ�
                    {
                        now_target = t;                      //���� ���� ����� ��ü�� t�� ���̹Ƿ�, now_target(���� ���� ����� �÷��̾ �����ϴ� ����) ������ �����Ѵ�.
                        UnityEngine.Debug.Log(now_target+"asdasdasdsdasd");
                    }
                }
                UnityEngine.Debug.DrawRay(transform.position,Vector3.forward*10, UnityEngine.Color.green);
                enemyHealth = now_target.gameObject.GetComponent<Enemy>();
                nav.SetDestination(now_target.transform.position);         //���� ������ ��������, ���� ����� �÷��̾��� ��ġ�� NavMeshAgent�� ��ǥ(�ٶ󺸴� ����)�� �����Ѵ�.



                float distance = Vector3.Distance(transform.position, now_target.transform.position);       //���� �ڽŰ� ���� ����� �÷��̾��� �Ÿ� �� = �����ִ� foreach���� temp_distance ������ ����

                if (distance <= min_distance)           //�÷��̾���� �ּ� �Ÿ� ���� �� ����, �ڽŰ� �÷��̾��� �Ÿ� ���� �� ������
                {
                    nav.speed = 0;                 //NavMeshAgent�� �ӵ��� 0���� �Ͽ� �÷��̾���� ���� �Ÿ��� ������ �� �ֵ��� �Ѵ�.
                    if (!isAttack) // ���� ���� �ƴ� ���
                    {
                        transform.LookAt(now_target.transform.position);
                        anim.SetBool("IsWalk", false);
                        
                        if (Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),out RaycastHit hitInfo, 10f))
                        {
                            
                            print("hitSomthing");
                        }
                        else
                        {
                            print("hitNothing@@");
                        }
                        isAttack = true; // ���� ���¸� Ȱ��ȭ�Ѵ�.
                        StartCoroutine(Attack()); // ���� �ڷ�ƾ�� ȣ���Ѵ�.
                    }
                }
                else                                        //�÷��̾���� �ּ� �Ÿ� ���� �� ����, �ڽŰ� �÷��̾��� �Ÿ� ���� �� �ָ�
                    nav.speed = default_speed;           //NavMeshAgent�� �ӵ��� �̸� ������ ���� �⺻ ������ �������´�.
            }
            else        //������ �������ڽ����� ������ ��ü�� ���� �ƹ��͵� ���ٸ�
            {
                if (isAttack) // ���� ���� ���
                {
                    isAttack = false; // ���� ���¸� ��Ȱ��ȭ�Ѵ�.
                    StopCoroutine("Attack"); // ���� �ڷ�ƾ�� �����Ѵ�.
                    anim.SetBool("IsWalk", true); // �̵� �ִϸ��̼��� ��Ȱ��ȭ�Ѵ�.
                }
                now_target = temp_target;
                nav.SetDestination(Main_Center.position);      //������ ��ġ ���� NavMeshAgent�� ��ǥ(�ٶ󺸴� ����)�� �����Ѵ�.
                nav.speed = default_speed;                     //NavMeshAgent�� �ӵ��� �̸� ������ ���� �⺻ ������ �������´�. (�ӵ� ���� ����� ���ƿ��� ���� �� �ֱ� ������ �ѹ� �� ����)
            }
        }
        else // ü���� 1 �̻��� �ƴ� ���
        {
            if (!isDie)
            {
                isDie = true; // ���� ���¸� �����.
                nav.speed = 0;
                StopAllCoroutines(); // �������� ��� �ڷ�ƾ�� �ߴ��Ѵ�.
                Destroy(gameObject, 2f); // 2�� �� ������Ʈ�� �ı��Ѵ�.
                anim.SetTrigger("DoDie"); // ü���� ������ ��� ��� ó��
            }
        }
    }
    IEnumerator Attack() {
        while (cols.Length != 0)
        {
            anim.SetTrigger("DoShoot");
            UnityEngine.Debug.Log(enemyHealth);
            enemyHealth.curHealth -= attackDamage;
            yield return new WaitForSeconds(0.5f);
        }
    }
    void OnDrawGizmos() {
        Gizmos.color = UnityEngine.Color.blue;
        Gizmos.DrawWireCube(transform.position + offset, size);
        
    }
}
