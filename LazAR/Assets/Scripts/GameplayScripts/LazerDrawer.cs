using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public enum RayResult
{
    HitInteractor,
    HitAffector,
    HitStopper,
    HitNothing
}
[ExecuteInEditMode]
public class LazerDrawer : MonoBehaviour
{
    [Header("Values")]
    [SerializeField]
    private float maxRayDistance;
    [SerializeField]
    private int maxTries;

    [Header("References")]
    [SerializeField]
    private Transform lazerOrigin;
    [SerializeField]
    private LineRenderer lineRenderer;

    [Header("Controls")]
    public bool updateLazer;
    public float minRegisteringDistance;
    public float debugSphereRadius;
    public List<Vector3> debugPoints;
    [Button("ProcessLazer")]
    public bool processLazerBtn;

    private List<ILazerAffector> affectors;
    private List<ILazerAffector> affectorsToRemove;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (updateLazer)
        {
            ProcessLazer();
        }
    }

    public void ProcessLazer()
    {
        if (lazerOrigin == null || lineRenderer == null)
            return;

        //Empty line renderer
        lineRenderer.positionCount = 0;

        Vector3 direction = lazerOrigin.forward;
        List<Vector3> pointsOfLine = new List<Vector3>
        {
            lazerOrigin.position
        };
        float heightOfLazer = lazerOrigin.position.y;
        float yDirOfLazer = lazerOrigin.forward.y;

        void AddToPoints(Vector3 point)
        {
            pointsOfLine.Add(new Vector3(point.x, heightOfLazer, point.z));
        }
        void AddRangeToPoints(List<Vector3> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                pointsOfLine.Add(new Vector3(points[i].x, heightOfLazer, points[i].z));
            }
        }
        void SetDirection(Vector3 newDir)
        {
            direction = new Vector3(newDir.x, yDirOfLazer, newDir.z);
        }

        affectors = new List<ILazerAffector>();
        affectorsToRemove = new List<ILazerAffector>();

        //Bool for stopping the whole loop if the lazer hits something that stops the lazer
        bool hitEndBreaker = false;

        //Local method for casting the ray out to a certain distance
        RayResult CastRay(float maxDist)
        {
            RaycastHit hit;
            //Debug.DrawRay(pointsOfLine[pointsOfLine.Count - 1], direction, UnityEngine.Color.yellow);
            if (Physics.Raycast(pointsOfLine[pointsOfLine.Count - 1], direction, out hit, maxDist))
            {
                if (hit.collider.TryGetComponent<ILazerInteractable>(out ILazerInteractable interComp))
                {
                    PieceResult result = interComp.ProcessLazerHit(hit.point, direction);
                    //pointsOfLine.AddRange(result.points);
                    AddRangeToPoints(result.points);
                    //direction = result.direction;
                    SetDirection(result.direction);
                    return RayResult.HitInteractor;
                }
                else if (hit.collider.TryGetComponent<ILazerAffector>(out ILazerAffector affectComp))
                {
                    //If the affector hit is already logged and being used, ignore this hit
                    if (affectors.Contains(affectComp))
                        return RayResult.HitNothing;

                    affectComp.ProcessLazerHit(this);
                    //pointsOfLine.Add(hit.point);
                    AddToPoints(hit.point);
                    return RayResult.HitAffector;

                }
                else
                {
                    if (hit.collider.TryGetComponent<LazerReciever>(out LazerReciever recieveComp))
                    {
                        recieveComp.LazerHit();
                    }
                    //If we hit something not interactable, finish the ray here.
                    //pointsOfLine.Add(hit.point);
                    AddToPoints(hit.point);
                    hitEndBreaker = true;
                    return RayResult.HitStopper;
                }
            }
            return RayResult.HitNothing;

        }

        //Loop until we hit emergency max tries, we hit a wall or something breaks the loop
        for (int i = 0; i < maxTries && hitEndBreaker == false; i++)
        {
            //Process affectors first, letting them dictate the raycast
            if (affectors.Count > 0)
            {
                //Make a copy of the affectors list, since affectors might remove themselves from the list during this process
                List<ILazerAffector> currentAffectors = new List<ILazerAffector>(affectors);

                for (int j = 0; j < currentAffectors.Count; j++)
                {
                    PieceResult result = currentAffectors[j].AffectLazer(pointsOfLine[pointsOfLine.Count - 1], direction);

                    //Do a raycast based on the distance between where the lazer has reached and what the affector returned
                    //If nothing was hit, store the values from the affector.
                    float dist = Vector3.Distance(pointsOfLine[pointsOfLine.Count - 1], result.points[result.points.Count - 1]);

                    if (CastRay(dist) == RayResult.HitNothing)
                    {
                        //pointsOfLine.AddRange(result.points);
                        AddRangeToPoints(result.points);
                        SetDirection(result.direction);
                    }
                }
                //Remove affectors slated to be removed
                if (affectorsToRemove.Count > 0)
                {
                    for (int k = 0; k < affectorsToRemove.Count; k++)
                    {
                        affectors.Remove(affectorsToRemove[k]);
                    }
                    affectorsToRemove = new List<ILazerAffector>();
                }
            }
            else //If there are no affectors currently, raycast like normal with maxRayDistance
            {
                RayResult result = CastRay(maxRayDistance);
                if (result == RayResult.HitStopper)
                {
                    break;
                }
                else if (result == RayResult.HitNothing)
                {
                    //pointsOfLine.Add(pointsOfLine[pointsOfLine.Count - 1] + (direction * maxRayDistance));
                    AddToPoints(pointsOfLine[pointsOfLine.Count - 1] + (direction * maxRayDistance));
                    break;
                }
            }
        }
        //Set the lineRenderer array to the right size, then set the positions therein
        List<Vector3> points = new List<Vector3>(pointsOfLine);
        for (int i = 1; i < pointsOfLine.Count; i++)
        {
            if (Vector3.Distance(pointsOfLine[i - 1], pointsOfLine[i]) < minRegisteringDistance)
            {
                if (pointsOfLine[i - 1] != pointsOfLine[i])
                {
                    pointsOfLine.RemoveAt(i);

                }
            }
        }
        lineRenderer.positionCount = pointsOfLine.Count;
        lineRenderer.SetPositions(pointsOfLine.ToArray());
        debugPoints = pointsOfLine;


    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < debugPoints.Count; i++)
        {
            Gizmos.DrawSphere(debugPoints[i], debugSphereRadius);
        }
    }
    /// <summary>
    /// Adds a lazer affector to the list of affectors, letting it affect the lazer
    /// </summary>
    /// <param name="affector">The affector to add</param>
    public void AddAffector(ILazerAffector affector)
    {
        if (affectors.Contains(affector))
            return;

        affectors.Add(affector);
    }
    /// <summary>
    /// Remove a lazer affector from the list of affectors, ending its influence on the lazer
    /// </summary>
    /// <param name="affector">The affector to remove</param>
    public void RemoveAffector(ILazerAffector affector)
    {
        if (affectors.Contains(affector) == false)
            return;

        affectorsToRemove.Add(affector);
    }
}
