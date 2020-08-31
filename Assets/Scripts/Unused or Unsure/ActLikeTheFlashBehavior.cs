using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

/*
   You will act like the Flash in 3 seconds.
   When you use the skill, during the next 3 seconds, everything is acting in half of the speed as before.
   For the same reason, the 3 second is actually 6 seconds in your vision.
*/
public class ActLikeTheFlashBehavior : MonoBehaviour
{
    public Image imgCoolDown, imgExist;
    public Text textCoolDown;

    private float coolDown = 5.0f, coolDownCounter = 5.0f;
    private float existTime = 10.0f, existTimeCounter = 10.0f;
    private bool exist = false, used = false;

    private GameObject teacher;
    private GameObject[] students;
    private GameObject bgm;
    private GameObject teacherHeel;
    private float slowDownRate = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        imgCoolDown.fillAmount = 0.0f;
        imgExist.fillAmount = 0.0f;
        textCoolDown.text = "";
        teacher = GameObject.FindGameObjectWithTag("TeacherAction");
        students = GameObject.FindGameObjectsWithTag("StudentAction");
        bgm = GameObject.FindGameObjectWithTag("backgroundmusic");
        teacherHeel = GameObject.FindGameObjectWithTag("teacher");
    }

    // Update is called once per frame
    void Update()
    {
        if (existTimeCounter > 0 && exist == true && used == false)
        {
            existTimeCounter -= Time.deltaTime;
            imgExist.fillAmount = 1 - existTimeCounter / existTime;
            textCoolDown.text = ((int)Mathf.Ceil(existTimeCounter)).ToString();
        }
        else if (existTimeCounter <= 0 && exist == true && used == false)
        {
            existTimeCounter = existTime;
            exist = false;
            imgExist.fillAmount = 0.0f;

            PlayerController teacherController = teacher.GetComponent<PlayerController>();
            NavMeshAgent teacherNavMesh = teacher.GetComponent<NavMeshAgent>();
            AudioSource bgmAudio = bgm.GetComponent<AudioSource>();
            AudioSource teacherHeelAudio = teacherHeel.GetComponents<AudioSource>()[1];
            //Animator teacherAni = teacher.GetComponent<Animator>();

            teacherController.speed *= slowDownRate;
            teacherNavMesh.speed *= slowDownRate;
            bgmAudio.pitch *= slowDownRate;
            teacherHeelAudio.pitch *= slowDownRate;
            //teacherAni.SetInteger("animation_int", 0);

            for (int i = 0; i < students.Length; i++)
            {
                TestPaperAuto tp = students[i].GetComponent<TestPaperAuto>();
                tp.slowDown = false;
            }

            used = true;
        }
        else if (coolDownCounter > 0 && used == true)
        {
            coolDownCounter -= Time.deltaTime;
            imgCoolDown.fillAmount = 1 - coolDownCounter / coolDown;
            textCoolDown.text = ((int)Mathf.Ceil(coolDownCounter)).ToString();
            //Debug.Log(coolDownCounter[0]);
        }
        else if (coolDownCounter <= 0 && used == true)
        {
            coolDownCounter = coolDown;
            textCoolDown.text = "";
            imgCoolDown.fillAmount = 0.0f;
            used = false;
        }
    }

    public bool isTrigger()
    {
        return exist == true;
    }

    public void ActLikeTheFlash()
    {
        if (exist == false && used == false)
        {
            exist = true;
            PlayerController teacherController = teacher.GetComponent<PlayerController>();
            NavMeshAgent teacherNavMesh = teacher.GetComponent<NavMeshAgent>();
            AudioSource bgmAudio = bgm.GetComponent<AudioSource>();
            AudioSource teacherHeelAudio = teacherHeel.GetComponents<AudioSource>()[1];
            //Animator teacherAni = teacher.GetComponent<Animator>();

            teacherController.speed /= slowDownRate;
            teacherNavMesh.speed /= slowDownRate;
            bgmAudio.pitch /= slowDownRate;
            teacherHeelAudio.pitch /= slowDownRate;
            //teacherAni.SetInteger("animation_int", 10);

            //Debug.Log(students.Length);
            for (int i = 0; i < students.Length; i++)
            {
                //Debug.Log(students[i].name);
                TestPaperAuto tp = students[i].GetComponent<TestPaperAuto>();
                //Debug.Log(tp);
                tp.slowDown = true;
            }
        }
        else
        {
            Debug.Log("Your skill need to be cooled down");
        }
    }
}
