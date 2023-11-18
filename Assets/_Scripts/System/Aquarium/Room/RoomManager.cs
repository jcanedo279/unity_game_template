using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct RoomData {
    public int roomId;
    public int floorIndex; // Index of the floor the room is on.
    public int roomIndex; // Index of the room in its floor (LTR).
}
public class RoomObject {
    public GameObject roomRuntime;
    public RoomData roomData;
}






[System.Serializable]
public class RoomManagerData {
    public Vector2 baseRoomSize = new Vector2(10,10);
    public Dictionary<int,RoomObject> roomIdToRoom;

    [SerializeField] private GameObject _rootObject;
    public GameObject rootObject {
        get { return _rootObject; }
    }

    [SerializeField] private GameObject _roomPrefab;
    public GameObject roomPrefab {
        get { return _roomPrefab; }
    }
}

public class RoomManager : MonoBehaviour {
    [SerializeField] RoomManagerData roomManagerData;

    void Awake() {
        FillData();
    }
    void Start() {
        RenderRooms();
    }

    // Eventually this needs to load from user data/disk.
    public void FillData() {
        roomManagerData.roomIdToRoom = new Dictionary<int,RoomObject> {
            {0,new RoomObject{
                roomData = new RoomData{ roomId=0, floorIndex=0, roomIndex=0 }}},
            {1,new RoomObject{
                roomData = new RoomData{ roomId=1, floorIndex=0, roomIndex=1 }}},
            {2,new RoomObject{
                roomData = new RoomData{ roomId=2, floorIndex=1, roomIndex=0 }}}
        };
    }

    public void RenderRooms() {
        foreach (RoomObject room in roomManagerData.roomIdToRoom.Values) {
            StartCoroutine(RenderRoom(room, roomManagerData));
        }
    }
    private IEnumerator RenderRoom(RoomObject room, RoomManagerData roomManagerData) {
        room.roomRuntime = Instantiate(roomManagerData.roomPrefab, roomManagerData.rootObject.transform);
        yield return 0;
        room.roomRuntime.transform.localPosition = RoomManager.GetRoomCenter(room,roomManagerData);
    }

    public static Vector2 GetRoomCenter(RoomObject room, RoomManagerData roomManagerData) {
        return new Vector2(room.roomData.roomIndex*roomManagerData.baseRoomSize.x,
                           room.roomData.floorIndex*roomManagerData.baseRoomSize.y);
    }

    public void OnRequestOpenUI() {
        Debug.Log("space pressed");
    }
}
