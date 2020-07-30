using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Broadcast_IP_Script : NetworkDiscovery
{
    public bool Started;
    public string[] Ip;

    public void StartBroadcast()
    {
        base.Initialize();

        if (Started)
            base.StopBroadcast();

        base.StartAsServer();
    }

    public void CatchBroadcast()
    {
        base.Initialize();
        base.StartAsClient();
    }

    public void StoopBroadcast()
    {
        base.Initialize();
        StopBroadcast();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);
        Debug.Log(fromAddress);
        Ip = fromAddress.Split(':');
    }
}
