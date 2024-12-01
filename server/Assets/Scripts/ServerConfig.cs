using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ServerConfigScriptObject : ScriptableObject
{
    public string SererIP;
    public string Port;

    public string LogApi;
    public string AcknowledgeApi;
}
