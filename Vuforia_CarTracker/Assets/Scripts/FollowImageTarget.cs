﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowImageTarget : MonoBehaviour
{

    public Transform imageTargetTransform;
    public DefaultTrackableEventHandler handler;
    public GameObject children;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Update is called once per frame
    void Update()
    {
        if (imageTargetTransform != null)
        {
            if ((handler.m_NewStatus == Vuforia.TrackableBehaviour.Status.EXTENDED_TRACKED) || (handler.m_NewStatus == Vuforia.TrackableBehaviour.Status.TRACKED))
            {
                transform.position = imageTargetTransform.position;
                transform.rotation = imageTargetTransform.rotation;
                if(!children.activeSelf)
                    children.SetActive(true);
            }
            else
            {
                if(children.activeSelf)
                    children.SetActive(false);
            }
        }
    }
}