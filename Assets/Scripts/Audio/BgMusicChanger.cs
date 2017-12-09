using UnityEngine;



/// <summary>
/// Stops all events and starts new background music.
/// </summary>
public class BgMusicChanger : MonoBehaviour {
    enum TriggerEvent {
        OnDestroy,
        Start,
        Awake,
        None
    }
	[SerializeField]
	private string backgroundMusic  = "Menu_Music";

    [SerializeField]
    private TriggerEvent triggerEvent = TriggerEvent.None;

    private void Awake() {
        if(triggerEvent == TriggerEvent.Awake) {
            ChangeMusic();
        }
    }
    private void Start() {
        if (triggerEvent == TriggerEvent.Start) {
            ChangeMusic();
        }
    }
    private void OnDestroy() {
        if (triggerEvent == TriggerEvent.OnDestroy) {
            ChangeMusic();
        }
    }

    /// <summary>
    /// Stops all the currently running events before changing the background music.
    /// </summary>
    public void ChangeMusic() {
		SoundManager.StopAllEvents();
		SoundManager.PlayEvent(backgroundMusic, gameObject);

	}
}
