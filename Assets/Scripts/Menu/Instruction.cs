using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;
using UnityEngine.UI;

public class Instruction : MonoBehaviour
{
    private Text main, shadow;
    private string init = "Become the one and only EXAM ARTIST!";

    void Start()
    {
        main = GameObject.Find("InstructionText").GetComponent<Text>();
        shadow = GameObject.Find("InstructionText2").GetComponent<Text>();
        main.text = init;
        shadow.text = init;
    }

    public void showInstructionX()
    {
        main.text = "Go to washroom for 10 seconds and at most 5 times\n (cost 1 min)";
        shadow.text = main.text;
    }

    public void showInstructionY()
    {
        main.text = "Show hint on your magic cheat sheet for 5 seconds";
        shadow.text = main.text;
    }

    public void showInstructionA()
    {
        main.text = "Meditate to think about the current answer \n(cost 1 min)";
        shadow.text = main.text;
    }

    public void showInstructionB()
    {
        main.text = "Let your classmate(s) distract the teacher";
        shadow.text = main.text;
    }

    public void showInstructionG()
    {
        main.text = "Hold to stop time for a total of 10 seconds";
        shadow.text = main.text;
    }
}
