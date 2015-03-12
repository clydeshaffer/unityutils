using System;
using UnityEngine;

namespace UnityUtils
{
	public class VolumeCalculator
	{
		public static double SignedVolumeOfTriangleD(Vector3 p1, Vector3 p2, Vector3 p3)
		{
			double v321 = p3.x * p2.y * p1.z;
			double v231 = p2.x * p3.y * p1.z;
			double v312 = p3.x * p1.y * p2.z;
			double v132 = p1.x * p3.y * p2.z;
			double v213 = p2.x * p1.y * p3.z;
			double v123 = p1.x * p2.y * p3.z;
			
			return (1.0d / 6.0d) * (-v321 + v231 + v312 - v132 - v213 + v123);
		}
		
		public static double VolumeOfMeshD(Mesh mesh)
		{
			double volume = 0;
			
			Vector3[] vertices = mesh.vertices;
			int[] triangles = mesh.triangles;
			
			for (int i = 0; i < mesh.triangles.Length; i += 3)
			{
				Vector3 p1 = vertices[triangles[i + 0]];
				Vector3 p2 = vertices[triangles[i + 1]];
				Vector3 p3 = vertices[triangles[i + 2]];
				volume += SignedVolumeOfTriangleD(p1, p2, p3);
			}
			return Math.Abs(volume);
		}
		
		public static double VolumeOfSizeD(Vector3 size) {
			return size.x * size.y * size.z;
		}
	
		private const double fourThirds = 4.0d / 3.0d;
	
		public static double ScaleFactorD(Transform t) {
			if(t) return ScaleFactorD(t.parent) * t.localScale.x * t.localScale.y * t.localScale.z;
			else return 1;
		}
	
		public static double GetVolumeD(SphereCollider c) {
			return ScaleFactorD(c.transform) * Math.Pow(c.radius, 3.0f) * Mathf.PI * fourThirds;
		}
		
		public static double GetVolumeD(CapsuleCollider c) {
			return ScaleFactorD(c.transform) * Math.PI * Math.Pow(c.radius, 2.0f) * ((fourThirds - 2.0f) * c.radius + c.height);
		}
		
		public static double GetVolumeD(BoxCollider c) {
			return ScaleFactorD(c.transform) * VolumeOfSizeD(c.size);
		}
		
		public static double GetVolumeD(MeshCollider c) {
			if(c.convex) {
				return ScaleFactorD(c.transform) * VolumeOfMeshD(c.sharedMesh);
			} else {
				return VolumeOfSizeD(c.bounds.size);
			}
		}
		
		public static double GetVolumeD(Collider c) {
			if(c is MeshCollider) return GetVolumeD(c as MeshCollider);
			else if(c is SphereCollider) return GetVolumeD(c as SphereCollider);
			else if(c is CapsuleCollider) return GetVolumeD(c as CapsuleCollider);
			else if(c is BoxCollider) return GetVolumeD(c as BoxCollider);
			else {
				Debug.LogWarning("Can't get volume for a " + c.GetType().ToString() + ", returning zero.");
				return 0;
			}
		}
		
		public static double TotalVolumeD(Rigidbody rigidbody) {
			Collider[] allColliders = rigidbody.GetComponentsInChildren<Collider>();
			double volume = 0;
			foreach(Collider collider in allColliders) {
				volume += GetVolumeD(collider);
			}
			return volume;
		}
	}
}

