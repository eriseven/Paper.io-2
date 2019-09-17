using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Character
{
	public float sensitivity = 300f;
	public float turnTreshold = 15f;
	private Vector3 mouseStartPos;

	public override void Update()
	{
		var mousePos = Input.mousePosition;
		if (Input.GetMouseButtonDown(0))
		{
			mouseStartPos = mousePos;
		}
		else if (Input.GetMouseButton(0))
		{
			float distance = (mousePos - mouseStartPos).magnitude;
			if (distance > turnTreshold)
			{
				if (distance > sensitivity)
				{
					mouseStartPos = mousePos - (curDir * sensitivity / 2f);
				}

				var curDir2D = -(mouseStartPos - mousePos).normalized;
				curDir = new Vector3(curDir2D.x, 0, curDir2D.y);
			}
		}
		else
		{
			curDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
		}

		base.Update();
	}

}