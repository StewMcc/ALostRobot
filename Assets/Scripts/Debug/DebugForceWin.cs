using UnityEngine;

public class DebugForceWin : MonoBehaviour {

	[SerializeField]
	private LittleLot.DebugOverlay debugOverlay = null;

	public void ForceFakeWin() {
		debugOverlay.CloseDebugPanel();
		EventManager.ShipFixed();
	}
}
