using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

  public GameObject fuego;
  public Transform robot;
  public Vector3 rotacion, distancia;
 
 void OnTriggerEnter(Collider other)
  {
    Vector3 dis = new Vector3(3, 0, 0);

    GameObject llamarada;
      llamarada = Instantiate(fuego, transform.position+distancia, Quaternion.Euler(rotacion)) as GameObject;
      llamarada.transform.parent = gameObject.transform;
    }
	}

