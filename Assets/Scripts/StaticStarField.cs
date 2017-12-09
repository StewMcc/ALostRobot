using System.Collections;
using UnityEngine;


public class StaticStarField : MonoBehaviour {

	[SerializeField]
	private ParticleSystem starFieldParticles;

	[SerializeField]
	int numberToEmit = 1000;
	
	IEnumerator Start() {
		starFieldParticles.Emit(numberToEmit);
		yield return new WaitForEndOfFrame();
		starFieldParticles.Pause();
	}
}
