using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARFoundation.VisualScripting;

public class TrackedImagesCounterDebug : MonoBehaviour
{
    public bool drawCube;
    public GameObject cubePrefab;

    ARTrackedImageManager arTrackedImageManager;

    // Start is called before the first frame update
    void Start()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    // Update is called once per frame
    void Update()
    {
        MyDebug.Watch("Trackables.Count", arTrackedImageManager.trackables.count);

        int trackedTrackables = 0;
        foreach (ARTrackedImage trackedImage in arTrackedImageManager.trackables)
        {
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                trackedTrackables++;
                if (drawCube)
                    if (trackedImage.transform.childCount == 0)
                        Instantiate(cubePrefab, trackedImage.transform, false);
            }
        }

        MyDebug.Watch("Tracking", trackedTrackables);

        int limitedTrackables = 0;
        foreach (ARTrackedImage trackedImage in arTrackedImageManager.trackables)
        {
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited)
                limitedTrackables++;
        }

        MyDebug.Watch("Limited", limitedTrackables);
    }
}
