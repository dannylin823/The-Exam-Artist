using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsBehavior : MonoBehaviour
{
    public Image fadeImg;
    public GameObject timer;
    public GameObject testPaper;
    private float[] coolDown = new float[4];
    private float[] coolDownCounter = { 0.0f, 0.0f, 0.0f, 0.0f };
    private bool[] used = { false, false, false, false };
    private int[] limit = { 2, 10000, 10000, 10000 };

    // Start is called before the first frame update
    void Start()
    {
        fadeImg.canvasRenderer.SetAlpha(0.0f);
        coolDown[0] = 10.0f;
        coolDownCounter[0] = 10.0f;
    }

    void Update()
    {
       if (coolDownCounter[0] > 0 && used[0] == true)
        {
            coolDownCounter[0] -= Time.deltaTime;
            //Debug.Log(coolDownCounter[0]);
        }
        else if (coolDownCounter[0] <= 0 && used[0] == true)
        {
            coolDownCounter[0] = coolDown[0];
            used[0] = false;
        }
    }
    public void GodOfWashroom()
    {
        if (used[0] == false && limit[0] > 0)
        {
            if (timer.GetComponent<Timer>().timeLeft < 150)
            { 
                Debug.Log("Not enough time to go to washroom!");
            }
            else
            {
                timer.GetComponent<Timer>().timeLeft -= 120;
                used[0] = true;
                fadeImg.CrossFadeAlpha(1, 1, false);

                fadeImg.CrossFadeAlpha(0, 1, false);
                int correctAns = testPaper.GetComponent<TestPaperBehavior>().getCurrentQuesAns();
                Debug.Log("The correct answer is " + correctAns.ToString());
                limit[0] -= 1;
            }
        }
        else
        {
            if (limit[0] == 0)
            {
                Debug.Log("You can just use this skill twice per test");
            }
            else
            {
                Debug.Log("The skill is cooling down.");
            }
        }
    }
}
