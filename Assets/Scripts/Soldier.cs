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
    [SerializeField] Transform Main_Center;     //본부의 위치 값

    [SerializeField] Collider now_target;       //가장 가까운 플레이어의 Collider를 지정하기 위한 변수
    [SerializeField] Collider temp_target;      //임의의 Collider (게임 플레이 중에 변하지 않음 / 고정 Collider)

    [SerializeField] float default_speed;         //NavMeshAgent의 처음 속도 값을 받아놓기 위한 변수
    [SerializeField] float min_distance;        //플레이어와의 최소 거리 지정 값

    [SerializeField] public float curHealth;
    [SerializeField] public float maxHealth;
    [SerializeField] public float attackDamage;

    [SerializeField] bool isAttack;
    [SerializeField] bool isDie;

    [SerializeField] Enemy enemyHealth;

    #region 기즈모 관련
    [SerializeField] Vector3 size;              //피직스 오버랩박스의 사이즈
    [SerializeField] Vector3 offset;            //피직스 오버랩박스의 위치
    [SerializeField] Collider[] cols;           //피직스 오버랩박스로 감지한 Collider 저장소
    [SerializeField] LayerMask target_layer;    //특정 레이어를 지닌 오브젝트를 감지하기 위한 레이어 마스크
    #endregion

    private void Awake() {
        Center = GameObject.Find("DefencePointEnemy"); // 처음 시작 시 본진 오브젝트 타겟팅
        Main_Center = Center.transform; // 본부 트랜스폼 값을 받아놓음
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        default_speed = nav.speed;               //NavMeshAgent의 처음 속도 값을 받아놓음
        anim.SetBool("IsWalk", true);
    }

    void FixedUpdate() 
        {
        if (curHealth > 0 && !isDie)
            {
                cols = Physics.OverlapBox(transform.position + offset, size / 2, Quaternion.identity, target_layer);        //target_layer 레이어 마스크를 지닌 오브젝트 감지

            if (now_target == null)             //현재 가장 가까운 플레이어가 없거나, 가까운 플레이어의 Collider가 없다면
                now_target = temp_target;       //임의로 지정 해 놓은 temp_target로 대처한다


            if (cols.Length != 0)               //피직스 오버랩박스에서 감지한 물체의 수가 0개가 아니면
            {
                foreach (var t in cols)          //피직스 오버랩박스에서 감지한 1개 이상의 물체를 foreach문에서만 존재시킬 t 변수에 하나씩 대입한다
                {
                    float temp_distance = Vector3.Distance(transform.position, now_target.transform.position);      //현재 자신과 가까운 플레이어의 거리 값 (또는 현재 자신과 temp_target의 거리 값)
                    float now_distance = Vector3.Distance(transform.position, t.transform.position);                //현재 자신과 t 변수에 들어있는 물체의 값

                    if (temp_distance > now_distance)        //temp_distance의 거리 값 보다 now_distance의 거리 값이 더 작다면
                    {
                        now_target = t;                      //현재 가장 가까운 물체는 t의 값이므로, now_target(현재 가장 가까운 플레이어를 지정하는 변수) 변수에 지정한다.
                        UnityEngine.Debug.Log(now_target+"asdasdasdsdasd");
                    }
                }
                UnityEngine.Debug.DrawRay(transform.position,Vector3.forward*10, UnityEngine.Color.green);
                enemyHealth = now_target.gameObject.GetComponent<Enemy>();
                nav.SetDestination(now_target.transform.position);         //위에 과정을 거쳤으면, 가장 가까운 플레이어의 위치를 NavMeshAgent의 목표(바라보는 방향)로 지정한다.



                float distance = Vector3.Distance(transform.position, now_target.transform.position);       //현재 자신과 제일 가까운 플레이어의 거리 값 = 위에있는 foreach문의 temp_distance 변수와 같음

                if (distance <= min_distance)           //플레이어와의 최소 거리 지정 값 보다, 자신과 플레이어의 거리 값이 더 가까우면
                {
                    nav.speed = 0;                 //NavMeshAgent의 속도를 0으로 하여 플레이어와의 일정 거리를 유지할 수 있도록 한다.
                    if (!isAttack) // 공격 중이 아닐 경우
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
                        isAttack = true; // 공격 상태를 활성화한다.
                        StartCoroutine(Attack()); // 공격 코루틴을 호출한다.
                    }
                }
                else                                        //플레이어와의 최소 거리 지정 값 보다, 자신과 플레이어의 거리 값이 더 멀면
                    nav.speed = default_speed;           //NavMeshAgent의 속도를 미리 저장해 놓은 기본 값으로 돌려놓는다.
            }
            else        //피직스 오버랩박스에서 감지한 물체의 수가 아무것도 없다면
            {
                if (isAttack) // 공격 중일 경우
                {
                    isAttack = false; // 공격 상태를 비활성화한다.
                    StopCoroutine("Attack"); // 공격 코루틴을 중지한다.
                    anim.SetBool("IsWalk", true); // 이동 애니메이션을 재활성화한다.
                }
                now_target = temp_target;
                nav.SetDestination(Main_Center.position);      //본부의 위치 값을 NavMeshAgent의 목표(바라보는 방향)로 지정한다.
                nav.speed = default_speed;                     //NavMeshAgent의 속도를 미리 저장해 놓은 기본 값으로 돌려놓는다. (속도 값이 제대로 돌아오지 않을 수 있기 때문에 한번 더 써줌)
            }
        }
        else // 체력이 1 이상이 아닐 경우
        {
            if (!isDie)
            {
                isDie = true; // 죽음 상태를 만든다.
                nav.speed = 0;
                StopAllCoroutines(); // 진행중인 모든 코루틴을 중단한다.
                Destroy(gameObject, 2f); // 2초 후 오브젝트를 파괴한다.
                anim.SetTrigger("DoDie"); // 체력이 없으면 사망 모션 처리
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
