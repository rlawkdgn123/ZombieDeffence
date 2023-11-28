using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameFinisher : MonoBehaviour
{
    public enum Type {Team,Enemy};
    public Type Base;
    public TextMeshProUGUI text;
    public float CurHealth;
    public float MaxHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Base == Type.Team && CurHealth == 0)
        {
            text.enabled = true;
            text.text = "Failed!!!";
        }
        else if(Base == Type.Enemy && CurHealth == 0)
        {
            text.enabled = true;
            text.text = "Victory!!!";
        }
    }
}
