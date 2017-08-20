using UnityEngine;

public class GridManager : MonoBehaviour {
	/// <summary>
	/// The size of the render.
	/// </summary>
	public int renderSize;
	/// <summary>
	/// The size of the cell.
	/// </summary>
	public int cellSize;

	float xTotalSize;
	float yTotalSize;

	void Update() {
		if (Input.GetMouseButtonUp(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				Vector3 cellPos = WorldPointToCellPos(hit.point, 1);

				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.position = new Vector3(cellPos.x, 0, cellPos.y);
				cube.transform.localScale = Vector3.one * cellSize;
					
				Debug.Log(hit.point.x + ", " + hit.point.z);
				Debug.Log(cellPos.x + ", " + cellPos.y);
			}
		}
	}

	public Vector2 WorldPointToCellPos(Vector3 position, int sizeMultipiler) {
		int cSize = cellSize * sizeMultipiler;
		float xAxis = (position.x - transform.position.x) / cSize;
		float yAxis = (position.z - transform.position.z) / cSize;
		xAxis = Mathf.Ceil(xAxis) * cSize - cSize * 0.5f;
		yAxis = Mathf.Ceil(yAxis) * cSize - cSize * 0.5f;

		return new Vector2(xAxis + transform.position.x, yAxis + transform.position.z);
	}

	void OnDrawGizmos() {
		xTotalSize = renderSize * cellSize;
		yTotalSize = renderSize * cellSize;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			Vector3 cellPos = WorldPointToCellPos(hit.point, 1);

			Gizmos.DrawCube(new Vector3(cellPos.x, 0, cellPos.y), Vector3.one * cellSize);
		}


		Gizmos.color = Color.cyan;
		for (int i = 0; i < Mathf.Abs(renderSize); i++) {
			Gizmos.DrawLine(new Vector3(renderSize * cellSize, 0, -i * cellSize) + transform.position,
											new Vector3(-renderSize * cellSize, 0, -i * cellSize) + transform.position);
			Gizmos.DrawLine(new Vector3(renderSize * cellSize, 0, i * cellSize) + transform.position,
											new Vector3(-renderSize * cellSize, 0, i * cellSize) + transform.position);
		}
		Gizmos.color = Color.red;
		for (int i = 0; i < Mathf.Abs(renderSize); i++) {
			Gizmos.DrawLine(new Vector3(-i * cellSize, 0, renderSize * cellSize) + transform.position,
											new Vector3(-i * cellSize, 0, -renderSize * cellSize) + transform.position);
			Gizmos.DrawLine(new Vector3(i * cellSize, 0, renderSize * cellSize) + transform.position,
											new Vector3(i * cellSize, 0, -renderSize * cellSize) + transform.position);
		}
	}
}
