using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] Soldier enemyHealth;

    [SerializeField] public float attackDamage;
    [SerializeField] public GameObject now_target;
    [SerializeField] public GameObject[] Hands;
    [SerializeField] public Collider[] HandsCol;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit(Collider other) {
        if (other == now_target && other.tag.Contains("PlayerTeam"))
        {
            enemyHealth.curHealth -= attackDamage / 2; // 왼손 오른손 2타기 때문에, 2회로 나누어 데미지 입히기
        }
    }
}
