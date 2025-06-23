using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LazerReciever : MonoBehaviour
{
    [SerializeField]
    private List<LazerContact> contactList;
    [Header("Events")]
    public UnityEvent onLazerEnter;
    public UnityEvent onAllContactsPowered;
    public UnityEvent onLazerExit;

    [Header("Debug Stuff")]
    [SerializeField, InspectorReadonly]
    private bool lazerIsHitting;
    private bool lazerHitLastFrame;
	 [SerializeField, InspectorReadonly]
	 private bool levelWon;

	 // Start is called before the first frame update
	 void Start()
	 {
        levelWon = false;
	 }
	 public void LazerHit()
    {
        lazerIsHitting = true;

        if(levelWon == false)
        {
            bool allPowered = true;
            for (int i = 0; i < contactList.Count; i++)
            {
                if(contactList[i].powered == false)
                {
                    allPowered = false;
                    break;
                }
            }
            if (allPowered)
            {
                onAllContactsPowered.Invoke();
                levelWon = true;
            }
        }

    }
	 private void LateUpdate()
	 {
        if (lazerIsHitting)
        {
				if (lazerHitLastFrame == false)
            {
					 lazerHitLastFrame = true;
                onLazerEnter.Invoke();
            }
            lazerIsHitting = false;
		  }
        else
        {
            if (lazerHitLastFrame)
            {
					 lazerHitLastFrame = false;
					 onLazerExit.Invoke();
				}
        }
	 }
    
	 

    // Update is called once per frame
    void Update()
    {
        
    }
}
