using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                var objs = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];
                if (objs.Length > 0)
                    instance = objs[0];
                if (objs.Length > 1)
                    Debug.LogError("More than one " + typeof(T).Name + " in the scene.");
                if (instance == null)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.hideFlags = HideFlags.HideAndDontSave;
                    gameObject.AddComponent<T>();
                    //Debug.Log("Singleton " + typeof(T).Name + " was created in the scene.");
                }
            }
            return instance;
        }
    }
}
