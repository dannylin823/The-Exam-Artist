using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class studentF : MonoBehaviour
{

    public Animator ani;

    private TestPaperBehavior playerTest;
    private GameObject chair;
    private AudioSource[] source;
    private AudioSource[] teacher;
    public Random ran = new Random();
    //public GameObject character;
    //private bool notMoved = true;
    private float timeLeft = 15.0f, collideLeft = 0.5f;
    private int animationIndex = 2;
    private bool writing = false, collide = false;
    private Vector3 original, chairOriginal;

    public void setAnimation(int i)
    {
        animationIndex = i;
    }

    void Start()
    {
        playerTest = GameObject.FindGameObjectWithTag("MainSelectHandler").GetComponent<TestPaperBehavior>();
        ani = gameObject.GetComponent<Animator>();
        source = GameObject.FindGameObjectWithTag("student").GetComponents<AudioSource>();
        teacher = GameObject.FindGameObjectWithTag("teacher").GetComponents<AudioSource>();

        ani.SetInteger("animation_int", 7);
        LevelSetting setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        timeLeft = setting.offset;
        if (setting.washroomed)
        {
            timeLeft = -1.0f;
        }
        original = gameObject.transform.localPosition;
        chair = gameObject.transform.parent.transform.Find("prop_sch_tablechair").transform.Find("prop_sch_chair").gameObject;
        chairOriginal = chair.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerTest.onPrepare)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            else
            {
                if (collide)
                {
                    if (collideLeft > 0)
                    {
                        collideLeft -= Time.deltaTime;
                    }
                    else
                    {
                        chair.transform.localPosition = chairOriginal;
                        collide = false;
                    }
                }
                if (!writing && !collide)
                {
                    setPosition(new Vector3(0.5f, -0.05f, 0.0f));
                    writing = true;
                }
                ani.SetInteger("animation_int", animationIndex);
            }
        }

        /*if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ani.SetInteger("animation_int", 0);//idle
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int index = Random.Range(7, 10);
            ani.SetInteger("animation_int", index);//sit
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (character != null && notMoved)
            {
                Debug.Log(character.transform.localPosition);
                character.transform.localPosition += new Vector3(0.7f, 0.0f, 0.0f);
                notMoved = false;
            }
            int index = Random.Range(1, 3);
            ani.SetInteger("animation_int", index);//write
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ani.SetInteger("animation_int", 4);//depress
            GameObject.FindGameObjectWithTag("backgroundmusic").GetComponents<AudioSource>()[0].Stop();
            GameObject.FindGameObjectWithTag("backgroundmusic").GetComponents<AudioSource>()[1].Stop();
            source[0].Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ani.SetInteger("animation_int", 6);//dance
            GameObject.FindGameObjectWithTag("backgroundmusic").GetComponents<AudioSource>()[0].Stop();
            GameObject.FindGameObjectWithTag("backgroundmusic").GetComponents<AudioSource>()[1].Stop();
            source[0].Stop();
            source[1].Play();
        }*/
    }

    public void setPosition(Vector3 position)
    {
        gameObject.transform.localPosition += position;
    }

    public void setChairPosition(Vector3 position)
    {
        chair.transform.localPosition += position;
    }

    public void resetPosition()
    {
        gameObject.transform.localPosition = original;
        if (animationIndex != 11)
        {
            chair.transform.localPosition = chairOriginal;
        }
        else
        {
            collide = true;
        }
        writing = false;
    }
}
