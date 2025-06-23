using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDebugger : MonoBehaviour
{
    [SerializeField]
    private string identifierText = "EventDebugger";
    public void WriteString(string text)
    {
        Debug.Log(identifierText + ": " + text);
    }
}
