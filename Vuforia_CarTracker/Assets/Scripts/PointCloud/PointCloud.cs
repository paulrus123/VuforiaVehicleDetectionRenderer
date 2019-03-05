using System;

[Serializable]
public class PointCloud
{
    public PointCloud(int _size = 1)
    {
        x = new float[_size];
        y = new float[_size];
        z = new float[_size];
        size = _size;
    }
    public int size;
    public float[] x;
    public float[] y;
    public float[] z;
}
