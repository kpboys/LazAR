using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Struct for returning the direction and position caluclated by a piece
/// </summary>
public struct PieceResult
{
    public Vector3 direction;
    public List<Vector3> points;
    public PieceResult(Vector3 direction, List<Vector3> points)
    {
        this.direction = direction;
        this.points = points;
    }
}
/// <summary>
/// Pieces that the lazer hits once
/// </summary>
public interface ILazerInteractable
{
    public abstract PieceResult ProcessLazerHit(Vector3 collisionPoint, Vector3 lazerDirection);
}
/// <summary>
/// Pieces that continuously affect the lazer
/// </summary>
public interface ILazerAffector
{
    public abstract void ProcessLazerHit(LazerDrawer drawer);

	 public abstract PieceResult AffectLazer(Vector3 collisionPoint, Vector3 lazerDirection);
}
