using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheatSheet : MonoBehaviour
{
    private static Vector3 originalPlace;
    private Vector3 newPlace;
    private bool collide = false;
    void Start()
    {
        originalPlace = gameObject.transform.position;
    }

    private void Update()
    {
        if (collide)
        {
            gameObject.transform.position = newPlace;
        }
    }


    public void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if(collision.collider.name == "prop_sch_table")
        {
            newPlace = collision.collider.transform.position;
            collide = true;
        }
    }

    public void OnCollisionExit(UnityEngine.Collision collision)
    {
        if (collision.collider.name == "prop_sch_table")
        {
            newPlace = gameObject.transform.position;
            collide = false;
        }
    }
}
