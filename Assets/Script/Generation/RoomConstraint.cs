using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class RoomConstraint : MonoBehaviour
{
    public enum RotationAzimuth
    {
        North = 0,
        West = 1,
        South = 2,
        East = 3,
    }

    public bool _canBeLinkedNorth = false;
    public bool _canBeLinkedSouth = false;
    public bool _canBeLinkedEast = false;
    public bool _canBeLinkedWest = false;

    public bool onEdgeOnly = false;

    public bool CanBeLinkedNorth
    {
        get { return _canBeLinkedNorth; }
        set { _canBeLinkedNorth = value; }
    }

    public bool CanBeLinkedSouth
    {
        get { return _canBeLinkedSouth; }
        set { _canBeLinkedSouth = value; }
    }

    public bool CanBeLinkedEast
    {
        get { return _canBeLinkedEast; }
        set { _canBeLinkedEast = value; }
    }

    public bool CanBeLinkedWest
    {
        get { return _canBeLinkedWest; }
        set { _canBeLinkedWest = value; }
    }
    

    public void RotateConstraint(RotationAzimuth rotation)
    {
        bool tmpNorth = _canBeLinkedNorth;
        bool tmpSouth = _canBeLinkedSouth;
        bool tmpEast = _canBeLinkedEast;
        bool tmpWest = _canBeLinkedWest;

        //North is in the exact same place, so we don't need to handle it

        //West 90
        if (rotation == RotationAzimuth.West)
        {
            _canBeLinkedNorth = tmpEast;
            _canBeLinkedWest = tmpNorth;
            _canBeLinkedSouth = tmpWest;
            _canBeLinkedEast = tmpSouth;
        }
        //South 180
        if (rotation == RotationAzimuth.South)
        {
            _canBeLinkedNorth = tmpSouth;
            _canBeLinkedWest = tmpEast;
            _canBeLinkedSouth = tmpNorth;
            _canBeLinkedEast = tmpWest;
        }
        //East 270
        if (rotation == RotationAzimuth.East)
        {
            _canBeLinkedNorth = tmpWest;
            _canBeLinkedWest = tmpSouth;
            _canBeLinkedSouth = tmpEast;
            _canBeLinkedEast = tmpNorth;
        }
    }
}
