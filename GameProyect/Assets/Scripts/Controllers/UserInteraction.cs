using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Towers {
	Coconut,
	Spears,
	FireSpears
}

public class UserInteraction : MonoBehaviour {
	public Camera playerCamera;
	public string interactionInput = "Fire1";
	public float interactRadio = 4;
	public IPickable objectInHand = null;

	public GridObject selectedGridObject;
	public GameObject placeObject;

	[SerializeField] Image pointer;
	[SerializeField] Sprite passivePointer;
	[SerializeField] Sprite activePointer;
	[SerializeField] Sprite ocupiedPointer;
#if UNITY_EDITOR
	[ReadOnly]
#endif
	[SerializeField]
	float InteractTimer;
	Ray ray;
	RaycastHit hit;

	void Awake() {
		if (playerCamera == null) {
			playerCamera = Camera.main;
		}
	}

	void Update() {
		ray = playerCamera.ScreenPointToRay(Input.mousePosition);
		IUserInteraction hitInteraction = Physics.Raycast(ray, out hit, interactRadio) ? hit.transform.GetComponent<IUserInteraction>() : null;
		bool realizedHit = hitInteraction != null;

		if (pointer != null) {
			if (realizedHit) {
				pointer.sprite = objectInHand != null ? ocupiedPointer : hitInteraction.canInteract ? activePointer : passivePointer;
			} else {
				pointer.sprite = objectInHand != null ? ocupiedPointer : passivePointer;
			}
		}

		if (InteractTimer >= 0.6f && objectInHand == null) {
			if (realizedHit) {
				IPickable objectToPick = hit.transform.GetComponent<IPickable>();
				if (objectToPick != null && objectToPick.canInteract) {
					objectInHand = objectToPick;
					objectToPick.Pick(hit.point);
				}
			}
		}

		if (objectInHand != null) {
			if (Vector3.Distance(playerCamera.transform.position, objectInHand.transform.position) > interactRadio * 2) {
				objectInHand.Drop();
				InteractTimer = 0;
				objectInHand = null;
			} else {
				objectInHand.Pick(transform.position + playerCamera.transform.forward * interactRadio);
			}
		}

		if (Input.GetButtonDown(interactionInput)) {
			if (selectedGridObject) {
				placeObject = placeObject == null ? Instantiate(selectedGridObject.gameObject) : placeObject;
			} else if (realizedHit && hitInteraction.canInteract && hitInteraction.gameObject.GetComponent<GridObject>()) {
				hitInteraction.Interact(hit.point, gameObject);
			}
			if (objectInHand != null) {
				InteractTimer = 0;
				objectInHand.Drop();
				objectInHand = null;
			}
		}

		if (Input.GetButton(interactionInput) && objectInHand == null) {
			if (realizedHit && hitInteraction.canInteract && placeObject) {
				hitInteraction.Interact(hit.point, placeObject.gameObject, null);
			}
			InteractTimer += Time.deltaTime;
		}


		if (Input.GetButtonUp(interactionInput) && objectInHand == null) {
			InteractTimer = 0;
			if (realizedHit && InteractTimer < 0.6f) {
				if (hitInteraction.gameObject.GetComponent<ITrigger>() != null) {
					hitInteraction.gameObject.GetComponent<ITrigger>().delegates[0].Invoke();
				}
				if (hitInteraction.canInteract && placeObject) {
					hitInteraction.Interact(hit.point, placeObject.gameObject);
				}
			} else {
				Destroy(placeObject);
			}
		}
	}

	public void SelectTower(GameObject obj) {
		if (obj.GetComponent<GridObject>()) {
			selectedGridObject = obj.GetComponent<GridObject>();
		}
	}
}
