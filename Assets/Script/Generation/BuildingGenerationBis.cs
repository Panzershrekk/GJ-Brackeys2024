using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BuildingGenerationBis : MonoBehaviour
{
    public Transform startingPoint;
    public RoomConfiguration[] roomPrefabs;
    public List<RoomFiller> roomFillers = new List<RoomFiller>();
    public int depthMax = 2;
    public int deadZoneInZ = 3;
    public int depthDecreasePercentChance = 80;

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
        foreach (RoomConfiguration room in roomConfigurations)
        {
            room.PlaceTransition();
        }

        foreach (RoomConfiguration room in roomConfigurations)
        {
            int rotation = Random.Range(0, 4);
            RoomFiller filler = Instantiate(GetFittingFiller(room, (RoomConstraint.RotationAzimuth)rotation), room.transform);
            filler.transform.eulerAngles = new Vector3(0, rotation * 90, 0);
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

    public RoomFiller GetFittingFiller(RoomConfiguration currentRoomConfig, RoomConstraint.RotationAzimuth rotationAzimuth = RoomConstraint.RotationAzimuth.North)
    {
        bool canLinkNorth = false;
        bool canLinkSouth = false;
        bool canLinkWest = false;
        bool canLinkEast = false;

        if (currentRoomConfig.northTransition != null && currentRoomConfig.northTransition.Type == RoomTransitionType.TransitionType.DoorWall)
            canLinkNorth = true;
        if (currentRoomConfig.southTransition != null && currentRoomConfig.southTransition.Type == RoomTransitionType.TransitionType.DoorWall)
            canLinkSouth = true;
        if (currentRoomConfig.westTransition != null && currentRoomConfig.westTransition.Type == RoomTransitionType.TransitionType.DoorWall)
            canLinkWest = true;
        if (currentRoomConfig.eastTransition != null && currentRoomConfig.eastTransition.Type == RoomTransitionType.TransitionType.DoorWall)
            canLinkEast = true;



        List<RoomFiller> fittingFiller = new List<RoomFiller>();
        fittingFiller = roomFillers.FindAll(delegate (RoomFiller filler)
        {
            RoomConstraint roomConstraint = filler.roomConstraint;
            roomConstraint.RotateConstraint(rotationAzimuth);
            int matchRequired = 0;
            int currentMatch = 0;
            //North
            if (roomConstraint.CanBeLinkedNorth == false)
            {
                matchRequired += 1;
                if (canLinkNorth == false)
                {
                    currentMatch += 1;
                }
            }
            //South
            if (roomConstraint.CanBeLinkedSouth == false)
            {
                matchRequired += 1;
                if (canLinkSouth == false)
                {
                    currentMatch += 1;
                }
            }
            //West
            if (roomConstraint.CanBeLinkedWest == false)
            {
                matchRequired += 1;
                if (canLinkWest == false)
                {
                    currentMatch += 1;
                }
            }
            //East
            if (roomConstraint.CanBeLinkedEast == false)
            {
                matchRequired += 1;
                if (canLinkEast == false)
                {
                    currentMatch += 1;
                }
            }
            return currentMatch == matchRequired;
        });

        RoomFiller choosenFiller = roomFillers[0];
        if (fittingFiller.Count >= 1)
            choosenFiller = fittingFiller[Random.Range(0, fittingFiller.Count)];
        //if (canLinkNorth == false && choosenFiller.roomConstraint.canBeLinkedNorth == false;
        return choosenFiller;
    }
}
