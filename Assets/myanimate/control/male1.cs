using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class male1 : MonoBehaviour
{
    public Animator ani;
    public Random ran = new Random();

    void Start()
    {
        ani.SetInteger("animation_int", 1);

    }

    void Update()
    {
        if (transform.position.z < 17.45f)
        {
            transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
        }
        else
        {
            var v = transform.localPosition;
            v.z = -17.45f;
            transform.localPosition = v;
        }
    }
}
