using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Valve.VR;

public class TimeFreezeBehavior : MonoBehaviour
{
    public Image imgCoolDown;
    public Text limitNum;
    private GameObject teacherCharacter, joggingCharacter, outsideCharacter, hallwayCharacter;
    private GameObject[] studentCharacters;
    private float teacherSpeed;
   
    private bool exist = false, used = false, wasRunning = false;
    private float limit = 10.0f;
    public bool hold = false;

    private AudioSource[] sound;
    public AudioClip timeFreezeAudioClip;


    void Start()
    {
        imgCoolDown.fillAmount = 0.0f;
        limitNum.text = limit.ToString();

        teacherCharacter = GameObject.FindGameObjectWithTag("TeacherAction");
        teacherSpeed = teacherCharacter.GetComponent<NavMeshAgent>().speed;
        studentCharacters = GameObject.FindGameObjectsWithTag("StudentCharacter");
        sound = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
        joggingCharacter = GameObject.FindGameObjectWithTag("JoggingCharacter");
        outsideCharacter = GameObject.FindGameObjectWithTag("OutsideCharacter");
        hallwayCharacter = GameObject.FindGameObjectWithTag("HallwayCharacter");
        imgCoolDown.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
    }

    void Update()
    {
        if (hold && limit > 0)
        {
            TimeFreeze();
            limit -= Time.deltaTime;
            limitNum.text = ((int)limit).ToString();
            imgCoolDown.fillAmount = 1 - limit / 10.0f;
        }
        else if ((!hold && exist) || limit < 0)
        {
            UnfreezeCharacters();
            exist = false;
            if (limit < 0)
            {
                imgCoolDown.fillAmount = 1.0f;
                limitNum.text = "";
            }
        }
    }

    public void TimeFreeze()
    {
        if (exist == false && used == false)
        {
            sound[0].PlayOneShot(timeFreezeAudioClip, 1.5f);
            teacherCharacter.GetComponent<Animator>().enabled = false;
            teacherCharacter.GetComponent<NavMeshAgent>().speed = 0;
            teacherCharacter.GetComponent<NavMeshAgent>().angularSpeed = 0;
            for (int i = 0; i < studentCharacters.Length; i++)
            {
                studentCharacters[i].GetComponent<Animator>().enabled = false;
            }
            joggingCharacter.GetComponent<Animator>().enabled = false;
            hallwayCharacter.GetComponent<Animator>().enabled = false;
            hallwayCharacter.GetComponent<male1>().enabled = false;
            outsideCharacter.GetComponent<Animator>().enabled = false;
            if (outsideCharacter.GetComponent<femaleoutside>().enabled)
            {
                wasRunning = true;
                outsideCharacter.GetComponent<femaleoutside>().enabled = false;
            }
            exist = true;
        }
    }

    private void UnfreezeCharacters()
    {
        teacherCharacter.GetComponent<Animator>().enabled = true;
        teacherCharacter.GetComponent<NavMeshAgent>().speed = teacherSpeed;
        teacherCharacter.GetComponent<NavMeshAgent>().angularSpeed = 120;
        for (int i = 0; i < studentCharacters.Length; i++)
        {
            studentCharacters[i].GetComponent<Animator>().enabled = true;
        }
        joggingCharacter.GetComponent<Animator>().enabled = true;
        hallwayCharacter.GetComponent<Animator>().enabled = true;
        hallwayCharacter.GetComponent<male1>().enabled = true;
        outsideCharacter.GetComponent<Animator>().enabled = true;
        if (wasRunning)
        {
            outsideCharacter.GetComponent<femaleoutside>().enabled = true;
        }
    }

    public bool isExisting()
    {
        return exist;
    }
}