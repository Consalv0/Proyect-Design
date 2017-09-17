using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torreta : MonoBehaviour, IUserInteraction {
	public bool canInteract {
		get {
			throw new NotImplementedException();
		}
	}

	public void Interact(Vector3 interactPosition, params GameObject[] objects) {
		throw new NotImplementedException();
	}
}
