using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI roundText;
    enum TimeList { normal=0,boss,bonus,rest };
    [Header ("NowTimeSet")]
    public int roundCount = 1;
    public int nowType = 4;
    public float nowTime = 0;
    
    [Header ("GameTimeSet")]
    public float normalTime = 30.0f;
    public float bossTime = 60.0f;
    public float bonusTime = 60.0f;
    public float restTime = 20.0f;
    [Header ("Commons")]
    public bool roundStart;
    [SerializeField]bool rest = true;
    [SerializeField]bool cycle = false;
    [Header("Reference")]
    public GameObject gameSystem;
    ZombieSpawn zombieSpawn;

    void Awake() {
        nowTime = restTime;
        zombieSpawn = gameSystem.GetComponent<ZombieSpawn>();
        
    }
    void Start() {
        roundText.text = "현재 라운드 : " + roundCount.ToString();
    }
    void Update()
    {
        TimeCountDown();            
    }
    void TimeCountDown() {
        if (nowTime > 0 && rest == false)
        {
            nowTime -= Time.deltaTime;
            timeText.text = "남은 전투 시간 : " + Mathf.Ceil(nowTime).ToString(); //mathf.Ceil = 소숫점 올림 처리
        }
        if (nowTime <= 0 && rest == false){
            roundStart = false;
            rest = true;
            nowTime = restTime;
        }
        if (nowTime > 0 && rest == true)
        {
            nowTime -= Time.deltaTime;
            timeText.text = "남은 휴식 시간 : " + Mathf.Ceil(nowTime).ToString(); //mathf.Ceil = 소숫점 올림 처리
        }
        if (nowTime <= 0 && rest == true)
        {
            roundStart = true;
            rest = false;
            cycle = true;
        }
        if (nowTime <= 0 && cycle == true)
        {
            RoundCalculator();
        }

    }
    void RoundCalculator() {
        roundCount++;
        roundText.text = "현재 라운드 : "+roundCount.ToString();

        
        if(roundCount % 2 == 0|| roundCount % 3 == 0|| roundCount % 4 == 0|| roundCount % 6 == 0
           || roundCount % 8 == 0|| roundCount % 9 == 0){
            nowType = (int)TimeList.normal;
        }else if (roundCount % 5 == 0 || roundCount % 10 == 0) { 
            nowType = (int)TimeList.boss;
        }else if (roundCount % 7 == 0)
            nowType = (int)TimeList.bonus;
        switch (nowType)
        {
            case (int)TimeList.normal:
                Debug.Log("normal");
                nowTime = normalTime;
                zombieSpawn.AddZombieCount();
                    break;
            case (int)TimeList.boss:
                Debug.Log("boss");
                nowTime = bossTime; break;
            case (int)TimeList.bonus:
                nowTime = bonusTime; break;
            default:
                nowTime = restTime; break;
        }
        cycle = false;

    }
}
