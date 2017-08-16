using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(PoolHolder))]
public class PoolHolderEditor : Editor {
	//public override void OnInspectorGUI() {
	//	DrawDefaultInspector();
	//	// get the target script as TestScript and get the stack from it
	//	var script = (PoolHolder)target;
	//	var stack = script.poolObjects;

	//	// some styling for the header, this is optional
	//	var bold = new GUIStyle();
	//	bold.fontStyle = FontStyle.Bold;
	//	GUILayout.Label("Items in my stack", bold);

	//	// add a label for each item, you can add more properties
	//	// you can even access components inside each item and display them
	//	// for example if every item had a sprite we could easily show it 
	//	foreach (var item in stack) {
	//		GUILayout.Label(item.name);
	//	}
	//}
}
