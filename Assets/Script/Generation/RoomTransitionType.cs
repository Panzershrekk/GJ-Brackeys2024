using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransitionType : MonoBehaviour
{
    public TransitionType Type { get; private set; }

    public enum TransitionType
    {
        None = 0,
        Wall = 1,
        DoorWall = 2,
    }

    public void SetType(TransitionType type)
    {
        Type = type;
    }
}
