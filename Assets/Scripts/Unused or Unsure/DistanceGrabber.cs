using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class DistanceGrabber : MonoBehaviour
{
    private SteamVR_LaserPointer laserPointer;
    private Hand hand;

    void Awake()
    {
        laserPointer = GetComponent<SteamVR_LaserPointer>();
        hand = GetComponent<Hand>();

        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.GetComponent<DistanceGrabbable>() != null)
        {
            e.target.GetComponent<DistanceGrabbable>().onDistanceGrab(hand);
        }
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.GetComponent<Interactable>() != null)
        {
            hand.ShowGrabHint();
            hand.IsStillHovering(e.target.GetComponent<Interactable>());

            laserPointer.thickness = 0.002f;
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.GetComponent<Interactable>() == null)
        {
            hand.HideGrabHint();
            laserPointer.thickness = 0.0f;
        }
    }
}
