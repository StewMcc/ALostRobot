/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuttleRooms : SoundManager {

    private float distance;
    public int MAXDIST = 100;
    private bool isFixed;
    private Vector3 roomPosition;
    private Vector3 playerPosition;
    private Room roomActive;


    private void Awake()
    {
        roomActive = GetComponent<Room>();
        isFixed = roomActive.IsFixed;
        roomPosition = GameObject.Find("PowerRoom").transform.localPosition;
    }

    private void Update()
    {
        if (isFixed != roomActive.isFixed)
        {
            isFixed = roomActive.IsFixed;
        }
        //if the room is activated (fixed)
        if (isFixed == true)
        {
            playerPosition = GameObject.FindGameObjectWithTag("PowerRoom").transform.localPosition;
            Debug.Log("player position is; " + playerPosition.x + playerPosition.y + playerPosition.z);
            // call player distance from the room calculator
            distance = DistanceCalc (playerPosition, roomPosition);
        }
        // distance handler: puts it in the correct range if it is out
        if (distance > MAXDIST)
        {
            distance = 100;
        }
        else
        {
            distance = distance * 100 / MAXDIST;
        }
        // call function for RTPC "Distance_Player_Room" to Wwise
        SoundManager.SetRTPC("Distance_Player_Room", distance);
    }
    private float DistanceCalc(Vector3 playerCoordinates, Vector3 roomCoordinates)
    {
        float result;
        Vector3 delta;
        delta = roomCoordinates - playerCoordinates;
        result = delta.magnitude;
        Debug.Log("distance is: " + result);
        return result;
    }
}
// http://www.lorenzo-salvadori.com/category/audio-programming/
*/