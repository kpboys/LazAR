using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squaremaker : MonoBehaviour
{
    public GameObject corner1;
    public GameObject corner2;
    GameObject corner3;
    GameObject corner4;

    // Start is called before the first frame update
    void Start()
    {
        corner3 = Instantiate(corner2);
        corner4 = Instantiate(corner2);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diagonal = corner2.transform.position - corner1.transform.position;
        diagonal *= 0.5f;

        diagonal.y *= 0;

        Vector3 corner3Vector = new Vector3(-diagonal.z, 0, diagonal.x);
        Vector3 corner4Vector = new Vector3(diagonal.z, 0, -diagonal.x);

        corner3.transform.position = corner1.transform.position + diagonal + corner3Vector;
        corner4.transform.position = corner1.transform.position + diagonal + corner4Vector;
    }
}
