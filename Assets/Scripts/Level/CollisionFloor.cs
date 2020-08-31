using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionFloor : MonoBehaviour
{
    //private float time = 25.0f;
    private GameObject obj;
    private bool hit = false, set = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private TeacherController control;
    private float timeLeft = 3.0f;

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.collider.tag == "Projectile")
        {
            obj = collision.collider.gameObject;
            hit = true;
        }
    }

    private void Start()
    {
        originalPosition = GameObject.FindGameObjectWithTag("Projectile").transform.position;
        originalRotation = GameObject.FindGameObjectWithTag("Projectile").transform.rotation;
        control = GameObject.FindGameObjectWithTag("TeacherAction").GetComponent<TeacherController>();
    }

    private void Update()
    {
        if (!set)
        {
            originalPosition = GameObject.FindGameObjectWithTag("Projectile").transform.position;
            originalRotation = GameObject.FindGameObjectWithTag("Projectile").transform.rotation;
            set = true;
        }
        if (hit)
        {
            if (!control.collision)
            {
                if (timeLeft > 0)
                {
                    timeLeft -= Time.deltaTime;
                }
                else
                {
                    obj.transform.position = originalPosition;
                    obj.transform.rotation = originalRotation;
                    hit = false;
                    timeLeft = 3.0f;
                }
            }
        }
    }
}
