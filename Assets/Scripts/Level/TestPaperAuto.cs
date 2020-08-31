using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class TestPaperAuto : MonoBehaviour
{
    public GameObject testPaper;
    public GameObject questionTextObj;
    //public GameObject Choices3D;
    public GameObject choiceA, choiceB, choiceC, choiceD;

    private TimeFreezeBehavior tf;
    private TestPaperBehavior playerTest;
    //private MeshRenderer[] choiceMesh;
    private GetQuestion question;
    private int tempQuestion = -1;
    private MultipleChoiceBehavior[] quesTrack;

    public float frequency = 0.0f;
    public bool slowDown = false;

    private float timeChange = 0.0f;
    private GameObject mainSelectHandler;
    private GameObject testPage, initialPage;
    private float offset;
    private bool start = false;

    void Start()
    {
        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();
        playerTest = GameObject.FindGameObjectWithTag("MainSelectHandler").GetComponent<TestPaperBehavior>();
        testPage = testPaper.transform.Find("TestPage").gameObject;
        initialPage = testPaper.transform.Find("InitialPage").gameObject;

        testPage.SetActive(false);
        initialPage.SetActive(true);
        //Choices3D.SetActive(false);

        offset = GameObject.Find("LevelSetting").GetComponent<LevelSetting>().offset;

        mainSelectHandler = GameObject.FindGameObjectWithTag("MainSelectHandler");
        timeChange = frequency;
        //choiceMesh = Choices3D.GetComponentsInChildren<MeshRenderer>();
    }

    void Update()
    {
        if (!playerTest.onPrepare)
        {
            if (offset > 0)
            {
                offset -= Time.deltaTime;
            }
            else
            {
                if (!start)
                {
                    initialPage.SetActive(false);
                    testPage.SetActive(true);
                    //Choices3D.SetActive(true);
                    if (question == null)
                    {
                        question = mainSelectHandler.GetComponent<TestPaperBehavior>().question.copy();
                        quesTrack = new MultipleChoiceBehavior[question.getQuesCount()];
                        next();
                    }
                    start = true;
                }
                if (!tf.isExisting())
                {
                    if (slowDown == true)
                    {
                        timeChange -= Time.deltaTime / 2;
                    }
                    else
                    {
                        timeChange -= Time.deltaTime;
                    }

                    if (timeChange <= 0)
                    {
                        timeChange = frequency;
                        next();
                    }
                }
            }
        }
    }

    public void next()
    {
        reset();
        if (tempQuestion < question.getQuesCount() - 1)
        {
            tempQuestion += 1;
        }
        else
        {
            tempQuestion = 0;
        }
        if (quesTrack[tempQuestion] == null)
        {
            quesTrack[tempQuestion] = new MultipleChoiceBehavior();
            JObject Q = question.getNextQuestion();
            quesTrack[tempQuestion].pushQuestion(Q);
        }
        GameObject[] choices = { choiceA, choiceB, choiceC, choiceD };
        quesTrack[tempQuestion].showQuestion(questionTextObj, choices, tempQuestion);
        int correctAns = quesTrack[tempQuestion].correctAns;
        quesTrack[tempQuestion].select(choices[correctAns], correctAns);
        Color higlight = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        //choiceMesh[correctAns].material.color = higlight;
    }

    public void reset()
    {
        Color white = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Button A = choiceA.GetComponentInChildren<Button>();
        Button B = choiceB.GetComponentInChildren<Button>();
        Button C = choiceC.GetComponentInChildren<Button>();
        Button D = choiceD.GetComponentInChildren<Button>();
        ColorBlock cb = A.colors;
        cb.normalColor = white;
        A.colors = cb;
        B.colors = cb;
        C.colors = cb;
        D.colors = cb;
        /*for(int i = 0; i < choiceMesh.Length; i++)
        {
            choiceMesh[i].material.color = white;
        }*/
    }
}

