using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;

public class PointCloudEncoder : MonoBehaviour
{
    public static PointCloud cloud;
    private void Start()
    {
        cloud = new PointCloud(3);
        cloud.x[0] = 1.0f;
        cloud.y[0] = 2.0f;
        cloud.z[0] = 3.0f;
    }

    static IFormatter formatter = new BinaryFormatter();

    public static void Encode(NetworkStream stream)
    {
        formatter.Serialize(stream, cloud);
    }

    public static void EncodeRaw(NetworkStream stream)
    {
        int cloudLength = cloud.x.Length;
        int sizeOfArray = cloudLength * 3 * 4; //x,y,x * sizeOfFloat(4)
        int index;

        byte[] array = new byte[sizeOfArray];

        //encode raw vals to array
        for(int i = 0; i < sizeOfArray; i+=4)
        {
            index = i / 3 / 4;

            switch (i % 3)
            {
                case 0: //x
                    Array.Copy(BitConverter.GetBytes(cloud.x[index]), 0, array, i, 4);
                    break;
                case 1: //y
                    Array.Copy(BitConverter.GetBytes(cloud.y[index]), 0, array, i, 4);
                    break;
                case 2: //z
                    Array.Copy(BitConverter.GetBytes(cloud.z[index]), 0, array, i, 4);
                    break;
            }
        }

    }
}
