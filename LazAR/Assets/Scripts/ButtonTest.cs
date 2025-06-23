using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour
{
    public int numParam;
    public GameObject objParam;
    [Button(nameof(TestMethod), new string[]{ nameof(numParam), nameof(objParam) })]
    public bool btn_testMethod;
    public float testFloat;
    public void TestMethod(int num, GameObject obj)
    {
        Debug.Log("Test Method: " + num);
        Debug.Log("Test Method: " + obj.transform.position);
    }
}
