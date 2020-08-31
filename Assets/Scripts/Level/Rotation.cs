using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Transform target;
    public int degree;
    private Vector3 point;

    void Start()
    {
        point = target.position;
    }

    void Update()
    {
        transform.RotateAround(point, Vector3.up, degree * Time.deltaTime);
    }
}
