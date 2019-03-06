using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTextHandler : MonoBehaviour
{
    public TcpClientHandler clientHandler;

    // Update is called once per frame
    void Update()
    {
        if(clientHandler.isConnected())
        {
            GetComponent<Text>().text = "Disconnect";
        }
        else
        {
            GetComponent<Text>().text = "Connect";
        }
    }
}
