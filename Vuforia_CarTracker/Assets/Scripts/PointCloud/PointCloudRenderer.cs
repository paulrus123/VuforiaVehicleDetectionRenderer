using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DisruptorUnity3d;

public class PointCloudRenderer : MonoBehaviour
{
    public GameObject pointPrefab;

    RingBuffer<PointCloud> buf = new RingBuffer<PointCloud>(5);

    private object _lock;

    // Start is called before the first frame update
    void Start()
    {
        _lock = new object();
        PointCloudDecoder.OnPointCloudDecoded += ReceivePointCloud;
    }

    void ReceivePointCloud(PointCloud cloud)
    {
        lock (_lock)
        {
            buf.Enqueue(cloud);
        }
    }

    private void Update()
    {
        RenderPointCloud();
    }

    void RenderPointCloud()
    {
        lock (_lock)
        {
            if (buf.Count <= 0)
                return;

            PointCloud cloud = buf.Dequeue();

            for (int i = 0; i < cloud.size; i++)
            {
                GameObject obj = Instantiate(pointPrefab, transform);
                Transform _t = obj.transform;
                _t.localPosition = new Vector3(cloud.x[i], cloud.y[i], cloud.z[i]);

            }
        }
    }
}
