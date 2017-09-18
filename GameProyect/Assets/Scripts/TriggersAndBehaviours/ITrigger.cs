using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ITrigger {
	UnityEvent[] delegates { get; }

	string name { get; }
	Transform transform { get; }
	GameObject gameObject { get; }
}
 