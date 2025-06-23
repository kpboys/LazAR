using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ImagePropsManager : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager arTrackedImageManager;

    [SerializeField]
    DefineBorders defineBorders;

    [SerializeField]
    List<GameObject> propPrefabs;



    int propPrefabIndex = 0;
    Dictionary<GameObject, GameObject> imageProps = new Dictionary<GameObject, GameObject>();



    void OnEnable() => arTrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => arTrackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            //  Instantiate the next imageProp
            //GameObject newImageProp = Instantiate(propPrefabs[propPrefabIndex], defineBorders.ActualAnchor.transform, false);
            GameObject newImageProp = Instantiate(propPrefabs[propPrefabIndex], newImage.transform, false);
            newImageProp.transform.localScale = new Vector3(1,1,1) * defineBorders.WorldScale;
            propPrefabIndex++;

            //  Add new imageProps to imageProps list with the new trackedImage object as key
            imageProps[newImage.gameObject] = newImageProp;
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            //  Get the imageProp using the updated trackedImage as key.
            GameObject imageProp = imageProps[updatedImage.gameObject];

            //  If the image is being tracked
            if (updatedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                //  Set its position to be equal to the position of the updated gameObject.
                imageProp.transform.parent = updatedImage.transform;
                imageProp.transform.localPosition = Vector3.zero;

                //  Set its height to the height of the tracked anchor
                imageProp.transform.position = new Vector3(
                    imageProp.transform.position.x,
                    defineBorders.ActualAnchor.transform.position.y,
                    imageProp.transform.position.z);

                ////  Set its facing(rotation) to be equal to a vertically projected version of the updated gameObject's.
                Vector3 projectedfacing = new Vector3(updatedImage.transform.forward.x, 0, updatedImage.transform.forward.z);
                imageProp.transform.forward = projectedfacing;
            }
            else
                imageProp.transform.parent = defineBorders.ActualAnchor.transform;
        }

        foreach (var removedImage in eventArgs.removed)
        {
            MyDebug.Log("Something got removed? What the heck?? >:(");
            
        }
    }
}
