using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityScript.Lang;

public static class UtilityBox {
	public static int[] ToArray(this Vector2 vector) {
		int[] value = new int[2];
		value[0] = (int)vector.x;
		value[1] = (int)vector.y;
		return value;
	}
	public static int[] ToArray(this Vector3 vector) {
		int[] value = new int[3];
		value[0] = (int)vector.x;
		value[1] = (int)vector.y;
		value[2] = (int)vector.z;
		return value;
	}
	public static int[] ToArray(this Vector4 vector) {
		int[] value = new int[4];
		value[0] = (int)vector.x;
		value[1] = (int)vector.y;
		value[2] = (int)vector.z;
		value[3] = (int)vector.w;
		return value;
	}

	public static Vector2 GetMendianPoint(Vector2[] points) {
		float[] x = new float[points.Length];
		float[] y = new float[points.Length];
		for (int i = 0; i < points.Length; i++) {
			x[i] = points[i].x;
			y[i] = points[i].y;
		}

		x = x.OrderBy(a => a).ToArray();
		y = y.OrderBy(a => a).ToArray();

		return new Vector2((x[0] + x.Last()) / 2, (y[0] + y.Last()) / 2);
	}
	public static Vector3 GetMendianPoint(Vector3[] points) {
		float[] x = new float[points.Length];
		float[] y = new float[points.Length];
		float[] z = new float[points.Length];
		for (int i = 0; i < points.Length; i++) {
			x[i] = points[i].x;
			y[i] = points[i].y;
			z[i] = points[i].z;
		}

		x = x.OrderBy(a => a).ToArray();
		y = y.OrderBy(a => a).ToArray();
		z = y.OrderBy(a => a).ToArray();

		return new Vector3((x[0] + x.Last()) / 2, (y[0] + y.Last()) / 2, (z[0] + z.Last()) / 2);
	}

	public static Vector3 GetLowerVertex(Mesh mesh) {
		return GetLowerVertex(new GameObject().transform, mesh);
	}
	public static Vector3 GetLowerVertex(Transform transform, Mesh mesh) {
		float minValue = Mathf.Infinity;
		Vector3 lower = mesh.vertices[0];
		Vector3 initialPos = transform.position;

		for (int i = 0; i < mesh.vertexCount; i++) {
			Vector3 v = mesh.vertices[i];

			transform.position = Vector3.zero;
			Vector3 transformedv = transform.TransformPoint(v);

			if (transformedv.y < minValue) {
				minValue = transformedv.y;
				lower = transformedv;
			}
		}

		transform.position = initialPos;
		return lower;
	}
}
