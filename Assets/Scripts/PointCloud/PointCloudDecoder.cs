using UnityEngine;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

public static class PointCloudDecoder
{
    static IFormatter formatter = new BinaryFormatter();

    public delegate void PointCloudDecoded(PointCloud cloud);
    public static event PointCloudDecoded OnPointCloudDecoded;

    public static int DecodeString(NetworkStream stream)
    {
        byte[] bytes = new byte[1024*2];
        int length;
        if ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
        {
            var data = new byte[length];
            Array.Copy(bytes, 0, data, 0, length);
            string serverMessage = Encoding.ASCII.GetString(data);
            List<string> stringList = serverMessage.Split(',').ToList();

            int numPoints = (int)Convert.ToDouble(stringList[0]);

            PointCloud cloud = new PointCloud(numPoints);

            for (int i = 0; i < numPoints; i++)
            {
                //Offset of +1 because of numPoints and the since y,z are after x they get incremented by +1, +2 
                cloud.x[i] = (float)Convert.ToDouble(stringList[i + 1]);
                cloud.y[i] = (float)Convert.ToDouble(stringList[i + 2]);
                cloud.z[i] = (float)Convert.ToDouble(stringList[i + 3]);

            }
            if (OnPointCloudDecoded != null)
                OnPointCloudDecoded(cloud);

            Debug.Log("Got an array");
        }
        return length;
    }
}
