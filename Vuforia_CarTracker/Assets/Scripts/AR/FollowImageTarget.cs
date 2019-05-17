using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowImageTarget : MonoBehaviour
{
    public Transform imageTargetTransform;
    public DefaultTrackableEventHandler handler;
    public GameObject children;
    Vuforia.TrackableBehaviour.Status m_status;

    private void Start()
    {
        //Lock screen to portrait mode
        Screen.orientation = ScreenOrientation.Portrait;
        handler.OnTrackableStatusUpdated += TrackableStateChange;
    }

    void TrackableStateChange(Vuforia.TrackableBehaviour.Status new_status)
    {
        m_status = new_status;
    }

    void Update()
    {
        if (imageTargetTransform != null)
        {
            if ((m_status == Vuforia.TrackableBehaviour.Status.EXTENDED_TRACKED) || (m_status == Vuforia.TrackableBehaviour.Status.TRACKED))
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
