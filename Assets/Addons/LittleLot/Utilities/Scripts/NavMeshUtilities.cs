using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// @StewMcc 10/10/2017
/// </summary>
namespace LittleLot {

	public class NavMeshUtilities : MonoBehaviour {

		/// <summary>
		/// Generates a random point on the navmesh within the radius around the centre.
		/// </summary>
		/// <param name="center"> Centre point to create sphere around. </param>
		/// <param name="radius"> Radius of Sphere. </param>
		/// <param name="result"> Resultant NavMesh Point found. Vector3.zero if not found. </param>
		/// <param name="maxAttempts"> Maximum number of times to try and find a point default value 30. </param>
		/// <returns> Whether or not a navmesh point was found. If false result is Vector3.zero (bad idea to use this) </returns>
		public static bool RandomPointOnNavMesh(Vector3 center, float radius, out Vector3 result, int maxAttempts = 30) {
			for (int i = 0; i < maxAttempts; i++) {
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
}
