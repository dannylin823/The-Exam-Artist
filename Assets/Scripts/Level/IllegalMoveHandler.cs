using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class IllegalMoveHandler : MonoBehaviour
{
    public GameObject playerCam;
    private AudioSource[] sound;
    public bool illegal = false;
    private bool soundOn = false;
    private float timeLeft = 0.0f;
    private LevelSetting setting;
    private TimeFreezeBehavior tf;

    void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("MainCamera");
        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        if (setting.washroomed)
        {
            timeLeft = 5.0f;
        }
        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();
    }

    void CameraMove()
    {
        float degreeY = playerCam.transform.localRotation.eulerAngles.y;
        float degreeX = playerCam.transform.localRotation.eulerAngles.x;
        float positionZ = playerCam.transform.localPosition.z;

        //Debug.Log(positionZ);
        illegal = ((degreeY >= 30 && degreeY <= 330) || 
            (positionZ >= 0.8) || (positionZ < 0.1) ||
            (positionZ <= 0.6 && degreeX >= 50 && degreeX <= 330)) ? true : false; 
    }

    void Update()
    {
        if (!setting.onPrepare && setting.illegalDetect)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            else
            {
                if (!tf.hold)
                {
                    CameraMove();
                    sound = GameObject.FindGameObjectWithTag("student").GetComponents<AudioSource>();
                    if (illegal && !soundOn)
                    {
                        if (!sound[2].isPlaying)
                        {
                            sound[2].Play();
                        }
                        else
                        {
                            sound[2].UnPause();
                        }
                        soundOn = true;
                    }
                    if (!illegal)
                    {
                        sound[2].Pause();
                        soundOn = false;
                    }
                }
            }
        }
    }
}
