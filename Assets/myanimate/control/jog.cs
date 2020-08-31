using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jog : MonoBehaviour { 

    public Animator ani;

    void OnEnable()
    {
        ani.SetInteger("animation_int", 10);
    }
    
}
