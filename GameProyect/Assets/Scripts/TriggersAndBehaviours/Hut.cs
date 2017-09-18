using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Hut : MonoBehaviour, ITrigger {
	public float lifeTime = 20;
	public float interactTime = 30;
	public float startWait, spwanWait;
  public int count;
	public float speedMult = 1;
	public Vector3 offsetInstantiate;
	public GameObject power;
	public UnityEvent activate;
	public Image timer;

	[SerializeField]
	float ready;
	Enemy[] enemies;
	GameObject target;

	public UnityEvent[] delegates {
		get {
			var dg = new UnityEvent[1];
			dg[0] = activate;
			return dg;
		}
	}

	void Awake() {
		transform.Rotate(0, Random.Range(0, 2) * 90, 0);
		ready = 1;
		StartCoroutine(SumInteractTime());
	}

	void Update() {
		timer.fillAmount = ready;
		if (ready >= 1) timer.fillAmount = 0;
	}

	public void Shoot() {
		if (ready >= 1) {
			enemies = FindObjectsOfType<Enemy>();
			StartCoroutine(shoot(power));
			ready = 0;
		}
	}

	GameObject GetClosestEnemy(Enemy[] ens) {
		GameObject bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;
		foreach (Enemy potentialTarget in enemies) {
			if (potentialTarget) {
				Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
				float dSqrToTarget = directionToTarget.sqrMagnitude;
				if (dSqrToTarget < closestDistanceSqr) {
					closestDistanceSqr = dSqrToTarget;
					bestTarget = potentialTarget.gameObject;
				}
			}
		}

		return bestTarget;
	}

  IEnumerator shoot(GameObject poder) {
    yield return new WaitForSeconds(startWait);
    for (int i = 0; i < count; i++) {
			GameObject obj = Instantiate(poder, transform.position + offsetInstantiate, Quaternion.identity);
			var bestEnemy = GetClosestEnemy(enemies);
			target = bestEnemy ? bestEnemy : gameObject;
			obj.GetComponent<Rigidbody>().velocity = new Vector3(0, (transform.position - target.transform.position).magnitude * speedMult, 0);
			obj.GetComponent<Spear>().target = target;
      yield return new WaitForSeconds(spwanWait);
    }
	}

	IEnumerator SumInteractTime() {
		while (gameObject) {
			if (ready < 1) {
				yield return new WaitForSecondsRealtime(0.06f / interactTime);
				ready += 0.06f / interactTime;
			}
			yield return new WaitForFixedUpdate();
		}
	}
}
