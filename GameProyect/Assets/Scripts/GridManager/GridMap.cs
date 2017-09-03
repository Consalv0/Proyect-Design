using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class GridMap : MonoBehaviour, IGLDraw, IUserInteraction {
	public Material mat;
	/// <summary>
	/// The size of the render.
	/// </summary>
	[Range(0, 100)]
	public int renderSize = 100;
	/// <summary>
	/// The size of the cell.
	/// </summary>
	public float cellSize = 1;

	public bool userInteraction;
	public bool canInteract {
		get {
			return userInteraction;
		}
	}

	Dictionary<Vector2, GameObject> objectsIn = new Dictionary<Vector2, GameObject>();

	void Awake() {
		
	}

	public bool TryPlaceObject(ref GridObject gridObject, Vector3 worldPosition) {
		GameObject obj = gridObject.gameObject;
		return TryPLaceObject(ref obj, worldPosition, gridObject.cells, gridObject.basePosition);
	}
	public bool TryPlaceObject(ref GameObject obj, Vector3 worldPosition, Vector2[] objectCells) {
		Vector3 basePosition = UtilityBox.GetLowerVertex(obj.transform, obj.GetComponent<MeshFilter>().sharedMesh);
		if (obj.GetComponent<MeshRenderer>()) {
			basePosition -= (obj.transform.position.y - obj.GetComponent<MeshRenderer>().bounds.center.y) * Vector3.up;
		}
		Vector2 medianPoint = UtilityBox.GetMendianPoint(objectCells);
		basePosition.x = medianPoint.x;
		basePosition.z = medianPoint.y;
		return TryPLaceObject(ref obj, worldPosition, objectCells, basePosition);
	}
	public bool TryPLaceObject(ref GameObject obj, Vector3 worldPosition, Vector2[] objectCells, Vector3 basePosition) {
		Vector2 cellPos = WorldPointToCellPos(worldPosition);
		Vector2[] objGridCells = new Vector2[objectCells.Length];
		for (int i = 0; i < objectCells.Length; i++) {
			objGridCells[i] = objectCells[i] + cellPos;
			if (objectsIn.ContainsKey(objGridCells[i])) {
				return false;
			}
		}
		foreach (var cell in objGridCells) {
			objectsIn.Add(cell, obj);
		}
		Vector3 cellWorldPos = WorldPointToWorldCellPoint(worldPosition);

		obj.transform.position = new Vector3(basePosition.x * cellSize, basePosition.y, basePosition.z * cellSize) + cellWorldPos;
		return true;
	}

	public GameObject GetGameObjectByCellPos(Vector2 position) {
		if (objectsIn.ContainsKey(position)) {
			return objectsIn[position];
		} else {
			return null;
		}
	}

	public Vector3 WorldPointToWorldCellPoint(Vector3 position) {
		Vector3 pos = WorldPointToCellPos(position) * cellSize;
		pos = new Vector3(pos.x, 0, pos.y);
		// pos += transform.position;
		return pos;
	}
	public Vector2 WorldPointToCellPos(Vector3 position) {
		float xAxis = position.x - cellSize / 2;
		float yAxis = position.z - cellSize / 2;
		xAxis = Mathf.Ceil(xAxis / cellSize);
		yAxis = Mathf.Ceil(yAxis / cellSize);
		
		return new Vector2(xAxis, yAxis);
	}

	public void Interact(Vector3 position, params GameObject[] objectToPlace) {
		if (objectToPlace.Length == 0 || objectToPlace[0] == null || objectToPlace[0].GetComponent<GridObject>() == null) {
			return;
		}
		GridObject gridObject = objectToPlace[0].GetComponent<GridObject>();
		TryPlaceObject(ref gridObject, position);
	}

	void OnDrawGizmos() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			Vector2 cellPos = WorldPointToCellPos(hit.point);
			if (GetGameObjectByCellPos(cellPos)) {
				Gizmos.color = Color.red;
			} else {
				Gizmos.color = Color.green;
			}
			Vector3 worldCellPos = WorldPointToWorldCellPoint(hit.point);
			Gizmos.DrawCube(worldCellPos, Vector3.one);
		}
		GLDraw();
	}

	float x, y, z;
	public void GLDraw() {
		if (!mat) {
			var shader = Shader.Find("Sprites/Default");
			mat = new Material(shader);
		}

		GL.PushMatrix();
		mat.SetPass(0);
		GL.Begin(GL.LINES);

		int renderSizeM = Mathf.Abs(renderSize);
		renderSizeM += renderSizeM % 2 == 1 ? 0 : 1;

		float gridSize = renderSizeM * cellSize;
		float halfGridSize = renderSizeM * 0.5f;
		float halfCellSize = cellSize * 0.5f;

		y = 0.01f + transform.position.y;
		for (int i = 0; i < renderSizeM; i++) {
			for (int j = 1; j < renderSizeM; j++) {
				x = -halfGridSize + i + transform.position.x / cellSize;
				z = -halfGridSize + j + transform.position.z / cellSize;
				x = Mathf.Ceil(x) * cellSize - halfCellSize;
				z = Mathf.Ceil(z) * cellSize - halfCellSize;
				GL.Color(new Color(0, 1, 1, 0.5f));
				GL.Vertex3(						x, y, z);
				GL.Vertex3(x + cellSize, y, z);

				x = -halfGridSize + i + transform.position.z / cellSize;
				z = -halfGridSize + j + transform.position.x / cellSize;
				x = Mathf.Ceil(x) * cellSize - halfCellSize;
				z = Mathf.Ceil(z) * cellSize - halfCellSize;
				GL.Color(new Color(1, 0, 0, 0.5f));
				GL.Vertex3(z, y, x);
				GL.Vertex3(z, y, x + cellSize);
			}
		}
		GL.End();
		GL.PopMatrix();
	}
}
