using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pato : Entity, IPoolObject {
	void OnCollisionEnter(Collision collision) {
		Recycle();
	}
}
