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

	float x, y, z;
	Dictionary<Vector2, GameObject> objectsIn = new Dictionary<Vector2, GameObject>();

	public void ForcePlaceObject(GridObject gridObject, Vector3 worldPosition, bool addToGrid, bool moveOnPLace) {
		Vector2 cellPos = WorldPointToCell(worldPosition);
		List<Vector2> objGridCells = new List<Vector2>();
		for (int i = 0; i < gridObject.cells.Count; i++) {
			if (objectsIn.ContainsKey(gridObject.cells[i] + cellPos)) {
				continue;
			}
			objGridCells.Add(gridObject.cells[i] + cellPos);
		}
		if (addToGrid) {
			foreach (var cell in objGridCells) {
				objectsIn.Add(cell, gridObject.gameObject);
			}
		}
		if (moveOnPLace) {
			gridObject.transform.position = WorldPointToWorldCellPoint(worldPosition) - gridObject.basePosition;
		}
	}
	public bool TryPlaceObject(GridObject gridObject, Vector3 worldPosition, bool addToGrid, bool moveOnPLace) {
		Vector2 cellPos = WorldPointToCell(worldPosition);
		Vector2[] objGridCells = new Vector2[gridObject.cells.Count];
		for (int i = 0; i < gridObject.cells.Count; i++) {
			objGridCells[i] = gridObject.cells[i] + cellPos;
			if (objectsIn.ContainsKey(objGridCells[i])) {
				return false;
			}
		}
		if (addToGrid) {
		foreach (var cell in objGridCells) {
			objectsIn.Add(cell, gridObject.gameObject);
			}
		}
		if (moveOnPLace) {
			gridObject.transform.position = WorldPointToWorldCellPoint(worldPosition) - gridObject.basePosition;
		}
		return true;
	}

	public GameObject GetGameObjectByCellPos(Vector2 position) {
		if (objectsIn.ContainsKey(position)) {
			return objectsIn[position];
		} else {
			return null;
		}
	}

	public Vector3 CellToWorldPoint(Vector2 cell) {
		x = cell.x + transform.position.x / cellSize;
		z = cell.y + transform.position.z / cellSize;
		x = Mathf.Ceil(x) * cellSize;
		z = Mathf.Ceil(z) * cellSize;
		return new Vector3(x, transform.position.y, z);
	}
	public Vector3 WorldPointToWorldCellPoint(Vector3 position) {
		Vector3 pos = WorldPointToCell(position) * cellSize;
		pos = new Vector3(pos.x, transform.position.y, pos.y);
		return pos;
	}
	public Vector2 WorldPointToCell(Vector3 position) {
		x = position.x - cellSize / 2;
		y = position.z - cellSize / 2;
		x = Mathf.Ceil(x / cellSize);
		y = Mathf.Ceil(y / cellSize);
		
		return new Vector2(x, y);
	}

	public void Interact(Vector3 position, params GameObject[] objectToPlace) {
		if (objectToPlace.Length == 0 || objectToPlace[0] == null || objectToPlace[0].GetComponent<GridObject>() == null) {
			return;
		}
		GridObject gridObject = objectToPlace[0].GetComponent<GridObject>();
		TryPlaceObject(gridObject, position, true, true);
	}

	void OnDrawGizmos() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			Vector2 cellPos = WorldPointToCell(hit.point);
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
