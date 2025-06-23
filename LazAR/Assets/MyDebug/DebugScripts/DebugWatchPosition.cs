using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugWatchPosition : MonoBehaviour
{
    void Update()
    {
        MyDebug.Watch($"{transform.name}.x", $"({transform.position.x.ToString("0.00")}, {transform.position.y.ToString("0.00")}, {transform.position.z.ToString("0.00")})");
    }
}
