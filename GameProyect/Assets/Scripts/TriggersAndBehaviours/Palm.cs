using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Palm : MonoBehaviour, ITrigger {
	public float interactTime = 10;
	public GameObject coconut;
	public Vector3 offsetInstantiate;
	public UnityEvent activate;
	public Image timer;

	[SerializeField]
	float ready;

	void Awake() {
		transform.Rotate(0, Random.Range(0, 2) * 90, 0);
		offsetInstantiate = transform.rotation * offsetInstantiate;
		ready = 1;
		StartCoroutine(SumInteractTime());
	}

	void Update() {
		timer.fillAmount = ready;
		if (ready >= 1) timer.fillAmount = 0;
	}

	public void DropCoconut() {
		if (ready >= 1) {
			var obj = Instantiate(coconut);
			obj.transform.position = transform.position + offsetInstantiate;
			ready = 0;
		}
	}

	IEnumerator SumInteractTime() {
		while(gameObject) {
			if (ready < 1) {
				yield return new WaitForSecondsRealtime(0.06f / interactTime);
				ready += 0.06f / interactTime;
			}
			yield return new WaitForFixedUpdate();
		}
	}

	public UnityEvent[] delegates {
		get {
			var dg = new UnityEvent[1];
			dg[0] = activate;
			return dg;
		}
	}
}
