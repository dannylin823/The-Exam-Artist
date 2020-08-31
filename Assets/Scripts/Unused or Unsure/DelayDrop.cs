using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDrop : MonoBehaviour
{
    public float time = 20.0f;

    // Update is called once per frame
    void Update()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
