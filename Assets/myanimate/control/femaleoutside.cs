using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class femaleoutside : MonoBehaviour
{
    public Animator ani;
    public Random ran = new Random();
    float m_Speed;

    void OnEnable()
    {
        m_Speed = 5.0f;
        ani.SetInteger("animation_int", 1);
    }

    void Update()
    {
        if (transform.position.z > -10)
        {
            transform.Translate(Vector3.right * Time.deltaTime * m_Speed, Space.Self);
        }
        else
        {
            var v = transform.localPosition;
            v.z = 18.2f;
            transform.localPosition = v;
            gameObject.GetComponent<femaleoutside>().enabled = false;
            ani.SetInteger("animation_int", 0);
        }
    }

    public void startAnimation()
    {
        ani.SetInteger("animation_int", 1);
    }

}
