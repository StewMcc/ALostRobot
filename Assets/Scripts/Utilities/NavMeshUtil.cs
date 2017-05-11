using UnityEngine;
using UnityEngine.AI;

public class NavMeshUtil : MonoBehaviour {

	/// <summary>
	/// Generates a random point on the navmesh within the radius around the centre.
	/// </summary>
	/// <param name="center"> Centre point to create sphere around. </param>
	/// <param name="radius"> Radius of Sphere. </param>
	/// <param name="result"> Resultant NavMesh Point found. Vector3.zero if not found. </param>
	/// <returns>Whether or not a navmesh point was found. </returns>
	public static bool RandomPointOnNavMesh(Vector3 center, float radius, out Vector3 result) {
		for (int i = 0; i < 30; i++) {
			Vector3 randomPoint = center + Random.insideUnitSphere * radius;
			NavMeshHit hit;
			if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
				result = hit.position;
				return true;
			}
		}
		result = Vector3.zero;
		return false;
	}
}

