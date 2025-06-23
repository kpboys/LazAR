using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MyDebug : Singleton<MyDebug>
{
    [SerializeField]
    GameObject watchLabelsTextBox;
    [SerializeField]
    GameObject watchValuesTextBox;
    [SerializeField, Space]
    GameObject logTextBox;

    [SerializeField, Tooltip("The maximum amount of logs shown on screen before older messages are deleted.")]
    /// <summary>
    /// The maximum amount of logs shown on screen before older messages are deleted.
    /// </summary>
    int MaxLogLength = 20;



    /// <summary>
    /// 
    /// </summary>
    List<string> screenLog = new List<string>();

    /// <summary>
    /// 
    /// </summary>
    Dictionary<string, string> screenWatch = new Dictionary<string, string>();

    /// <summary>
    /// 
    /// </summary>
    TextMeshProUGUI screenLogTextComponent;

    /// <summary>
    /// 
    /// </summary>
    TextMeshProUGUI screenWatchLabelTextComponent;

    /// <summary>
    /// 
    /// </summary>
    TextMeshProUGUI screenWatchValueTextComponent;

    /// <summary>
    /// 
    /// </summary>
    Dictionary<string, List<Action>> keyHeld = new Dictionary<string, List<Action>>();

    /// <summary>
    /// 
    /// </summary>
    Dictionary<string, List<Action>> keyPressed = new Dictionary<string, List<Action>>();



    // Start is called before the first frame update
    void Start()
    {
        screenWatchLabelTextComponent = watchLabelsTextBox.GetComponent<TextMeshProUGUI>();
        screenWatchValueTextComponent = watchValuesTextBox.GetComponent<TextMeshProUGUI>();
        screenLogTextComponent = logTextBox.GetComponent<TextMeshProUGUI>();

        Log("MyDebug started");
    }

    // Update is called once per frame
    void Update()
    {
        //  Draw watch list 
        //! Maybe change this so that you can choose that the value stays on screen.
        Instance.screenWatchLabelTextComponent.text = "";
        Instance.screenWatchValueTextComponent.text = "";
        foreach (KeyValuePair<string, string> w in Instance.screenWatch)
        {
            Instance.screenWatchLabelTextComponent.text += w.Key + "<br>";
            Instance.screenWatchValueTextComponent.text += w.Value + "<br>";
        }

        //  Check keys held and invoke callbacks
        foreach (KeyValuePair<string, List<Action>> keyValuePair in keyHeld)
        {
            if (Input.GetKey(keyValuePair.Key))
                foreach (Action callback in keyValuePair.Value)
                    callback.Invoke();
        }

        //  Check keys pressed and invoke callbacks
        foreach (KeyValuePair<string, List<Action>> keyValuePair in keyPressed)
        {
            if (Input.GetKeyDown(keyValuePair.Key))
                foreach (Action callback in keyValuePair.Value)
                    callback.Invoke();
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="callback"></param>
    public static void SubscribeKeyHeld(string keyName, Action callback)
    {
        if (Debug.isDebugBuild)
        {
            if (!Instance.keyHeld.ContainsKey(keyName))
                Instance.keyHeld[keyName] = new List<Action>();
            Instance.keyHeld[keyName].Add(callback);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="callback"></param>
    public static void SubscribeKeyPressed(string keyName, Action callback)
    {
        if (Debug.isDebugBuild)
        {
            if (!Instance.keyPressed.ContainsKey(keyName))
                Instance.keyPressed[keyName] = new List<Action>();
            Instance.keyPressed[keyName].Add(callback);
        }
    }

    /// <summary>
    /// Add a message to the on screen watch list. <br />
    /// Only shows the watch on screen in the frames where the method is called.
    /// </summary>
    /// <param name="label">The label to be written alongside the value. <br /> </param>
    /// <param name="value"></param>
    public static void Watch(string label, string value)
    {
        //! Maybe change this so that you can choose that the value stays on screen.
        Instance.screenWatch[label] = value;
    }

    #region Watch overloads

    /// <summary>
    /// Add a message to the on screen watch list. <br />
    /// Only shows the watch on screen in the frames where the method is called.
    /// </summary>
    public static void Watch<T>(string label, T value)
    {
        Watch(label, value.ToString());
    }

    /// <summary>
    /// Add a message to the on screen watch list. <br />
    /// Only shows the watch on screen in the frames where the method is called.
    /// </summary>
    public static void Watch(string label, bool value)
    {
        Watch(label, value.ToString());
    }

    /// <summary>
    /// Add a message to the on screen watch list. <br />
    /// Only shows the watch on screen in the frames where the method is called.
    /// </summary>
    public static void Watch(string label, char value)
    {
        Watch(label, value.ToString());
    }

    /// <summary>
    /// Add a message to the on screen watch list. <br />
    /// Only shows the watch on screen in the frames where the method is called.
    /// </summary>
    public static void Watch(string label, byte value)
    {
        Watch(label, value.ToString());
    }

    /// <summary>
    /// Add a message to the on screen watch list. <br />
    /// Only shows the watch on screen in the frames where the method is called.
    /// </summary>
    public static void Watch(string label, sbyte value)
    {
        Watch(label, value.ToString());
    }

    /// <summary>
    /// Add a message to the on screen watch list. <br />
    /// Only shows the watch on screen in the frames where the method is called.
    /// </summary>
    public static void Watch(string label, int value)
    {
        Watch(label, value.ToString());
    }

    /// <summary>
    /// Add a message to the on screen watch list. <br />
    /// Only shows the watch on screen in the frames where the method is called.
    /// </summary>
    public static void Watch(string label, double value)
    {
        Watch(label, value.ToString());
    }

    /// <summary>
    /// Add a message to the on screen watch list. <br />
    /// Only shows the watch on screen in the frames where the method is called.
    /// </summary>
    public static void Watch(string label, float value)
    {
        Watch(label, value.ToString());
    }

    /// <summary>
    /// Add a message to the on screen watch list. <br />
    /// Only shows the watch on screen in the frames where the method is called.
    /// </summary>
    public static void Watch(string label, decimal value)
    {
        Watch(label, value.ToString());
    }

    #endregion

    /// <summary>
    /// Adds a message to the on screen log.
    /// </summary>
    /// <param name="message"></param>
    public static void Log(string message)
    {
        Instance.screenLog.Add(message + " @ " + DateTime.Now.Second);

        if (Instance.screenLog.Count > Instance.MaxLogLength)
            Instance.screenLog.RemoveAt(0);

        Instance.screenLogTextComponent.text = "";
        foreach (string m in Instance.screenLog)
        {
            Instance.screenLogTextComponent.text += m + "<br>";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static void ClearLog()
    {
        Instance.screenLog.Clear();
        Instance.screenLogTextComponent.text = "";
    }
}
