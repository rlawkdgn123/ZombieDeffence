using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public Camera mainCamera;
    GameObject soldier;
    Renderer mat_Soldier;
    int select = -1;
    bool test = false;

    private void Awake() {
        soldier = GetComponentInChildren<GameObject>();
        mat_Soldier = soldier.GetComponent<MeshRenderer>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                mat_Soldier.enabled = true;
                //Instantiate(soldier, new Vector3(raycastHit.point.x, 0, raycastHit.point.z), Quaternion.identity);
                Debug.Log("aa");
            }
            soldier.transform.position = new Vector3(raycastHit.point.x, 0, raycastHit.point.z);
        }
        Debug.Log("bb");
    }
    bool CanSpawn() {
        if(select == -1)
                return false;
            else
                return true;
    }
    void DetectSpawnPoint() {
        if (Input.GetMouseButtonDown(0))
        {
                
        }

    }
    
    void SelectTower() {

    }
    void DeselectButton() {

    }
    void SpawnSoldier() {
        mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                mat_Soldier.enabled = true;
                //Instantiate(soldier, new Vector3(raycastHit.point.x, 0, raycastHit.point.z), Quaternion.identity);
                Debug.Log("aa");
            }
            soldier.transform.position = new Vector3(raycastHit.point.x, 0, raycastHit.point.z);
        }
        Debug.Log("bb");
    }
}
