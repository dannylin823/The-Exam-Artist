using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritualRealmBehavior : MonoBehaviour
{
    public GameObject skills;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SpiritualRealm()
    {
        for (int i = 0; i < 7; i++)
        {
            skills.transform.GetChild(i).gameObject.SetActive(i > 3);
        }
    }

    public void Back()
    {
        for (int i = 0; i < 7; i++)
        {
            skills.transform.GetChild(i).gameObject.SetActive(i < 4);
        }
    }
}
