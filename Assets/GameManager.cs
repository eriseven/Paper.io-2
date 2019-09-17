using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
	public static GameManager gm;
	public MainMenu mm;

	private void Awake()
	{
		if (!gm)
		{
			gm = this;
			DontDestroyOnLoad(this);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{			
			RestartScene();			
		}
	}

	public void RestartScene()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void GameOver()
	{
		Time.timeScale = 0;
		mm.GameOver();
	}

	public static void DeformCharacterArea(Character character, List<Vector3> newAreaVertices)
	{
		int newAreaVerticesCount = newAreaVertices.Count;
		if (newAreaVerticesCount > 0)
		{
			List<Vector3> areaVertices = character.areaVertices;
			int startPoint = character.GetClosestAreaVertice(newAreaVertices[0]);
			int endPoint = character.GetClosestAreaVertice(newAreaVertices[newAreaVerticesCount - 1]);

			// CLOCKWISE AREA
			// Select redundant vertices
			List<Vector3> redundantVertices = new List<Vector3>();
			for (int i = startPoint; i != endPoint; i++)
			{
				if (i == areaVertices.Count)
				{
					if (endPoint == 0)
					{
						break;
					}

					i = 0;
				}
				redundantVertices.Add(areaVertices[i]);
			}
			redundantVertices.Add(areaVertices[endPoint]);

			// Add new vertices to clockwise temp area
			List<Vector3> tempAreaClockwise = new List<Vector3>(areaVertices);
			for (int i = 0; i < newAreaVerticesCount; i++)
			{
				tempAreaClockwise.Insert(i + startPoint, newAreaVertices[i]);
			}

			// Remove the redundat vertices & calculate clockwise area's size
			tempAreaClockwise = tempAreaClockwise.Except(redundantVertices).ToList();
			float clockwiseArea = Mathf.Abs(tempAreaClockwise.Take(tempAreaClockwise.Count - 1).Select((p, i) => (tempAreaClockwise[i + 1].x - p.x) * (tempAreaClockwise[i + 1].z + p.z)).Sum() / 2f);

			// COUNTERCLOCKWISE AREA
			// Select redundant vertices
			redundantVertices.Clear();
			for (int i = startPoint; i != endPoint; i--)
			{
				if (i == -1)
				{
					if (endPoint == areaVertices.Count - 1)
					{
						break;
					}

					i = areaVertices.Count - 1;
				}
				redundantVertices.Add(areaVertices[i]);
			}
			redundantVertices.Add(areaVertices[endPoint]);

			// Add new vertices to clockwise temp area
			List<Vector3> tempAreaCounterclockwise = new List<Vector3>(areaVertices);
			for (int i = 0; i < newAreaVerticesCount; i++)
			{
				tempAreaCounterclockwise.Insert(startPoint, newAreaVertices[i]);
			}

			// Remove the redundant vertices & calculate counterclockwise area's size
			tempAreaCounterclockwise = tempAreaCounterclockwise.Except(redundantVertices).ToList();
			float counterclockwiseArea = Mathf.Abs(tempAreaCounterclockwise.Take(tempAreaCounterclockwise.Count - 1).Select((p, i) => (tempAreaCounterclockwise[i + 1].x - p.x) * (tempAreaCounterclockwise[i + 1].z + p.z)).Sum() / 2f);

			// Find the area with greatest size
			character.areaVertices = clockwiseArea > counterclockwiseArea ? tempAreaClockwise : tempAreaCounterclockwise;
		}

		character.UpdateArea();
	}

	// https://codereview.stackexchange.com/questions/108857/point-inside-polygon-check
	public static bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
	{
		int polygonLength = polygon.Length, i = 0;
		bool inside = false;
		float pointX = point.x, pointY = point.y;
		float startX, startY, endX, endY;
		Vector2 endPoint = polygon[polygonLength - 1];
		endX = endPoint.x;
		endY = endPoint.y;
		while (i < polygonLength)
		{
			startX = endX; startY = endY;
			endPoint = polygon[i++];
			endX = endPoint.x; endY = endPoint.y;
			inside ^= (endY > pointY ^ startY > pointY) && ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
		}
		return inside;
	}
}