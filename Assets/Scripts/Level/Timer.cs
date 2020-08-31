using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public float timeLeft;
    private bool timesUp = false;
    private TestPaperBehavior test;
    private LevelSetting setting;
    private TimeFreezeBehavior tf;

    private float offset;
    private bool onPrepare;

    void Start()
    {
        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();
        test = GameObject.FindGameObjectWithTag("MainSelectHandler").gameObject.GetComponent<TestPaperBehavior>();
        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        offset = setting.offset;
        onPrepare = setting.onPrepare;
        if (setting.timeLeft > 0)
        {
            timeLeft = setting.timeLeft;
        }
        else
        {
            timeLeft = setting.initialTime;
        }
        timeLeft += 1;
    }
    void Update()
    {
        if (!onPrepare)
        {
            if (offset > 0)
            {
                offset -= Time.deltaTime;
            }
            else
            {
                if (timeLeft > 0)
                {
                    if (!tf.isExisting())
                    {
                        timeLeft -= Time.deltaTime;
                        string minutes = ((int)timeLeft / 60).ToString();
                        string seconds = ((int)timeLeft % 60).ToString();
                        if ((int)timeLeft / 60 < 10) { minutes = "0" + minutes; }
                        if ((int)timeLeft % 60 < 10) { seconds = "0" + seconds; }
                        timerText.text = minutes + ":" + seconds;
                    }
                }
                else if (timeLeft < 0 && !timesUp)
                {
                    test.writeAnsToJson();
                    timesUp = true;
                }
            }
        }
        else
        {
            onPrepare = setting.onPrepare;
        }
    }
}
