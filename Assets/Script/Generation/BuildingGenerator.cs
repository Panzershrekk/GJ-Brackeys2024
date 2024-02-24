using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
  public RoomConfiguration[] roomPrefabs;
  public GameObject building;
  
  void Start()
  {
    Dictionary<Transform, List<Transform>> buildingDictionary = CreateBuildingDictionary();

    generateBuilding(buildingDictionary, roomPrefabs);
  }

  private void generateBuilding(Dictionary<Transform, List<Transform>> buildingDictionary, RoomConfiguration[] roomPrefabs)
  {
    roomPrefabs = ShuffleRoomPrefabs(roomPrefabs);

    foreach (KeyValuePair<Transform, List<Transform>> floorEntry in buildingDictionary)
    {
      Transform floorTransform = floorEntry.Key;
      List<Transform> roomTransforms = floorEntry.Value;

      foreach (Transform roomTransform in roomTransforms)
      {
        RoomConfiguration roomConfiguration = Instantiate(GetRandomPrefab(roomPrefabs), roomTransform.position, Quaternion.identity);
      }
    }
  }

  private RoomConfiguration[] ShuffleRoomPrefabs(RoomConfiguration[] roomPrefabs)
  {
    RoomConfiguration[] shuffledArray = (RoomConfiguration[])roomPrefabs.Clone();
    for (int i = 0; i < shuffledArray.Length; i++)
    {
      RoomConfiguration temp = shuffledArray[i];
      int randomIndex = UnityEngine.Random.Range(0, shuffledArray.Length);
      shuffledArray[i] = shuffledArray[randomIndex];
      shuffledArray[randomIndex] = temp;
    }
    return shuffledArray;
  }

  private Dictionary<Transform, List<Transform>> CreateBuildingDictionary()
  {
    if (roomPrefabs != null || building != null)
    {

      List<Transform> transformFloors = CollectFloors();
      Dictionary<Transform, List<Transform>> buildingDictionary = new Dictionary<Transform, List<Transform>>();

      foreach (Transform floor in transformFloors)
      {
        buildingDictionary[floor] = CollectRooms(floor);
      }

      foreach (var entry in buildingDictionary)
      {
        string floorName = entry.Key.name;
        List<string> roomNames = entry.Value.ConvertAll(room => room.name);
        Debug.Log($"Floor: {entry.Key.name}, Rooms: {string.Join(", ", roomNames)}");
      }

      // TODO: check if buildingDictionary is empty or null
      return (buildingDictionary);
    }
    else
    {
      Debug.LogError("roomPrefabs or building is not assigned!");

      return (null);
    }
  }

  private List<Transform> CollectFloors()
  {
    List<Transform> floors = new List<Transform>();
    Transform buildingTransform = building.transform;

    for (int index = 0; index < buildingTransform.childCount; index++)
    {
      Transform floorTransform = buildingTransform.GetChild(index);

      // TODO: move to Class for Floor/Room etc.
      // GetCompoentInChildren with plural or not
      if (floorTransform.name.Contains("FLOOR_"))
      {
        floors.Add(floorTransform);
      }
    }

    return (floors);
  }
  private List<Transform> CollectRooms(Transform floorTransform)
  {
    List<Transform> rooms = new List<Transform>();

    for (int index = 0; index < floorTransform.childCount; index++)
    {
      Transform roomTransform = floorTransform.GetChild(index);

      if (roomTransform.name.Contains("ROOM_"))
      {
        rooms.Add(roomTransform);
      }
    }

    // Optionally, print the names of the collected floors to the console
    foreach (Transform room in rooms)
    {
      Debug.Log("Collected room: " + room.name);
    }

    return (rooms);
  }

  private RoomConfiguration GetRandomPrefab(RoomConfiguration[] prefabs)
  {
    if (prefabs == null || prefabs.Length == 0)
    {
      Debug.LogError("Prefabs array is empty or null!");
      return null;
    }

    int randomIndex = UnityEngine.Random.Range(0, prefabs.Length);
    return prefabs[randomIndex];
  }
}
