using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teacher : MonoBehaviour
{

    public Animator ani;
    public Random ran = new Random();
    private AudioSource[] source;
    private AudioSource[] student;
    private AudioSource[] bgm;


    // Start is called before the first frame update
    void Start()
    {
        bgm = GameObject.FindGameObjectWithTag("backgroundmusic").GetComponents<AudioSource>();
        source = GameObject.FindGameObjectWithTag("teacher").GetComponents<AudioSource>();
        student = GameObject.FindGameObjectWithTag("student").GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ani.SetInteger("animation_int", 0);
            source[0].Stop();
            source[1].Stop();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {//talk
            ani.SetInteger("animation_int", 9);
            source[0].Play();
            source[1].Stop();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ani.SetInteger("animation_int", 1);//walk
            bgm[0].Pause();
            bgm[1].Play();
            source[1].Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ani.SetInteger("animation_int", 2);//lookaround
            source[0].Stop();
            source[1].Stop();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ani.SetInteger("animation_int", 3);//aware
            source[0].Stop();
            source[1].Stop();
            source[2].Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            int index = Random.Range(4, 7);
            ani.SetInteger("animation_int", index);//angry
            source[0].Stop();
            source[1].Stop();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ani.SetInteger("animation_int", 8);//getout
            source[0].Stop();
            source[1].Stop();
        }


    }

}
