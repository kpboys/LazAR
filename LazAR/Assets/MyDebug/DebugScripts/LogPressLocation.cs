using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPressLocation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TouchInputManager.Instance.OnPress.AddListener(() => 
            MyDebug.Log("Press at " + TouchInputManager.PrimaryTouchPosition));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
