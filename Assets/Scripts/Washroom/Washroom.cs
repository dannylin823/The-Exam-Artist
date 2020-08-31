using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR;

public class Washroom : MonoBehaviour
{
    public LevelSetting setting;
    public GameObject LoadSceneHandler, current, Level, projectile;
    public float timer;
    private GameObject paper;
    private float duration = 2.0f;
    private bool isWashroom = false;

    void OnEnable()
    {
        isWashroom = true;
        List<int> randomArray = new List<int>();
        if (GameObject.Find("LevelSetting") != null)
        {
            setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
            timer = setting.washroomDuration;
            projectile = setting.projectile;

            paper = GameObject.Find("Paper");

            int time = (int)Mathf.Ceil((float)setting.timeLeft / 60.0f);
            string text = "";

            if (setting.timeLeft > (float)setting.initialTime / 2.0f)
            {
                for (int i = 0; i < time; i++)
                {
                    randomArray.Add(0);
                }
                randomArray.Add(1);
            }
            else
            {
                for (int i = 0; i < time + (int)Mathf.Ceil((float)setting.initialTime / 120.0f); i++)
                {
                    randomArray.Add(1);
                }
                randomArray.Add(2);
            }
            //Debug.Log(randomArray);
            int numQues = randomArray[Random.Range(0, randomArray.Count)];

            List<int> counter = new List<int>();
            for (int i = 0; i < setting.unansweredQues.Count; i++)
            {
                counter.Add(i);
            }
            //Debug.Log(numQues);
            for (int i = 0; i < numQues; i++)
            {
                int tempIdx = Random.Range(0, counter.Count);
                int tempQues = counter[tempIdx];
                counter.RemoveAt(tempIdx);
                text += setting.answer[setting.unansweredQues[tempQues]] + "\n";
            }
            if (numQues == 0)
            {
                text = "Unfortunately, I failed to find answers :(\n\nBetter luck next time~";
            }
            paper.GetComponentInChildren<Text>().text = text;
        }
    }

    void Update()
    {
        if (GameObject.Find("LevelSetting") != null)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if (isWashroom)
                {
                    FadeOut();
                    Invoke("Change", duration);
                    Invoke("FadeIn", duration * 2);
                }
                //LoadSceneHandler.SetActive(true);
            }
        }
    }


    private void FadeOut()
    {
        SteamVR_Fade.Start(Color.black, duration);
    }

    private void FadeIn()
    {
        SteamVR_Fade.Start(Color.clear, duration);
        isWashroom = false;
    }


    private void Change()
    {
        Level.SetActive(true);
        current.SetActive(false);
        projectile.SetActive(true);
    }

    public bool inWashroom()
    {
        return isWashroom;
    }
}