using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class GridObject : MonoBehaviour {
	public Vector2[] cells = new Vector2[1];
	public Vector3 basePosition;
	[SerializeField]
	bool calculateBase;

	void Awake() {
		if (calculateBase && GetComponent<MeshFilter>()) {
			CalculateBase();
		}
	}

	public void CalculateBase() {
		basePosition = UtilityBox.GetLowerVertex(transform, GetComponent<MeshFilter>().sharedMesh);
		basePosition = -basePosition + (transform.position.y - GetComponent<MeshRenderer>().bounds.center.y) * Vector3.up;
		Vector2 medianPoint = UtilityBox.GetMendianPoint(cells);
		basePosition.x = medianPoint.x;
		basePosition.z = medianPoint.y;
		transform.position = basePosition;
	}

}
