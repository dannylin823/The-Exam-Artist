using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public float speed = 2.0f;
    public NavMeshAgent teacher;
    public GameObject student, gameoverTarget, giftTarget;
    public Animator ani;
    public Text text;
    public Random ran = new Random();
    public AudioClip wow;
    public TestPaperBehavior test;
    public IllegalMoveHandler illegalmove;
    public GiftBlindEyesBehavior giftSkillTrigger;
    public ActLikeTheFlashBehavior flashSkillTrigger;


    private int behaviour = 0; // moving
    private bool inEyesight = false, gameover = false;
    private float minDistance = 10000f, minAngle = 120f, angle;
    private GameObject target;
    private AudioSource[] studentsound, teachersound;
    
    private float timeLeft;

    void Start()
    {
        studentsound = GameObject.FindGameObjectWithTag("student").GetComponents<AudioSource>();
        teachersound = GameObject.FindGameObjectWithTag("teacher").GetComponents<AudioSource>();

        LevelSetting setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        timeLeft = setting.offset;
        teacher.speed = speed;
        if (setting.washroomed)
        {
            int i = Random.Range(1, 12);
            target = GameObject.Find("target" + i.ToString());
            teacher.transform.position = target.transform.position;
        }
        else
        {
            target = GameObject.Find("target1");
        }
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            if (timeLeft < 13 && timeLeft > 12)
            {
                ani.SetInteger("animation_int", 9);
                teachersound[0].Play();
            }
            timeLeft -= Time.deltaTime;
        }
        else
        {
            if (behaviour == 0) // start status, set status as 1 to moving
            {
                behaviour = 1;
            }
            else if (behaviour == 1) // moving status
            {
                Moving();
            }
            else if (behaviour == 2) // idle status
            {
                
            }
            else if (behaviour == 3) // pausing and watching status
            {
                //Debug.Log("Pausing");
                StartCoroutine(Pausing());
            }
            else if (behaviour == 4)
            {
                teacher.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                behaviour = 2;
                teachersound[1].Pause();
                test.writeAnsToJson();
            }
            else if (behaviour == 5)
            {
                if (giftSkillTrigger.isTrigger() == false)
                {
                    behaviour = 1;
                    //teacher.speed = speed;
                }
                if (Vector3.Distance(teacher.transform.position, teacher.destination) < 1.0f)
                {
                    teacher.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                    ani.SetInteger("animation_int", 10);
                }
                    
            }
            if (!gameover)
            {
                eyesightCheck();
            }
        }
    }

    void Moving()
    {
        if (!teachersound[1].isPlaying)
        {
            teachersound[1].Play();
        }
        else
        {
            teachersound[1].UnPause();
        }
        Transform destination = target.transform;
        if (flashSkillTrigger.isTrigger() == false)
        {
            ani.SetInteger("animation_int", 1);
        }
        else
        {
            ani.SetInteger("animation_int", 12);
        }
        
        if (gameover)
        {
            teacher.SetDestination(gameoverTarget.transform.position);
            if (Vector3.Distance(teacher.transform.position, teacher.destination) < 1.0f)
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
                teachersound[2].PlayOneShot(wow, 0.3f);
                gameover = true;
                teacher.SetDestination(gameoverTarget.transform.position);
            }
            else
            {
                if (giftSkillTrigger.isTrigger() == true)
                {
                    teacher.SetDestination(giftTarget.transform.position);
                    //teacher.speed *= 2;
                    behaviour = 5;
                }
                else
                {
                    teacher.SetDestination(destination.position);
                } 
            }

            if (target.GetComponent<PointFind>().nextPos && !gameover)
            {
                if (Vector3.Distance(teacher.transform.position, destination.position) < 1.0f)
                {
                    teacher.ResetPath();
                    behaviour = 3;

                    target = target.GetComponent<PointFind>().nextPos;  // target赋值为下一个点的坐标
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

        //Debug.Log(angle.ToString() + distance.ToString());
        //string debug = "teacher pos" + teaPos + "student pos" + stuPos + angle.ToString() + "and" + distance.ToString();

        if (distance < minDistance && angle < minAngle / 2)
        {
            text.text = "in eyesight";
            inEyesight = true;
            if (illegalmove.illegal)
            {
                behaviour = 3;
            }
        }
        else
        {
            text.text = "not in eyesight";
            inEyesight = false;
        }
    }

    IEnumerator Pausing()
    {
        teachersound[1].Pause();
        if (gameover)
        {
            //int index = Random.Range(5, 7);
            ani.SetInteger("animation_int", 3);
            yield return new WaitForSeconds(3);

            //Debug.Log("Before Waiting 3 seconds");
            behaviour = 1;
        }
        else
        {
            int index = Random.Range(2, 3);
            ani.SetInteger("animation_int", index);
            //Debug.Log("Before Waiting 3 seconds");

            teacher.transform.Rotate(new Vector3(0, -30 * Time.deltaTime, 0));
            yield return new WaitForSeconds(3);

            //Debug.Log("After Waiting 3 Seconds");
            behaviour = 1;
        }

    }
}