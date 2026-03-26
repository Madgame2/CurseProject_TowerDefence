using UnityEngine;

public class NetworkConfig
{
    public string ServerUrl { get; set; }
    public int Port { get; set; }


    public NetworkConfig(string serverUrl, int port)
    {
        ServerUrl = serverUrl;
        Port = port;
    }
}
