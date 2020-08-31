using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class student : MonoBehaviour { 

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
    private bool writing = false, collide = false, start = false;
    private Vector3 original, chairOriginal;

    public void setAnimation(int i)
    {
        animationIndex = i;
    }

    void Start()
    {
        playerTest = GameObject.FindGameObjectWithTag("MainSelectHandler").GetComponent<TestPaperBehavior>();
        ani = gameObject.GetComponent<Animator>();
        source = GameObject.FindGameObjectWithTag ("student").GetComponents<AudioSource> ();
        teacher = GameObject.FindGameObjectWithTag ("teacher").GetComponents<AudioSource> ();

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

        setChairPosition(new Vector3(0.0f, -0.091f, -0.141f));
        setPosition(new Vector3(0.36f, 0.0f, 0.0f));
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
                if (!start)
                {
                    resetPosition();
                    start = true;
                }
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
