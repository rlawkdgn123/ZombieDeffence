using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    float x, z;
    public float moveSpeed = 40;
    public float rotateSpeed = 40;
    Vector3 camVec;
    Transform posNow;
    void Start() {

    }
    private void FixedUpdate() {
        CameraMoveMent();
    }
    void CameraMoveMent() {
        z = Input.GetAxis("Vertical");
        x = Input.GetAxis("Horizontal");
        camVec = new Vector3(x, 0, z).normalized;
        transform.position += moveSpeed * Time.deltaTime * camVec;
    }
}
