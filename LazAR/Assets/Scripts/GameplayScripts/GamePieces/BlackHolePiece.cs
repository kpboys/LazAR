using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BlackHolePiece : MonoBehaviour, ILazerAffector
{
	 [Header("Values")]
	 [SerializeField]
	 private float pullPower;
	 [SerializeField]
	 private float lazerSpeed;

	 [Header("References")]
	 [SerializeField]
	 private Transform blackHoleObject;
	 [SerializeField]
	 private DefineBorders borderSystem;

	 [Header("Debug")]
	 [SerializeField]
	 private int maxTries;
	 [SerializeField]
	 [Tooltip("The Pull Power and Lazer Speed are divided by this value to make them easier to work with in inspector")]
	 private int valueScaling;

	 private SphereCollider sphCollider;
	 private LazerDrawer drawer;

	 /// <summary>
	 /// Use the radius of the attached sphere collider as the range value
	 /// </summary>
    public float Range
    {
        get 
        { 
            if(sphCollider == null)
            {
                sphCollider = GetComponent<SphereCollider>();
            }
				if(borderSystem.WorldScale == 0)
					 return sphCollider.radius;
				return sphCollider.radius * borderSystem.WorldScale;

		  }
        set 
        {
				if (sphCollider == null)
				{
					 sphCollider = GetComponent<SphereCollider>();
				}
				sphCollider.radius = value;

		  }
    }

	 public float PullPower 
	 { 
		  get
		  {
				if(borderSystem.WorldScale == 0)
					 return pullPower;
				return pullPower * borderSystem.WorldScale;
		  }
		  set => pullPower = value; 
	 }

	 public float LazerSpeed 
	 { 
		  get
		  {
				if (borderSystem.WorldScale == 0)
					 return lazerSpeed;
				return lazerSpeed * borderSystem.WorldScale;
		  }
		  set => lazerSpeed = value; 
	 }

	 public void ProcessLazerHit(LazerDrawer drawer)
	 {
		  //Add this affector to the LazerDrawer and store the drawer for later
		  drawer.AddAffector(this);
		  this.drawer = drawer;
	 }
	 public PieceResult AffectLazer(Vector3 collisionPoint, Vector3 lazerDirection)
	 {
		  Vector3 posCurrent = collisionPoint;
		  Vector3 dirCurrent = lazerDirection.normalized;

		  //Divide these values by a scaling number, making them easier to understand in inspector
		  float pullPowerScaled = PullPower / valueScaling;
		  float lazerSpeedScaled = LazerSpeed / valueScaling;

		  //Scale pull power based on how close the ray is to the black hole
		  float adjustedPullPower = pullPowerScaled * (1 - Vector3.Distance(posCurrent, blackHoleObject.position) / Range);

		  //Calculate the vectors for the hole's gravity and the lazer's movement, then combine them.
		  Vector3 holePull = (blackHoleObject.position - posCurrent).normalized * adjustedPullPower;
		  Vector3 lazerVelocity = dirCurrent * lazerSpeedScaled;
		  Vector3 totalMovement = holePull + lazerVelocity;

		  posCurrent = posCurrent + totalMovement;
		  dirCurrent = totalMovement.normalized;

		  //If this new movement has put the lazer outside the hole's reach, remove it from the LazerDrawer
		  if (Vector3.Distance(posCurrent, blackHoleObject.position) > Range)
		  {
				drawer.RemoveAffector(this);
		  }

		  return new PieceResult(dirCurrent, new List<Vector3>{ posCurrent });
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
