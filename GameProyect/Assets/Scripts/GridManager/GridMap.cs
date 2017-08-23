using System;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour, IGLDraw {
	public Material mat;
	/// <summary>
	/// The size of the render.
	/// </summary>
	public int renderSize = 100;
	/// <summary>
	/// The size of the cell.
	/// </summary>
	public float cellSize = 1;
	[Range(1, 5)]
	public int objectSize = 1;

	Dictionary<Vector2, GameObject> objectsIn = new Dictionary<Vector2, GameObject>();

	void Awake() {
		transform.position = new Vector3(Mathf.Floor(transform.position.x),
		                                 transform.position.y,
		                                 Mathf.Floor(transform.position.z));
	}
	void Update() {
		if (Input.GetMouseButtonUp(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				Vector2 cellPos = WorldPointToCellPos(hit.point);

				Debug.Log(cellPos.x + ", " + cellPos.y);
				if (GetGameObjectByCellPos(cellPos) == null) {
					GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					objectsIn.Add(cellPos, cube);
					cellPos = WorldPointToWorldCellPoint(hit.point);
					cube.transform.position = new Vector3(cellPos.x, 0, cellPos.y) + transform.position;
					cube.transform.localScale = Vector3.one * objectSize;
				}
			}
		}
	}

	public GameObject GetGameObjectByCellPos(Vector2 position) {
		if (objectsIn.ContainsKey(position)) {
			return objectsIn[position];
		} else {
			return null;
		}
	}

	void OnDrawGizmosSelected() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			Vector2 cellPos = WorldPointToCellPos(hit.point);
			if (GetGameObjectByCellPos(cellPos)) {
				Gizmos.color = Color.red;
			} else {
				Gizmos.color = Color.green;
			}
			cellPos = WorldPointToWorldCellPoint(hit.point);
			Gizmos.DrawCube(new Vector3(cellPos.x, 0, cellPos.y) + transform.position,
			                Vector3.one * objectSize);
		}
		GLDraw();
	}

	public void GLDraw() {
		if (!mat) {
			var shader = Shader.Find("Sprites/Default");
			mat = new Material(shader);
		}

		GL.PushMatrix();
		mat.SetPass(0);
		GL.Begin(GL.LINES);
		GL.Color(Color.cyan);
		float gridSize = renderSize * cellSize;
		float halfGridSize = gridSize * 0.5f;
		for (int i = 0; i < Mathf.Abs(renderSize); i++) {
			for (int j = 1; j < Mathf.Abs(renderSize); j++) {
				GL.Vertex(new Vector3(-halfGridSize + (i * cellSize), 0, -halfGridSize + (j * cellSize)) + transform.position);
				GL.Vertex(new Vector3(-halfGridSize + (i * cellSize + cellSize), 0, -halfGridSize + (j * cellSize)) + transform.position);
			}
		}
		GL.Color(Color.red);
		for(int i = 0; i < Mathf.Abs(renderSize); i++) {
			for (int j = 1; j < Mathf.Abs(renderSize); j++) {
				GL.Vertex(new Vector3(-halfGridSize + (j * cellSize), 0 , -halfGridSize + (i * cellSize)) + transform.position);
				GL.Vertex(new Vector3(-halfGridSize + (j * cellSize) ,0, -halfGridSize + (i * cellSize + cellSize)) + transform.position);
			}
		}
		GL.End();
		GL.PopMatrix();
	}

	public Vector2 WorldPointToWorldCellPoint(Vector3 position) {
		Vector2 pos = WorldPointToCellPos(position) * cellSize;
		pos = new Vector2(pos.x + (1 - cellSize) * 0.5f, pos.y + (1 - cellSize) * 0.5f);
		return pos;
	}
	public Vector2 WorldPointToCellPos(Vector3 position) {
		float xAxis = (position.x - transform.position.x) - 0.5f;
		float yAxis = (position.z - transform.position.z) - 0.5f;
		xAxis = Mathf.Ceil(xAxis / cellSize);
		yAxis = Mathf.Ceil(yAxis / cellSize);
		
		return new Vector2(xAxis, yAxis);
	}
}
