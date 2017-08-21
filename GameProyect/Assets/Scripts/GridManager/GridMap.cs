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
			// Unity has a built-in shader that is useful for drawing
			// simple colored things. In this case, we just want to use
			// a blend mode that inverts destination colors.
			var shader = Shader.Find("Hidden/Internal-Colored");
			mat = new Material(shader);
			mat.hideFlags = HideFlags.HideAndDontSave;
			// Set blend mode to invert destination colors.
			mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
			mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
			// Turn off backface culling, depth writes, depth test.
			mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
			mat.SetInt("_ZWrite", 0);
			mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
		}

		GL.PushMatrix();
		mat.SetPass(0);
		GL.Begin(GL.LINES);
		GL.Color(Color.cyan);
		for (int i = 0; i < Mathf.Abs(renderSize); i++) {
			GL.Vertex(new Vector3(renderSize * cellSize + 0.5f, 0.5f, -i * cellSize + 0.5f) + transform.position);
			GL.Vertex(new Vector3(-renderSize * cellSize + 0.5f, 0.5f, -i * cellSize + 0.5f) + transform.position);
			GL.Vertex(new Vector3(renderSize * cellSize + 0.5f, 0.5f, i * cellSize + 0.5f) + transform.position);
			GL.Vertex(new Vector3(-renderSize * cellSize + 0.5f, 0.5f, i * cellSize + 0.5f) + transform.position);
		}

		GL.Color(Color.red);
		for (int i = 0; i < Mathf.Abs(renderSize); i++) {
			GL.Vertex(new Vector3(-i * cellSize + 0.5f, 0.5f, renderSize * cellSize + 0.5f) + transform.position);
			GL.Vertex(new Vector3(-i * cellSize + 0.5f, 0.5f, -renderSize * cellSize + 0.5f) + transform.position);
			GL.Vertex(new Vector3(i * cellSize + 0.5f, 0.5f, renderSize * cellSize + 0.5f) + transform.position);
			GL.Vertex(new Vector3(i * cellSize + 0.5f, 0.5f, -renderSize * cellSize + 0.5f) + transform.position);
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
