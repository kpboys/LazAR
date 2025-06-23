using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineWidthScaler : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float baseWidth;
	 public Vector2 baseTextureScale;
	 public float manualMultiplier;
	 [Button(nameof(TestNewWidth_Btn))]
	 public bool manualBtn;
	 public void GiveNewWidth(float multiplier)
    {
		  if(baseWidth == 0)
		  {
				baseWidth = lineRenderer.widthMultiplier;
		  }
		  if (baseTextureScale == Vector2.zero)
		  {
				baseTextureScale = lineRenderer.textureScale;
		  }
		  lineRenderer.widthMultiplier = baseWidth * multiplier;
		  lineRenderer.textureScale = new Vector2(baseTextureScale.x / multiplier, baseTextureScale.y);
	 }
	 public void TestNewWidth_Btn()
	 {
		  GiveNewWidth(manualMultiplier);
	 }
}
