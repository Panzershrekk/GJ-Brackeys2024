using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class RoomConfiguration : MonoBehaviour
{
    public enum Azimuth
    {
        North = 0,
        South = 1,
        East = 2,
        West = 3,
    }

    public Azimuth AzimuthEnum { get; private set; }

    public RoomConfiguration north;
    public RoomConfiguration east;
    public RoomConfiguration south;
    public RoomConfiguration west;

    public RoomTransitionType northTransition;
    public RoomTransitionType eastTransition;
    public RoomTransitionType southTransition;
    public RoomTransitionType westTransition;

    public Collider colliderRoomSize;

    public List<RoomTransitionType> doorWall;
    public List<RoomTransitionType> wall;
    public LayerMask layerMask;
    [HideInInspector]
    public bool isInitialRoom = false;
    private BuildingGenerationBis buildingGenerationBis;

    public void CreateLinkedRoom(BuildingGenerationBis gen, int depth)
    {
        buildingGenerationBis = gen;
        Bounds bounds = colliderRoomSize.bounds;
        Vector3 northPos = this.transform.position + new Vector3(bounds.size.x, 0, 0);
        Vector3 southPos = this.transform.position + new Vector3(-bounds.size.x, 0, 0);
        Vector3 eastPos = this.transform.position + new Vector3(0, 0, bounds.size.z);
        Vector3 westPos = this.transform.position + new Vector3(0, 0, -bounds.size.z);
        if (north == null)
        {
            north = CheckIfObjectExistsAtPoint(northPos);
            if (north != null)
            {
                north.south = this;
            }
        }
        if (south == null)
        {
            south = CheckIfObjectExistsAtPoint(southPos);
            if (south != null)
            {
                south.north = this;
            }
        }
        if (east == null)
        {
            east = CheckIfObjectExistsAtPoint(eastPos);
            if (east != null)
            {
                east.west = this;
            }
        }
        if (west == null)
        {
            west = CheckIfObjectExistsAtPoint(westPos);
            if (west != null)
            {
                west.east = this;
            }
        }

        if (depth == 0)
        {
            return;
        }


        //North
        if (north == null)
            north = CreateRoom(northPos);
        if (north != null)
        {
            north.name = "ROOM_NORTH_" + depth;
            if (!buildingGenerationBis.roomConfigurations.Contains(north))

                buildingGenerationBis.roomConfigurations.Add(north);
            north.south = this;
        }

        //South
        if (south == null)
            south = CreateRoom(southPos);
        if (south != null)
        {
            south.name = "ROOM_SOUTH_" + depth;
            if (!buildingGenerationBis.roomConfigurations.Contains(south))

                buildingGenerationBis.roomConfigurations.Add(south);
            south.north = this;
        }

        //East
        if (east == null)
            east = CreateRoom(eastPos);
        if (east != null)
        {
            east.name = "ROOM_EAST_" + depth;
            if (!buildingGenerationBis.roomConfigurations.Contains(east))
                buildingGenerationBis.roomConfigurations.Add(east);
            east.west = this;
        }

        //West
        if (west == null)
            west = CreateRoom(westPos);
        if (west != null)
        {
            west.name = "ROOM_WEST_" + depth;
            if (!buildingGenerationBis.roomConfigurations.Contains(west))
                buildingGenerationBis.roomConfigurations.Add(west);
            west.east = this;
        }


        north?.CreateLinkedRoom(buildingGenerationBis, DepthDecrease(depth));
        south?.CreateLinkedRoom(buildingGenerationBis, DepthDecrease(depth));
        east?.CreateLinkedRoom(buildingGenerationBis, DepthDecrease(depth));
        west?.CreateLinkedRoom(buildingGenerationBis, DepthDecrease(depth));
        if (isInitialRoom && eastTransition == null)
        {
            eastTransition = Instantiate(GetRandomDoorWall(), this.transform.position, transform.rotation * Quaternion.Euler(0f, 0, 0f), this.transform);
            eastTransition.SetType(RoomTransitionType.TransitionType.DoorWall);
        }
    }

    public RoomConfiguration CheckIfObjectExistsAtPoint(Vector3 position)
    {
        RaycastHit[] hits = Physics.RaycastAll(position, Vector3.down, Mathf.Infinity, layerMask);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.GetComponent<RoomConfiguration>() != null)
            {
                return hit.transform.GetComponent<RoomConfiguration>();
            }
        }
        return null;
    }

    public void End()
    {

        //Place wall at the edge
        if (north == null && northTransition == null)
        {
            northTransition = Instantiate(GetRandomWall(), this.transform.position, transform.rotation * Quaternion.Euler(0f, 90, 0f), this.transform);
            northTransition.SetType(RoomTransitionType.TransitionType.Wall);

        }
        if (south == null && southTransition == null)
        {
            southTransition = Instantiate(GetRandomWall(), this.transform.position, transform.rotation * Quaternion.Euler(0f, 270, 0f), this.transform);
            southTransition.SetType(RoomTransitionType.TransitionType.Wall);
        }
        if (east == null && !isInitialRoom && eastTransition == null)
        {
            eastTransition = Instantiate(GetRandomWall(), this.transform.position, transform.rotation * Quaternion.Euler(0f, 0, 0f), this.transform);
            eastTransition.SetType(RoomTransitionType.TransitionType.Wall);

        }
        if (west == null && westTransition == null)
        {
            westTransition = Instantiate(GetRandomWall(), this.transform.position, transform.rotation * Quaternion.Euler(0f, 180, 0f), this.transform);
            westTransition.SetType(RoomTransitionType.TransitionType.Wall);

        }
        // TRANSITION

        if (northTransition == null)
        {
            northTransition = Instantiate(GetRandomWall(), this.transform.position, transform.rotation * Quaternion.Euler(0f, 90, 0f), this.transform);
            northTransition.SetType(RoomTransitionType.TransitionType.Wall);
            if (north != null)
                north.southTransition = northTransition;
        }
        if (southTransition == null)
        {
            southTransition = Instantiate(GetRandomWall(), this.transform.position, transform.rotation * Quaternion.Euler(0f, 270, 0f), this.transform);
            southTransition.SetType(RoomTransitionType.TransitionType.Wall);
            if (south != null)
                south.northTransition = southTransition;
        }
        if (eastTransition == null)
        {
            eastTransition = Instantiate(GetRandomWall(), this.transform.position, transform.rotation * Quaternion.Euler(0f, 0, 0f), this.transform);
            eastTransition.SetType(RoomTransitionType.TransitionType.Wall);
            if (east != null)
                east.westTransition = eastTransition;
        }
        if (westTransition == null)
        {
            westTransition = Instantiate(GetRandomWall(), this.transform.position, transform.rotation * Quaternion.Euler(0f, 180, 0f), this.transform);
            westTransition.SetType(RoomTransitionType.TransitionType.Wall);
            if (west != null)
                west.eastTransition = westTransition;
        }
    }

    public void CreateTransitionByAzimuth(RoomTransitionType transition, RoomTransitionType.TransitionType type, Azimuth azimuth, bool destroyPrevious = false)
    {
        //North
        if (azimuth == Azimuth.North)
        {
            if (destroyPrevious == true)
            {
                Destroy(northTransition.gameObject);
                northTransition = null;
            }
            northTransition = Instantiate(transition, this.transform.position, transform.rotation * Quaternion.Euler(0f, 90, 0f), this.transform);
            northTransition.SetType(type);
            if (north != null)
                north.southTransition = northTransition;
        }
        //South
        if (azimuth == Azimuth.South)
        {
            if (destroyPrevious == true)
            {
                Destroy(southTransition.gameObject);
                southTransition = null;
            }
            southTransition = Instantiate(transition, this.transform.position, transform.rotation * Quaternion.Euler(0f, 270, 0f), this.transform);
            southTransition.SetType(type);
            if (south != null)
                south.northTransition = southTransition;
        }

        //East
        if (azimuth == Azimuth.East)
        {
            if (destroyPrevious == true)
            {
                Destroy(eastTransition.gameObject);
                eastTransition = null;
            }
            eastTransition = Instantiate(transition, this.transform.position, transform.rotation * Quaternion.Euler(0f, 0, 0f), this.transform);
            eastTransition.SetType(type);
            if (east != null)
                east.westTransition = eastTransition;
        }

        //West
        if (azimuth == Azimuth.West)
        {
            if (destroyPrevious == true)
            {
                Destroy(westTransition.gameObject);
                westTransition = null;
            }
            westTransition = Instantiate(transition, this.transform.position, transform.rotation * Quaternion.Euler(0f, 180, 0f), this.transform);
            westTransition.SetType(type);
            if (west != null)
                west.eastTransition = westTransition;
        }
    }

    public void EndBis()
    {
        int roomAroundCount = GetNeighborCount();
        int minimumDoor = roomAroundCount <= 2 ? 1 : 2;
        int maximumDoor = roomAroundCount;
        int currentNumberOfDoor = GetNeighborDoorCount();
        int numberOfDoor = UnityEngine.Random.Range(minimumDoor, maximumDoor);
        List<RoomConfiguration> roomConfigurations = GetRoomWithActualNeighborWithoutDoor();

        //Debug.Log(String.Format("RoomName: {0}, Room Around: {1}, MinimumDoor: {2}, MaximumDoor: {3} CurrentNumberOfDoor: {4} NumberOfDoor {5}", transform.name, roomAroundCount, minimumDoor, maximumDoor, currentNumberOfDoor, numberOfDoor));

        while (currentNumberOfDoor < numberOfDoor)
        {
            RoomConfiguration selectedConfiguration = roomConfigurations[UnityEngine.Random.Range(0, roomConfigurations.Count)];

            CreateTransitionByAzimuth(GetRandomDoorWall(), RoomTransitionType.TransitionType.DoorWall, selectedConfiguration.AzimuthEnum, true);
            roomConfigurations.Remove(selectedConfiguration);
            currentNumberOfDoor += 1;
        }
    }

    public int GetNeighborCount()
    {
        int count = 0;
        count += north != null ? 1 : 0;
        count += south != null ? 1 : 0;
        count += east != null ? 1 : 0;
        count += west != null ? 1 : 0;

        return count;
    }

    public int GetNeighborDoorCount()
    {
        int count = 0;
        if (northTransition != null && northTransition.Type == RoomTransitionType.TransitionType.DoorWall)
            count += 1;
        if (southTransition != null && southTransition.Type == RoomTransitionType.TransitionType.DoorWall)
            count += 1;
        if (eastTransition != null && eastTransition.Type == RoomTransitionType.TransitionType.DoorWall)
            count += 1;
        if (westTransition != null && westTransition.Type == RoomTransitionType.TransitionType.DoorWall)
            count += 1;
        return count;
    }

    public List<RoomConfiguration> GetRoomWithActualNeighborWithoutDoor()
    {
        List<RoomConfiguration> roomTransitionTypes = new List<RoomConfiguration>();
        if (north != null && northTransition.Type != RoomTransitionType.TransitionType.DoorWall)
        {
            roomTransitionTypes.Add(north);
            north.SetAzimuth(Azimuth.North);
        }
        if (south != null && southTransition.Type != RoomTransitionType.TransitionType.DoorWall)
        {
            roomTransitionTypes.Add(south);
            south.SetAzimuth(Azimuth.South);
        }
        if (east != null && eastTransition.Type != RoomTransitionType.TransitionType.DoorWall)
        {
            roomTransitionTypes.Add(east);
            east.SetAzimuth(Azimuth.East);
        }
        if (west != null && westTransition.Type != RoomTransitionType.TransitionType.DoorWall)
        {
            roomTransitionTypes.Add(west);
            west.SetAzimuth(Azimuth.West);
        }
        return roomTransitionTypes;
    }

    private int DepthDecrease(int depth)
    {
        int r = UnityEngine.Random.Range(0, 100);
        int depthDec = depth;
        if (r < buildingGenerationBis.depthDecreasePercentChance)
        {
            depthDec -= 1;
        }
        return depthDec;
    }

    public void SetAzimuth(Azimuth azimuth)
    {
        AzimuthEnum = azimuth;
    }

    private RoomTransitionType GetRandomWall()
    {
       return wall[UnityEngine.Random.Range(0, wall.Count)];
    }

    private RoomTransitionType GetRandomDoorWall()
    {
        return doorWall[UnityEngine.Random.Range(0, doorWall.Count)];
    }

    private RoomConfiguration CreateRoom(Vector3 position)
    {
        if (position.z < buildingGenerationBis.deadZoneInZ)
            return Instantiate(buildingGenerationBis.GetRandomPrefab(), position, Quaternion.identity);
        else
            return null;
    }
}
