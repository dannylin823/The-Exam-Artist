using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class KeyBoardFunction : MonoBehaviour
{
    private HideAndShowSkills hns;
    private float offset;
    private Transform originalPlace;
    private LevelSetting setting;
    private bool gameover = false, freeze = false;
    public TeacherController tc;
    public TimeFreezeBehavior tf;
    public Washroom wash;
    public MeditationHandler mh;
    private float holdTime = 2.0f;

    private void Start()
    {
        hns = GameObject.Find("SkillsScript").GetComponent<HideAndShowSkills>();
        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        offset = setting.offset;

        tc = GameObject.FindGameObjectWithTag("TeacherAction").GetComponent<TeacherController>();
        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();
    }

    private void Update()
    {
        if (!SteamVR.active && !setting.onPrepare)
        {
            if (offset > 0)
            {
                offset -= Time.deltaTime;
            }
            else
            {
                if (!gameover)
                {
                    gameover = tc.gameover;
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (!wash.inWashroom() && !mh.inMeditation())
                        {
                            hns.Show();
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.Space))
                    {
                        if (!wash.inWashroom() && !mh.inMeditation())
                        {
                            hns.Hide();
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (!wash.inWashroom() && !mh.inMeditation())
                        {
                            freeze = true;
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.F))
                    {
                        if (!wash.inWashroom() && !mh.inMeditation())
                        {
                            freeze = false;
                            tf.hold = false;
                        }
                    }
                    if (freeze)
                    {
                        if (holdTime > 0)
                        {
                            holdTime -= Time.deltaTime;
                        }
                        else
                        {
                            tf.hold = true;
                        }
                    }
                    else
                    {
                        holdTime = 2.0f;
                    }
                }
            }
        }
    }

    public void freezeOn()
    {
        freeze = true;
    }

    public void freezeOff()
    {
        freeze = false;
        tf.hold = false;
    }
}
