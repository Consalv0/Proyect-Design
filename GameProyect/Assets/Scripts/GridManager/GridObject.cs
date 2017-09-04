using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MeshRenderer))]
public class GridObject : MonoBehaviour {
	public GridMap gridMap;
	[ReadOnly]
	public Vector2 cellPosition;
	[HideInInspector][SerializeField]
	bool placeOnAwake = true;
	[HideInInspector]
	public bool moveOnPlace = true;
	[HideInInspector]
	public List<Vector2> cells = new List<Vector2>(1);
	[HideInInspector]
	public Vector3 basePosition;

	void Awake() {
		if (placeOnAwake && gridMap != null) {
			gridMap.ForcePlaceObject(this, transform.position + basePosition, true, moveOnPlace);
		}
	}

	public bool PlaceObjectInGrid(GridMap grid, bool addToGrid, bool moveOnPlace) {
		if (!grid.TryPlaceObject(this, transform.position + basePosition, addToGrid, moveOnPlace)) {
			return false;
		}
		gridMap = grid;
		cellPosition = grid.WorldPointToCell(transform.position + basePosition);
		return true;
	}

	public void CalculateBase() {
		basePosition = UtilityBox.GetLowerVertex(transform, GetComponent<MeshFilter>().sharedMesh);
		basePosition = basePosition - (transform.position.y - GetComponent<MeshRenderer>().bounds.center.y) * Vector3.up;
		Vector2 medianPoint = UtilityBox.GetMendianPoint(cells.ToArray());
		basePosition.x = -medianPoint.x;
		basePosition.z = -medianPoint.y;
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(basePosition + transform.position, 0.2f);
		if (gridMap) {
			Gizmos.color = Color.green;
			foreach (var cell in cells) {
				Gizmos.DrawCube(new Vector3(cell.x, 0, cell.y) * gridMap.cellSize + basePosition + transform.position,
				                new Vector3(gridMap.cellSize, 0.1f, gridMap.cellSize));
			}
		}
	}
}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(GridObject))]
public class GridObjectEditor : Editor {
	ReorderableList prefabList;
	GridObject script;
	SerializedProperty placeOnAwake;

	void OnEnable() {
		script = (GridObject)target;
		placeOnAwake = serializedObject.FindProperty("placeOnAwake");
		prefabList = new ReorderableList(serializedObject, serializedObject.FindProperty("cells"), true, true, true, true) {
			drawHeaderCallback = (Rect rect) => {
				EditorGUI.LabelField(rect, "Internal Cell Positions");
			}
		};
		prefabList.drawElementCallback =
		(Rect rect, int index, bool isActive, bool isFocused) => {
			var element = prefabList.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 2;
			EditorGUI.LabelField(new Rect(rect.width * 0.13f, rect.y,
																		rect.width * 0.1f, EditorGUIUtility.singleLineHeight), index.ToString() + ":");
			EditorGUI.LabelField(new Rect(rect.width * 0.15f + 15, rect.y,
																		rect.width * 0.14f, EditorGUIUtility.singleLineHeight), "X");
			EditorGUI.PropertyField(new Rect(rect.width * 0.2f + 15,rect.y,
			                                 rect.width * 0.4f, EditorGUIUtility.singleLineHeight),
					element.FindPropertyRelative("x"), GUIContent.none);
			element.FindPropertyRelative("x").floatValue = Mathf.Round(element.FindPropertyRelative("x").floatValue);
			EditorGUI.LabelField(new Rect(rect.width * 0.61f + 15, rect.y,
																		rect.width * 0.14f, EditorGUIUtility.singleLineHeight), "Y");
			EditorGUI.PropertyField(new Rect(rect.width * 0.66f + 15, rect.y,
			                                 rect.width * 0.41f, EditorGUIUtility.singleLineHeight),
					element.FindPropertyRelative("y"), GUIContent.none);
			element.FindPropertyRelative("y").floatValue = Mathf.Round(element.FindPropertyRelative("y").floatValue);
		};
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		EditorGUI.BeginDisabledGroup(script.gridMap == null);
		placeOnAwake.boolValue = EditorGUILayout.Toggle("Place On Awake", placeOnAwake.boolValue);
		EditorGUI.EndDisabledGroup();
		script.moveOnPlace = EditorGUILayout.Toggle("Move On Place", script.moveOnPlace);
		prefabList.DoLayoutList();
		script.basePosition = EditorGUILayout.Vector3Field("Base Position", script.basePosition);

		EditorGUILayout.Space();
		if (GUILayout.Button("Calculate Base")) {
			Undo.RecordObject(script, "calculateBase");
			script.CalculateBase();
		}
		EditorGUI.BeginDisabledGroup(script.gridMap == null);
		if (GUILayout.Button("Simulate Place In Grid")) {
			Undo.RecordObject(script, "placeInGrid");
			if (script.gridMap) {
				script.PlaceObjectInGrid(script.gridMap, false, true);
			} else {
				Debug.Log("You need to indicate the GridMap first", script);
			}
		}
		EditorGUI.EndDisabledGroup();
		serializedObject.ApplyModifiedProperties();
	}
}
#endif

