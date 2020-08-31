using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class TeacherController : MonoBehaviour
{
    public float speed = 2.0f;
    public NavMeshAgent teacher;
    public GameObject student, gameoverTarget;
    public Animator ani;
    public Random ran = new Random();
    public AudioClip wow;
    public IllegalMoveHandler illegalmove;
    public GiftBlindEyesBehavior giftSkillTrigger;
    public TimeFreezeBehavior freezeSkillTrigger;
    public float MaxPauseTime = 3.0f;
    public float MaxCheckTime = 5.5f;
    public bool collision = false, gameover = false;

    private int behaviour = 1;
    private bool inEyesight = false, play = false;
    private float angle, minDistance = 0.2f, minAngle = 120f, minEyesight = 10000f;
    //private float minDistance = 10000f, 
    private GameObject target, giftTarget;
    private AudioSource[] studentsound, teachersound;
    private float timePaused = 0.0f;
    //private float timeChecked = 0.0f;
    private float timeLeft = 15.0f;
    private TestPaperBehavior test;
    private LevelSetting setting;

    void Start()
    {
        studentsound = GameObject.FindGameObjectWithTag("student").GetComponents<AudioSource>();
        teachersound = GameObject.FindGameObjectWithTag("teacher").GetComponents<AudioSource>();

        freezeSkillTrigger = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();
        test = GameObject.FindGameObjectWithTag("MainSelectHandler").GetComponent<TestPaperBehavior>();

        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        timeLeft = setting.offset;
        teacher.speed = speed;
        target = GameObject.Find("target1");
        target.transform.position = GetRandomPosition();
    }

    void Update()
    {
        if (!test.onPrepare)
        {
            if (timeLeft > 0)
            {
                if (!play)
                {
                    ani.SetInteger("animation_int", 9);
                    teachersound[0].Play();
                    play = true;
                }
                timeLeft -= Time.deltaTime;
            }
            else
            {
                if (!gameover)
                {
                    eyesightCheck();
                }
                switch (behaviour)
                {
                    case 0:
                        behaviour = 1;   //switch behavior from initialization to move
                        break;
                    case 1:
                        Moving();  // move
                        break;
                    case 2:
                    case 3:
                        Pausing();
                        break;
                    case 4:
                        setting.setFailed(true);
                        teacher.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                        behaviour = 2;
                        teachersound[1].Pause();
                        test.writeAnsToJson();
                        break;
                    case 5:
                        bribeBehavior();
                        break;
                }
            }
        }
    }

    void Moving()
    {
        if (!freezeSkillTrigger.isExisting())
        {
            if (!teachersound[1].isPlaying)
            {
                teachersound[1].Play();
            }
            else
            {
                teachersound[1].UnPause();
            }
        }
        else
        {
            teachersound[1].Pause();
        }
        Transform destination = target.transform;
        // teacher.SetDestination(destination.position);
        // Debug.Log(target.transform.position);
        ani.SetInteger("animation_int", 1);
        if (gameover)
        {
            teacher.SetDestination(gameoverTarget.transform.position);
            if (Vector3.Distance(teacher.transform.position, teacher.destination) < minDistance)
            {
                int index = Random.Range(5, 7);
                ani.SetInteger("animation_int", index);
                teacher.ResetPath();
                behaviour = 4;
            }
        }
        else
        {
            if (illegalmove.illegal && inEyesight)
            {
                teacher.SetDestination(gameoverTarget.transform.position);
            }
            else
            {
                if (giftSkillTrigger.isTrigger() == true)
                {
                    setBribeTarget();
                    teacher.SetDestination(giftTarget.transform.position);
                    behaviour = 5;
                }
                else
                {
                    teacher.SetDestination(destination.position);
                    if (!teacher.hasPath && !collision)
                    {
                        target.transform.position = GetRandomPosition();
                    }
                }
            }

            if (!gameover)
            {
                //Debug.Log(Vector3.Distance(teacher.transform.position, destination.position));
                if (Vector3.Distance(teacher.transform.position, destination.position) < minDistance)
                {
                    Debug.Log("Reached");
                    teacher.ResetPath();
                    behaviour = 3;
                    target.transform.position = GetRandomPosition();
                }
            }
        }
    }

    void eyesightCheck()
    {
        Vector3 teaPos = transform.position;
        Vector3 stuPos = student.transform.position;
        float distance = Vector3.Distance(teaPos, stuPos);

        Vector3 srcLocalVect = stuPos - teaPos;
        srcLocalVect.y = 0;
        Vector3 forwardLocalPos = teacher.transform.forward * 1 + teaPos;
        Vector3 forwardLocalVect = forwardLocalPos - teaPos;
        forwardLocalVect.y = 0;
        float angle = Vector3.Angle(srcLocalVect, forwardLocalVect);
        if (distance < minEyesight && angle < minAngle / 2)
        {
            inEyesight = true;
            if (illegalmove.illegal)
            {
                teachersound[2].PlayOneShot(wow, 0.3f);
                gameover = true;
                test.gameOver();
                behaviour = 3;
                teacher.speed = 2.0f;
                MaxPauseTime = 3.0f;
            }
        }
        else
        {
            inEyesight = false;
        }
    }

    void Pausing()
    {
        teachersound[1].Pause();
        if (!freezeSkillTrigger.isExisting())
        {
            timePaused += Time.deltaTime;

            if (gameover)
            {
                //int index = Random.Range(5, 7);
                ani.SetInteger("animation_int", 3);
                behaviour = 1;
            }
            else
            {
                if (timePaused >= MaxPauseTime)
                {
                    behaviour = 1;
                    timePaused = 0.0f;
                    if (collision)
                    {
                        collision = false;
                        teacher.speed = 2.0f;
                        MaxPauseTime = 3.0f;
                    }
                }
                else
                {
                    if (collision)
                    {
                        teacher.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                        ani.SetInteger("animation_int", 10);
                    }
                    else
                    {
                        int index = Random.Range(2, 3);
                        ani.SetInteger("animation_int", index);
                        teacher.transform.Rotate(new Vector3(0, -30 * Time.deltaTime, 0));
                    }
                    teacher.SetDestination(teacher.transform.position);
                    //teacher.ResetPath();
                }
            }
        }
    }

    void bribeBehavior()
    {
        if (giftSkillTrigger.isTrigger() == false)
        {
            behaviour = 1;
        }
        if (Vector3.Distance(teacher.transform.position, teacher.destination) < minDistance)
        {
            teachersound[1].Pause();
            teacher.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
            ani.SetInteger("animation_int", 10);
        }
    }

    /*void Checking()
    {
        timeChecked += Time.deltaTime;
        teachersound[1].Pause();

        //ani.SetInteger("animation_int", 11);
        //Debug.Log("Before Waiting 3 seconds");

        if (timeChecked >= MaxCheckTime)
        {
            behaviour = 1;
            clapBombTrigger.targetStudentPos = null;
            timeChecked = 0.0f;
        }
        else
        {
            ani.SetInteger("animation_int", 10);
        }
    }

    void checkStudentBehavior()
    {
        if (Vector3.Distance(teacher.transform.position, teacher.destination) < minDistance)
        {
            teachersound[1].Pause();
            teacher.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
            Checking();
        }
    }*/

    public void setTarget(GameObject t)
    {
        if (!gameover)
        {
            target.transform.position = t.transform.position;

            //Debug.Log("Collide: " + target.transform.position);
            behaviour = 1;
            teacher.speed = 4.0f;
            MaxPauseTime = 5.0f;
            collision = true;
        }
    }

    void setBribeTarget()
    {
        giftTarget = GameObject.Find("Student" + giftSkillTrigger.target).transform.Find("Position").gameObject;
    }

    public Vector3 GetRandomPosition()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        int t = Random.Range(0, navMeshData.indices.Length - 3);
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        point = Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);
        //Debug.Log("generate: " + point);
        return point;
    }
}