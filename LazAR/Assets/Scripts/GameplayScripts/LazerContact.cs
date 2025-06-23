using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerContact : MonoBehaviour, ILazerInteractable
{
    [InspectorReadonly]
    public bool powered;

	 [SerializeField]
	 private Color offColor;
	 [SerializeField]
	 private Color onColor;
	 [SerializeField]
	 private Renderer rend;

	 private bool poweredLastFrame;

	 private MaterialPropertyBlock propBlock;

	 public PieceResult ProcessLazerHit(Vector3 collisionPoint, Vector3 lazerDirection)
	 {
		  powered = true;
		  poweredLastFrame = true;
		  return new PieceResult(lazerDirection, new List<Vector3> { collisionPoint });
	 }
	 // Start is called before the first frame update
	 void Start()
    {
		  powered = false;
		  poweredLastFrame = false;
		  propBlock = new MaterialPropertyBlock();
		  propBlock.SetColor("_BaseColor", offColor);
		  rend.SetPropertyBlock(propBlock);
    }
	 private void LateUpdate()
	 {
		  if (powered)
		  {
				if(propBlock.GetColor("_BaseColor") == offColor)
				{
					 propBlock.SetColor("_BaseColor", onColor);
					 rend.SetPropertyBlock(propBlock);
				}
				if (poweredLastFrame)
				{
					 poweredLastFrame = false;
				}
				else
				{
					 powered = false;
				}
		  }
		  else
		  {
				if (propBlock.GetColor("_BaseColor") == onColor)
				{
					 propBlock.SetColor("_BaseColor", offColor);
					 rend.SetPropertyBlock(propBlock);
				}
		  }

	 }

    // Update is called once per frame
    void Update()
    {
        
    }

	 
}
