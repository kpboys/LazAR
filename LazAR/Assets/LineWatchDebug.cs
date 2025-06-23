using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineWatchDebug : MonoBehaviour
{
    LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MyDebug.Watch("line positions", lr.positionCount);
        Vector3[] positions = new Vector3[30]; ;
        lr.GetPositions(positions);
        int i = 0;
        foreach (Vector3 pos in positions)
        {
            if (!(pos.x == 0 && pos.y == 0 && pos.z == 0))
            {
                MyDebug.Watch($"position {i}", $"({pos.x.ToString("0.00")}, {pos.y.ToString("0.00")}, {pos.z.ToString("0.00")})");
                i++;
            }
        }
    }
}
