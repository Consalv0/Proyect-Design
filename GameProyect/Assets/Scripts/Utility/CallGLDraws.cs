using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallGLDraws : MonoBehaviour {
	public List<GameObject> checkForGLDraw = new List<GameObject>();
	readonly List<IGLDraw> GLDraws = new List<IGLDraw>();

	public void UpdateBroadcastCalls() {
		GLDraws.Clear();
		for (int i = 0; i < checkForGLDraw.Count; i++) {
			if (checkForGLDraw[i] == null) {
				checkForGLDraw.RemoveAt(i);
				UpdateBroadcastCalls();
				return;
			} else if (checkForGLDraw[i].GetComponent<IGLDraw>() == null) {
				checkForGLDraw.RemoveAt(i);
				UpdateBroadcastCalls();
				return;
			}
			for (int j = 0; j < checkForGLDraw.Count; j++) {
				if (i == j) {
				} else if (ReferenceEquals(checkForGLDraw[i], checkForGLDraw[j])) {
					checkForGLDraw.RemoveAt(j);
					UpdateBroadcastCalls();
					return;
				}
			}
		}
		foreach (var go in checkForGLDraw) {
			if (go.GetComponent<IGLDraw>() != null) {
				GLDraws.Add(go.GetComponent<IGLDraw>());
			}
		}
	}

	void Awake() {
		UpdateBroadcastCalls();
	}

	void OnPostRender() {
		foreach (var GLs in GLDraws) {
			GLs.GLDraw();
		}
	}
}
