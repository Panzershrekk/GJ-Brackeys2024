using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BuildingGenerationBis : MonoBehaviour
{
    public Transform startingPoint;
    public RoomConfiguration[] roomPrefabs;
    public int depthMax = 2;
    public int deadZoneInZ = 3;

    public List<RoomConfiguration> roomConfigurations = new List<RoomConfiguration>();
    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    private void Generate()
    {
        int currenDepth = depthMax;
        RoomConfiguration roomConfiguration = Instantiate(GetRandomPrefab(), startingPoint.position, Quaternion.identity);
        roomConfiguration.isInitialRoom = true;
        roomConfigurations.Add(roomConfiguration);
        roomConfiguration.CreateLinkedRoom(this, currenDepth - 1);
        foreach (RoomConfiguration room in roomConfigurations)
        {
            room.End();
        }
    }

    public RoomConfiguration GetRandomPrefab()
    {
        if (roomPrefabs == null || roomPrefabs.Length == 0)
        {
            Debug.LogError("Prefabs array is empty or null!");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, roomPrefabs.Length);
        return roomPrefabs[randomIndex];
    }
}
