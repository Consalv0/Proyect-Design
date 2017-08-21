using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
