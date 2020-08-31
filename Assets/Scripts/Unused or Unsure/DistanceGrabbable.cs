using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class DistanceGrabbable : MonoBehaviour
{
    protected Rigidbody rb;
    protected bool originalKinematicState;
    protected Transform originalParent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalParent = transform.parent;
        originalKinematicState = rb.isKinematic;
    }

    public void onDistanceGrab(Hand hand)
    {
        rb.isKinematic = true;
        transform.SetParent(hand.gameObject.transform);
        hand.AttachObject(gameObject, GrabTypes.Trigger);
    }

    private void OnDetachedFromHand(Hand hand)
    {
        rb.isKinematic = false;
        transform.SetParent(originalParent);
    }
}
