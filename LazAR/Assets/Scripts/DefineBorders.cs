using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;

public class DefineBorders : MonoBehaviour
{
    #region Inspector
    public ARPlaneManager planeManager;
    public ARRaycastManager raycastManager;
    public ARAnchorManager anchorManager;
    public LineWidthScaler lineWidthScaler;
    [Space]
    public GameObject cornerPrefab;
    public GameObject levelObject;
    [Space]
    public float touchTime = 0.5f;
    [Space]
    public UnityEvent onBorderMade;
    [Space]
    public bool playmodeTest = false;
    #endregion

    private GameObject actualAnchor;
    public GameObject ActualAnchor { get { return actualAnchor; } }
    private GameObject anchorIshCube;
    private GameObject actualCorner;

    private List<GameObject> markers = new List<GameObject>();

    private Vector3[] cornerPositions = new Vector3[4];
    public Vector3[] CornerPositions { get { return cornerPositions; } }

    private bool beginDefiningBorders = true;
    private bool detectedTouch = false;

    private ARPlane planeHit;
    private ARPlane planeToRemember;
    private Pose poseHit;

    private int touchCount = 0;

    private readonly float standardDistance = 38.18377f;

    private float worldScale;
    public float WorldScale { get { return worldScale; } }

    // Start is called before the first frame update
    void Start()
    {
        if (playmodeTest)
        {
            beginDefiningBorders = false;
            planeManager.enabled = false;
            worldScale = levelObject.transform.localScale.x;
            lineWidthScaler.GiveNewWidth(WorldScale);
            levelObject.SetActive(true);
        }
        else
        {
            AddTouchEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (beginDefiningBorders && detectedTouch)
        {
            switch (touchCount)
            {
                case 1:
                    actualAnchor = AnchorOnPlane(planeHit, poseHit);
                    AttachToAnchor(anchorIshCube = CreateMarker(cornerPrefab, poseHit.position));
                    planeToRemember = planeHit;
                    detectedTouch = false;
                    AddTouchEvent();
                    break;
                case 2:
                    Vector3 cornerPos = poseHit.position;
                    cornerPos.y = anchorIshCube.transform.position.y;
                    AttachToAnchor(actualCorner = CreateMarker(cornerPrefab, cornerPos));
                    beginDefiningBorders = false;
                    planeManager.enabled = false;
                    SetCornerPositions();
                    HideMarkers();
                    SetLevel();
                    lineWidthScaler.GiveNewWidth(WorldScale);
                    HidePlanes();
                    onBorderMade?.Invoke();
                    break;
            }
        }
    }

    private void TouchEvent()
    {
        if (RaycastPlane(TouchInputManager.PrimaryTouchPosition, out planeHit, out poseHit))
            if (planeToRemember == null || planeToRemember == planeHit)
            {
                detectedTouch = true;
                touchCount++;
                RemoveTouchEvent();
            }
    }

    private void AddTouchEvent()
    {
        TouchInputManager.Instance.OnTouchEnded.AddListener(TouchEvent);
    }

    private void RemoveTouchEvent()
    {
        TouchInputManager.Instance.OnTouchEnded.RemoveListener(TouchEvent);
    }

    private bool RaycastPlane(Vector3 touchPos, out ARPlane planeHit, out Pose poseHit)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        planeHit = null;
        poseHit = new Pose();
        if (raycastManager.Raycast(touchPos, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
        {
            planeHit = hits[0].trackable.gameObject.GetComponent<ARPlane>();
            poseHit = hits[0].pose;
            return true;
        }
        return false;
    }

    private GameObject CreateMarker(GameObject markerPrefab, Vector3 position)
    {
        GameObject marker = Instantiate(markerPrefab, position, Quaternion.identity);
        markers.Add(marker);
        return marker;
    }

    private GameObject AnchorOnPlane(ARPlane plane, Pose pose)
    {
        GameObject anchorPlane = anchorManager.AttachAnchor(plane, pose).gameObject;
        return anchorPlane;
    }

    private void SetCornerPositions()
    {
        Vector3 A = anchorIshCube.transform.position;
        Vector3 D = actualCorner.transform.position;

        cornerPositions[0] = A;
        cornerPositions[1] = D;

        Vector3 center = (A + D) / 2;
        Vector3 diagonal = (A - D) / 2;
        Vector3 B = new Vector3(center.x - diagonal.z, A.y, center.z + diagonal.x);
        Vector3 C = new Vector3(center.x + diagonal.z, A.y, center.z - diagonal.x);

        cornerPositions[2] = B;
        cornerPositions[3] = C;
        if (!playmodeTest)
        {
            AttachToAnchor(CreateMarker(cornerPrefab, B));
            AttachToAnchor(CreateMarker(cornerPrefab, C));
        }
    }

    private void SetLevel()
    {
        //Scale level here!!!
        float distance = Vector3.Distance(anchorIshCube.transform.position, actualCorner.transform.position);
        float newScale = distance / standardDistance;
        worldScale = newScale;
        levelObject.transform.localScale = new Vector3(newScale, newScale, newScale);
        Vector3 center = (cornerPositions[0] + cornerPositions[1]) / 2;
        levelObject.transform.position = center;
        levelObject.transform.LookAt(actualCorner.transform);
        levelObject.transform.Rotate(new Vector3(0, 45, 0));
        levelObject.SetActive(true);
    }

    private void HideMarkers()
    {
        foreach (GameObject marker in markers)
        {
            marker.SetActive(false);
        }
    }

    public void AttachToAnchor(GameObject gameObject)
    {
        if (actualAnchor != null)
            gameObject.transform.SetParent(actualAnchor.transform);
        else
            MyDebug.Log("Error in DefineBorders script: Failed to attach object to anchor, anchor is probably null.");
    }

    public void ResetBorders()
    {
        actualAnchor = null;
        actualCorner = null;
        anchorIshCube = null;
        foreach (GameObject marker in markers)
            Destroy(marker);
        markers.Clear();

        planeHit = null;
        planeToRemember = null;

        beginDefiningBorders = true;
        detectedTouch = false;
        touchCount = 0;
        planeManager.enabled = true;
        RemoveTouchEvent();
        StartCoroutine(TouchTimer());
    }

    private IEnumerator TouchTimer()
    {
        float currentTime = 0;
        while (true)
        {
            float time = 0.1f;
            currentTime += time;
            if (currentTime >= touchTime)
                break;
            else
                yield return new WaitForSeconds(0.1f);
        }
        AddTouchEvent();
    }

    private void HidePlanes()
    {
        foreach (ARPlane item in planeManager.trackables)
        {
            MeshRenderer renderer = item.gameObject.GetComponent<MeshRenderer>();
            ARPlaneMeshVisualizer visualizer = item.gameObject.GetComponent<ARPlaneMeshVisualizer>();
            ARFeatheredPlaneMeshVisualizer feather = item.gameObject.GetComponent<ARFeatheredPlaneMeshVisualizer>();
            if (renderer != null)
                renderer.enabled = false;
            if (visualizer != null)
                visualizer.enabled = false;
            if (feather != null)
                feather.enabled = false;
        }
    }

}
