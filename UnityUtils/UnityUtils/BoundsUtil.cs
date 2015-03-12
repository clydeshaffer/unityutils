using UnityEngine;

public class BoundsUtil {
	
	public static Vector3[] Corners(Bounds b) {
		Vector3[] corners = new Vector3[8];
		
		for(int i = 0; i < 8; i++) {
			corners[i] = b.extents;
			if((i & 1) != 0) corners[i].x *= -1;
			if((i & 2) != 0) corners[i].y *= -1;
			if((i & 4) != 0) corners[i].z *= -1;
		}
		
		return corners;
	}
}
