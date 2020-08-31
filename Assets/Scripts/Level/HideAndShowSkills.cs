using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndShowSkills : MonoBehaviour
{
    private GameObject skillCanvas;

    void Awake()
    {
        GameObject table = GameObject.Find("PlayerTable");
        skillCanvas = table.transform.Find("SkillsOverlay").gameObject;
    }
    public void Hide()
    {
        skillCanvas.SetActive(false);
    }

    public void Show()
    {
        skillCanvas.SetActive(true);
    }
}
