using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInteraction : MonoBehaviour {
	public Camera playerCamera;
	public string interactionInput = "Fire1";
	public float interactRadio = 4;
	public IPickable objectInHand = null;

	public GridObject placeObject;

	[SerializeField] Image pointer;
	[SerializeField] Sprite passivePointer;
	[SerializeField] Sprite activePointer;
	[SerializeField] Sprite ocupiedPointer;
#if UNITY_EDITOR
	[ReadOnly]
#endif
	[SerializeField]
	float InteractTimer;

	void Awake() {
		if (playerCamera == null) {
			playerCamera = Camera.main;
		}
	}

	void Update() {
		Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
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

		if (Input.GetButton(interactionInput) && objectInHand == null) {
			InteractTimer += Time.deltaTime;
		}

		if (Input.GetButtonDown(interactionInput)) {
			if (objectInHand != null) {
				InteractTimer = 0;
				objectInHand.Drop();
				objectInHand = null;
			}
		}

		if (Input.GetButtonUp(interactionInput) && objectInHand == null) {
			InteractTimer = 0;
			if (realizedHit && InteractTimer < 0.6f) {
				if (hitInteraction.canInteract && placeObject) {
					hitInteraction.Interact(hit.point, placeObject.gameObject);
				}
			}
		}
	}
}
