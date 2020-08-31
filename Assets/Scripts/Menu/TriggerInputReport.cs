using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class TriggerInputReport : MonoBehaviour
{
    public SteamVR_Action_Boolean LeftNorth;
    public SteamVR_Action_Boolean LeftSouth;
    public SteamVR_Action_Boolean RightNorth;
    public SteamVR_Action_Boolean RightSouth;

    public SteamVR_Input_Sources left;
    public SteamVR_Input_Sources right;

    public ScoreCalculate report;
    public GameObject scoreboard;

    void OnDestroy()
    {
        LeftNorth.RemoveOnStateDownListener(TriggerDownU, left);
        LeftSouth.RemoveOnStateDownListener(TriggerDownD, left);
        RightNorth.RemoveOnStateDownListener(TriggerDownU, right);
        RightSouth.RemoveOnStateDownListener(TriggerDownD, right);
    }

    void Start()
    {
        LeftNorth.AddOnStateDownListener(TriggerDownU, left);
        LeftSouth.AddOnStateDownListener(TriggerDownD, left);
        RightNorth.AddOnStateDownListener(TriggerDownU, right);
        RightSouth.AddOnStateDownListener(TriggerDownD, right);
    }

    public void TriggerDownU(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (scoreboard.activeSelf)
        {
            report.prevReport();
        }
    }

    public void TriggerDownD(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (scoreboard.activeSelf)
        {
            report.nextReport();
        }
    }
}
