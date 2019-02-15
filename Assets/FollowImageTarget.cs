using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowImageTarget : MonoBehaviour
{

    public Transform imageTargetTransform;

    // Update is called once per frame
    void Update()
    {
        if (imageTargetTransform != null)
        {
            transform.position = imageTargetTransform.position;
            transform.rotation = imageTargetTransform.rotation;
        }
    }
}
