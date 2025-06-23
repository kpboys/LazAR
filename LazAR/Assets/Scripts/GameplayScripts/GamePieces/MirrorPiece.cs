using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorPiece : MonoBehaviour, ILazerInteractable
{
    //public int extraPoints = 4;
	 public PieceResult ProcessLazerHit(Vector3 collisionPoint, Vector3 lazerDirection)
	 {
        Vector3 reflectDirection = Vector3.Reflect(lazerDirection, transform.forward).normalized;

        //Adding more of the same point because apparently that helps the line render :/
        List<Vector3> points = new List<Vector3> { collisionPoint, collisionPoint, collisionPoint, collisionPoint };

        //List<Vector3> points = new List<Vector3>();
        //for (int i = 0; i < extraPoints; i++)
        //{
        //    points.Add(collisionPoint);
        //}

        return new PieceResult(reflectDirection, points);
	 }

	 // Start is called before the first frame update
	 void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
