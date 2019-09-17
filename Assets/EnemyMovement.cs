using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Character
{
	public int positionsTillReturning = 2;
	private int curPosition;
	private Vector3 targetPos;

	public override void Start()
	{
		targetPos = transform.position;

		base.Start();
	}

	public override void Update()
	{
		var transPos = transform.position;
		if ((targetPos - transPos).magnitude < .5f)
		{
			if (curPosition < positionsTillReturning)
			{
				curPosition++;
				var targetPos2D = 24.5f * Random.insideUnitCircle;
				targetPos = new Vector3(targetPos2D.x, 0, targetPos2D.y);
			}
			else
			{
				curPosition = 0;
				targetPos = areaVertices[GetClosestAreaVertice(transPos)];
			}
		}

		curDir = (targetPos - transPos).normalized;

		base.Update();
	}
}