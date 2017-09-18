using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Spear : MonoBehaviour {
	public float lifeTime = 20;
	public float velocity = 5;
	public GameObject target;
	public Vector3 targetOffset;

	bool onAir = true;
	Rigidbody rigidBody;

	void Awake() {
		rigidBody = GetComponent<Rigidbody>();
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.GetComponent<Enemy>()) {
			Destroy(collision.gameObject);
		}
		onAir = false;
	}

	void FixedUpdate() {
		if (onAir && target) {
			transform.rotation = Quaternion.LookRotation(rigidBody.velocity);
			rigidBody.AddForce((target.transform.position + targetOffset - transform.position).normalized * Random.Range(velocity * 0.8f, velocity)); 
		}
		if (lifeTime <= 0) Destroy(gameObject);
		lifeTime -= Time.fixedDeltaTime;
	}
}
