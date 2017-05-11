
[System.Serializable]
public class ItemResponse {
	public int[] roomsToBreak;

	public void Initialise(int numberOfRoomsToBreak) {
		roomsToBreak = new int[numberOfRoomsToBreak];
	}
}
