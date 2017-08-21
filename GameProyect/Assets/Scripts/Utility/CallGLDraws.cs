using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallGLDraws : MonoBehaviour {
	public List<Component> components = new List<Component>();
	public List<IGLDraw> GLDraws = new List<IGLDraw>();

	void Awake() {
		foreach (var component in components) {
			if (component.GetComponent<IGLDraw>() != null) {
				GLDraws.Add(component.GetComponent<IGLDraw>());
			}
		}
	}

	void OnPostRender() {
		foreach (var GLs in GLDraws) {
			GLs.GLDraw();
		}
	}
}
