using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class TriggerInput : MonoBehaviour
{
    public SteamVR_Action_Boolean Ypressed;
    public SteamVR_Action_Boolean Xpressed;
    public SteamVR_Action_Boolean Spressed;
    public SteamVR_Action_Boolean Apressed;
    public SteamVR_Action_Boolean Bpressed;
    public SteamVR_Action_Boolean Gpressed;
    public SteamVR_Action_Boolean LeftEast;
    public SteamVR_Action_Boolean LeftWest;
    public SteamVR_Action_Boolean RightEast;
    public SteamVR_Action_Boolean RightWest;

    public SteamVR_Input_Sources left;
    public SteamVR_Input_Sources right;

    public GodOfWashroomBehavior washroom;
    public MagicCheatSheetBehavior hint;
    public HideAndShowSkills hns;
    public TestPaperBehavior test;
    public GiftBlindEyesBehavior gbe;
    public MeditationBehavior mb;
    public TeacherController tc;
    public TimeFreezeBehavior tf;
    public Washroom wash;
    public MeditationHandler mh;

    private bool onPrepare = true, gameover = false, freeze = false;
    public float offset;
    private float holdTime = 2.0f;
    private List<Sprite> bribeList = new List<Sprite>();

    void OnDestroy()
    {
        Ypressed.RemoveOnStateDownListener(TriggerDownY, left);
        Xpressed.RemoveOnStateDownListener(TriggerDownX, left);
        Spressed.RemoveOnStateUpListener(TriggerUpS, left);
        Spressed.RemoveOnStateDownListener(TriggerDownS, left);
        Spressed.RemoveOnStateUpListener(TriggerUpS, right);
        Spressed.RemoveOnStateDownListener(TriggerDownS, right);
        Apressed.RemoveOnStateDownListener(TriggerDownA, right);
        Bpressed.RemoveOnStateDownListener(TriggerDownB, right);
        Gpressed.RemoveOnStateDownListener(TriggerDownG, left);
        Gpressed.RemoveOnStateUpListener(TriggerUpG, left);
        Gpressed.RemoveOnStateDownListener(TriggerDownG, right);
        Gpressed.RemoveOnStateUpListener(TriggerUpG, right);

        LeftEast.RemoveOnStateDownListener(TriggerDownR, left);
        LeftWest.RemoveOnStateDownListener(TriggerDownL, left);
        RightEast.RemoveOnStateDownListener(TriggerDownR, right);
        RightWest.RemoveOnStateDownListener(TriggerDownL, right);
    }

    void Start()
    {
        washroom = GameObject.Find("SkillsScript").GetComponent<GodOfWashroomBehavior>();
        hint = GameObject.Find("SkillsScript").GetComponent<MagicCheatSheetBehavior>();
        hns = GameObject.Find("SkillsScript").GetComponent<HideAndShowSkills>();
        gbe = GameObject.Find("SkillsScript").GetComponent<GiftBlindEyesBehavior>();
        mb = GameObject.Find("SkillsScript").GetComponent<MeditationBehavior>();

        test = GameObject.FindGameObjectWithTag("MainSelectHandler").gameObject.GetComponent<TestPaperBehavior>();

        offset = GameObject.Find("LevelSetting").GetComponent<LevelSetting>().offset;
        onPrepare = GameObject.Find("LevelSetting").GetComponent<LevelSetting>().onPrepare;

        tc = GameObject.FindGameObjectWithTag("TeacherAction").GetComponent<TeacherController>();
        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();

        bribeList = GameObject.Find("LevelSetting").GetComponent<LevelSetting>().bribeList;

        Ypressed.AddOnStateDownListener(TriggerDownY, left);
        Xpressed.AddOnStateDownListener(TriggerDownX, left);
        Spressed.AddOnStateUpListener(TriggerUpS, left);
        Spressed.AddOnStateDownListener(TriggerDownS, left);
        Spressed.AddOnStateUpListener(TriggerUpS, right);
        Spressed.AddOnStateDownListener(TriggerDownS, right);
        Apressed.AddOnStateDownListener(TriggerDownA, right);
        Bpressed.AddOnStateDownListener(TriggerDownB, right);
        Gpressed.AddOnStateDownListener(TriggerDownG, left);
        Gpressed.AddOnStateUpListener(TriggerUpG, left);
        Gpressed.AddOnStateDownListener(TriggerDownG, right);
        Gpressed.AddOnStateUpListener(TriggerUpG, right);

        LeftEast.AddOnStateDownListener(TriggerDownR, left);
        LeftWest.AddOnStateDownListener(TriggerDownL, left);
        RightEast.AddOnStateDownListener(TriggerDownR, right);
        RightWest.AddOnStateDownListener(TriggerDownL, right);
    }

    void Update()
    {
        if (onPrepare)
        {
            onPrepare = GameObject.Find("LevelSetting").GetComponent<LevelSetting>().onPrepare;
        }
        else
        {
            if (offset >= 0)
            {
                offset -= Time.deltaTime;
            }
            if (!gameover)
            {
                gameover = tc.gameover;
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

    public void TriggerDownY(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation() && !freeze)
            {
                if (test.isBribeSkillActive())
                {
                    if (bribeList.Count >= 1)
                    {
                        GameObject option = GameObject.Find("BribeSkillPage").transform.Find("Opt1").gameObject;
                        GameObject image = option.transform.Find("Canvas").transform.Find("Image").gameObject;
                        test.ChooseBribee(image);
                    }
                }
                else
                {
                    hint.MagicCheatSheet();
                }
            }
        }
    }
    public void TriggerDownX(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if(!wash.inWashroom() && !mh.inMeditation() && !freeze)
            {
                if (test.isBribeSkillActive())
                {
                    if (bribeList.Count >= 2)
                    {
                        GameObject option = GameObject.Find("BribeSkillPage").transform.Find("Opt2").gameObject;
                        GameObject image = option.transform.Find("Canvas").transform.Find("Image").gameObject;
                        test.ChooseBribee(image);
                    }
                }
                else
                {
                     washroom.GodOfWashroom();
                }
            }
        }
    }
    public void TriggerUpS(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation())
            {
                hns.Hide();
            }
        }
    }
    public void TriggerDownS(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation())
            {
                hns.Show();
            }
        }
    }
    public void TriggerDownL(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (test.isBribeActive())
        {
            test.bribePagePrev();
        }
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation())
            {
                test.previous();
            }
        }
    }
    public void TriggerDownR(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (test.isBribeActive())
        {
            test.bribePageNext();
        }
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation())
            {
                test.next();
            }
        }
    }

    public void TriggerDownA(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation() && !freeze)
            {
                if (test.isBribeSkillActive())
                {
                    if (bribeList.Count >= 3)
                    {
                        GameObject option = GameObject.Find("BribeSkillPage").transform.Find("Opt3").gameObject;
                        GameObject image = option.transform.Find("Canvas").transform.Find("Image").gameObject;
                        test.ChooseBribee(image);
                    }
                }
                else
                {
                    mb.Meditation();
                }
            }
        }
    }

    public void TriggerDownB(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation() && !freeze)
            {
                if (test.isBribeSkillActive())
                {
                    test.backToTest();
                }
                else
                {
                    test.showBribeSkillPage();
                }
            }
        }
    }

    public void TriggerDownG(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation())
            {
                freeze = true;
                //tf.hold = true;
            }
        }
    }

    public void TriggerUpG(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation())
            {
                freeze = false;
                tf.hold = false;
            }
        }
    }

}
