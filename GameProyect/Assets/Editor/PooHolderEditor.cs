using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(PoolHolder))]
public class PoolHolderEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
	}
}
