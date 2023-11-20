using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    public GameObject UI;
    public GameObject normalZombie;
    public GameObject runnerZombie;
    GameTimer gameTimer;
    public Transform[] zombieSpawnPos;
    public enum ZombieType { normalType = 0, runnerType };
    //int EnumSize = Enum.GetValues(typeof(ZombieType)).Length; // ������ ���� �޾ƿ���. ���Ŀ� �迭 ���¸� ����Ʈ�� �ٲ� ��.
    public float[] SpawnInterval; // �� ��ȯ ����
    public int[] zombieCount;
    int[] zombieCountTemp;
    public bool isLocked;
    // Start is called before the first frame update
    void Awake() {
        gameTimer = UI.GetComponent<GameTimer>();
        zombieCountTemp = new int[zombieCount.Length]; //temp �迭 �ʱ�ȭ
        TempReset();

    }
    
    // Update is called once per frame
    void Update() {
        if (gameTimer.roundStart == true)
        { 
            switch (gameTimer.nowType)
            {
                case 0:
                    UnityEngine.Debug.Log("����");
                    if (zombieCountTemp[(int)ZombieType.normalType] > 0 && !isLocked)
                    {
                        StartCoroutine(SpawnEnemy_Normal());
                        isLocked = true;
                    }
                    if (gameTimer.roundCount > 6)
                        StartCoroutine(SpawnEnemy_Runner());
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default: break;

            }
        }

    }
    IEnumerator SpawnEnemy_Normal() {
        if (zombieCountTemp[(int)ZombieType.normalType] > 0 )
        {
            int spawnPoint = UnityEngine.Random.Range(0, 4);
            GameObject Zombie_Normal = Instantiate(normalZombie, zombieSpawnPos[spawnPoint].position, zombieSpawnPos[spawnPoint].rotation);
            UnityEngine.Debug.Log("��ȯ");
            yield return new WaitForSeconds(SpawnInterval[(int)ZombieType.normalType]);
            zombieCountTemp[(int)ZombieType.normalType]--;
            UnityEngine.Debug.Log("����");
            isLocked = false;
        }



    }
    IEnumerator SpawnEnemy_Runner() {
        while (zombieCountTemp[(int)ZombieType.runnerType] > 0)
        {
            int spawnPoint = UnityEngine.Random.Range(0, 4);
            GameObject Zombie_Runner = Instantiate(runnerZombie, zombieSpawnPos[spawnPoint].position, zombieSpawnPos[spawnPoint].rotation); 
            yield return new WaitForSeconds(SpawnInterval[(int)ZombieType.runnerType]);
        }
    }
    void TempReset() {
        for (int i = 0; i < zombieCount.Length; i++)
        {
            zombieCountTemp[i] = zombieCount[i];
        }
    }
    public void AddZombieCount() {
        zombieCount[(int)ZombieType.normalType]++;
        if (gameTimer.roundCount >= 3)
        {
            zombieCount[(int)ZombieType.normalType]++;
        }
        if (gameTimer.roundCount >= 8 && gameTimer.roundCount % 2 == 0)
        {
            zombieCount[(int)ZombieType.runnerType] += 1;
        }
    }
}