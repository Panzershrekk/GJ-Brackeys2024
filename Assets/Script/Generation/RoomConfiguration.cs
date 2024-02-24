using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class RoomConfiguration : MonoBehaviour
{
    public RoomConfiguration north;
    public RoomConfiguration east;
    public RoomConfiguration south;
    public RoomConfiguration west;

    public GameObject northTransition;
    public GameObject eastTransition;
    public GameObject southTransition;
    public GameObject westTransition;

    public Collider colliderRoomSize;

    public GameObject doorWall;
    public GameObject wall;
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
            buildingGenerationBis.roomConfigurations.Add(north);
            north.south = this;
        }

        //South
        if (south == null)
            south = CreateRoom(southPos);
        if (south != null)
        {
            south.name = "ROOM_SOUTH_" + depth;
            buildingGenerationBis.roomConfigurations.Add(south);
            south.north = this;
        }

        //East
        if (east == null)
            east = CreateRoom(eastPos);
        if (east != null)
        {
            east.name = "ROOM_EAST_" + depth;
            buildingGenerationBis.roomConfigurations.Add(east);
            east.west = this;
        }

        //West
        if (west == null)
            west = CreateRoom(westPos);
        if (west != null)
        {
            west.name = "ROOM_WEST_" + depth;
            buildingGenerationBis.roomConfigurations.Add(west);
            west.east = this;
        }

        north?.CreateLinkedRoom(buildingGenerationBis, depth - 1);
        south?.CreateLinkedRoom(buildingGenerationBis, depth - 1);
        east?.CreateLinkedRoom(buildingGenerationBis, depth - 1);
        west?.CreateLinkedRoom(buildingGenerationBis, depth - 1);
        if (isInitialRoom)
        {
            Instantiate(doorWall, this.transform.position, transform.rotation * Quaternion.Euler(0f, 0, 0f), this.transform);
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

    public void CreateEnviro(Quaternion rotation)
    {
    }

    public void End()
    {
        if (north == null)
        {
            northTransition = Instantiate(wall, this.transform.position, transform.rotation * Quaternion.Euler(0f, 90, 0f), this.transform);
        }
        if (south == null)
        {
            southTransition = Instantiate(wall, this.transform.position, transform.rotation * Quaternion.Euler(0f, 270, 0f), this.transform);
        }
        if (east == null && !isInitialRoom)
        {
            eastTransition = Instantiate(wall, this.transform.position, transform.rotation * Quaternion.Euler(0f, 0, 0f), this.transform);
        }
        if (west == null)
        {
            westTransition = Instantiate(wall, this.transform.position, transform.rotation * Quaternion.Euler(0f, 180, 0f), this.transform);
        }
        // TRANSITION
        if (northTransition == null)
        {
            northTransition = Instantiate(doorWall, this.transform.position, transform.rotation * Quaternion.Euler(0f, 90, 0f), this.transform);
            if (north != null)
                north.southTransition = northTransition;
        }
        if (southTransition == null)
        {
            southTransition = Instantiate(doorWall, this.transform.position, transform.rotation * Quaternion.Euler(0f, 270, 0f), this.transform);
            if (south != null)
                south.northTransition = southTransition;
        }
        if (eastTransition == null)
        {
            eastTransition = Instantiate(doorWall, this.transform.position, transform.rotation * Quaternion.Euler(0f, 0, 0f), this.transform);
            if (east != null)
                east.westTransition = eastTransition;
        }
        if (westTransition == null)
        {
            westTransition = Instantiate(doorWall, this.transform.position, transform.rotation * Quaternion.Euler(0f, 180, 0f), this.transform);
            if (west != null)
                west.eastTransition = westTransition;
        }
    }

    private RoomConfiguration CreateRoom(Vector3 position)
    {
        if (position.z < buildingGenerationBis.deadZoneInZ)
            return Instantiate(buildingGenerationBis.GetRandomPrefab(), position, Quaternion.identity);
        else
            return null;
    }
}
