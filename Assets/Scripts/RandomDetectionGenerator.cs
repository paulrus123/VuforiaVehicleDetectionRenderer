using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDetectionGenerator : MonoBehaviour
{

    //x,y, and z min correspond to vehicle dimensions (with [0,0,0] at center of vehicle)
    float zMin = 13.5f * 0.5f;
    float zMax = 13.5f;
    float yMin = 5f * 0.5f;
    float yMax = 5f;
    float xMin = 5.5f * 0.5f;
    float xMax = 5.5f;
    
    GameObject[] detections;
    public int numDetections = 30;
    public GameObject detectionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        detections = new GameObject[numDetections];

        for(int i = 0; i < numDetections; i++)
        {
            var detection = Instantiate(detectionPrefab);
            detections[i] = detection;
            detection.transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < numDetections; i++)
        {
            var _transform = detections[i].GetComponent<Transform>();
            var _color = detections[i].GetComponent<MeshRenderer>();

            //Set a random x,y, and z value scaled to the vehicle proportions
            var randx = xMax * Random.value;
            var randy = yMax * Random.value;
            var randz = zMax * Random.value;

            randx *= Random.value > 0.5 ? 1 : -1;
            randy *= Random.value > 0.5 ? 1 : -1;
            randz *= Random.value > 0.5 ? 1 : -1;

            //Make sure that detection is not within the area of the vehicle (with 2m buffer)
            if((Mathf.Abs(randx) > (2 +xMin)) || (Mathf.Abs(randz) > (2+zMin)))
            {
                _transform.localPosition = new Vector3(randx, randy, randz);
                _color.material.color = Random.ColorHSV(); //give the detection block a random HSV color
            }
        }
    }
}
