using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Camera mainCamera;
    public Vector3 offset;
    float z;
    private void FixedUpdate() {
        z = mainCamera.transform.position.z;
        this.transform.position = new Vector3(0,0,z)+offset;
    }
}
