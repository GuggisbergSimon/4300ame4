using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
	[SerializeField] private float zPosition = 10.0f;
	[SerializeField] private GameObject planePrefab = null;
	private const float PLANE_WIDTH = 10.0f;

	private void Awake()
	{
		EdgeCollider2D myEdgeCollider = GetComponent<EdgeCollider2D>();

		for (int i = 0; i < myEdgeCollider.pointCount; i++)
		{
			Vector2[] points = myEdgeCollider.points;
			Vector2 currentPoint = points[i];
			Vector3 diff = points[(i + 1) % myEdgeCollider.pointCount] - currentPoint;
			Vector3 pos = transform.position + (Vector3) currentPoint + diff / 2 + Vector3.forward * zPosition;
			float angle = Vector3.SignedAngle(diff, Vector3.right, -Vector3.forward);
			GameObject plane = Instantiate(planePrefab, pos, Quaternion.Euler(0, 0, angle), transform);
			plane.transform.localScale =
				Vector3.right * diff.magnitude / PLANE_WIDTH + Vector3.forward * plane.transform.localScale.z;
		}
	}
}